class TeamTask
{
    public int Id { get; private set; } = 0; // is this still readonly`?

    public required string Title { get; set; }

    public string? Description;

    private string? AssignedTo;
    public  string? Label => AssignedTo ?? "Unassigned"; // is reaonly because of expression-bodied syntax

    public bool IsOverdue => DueDate < DateTime.Now; // should handle due date is null
    public DateTime? DueDate;

    public  TaskStatus Status {get; private set;} = TaskStatus.Backlog;

    public void Assign(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            throw ArgumentException.ThrowIfNullOrWhiteSpace("User cannot be null or empty");
        }

        AssignedTo = user;
    }

    public void Transition(TaskStatus newStatus)
    {
        if (!newStatus.Equals(Status))
        {
            Status = newStatus;
            return;
        }
    }
}