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
                    .Where(g => g.Deck.Cards.Any())
                    .GroupBy(g => g.User.Email)
                    .Select(u => new
                    {
                        Email = u.Key,
                        GoalInfo = u.Select(g => new
                        {
                            Goal = g,
                            GoalDate = g.GoalDate,
                            AssessDate = g.SelfAssessmentDate,
                            StudyLogs = g.Deck.Cards.Select(c => c.Card.Logs.OrderByDescending(l => l.Timestamp).FirstOrDefault())
                        })
                    });

                foreach (var emailGoals in userEmailsWithGoals)
                {
                    var studyGoalNameList = "";
                    var assessGoalNameList = "";
                    var emailBody = "";


                    // Study reminders
                    var studyGoalsToRemind = emailGoals.GoalInfo
                        .Where(
                        g => (g.GoalDate.Date >= DateTime.UtcNow.Date) &&
                            (
                                g.StudyLogs.Any(l => l == null) || 
                                g.StudyLogs.Any(l => l.Stage == 1 && (DateTime.UtcNow - l.Timestamp).TotalHours > 12) ||
                                g.StudyLogs.Any(l => l.Stage == 2 && (DateTime.UtcNow - l.Timestamp).TotalHours > 24) ||
                                g.StudyLogs.Any(l => l.Stage == 3 && (DateTime.UtcNow - l.Timestamp).TotalHours > 36) ||
                                g.StudyLogs.Any(l => l.Stage == 4 && (DateTime.UtcNow - l.Timestamp).TotalHours > 48)
                            )
                        ).ToList();

                    if (studyGoalsToRemind.Any())
                    {
                        var activeGoalNames = studyGoalsToRemind.Select(g => g.Goal.Title).ToList();
                        studyGoalNameList = string.Join(", ", activeGoalNames);
                        emailBody += $"<b>Goals Needing Study:</b> {studyGoalNameList}<br/>";
                    }

                    // Assess Reminders
                    var assessGoalsToRemind = emailGoals.GoalInfo
                        .Where( g => (g.GoalDate.Date < DateTime.UtcNow.Date) && (g.AssessDate <= DateTime.MinValue))
                        .ToList();

                    if (assessGoalsToRemind.Any())
                    {
                        var activeGoalNames = assessGoalsToRemind.Select(g => g.Goal.Title).ToList();
                        assessGoalNameList = string.Join(", ", activeGoalNames);
                        emailBody += $"<b>Goals Needing Assessments:</b> {assessGoalNameList}";
                    }

                    if (!String.IsNullOrWhiteSpace(emailBody))
                    {
                        _emailSender.SendEmailAsync(emailGoals.Email, $"Study Reg Reminders", emailBody);
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
