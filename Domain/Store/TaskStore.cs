class TaskStore
{
    private List<TeamTask> teamTasks = new List<TeamTask>();

    private int _nextId = 1;

    public IAssignable? GetAssignableEntity(int id)
    {
        return teamTasks.FirstOrDefault(t => t.Id == id);
    }

    public ITransitionable? GetTransitionalEntity(int id)
    {
        return teamTasks.FirstOrDefault(t => t.Id == id);
    }

    public ISchedulable? GetSchedulableEntity(int id)
    {
        return teamTasks.FirstOrDefault(t => t.Id == id);
    }
    
    public TeamTask? GetTeamTaskById(int taskId)
    {
        return teamTasks.FirstOrDefault(task => task.Id == taskId);
    }

    public List<TeamTask> GetTeamTasks()
    {
        return teamTasks;
    }

    public TeamTask CreateTask(string title, string? description, string? assignedTo, DateTime? dueDate)
    {
        TeamTask newTask  = new()
        {
            Id = _nextId++,
            Title = title,
            Description = description,
            DueDate = dueDate
        };

        if(!string.IsNullOrWhiteSpace(assignedTo))
        {
            newTask.Assign(assignedTo);
        }

        teamTasks.Add(newTask);
        return newTask;
    }

    public List<TeamTask> GetOverdueTasks()
    {
        return teamTasks.Where(task => task.IsOverdue).ToList();
    }
}