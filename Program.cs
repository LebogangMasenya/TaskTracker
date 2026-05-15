var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

AuditLogger auditLogger = new AuditLogger();

var task = new TeamTask {Title = "Fix login bug"};
task.StatusChanged += auditLogger.LogEvent;
task.StatusChanged += (sender, taskEvent) => 
{
    var teamTask = (TeamTask)sender!;

    Console.WriteLine($"[Notify]\t \"{teamTask.Title}\" is now {teamTask.Status}");
};
task.StatusChanged += (sender, taskEvent) =>
{
    if(taskEvent.NewStatus.Equals("Done"))
    {
        var teamTask = (TeamTask)sender!;
        string assignee = teamTask.AssignedTo ?? "Someone";
        Console.WriteLine($"\"{teamTask.Title}\" marked as Done by {assignee}");
    }
};


task.Assign("Alice");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

