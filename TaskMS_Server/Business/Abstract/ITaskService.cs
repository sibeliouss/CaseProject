using Entities.Concrete;
using Entities.Dtos;
using Entities.Enums;

namespace Business.Abstract;

public interface ITaskService
{
    Task AddAsync(TaskAddDto taskAddDto, Guid userId);
    Task<TaskEnt?> GetByIdAsync(Guid taskId);
    Task<List<TaskResponseDto>> GetAllAsync(string roles, Guid userId);
    Task UpdateAsync(TaskUpdateDto taskUpdateDto);
    Task DeleteAsync(Guid taskId);
    Task<List<TaskResponseDto>> FilterByStatusAsync(TasksStatus status, Guid userId, bool isAdmin);
}