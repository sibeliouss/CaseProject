using Core.Entities;

namespace Entities.Concrete;

public class TaskFile : Entity<Guid>
{
    public Guid TaskId { get; set; }
    public string FileUrl { get; set; } = string.Empty;
}