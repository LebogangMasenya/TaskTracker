using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
// registering services as Signletons


builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddSingleton<INotifier, ConsoleNotifier>();
builder.Services.AddSingleton<INotifier, AuditNotifier>();

var app = builder.Build();


ConsoleNotifier consoleNotifier = new ConsoleNotifier();
AuditNotifier auditNotifier = new AuditNotifier();

var task = new TeamTask { Title = "Fix login bug" };
task.StatusChanged += auditNotifier.Notify;
task.StatusChanged += consoleNotifier.Notify;
task.Assign("Alice");

task.Transition(TaskStatus.InProgress);
task.Transition(TaskStatus.InReview);
task.Transition(TaskStatus.Done);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.RegisterEndpoints();



app.Run();