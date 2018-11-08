using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudyReg.Web.Areas.Identity.Data;
using StudyReg.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudyReg.Web.Services
{
    internal class NotifierService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEmailSender _emailSender;
        private Timer _timer;

        public NotifierService(IServiceScopeFactory scopeFactory, IEmailSender emailSender)
        {
            _scopeFactory= scopeFactory;
            _emailSender = emailSender;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var userEmailsWithGoals = ctx.Goal
                    .AsNoTracking()
                    .Include(g => g.User)
                    .Include(g => g.Deck.Cards)
                    .Where(g => (g.GoalDate.Date >= DateTime.UtcNow.Date) && g.Deck.Cards.Any())
                    .GroupBy(g => g.User.Email)
                    .Select(u => new
                    {
                        Email = u.Key,
                        GoalInfo = u.Select(g => new
                        {
                            Goal = g,
                            StudyLogs = g.Deck.Cards.Select(c => c.Card.Logs.OrderByDescending(l => l.Timestamp).FirstOrDefault())
                        })
                    });

                foreach (var emailGoals in userEmailsWithGoals)
                {
                    var goalsToRemind = emailGoals.GoalInfo
                        .Where(
                        g => g.StudyLogs.Any(l => l == null) || 
                        g.StudyLogs.Any(l => l.Stage == 1 && (DateTime.UtcNow - l.Timestamp).TotalHours > 12) ||
                        g.StudyLogs.Any(l => l.Stage == 2 && (DateTime.UtcNow - l.Timestamp).TotalHours > 24) ||
                        g.StudyLogs.Any(l => l.Stage == 3 && (DateTime.UtcNow - l.Timestamp).TotalHours > 36) ||
                        g.StudyLogs.Any(l => l.Stage == 4 && (DateTime.UtcNow - l.Timestamp).TotalHours > 48)
                        ).ToList();

                    if (goalsToRemind.Any())
                    {
                        var activeGoalNames = goalsToRemind.Select(g => g.Goal.Title).ToList();

                        _emailSender.SendEmailAsync(emailGoals.Email, $"Study Reminder", string.Join(Environment.NewLine, activeGoalNames));
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
