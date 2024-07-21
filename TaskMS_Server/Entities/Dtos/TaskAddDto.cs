using Microsoft.AspNetCore.Http;

namespace Entities.Dtos;

public record TaskAddDto(
    string Title,
    string Description,
    List<IFormFile>? Files);