
class ConsoleNotifier: INotifier
{
   public void Notify(object sender, TaskStatusChangedArgs args)
    {
        var teamTask = (TeamTask)sender;

        Console.WriteLine($"[Notify]\t \"{teamTask.Title}\" is now {args.NewStatus}");

        if (args.NewStatus == TaskStatus.Done)
        {
            string assignee = teamTask.Label ?? "Someone";
            Console.WriteLine($"\"{teamTask.Title}\" marked as Done by {assignee}");
        }
    }
}