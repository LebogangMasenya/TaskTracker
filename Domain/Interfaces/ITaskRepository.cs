public interface ITaskRepository
{
    TeamTask? GetTeamTaskById(int taskId);

    TeamTask CreateTask(string title, string? description, string? assignedTo, DateTime? dueDate);
    List<TeamTask> GetTeamTasks();
    public List<TeamTask> GetOverdueTasks();
    public IAssignable? GetAssignableEntity(int id);
    public ITransitionable? GetTransitionalEntity(int id);
    public ISchedulable? GetSchedulableEntity(int id);

}