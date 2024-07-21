using Entities.Concrete;
using Entities.Enums;

namespace Entities.Dtos;

public record TaskResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public User? User { get; set; }
    public TasksStatus Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CreateAt { get; set; } = string.Empty;
}