public class TaskStatusChangedArgs : EventArgs 
{
    public int TaskId {get; init;}
    public string Title {get; init;} = "";
    public TaskStatus OldStatus {get; init;} 
    public TaskStatus NewStatus {get; init;}

    public string? AssignedTo {get; init;}
}