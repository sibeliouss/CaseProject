using Business.Abstract;
using Entities.Dtos;
using Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class TasksController : ApiController
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }
   
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] TaskAddDto request)
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(p => p.Type == "UserId");
        if (userIdClaim is null)
        {
            return BadRequest(new { Message = "Kullanıcı bulunmadı!" });
        }

        var userId = Guid.Parse(userIdClaim.Value);

        try
        {
            await _taskService.AddAsync(request, userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid taskId)
    {
        var details = await _taskService.GetByIdAsync(taskId);
        if (details is null)
        {
            return NotFound(new { Message = "Görev bulunamadı!" });
        }

        return Ok(details);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(GetAllTaskDto request)
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(p => p.Type == "UserId");
        if (userIdClaim is null)
        {
            return BadRequest(new { Message = "Kullanıcı bulunmadı!" });
        }

        var userId = Guid.Parse(userIdClaim.Value);

        var tasks = await _taskService.GetAllAsync(request.Roles, userId);
        return Ok(tasks);
    }


    [HttpPut]
    public async Task<IActionResult> Update(TaskUpdateDto request)
    {
        try
        {
            await _taskService.UpdateAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
    
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> Delete(Guid taskId)
    {
        try
        {
            await _taskService.DeleteAsync(taskId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> FilterByStatus([FromQuery] string status)
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(p => p.Type == "UserId");
        if (userIdClaim is null)
        {
            return BadRequest(new { Message = "Kullanıcı bulunmadı!" });
        }

        var userId = Guid.Parse(userIdClaim.Value);

        var isAdmin = HttpContext.User.Claims.Any(p => p.Type == "Roles" && p.Value == "Admin");

        try
        {
            var tasks = await _taskService.FilterByStatusAsync(Enum.Parse<TasksStatus>(status, true), userId, isAdmin);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}

