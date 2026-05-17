 using System.Text.Json.Serialization;

 
 interface ITransitionable
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TaskStatus Status {get;  set;}

    public void Transition(TaskStatus newStatus);
}