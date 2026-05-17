var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();
// registering services as Signletons
builder.Services.AddSingleton<AuditLogger>();
builder.Services.AddSingleton<TaskStore>();




AuditLogger auditLogger = new AuditLogger();

var task = new TeamTask { Title = "Fix login bug" };
task.StatusChanged += auditLogger.LogEvent;
task.StatusChanged += (sender, taskEvent) =>
{
    var teamTask = (TeamTask)sender!;

    Console.WriteLine($"[Notify]\t \"{teamTask.Title}\" is now {teamTask.Status}");
};
task.StatusChanged += (sender, taskEvent) =>
{
    if (taskEvent.NewStatus.Equals("Done"))
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


app.MapGet("/api/tasks", (TaskStore store) =>
{
    var teamTasks = store.GetTeamTasks();

    return Results.Ok(teamTasks);
}).WithDisplayName("GetAllTasks");


app.MapGet("/api/tasks/{id}", (int id, TaskStore store) =>
{
    var task = store.GetTeamTaskById(id);

    if (task == null)
    {
        return Results.NotFound(new { message = $"Task {id} not found" });
    }

    return Results.Ok(task);
}).WithDisplayName("GetTask");


app.MapPost("/api/tasks", (TaskStore store, CreateTaskRequest request, AuditLogger logger) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new { message = "Title  is required" });

    }
    else
    {
        var createdTask = store.CreateTask(request.Title, request.Description, request.AssignedTo, request.DueDate);

        createdTask.StatusChanged += logger.LogEvent;

        return Results.CreatedAtRoute("GetTask", new { id = createdTask.Id });
    }

}).WithDisplayName("CreateTask");


app.MapPatch("/api/tasks/{id}/assign", (int id, AssignRequest request, TaskStore store) =>
{
    var updateTask = store.GetTeamTasks().FirstOrDefault(task => task.Id == id);

    if (updateTask == null)
    {
        return Results.NotFound();
    }
       
    if(string.IsNullOrWhiteSpace(request.User))
    {
        return Results.BadRequest(new {message = "Request missing user"});
    }
    
    updateTask.Assign(request.User);
    return Results.NoContent();
    
}).WithDisplayName("UpdateTaskAssignee");


app.MapPatch("/api/tasks/{id}/status", (int id, TransitionRequest request, TaskStore store) =>
{
    var updateTask = store.GetTeamTasks().FirstOrDefault(task => task.Id == id);

    if (updateTask == null)
    {
        return Results.NotFound();
    }
       
    if(string.IsNullOrWhiteSpace(request.NewStatus.ToString()))
    {
        return Results.BadRequest(new {message = "Request missing NewStatus"});
    }

    if(updateTask.Status == request.NewStatus)
    {
        return Results.BadRequest(new {message = "Current status is the same as the new status."});
    }

    updateTask.Transition(request.NewStatus);
    return Results.NoContent();
}).WithDisplayName("UpdateTaskStatus");

app.MapGet("/api/tasks/overdue", (TaskStore store) =>
{
    var teamTasks = store.GetOverdueTasks();

    return Results.Ok(teamTasks);

}).WithDisplayName("GetOverdueTasks");


app.Run();

