class AuditLogger
{
    public List<string> Log = new List<string>();

    public void LogEvent(object sender, TaskStatusChangedArgs taskArguments)
    {
        Log.Add($"[{DateTime.Now}]\t Task #{taskArguments.TaskId}\t \"{taskArguments.Title}\"\t {taskArguments.OldStatus} -> {taskArguments.NewStatus}");
    }
}