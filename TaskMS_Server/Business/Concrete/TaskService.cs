using Business.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskAddDto taskAddDto, Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId);
        if (user is null)
        {
            throw new Exception("Kullanıcı bulunmadı!");
        }

        var checkTaskIsUnique = await _context.TaskEnts.AnyAsync(p => p.Title == taskAddDto.Title);
        if (checkTaskIsUnique)
        {
            throw new Exception("Bu görev başlığı zaten mevcut");
        }

        var task = new TaskEnt
        {
            Id = Guid.NewGuid(),
            CreateAt = DateTime.Now,
            UserId = userId,
            Title = taskAddDto.Title,
            Description = taskAddDto.Description,
            Status = TasksStatus.New
        };

        if (taskAddDto.Files is not null)
        {
            task.FileUrls = new List<TaskFile>();

            foreach (var file in taskAddDto.Files)
            {
                var fileFormat = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var fileName = Guid.NewGuid().ToString() + fileFormat;
                using (var stream = System.IO.File.Create("/Users/sozey/Projects/toDoProject/TaskMSClient/public/img/" + fileName))
                {
                    file.CopyTo(stream);
                }

                var taskFile = new TaskFile
                {
                    Id = Guid.NewGuid(),
                    TaskId = task.Id,
                    FileUrl = fileName
                };

                task.FileUrls.Add(taskFile);
            }
        }

        _context.TaskEnts.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskEnt?> GetByIdAsync(Guid taskId)
    {
        return await _context.TaskEnts
            .Include(p => p.User)
            .Include(p => p.FileUrls)
            .FirstOrDefaultAsync(p => p.Id == taskId);
    }

    public async Task<List<TaskResponseDto>> GetAllAsync(string roles, Guid userId)
    {
        IQueryable<TaskResponseDto> tasks = _context.TaskEnts
            .Include(p => p.User)
            .OrderByDescending(t => t.CreateAt)
            .Select(s => new TaskResponseDto
            {
                Id = s.Id,
                UserId = s.UserId,
                User = s.User,
                UserName = s.User!.GetName(),
                CreateAt = s.CreateAt.ToString("o"),
                Title = s.Title,
                Description = s.Description,
                Status = s.Status
            });

        if (!roles.Contains("Admin"))
        {
            tasks = tasks.Where(p => p.UserId == userId);
        }

        return await tasks.ToListAsync();
    }

    public async Task UpdateAsync(TaskUpdateDto taskUpdateDto)
    {
        var task = await _context.TaskEnts.FirstOrDefaultAsync(p => p.Id == taskUpdateDto.Id);
        if (task == null)
        {
            throw new Exception("Görev bulunamadı!");
        }

        task.Title = taskUpdateDto.Title;
        task.Description = taskUpdateDto.Description;
        task.Status = taskUpdateDto.Status;

        _context.TaskEnts.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid taskId)
    {
        var task = await _context.TaskEnts.FirstOrDefaultAsync(p => p.Id == taskId);
        if (task == null)
        {
            throw new Exception("Görev bulunamadı!");
        }

        var taskFiles = await _context.TaskFiles.Where(tf => tf.TaskId == taskId).ToListAsync();
        foreach (var taskFile in taskFiles)
        {
            var filePath = Path.Combine("/Users/sozey/Projects/toDoProject/TaskMSClient/public/img", taskFile.FileUrl);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        _context.TaskFiles.RemoveRange(taskFiles);
        _context.TaskEnts.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TaskResponseDto>> FilterByStatusAsync(TasksStatus status, Guid userId, bool isAdmin)
    {
        IQueryable<TaskEnt> tasks = _context.TaskEnts.Include(p => p.User);

        if (!isAdmin)
        {
            tasks = tasks.Where(p => p.UserId == userId);
        }

        tasks = tasks.Where(p => p.Status == status);

        var taskResponseDtos = tasks.OrderByDescending(t => t.CreateAt).Select(s => new TaskResponseDto
        {
            Id = s.Id,
            UserId = s.UserId,
            User = s.User,
            UserName = s.User!.GetName(),
            CreateAt = s.CreateAt.ToString("o"),
            Title = s.Title,
            Description = s.Description,
            Status = s.Status
        });

        return await taskResponseDtos.ToListAsync();
    }
}