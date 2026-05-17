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


app.MapGet("/api/tasks", (ITaskRepository store) =>
{
    var teamTasks = store.GetTeamTasks();

    return Results.Ok(teamTasks);
}).WithDisplayName("GetAllTasks");


app.MapGet("/api/tasks/{id}", (int id, ITaskRepository store) =>
{
    var task = store.GetTeamTaskById(id);

    if (task == null)
    {
        return Results.NotFound(new { message = $"Task {id} not found" });
    }

    return Results.Ok(task);
}).WithDisplayName("GetTask").WithName("GetTask");
//WIthName is an internal system identifier, displayName is for human readability

app.MapPost("/api/tasks", (ITaskRepository repo, CreateTaskRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new { message = "Title  is required" });
    }
    else
    {
        var createdTask = repo.CreateTask(request.Title, request.Description, request.AssignedTo, request.DueDate);

        createdTask.StatusChanged += AuditLogger.LogEvent;

        return Results.CreatedAtRoute("GetTask", new { id = createdTask.Id });
    }

}).WithDisplayName("CreateTask");


app.MapPatch("/api/tasks/{id}/assign", (int id, AssignRequest request, ITaskRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(request.User))
    {
        return Results.BadRequest(new { message = "Request missing user" });
    }

    IAssignable? entity = repo.GetAssignableEntity(id);
    if (entity == null)
    {
        return Results.NotFound($"No item to assign with id {id}");
    }
    entity.Assign(request.User);
    return Results.NoContent();

}).WithDisplayName("UpdateTaskAssignee");


app.MapPatch("/api/tasks/{id}/status", (int id, TransitionRequest request, ITaskRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(request.NewStatus.ToString()))
    {
        return Results.BadRequest(new { message = "Request missing NewStatus" });
    }

    ITransitionable? entity = repo.GetTransitionalEntity(id);

    if (entity == null)
    {
        return Results.NotFound($"No item to transition with id {id}");
    }

    if (entity.Status == request.NewStatus)
    {
        return Results.BadRequest(new { message = "Current status is the same as the new status." });
    }

    entity.Transition(request.NewStatus);
    return Results.NoContent();
}).WithDisplayName("UpdateTaskStatus");

app.MapGet("/api/tasks/overdue", (ITaskRepository repo) =>
{
    var teamTasks = repo.GetOverdueTasks();

    return Results.Ok(teamTasks);

}).WithDisplayName("GetOverdueTasks");


app.Run();