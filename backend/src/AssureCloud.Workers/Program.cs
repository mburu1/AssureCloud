using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<AssureCloud.Workers.Assessment.AssessmentWorker>();
builder.Services.AddHostedService<AssureCloud.Workers.Notifications.NotificationsWorker>();
builder.Services.AddHostedService<AssureCloud.Workers.Reports.ReportsWorker>();
builder.Services.AddHostedService<AssureCloud.Workers.Certification.CertificationWorker>();

var app = builder.Build();

app.Run();
