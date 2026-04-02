using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microondas.Domain.DTOs;
using Microondas.Domain.Entities;
using Microondas.Domain.Factories;
using Microondas.Domain.Interfaces;
using Microondas.Infrastructure.Repositories;

namespace Microondas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgramsController : ControllerBase
{
    private readonly IProgramRepository _repository;

    public ProgramsController(IProgramRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPrograms()
    {
        try
        {
            var predefinedPrograms = PredefinedProgramsFactory.GetDefaultPrograms();
            var customPrograms = await _repository.GetAllCustomProgramsAsync();

            var allPrograms = predefinedPrograms.Concat(customPrograms).ToList();
            var dtos = allPrograms.Select(p => new HeatingProgramDto
            {
                Name = p.Name,
                Food = p.Food,
                TimeInSeconds = p.TimeInSeconds,
                Power = p.Power,
                HeatingCharacter = p.HeatingCharacter,
                Instructions = p.Instructions,
                IsCustom = p.IsCustom
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetProgram(string name)
    {
        try
        {
            var predefinedPrograms = PredefinedProgramsFactory.GetDefaultPrograms();
            var program = predefinedPrograms.FirstOrDefault(p => p.Name == name);

            if (program == null)
            {
                program = await _repository.GetProgramByNameAsync(name);
            }

            if (program == null)
                return NotFound(new { message = $"Program '{name}' not found" });

            var dto = new HeatingProgramDto
            {
                Name = program.Name,
                Food = program.Food,
                TimeInSeconds = program.TimeInSeconds,
                Power = program.Power,
                HeatingCharacter = program.HeatingCharacter,
                Instructions = program.Instructions,
                IsCustom = program.IsCustom
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProgram([FromBody] HeatingProgramDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Food))
                return BadRequest(new { message = "Name and Food are required" });

            if (dto.TimeInSeconds < 1)
                return BadRequest(new { message = "Time must be at least 1 second" });

            if (dto.Power < 1 || dto.Power > 10)
                return BadRequest(new { message = "Power must be between 1 and 10" });

            var predefinedPrograms = PredefinedProgramsFactory.GetDefaultPrograms();
            var usedCharacters = predefinedPrograms.Select(p => p.HeatingCharacter).ToList();
            var customPrograms = await _repository.GetAllCustomProgramsAsync();
            usedCharacters.AddRange(customPrograms.Select(p => p.HeatingCharacter));

            if (dto.HeatingCharacter == '.' || usedCharacters.Contains(dto.HeatingCharacter))
                return BadRequest(new { message = "This character is already in use or reserved" });

            var program = new HeatingProgram
            {
                Name = dto.Name,
                Food = dto.Food,
                TimeInSeconds = dto.TimeInSeconds,
                Power = dto.Power,
                HeatingCharacter = dto.HeatingCharacter,
                Instructions = dto.Instructions,
                IsCustom = true
            };

            await _repository.AddProgramAsync(program);
            return StatusCode(201, new { message = "Program created successfully", program = dto });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteProgram(string name)
    {
        try
        {
            var predefinedPrograms = PredefinedProgramsFactory.GetDefaultPrograms();
            if (predefinedPrograms.Any(p => p.Name == name))
                return BadRequest(new { message = "Cannot delete predefined programs" });

            var program = await _repository.GetProgramByNameAsync(name);
            if (program == null)
                return NotFound(new { message = $"Program '{name}' not found" });

            await _repository.DeleteProgramAsync(name);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}


