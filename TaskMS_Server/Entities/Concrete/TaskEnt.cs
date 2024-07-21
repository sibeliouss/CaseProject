using Core.Entities;
using Entities.Enums;

namespace Entities.Concrete;

public class TaskEnt : Entity<Guid>
{
    public string Title { get; set; } 
    public string Description { get; set; }
    public TasksStatus Status { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
  
    public ICollection<TaskFile>? FileUrls { get; set; }= new List<TaskFile>();
}