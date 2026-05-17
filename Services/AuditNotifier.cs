
public class AuditNotifier : INotifier
{
    public void Notify(object sender,TaskStatusChangedArgs args)
    {
       AuditLogger.LogEvent(sender, args);
    }
}