using Entities.Enums;

namespace Entities.Dtos;

public record TaskUpdateDto(
    Guid Id,
    string Title,
    string Description,
    TasksStatus Status
);