record CreateTaskRequest(string Title, string? Description, string? AssignedTo, DateTime? DueDate);
record AssignRequest(string User);
record TransitionRequest(TaskStatus NewStatus);
