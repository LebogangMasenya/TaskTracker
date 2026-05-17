using System.Text.Json.Serialization;

public class TeamTask: IAssignable, ITransitionable, ISchedulable
{
    public  int Id { get;  init; }

    public required string Title { get; set; }

    public string? Description {get;  set;}

    private string? AssignedTo {get;  set;}
    public  string? Label => AssignedTo ?? "Unassigned"; // is reaonly because of expression-bodied syntax

    public bool IsOverdue => DueDate.HasValue && DueDate < DateTime.Now; 
    public DateTime? DueDate  {get;  set;}

    // JsonConverter ensures this prints as "Backlog" instead of a number like 0 in  JSON response
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TaskStatus Status {get;  set;} = TaskStatus.Backlog;


    public event EventHandler<TaskStatusChangedArgs>? StatusChanged;

    public void Assign(string user)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(user, nameof(user));

        AssignedTo = user;
    }

    public void Transition(TaskStatus newStatus)
    {
        if (!newStatus.Equals(Status))
        {
            Status = newStatus;

            var eventArgument = new TaskStatusChangedArgs
            {
                TaskId = Id,
                OldStatus = Status,
                NewStatus = newStatus
            };

            // have null guarding
            StatusChanged?.Invoke(this, eventArgument);
        }
    }


}