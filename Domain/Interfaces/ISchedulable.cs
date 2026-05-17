public interface ISchedulable
{
    public bool IsOverdue => DueDate.HasValue && DueDate < DateTime.Now;
    public DateTime? DueDate { get; set; }
}