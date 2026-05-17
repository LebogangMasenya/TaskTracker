
class AuditNotifier : INotifier
{
    private readonly AuditLogger _logger = new AuditLogger();


    public void Notify(object sender,TaskStatusChangedArgs args)
    {
       _logger.LogEvent(sender, args);
    }
}