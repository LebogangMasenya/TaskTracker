 using System.Text.Json.Serialization;

 
 public interface ITransitionable
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
     TaskStatus Status {get;  set;}

     void Transition(TaskStatus newStatus);
}