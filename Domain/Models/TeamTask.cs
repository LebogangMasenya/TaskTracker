class TeamTask
{
    public int Id { get;  set; } = 0; // is this still readonly`?

    public required string Title { get; set; }

    public string? Description;

    public string? AssignedTo {get; private set;}
    public  string? Label => AssignedTo ?? "Unassigned"; // is reaonly because of expression-bodied syntax

    public bool IsOverdue => DueDate < DateTime.Now; // should handle due date is null
    public DateTime? DueDate;

    public  TaskStatus Status {get; private set;} = TaskStatus.Backlog;


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
            return;
        }
    }


}