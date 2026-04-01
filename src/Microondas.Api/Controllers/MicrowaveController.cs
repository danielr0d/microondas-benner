using Microsoft.AspNetCore.Mvc;
using Microondas.Domain.Exceptions;
using Microondas.Infrastructure.Services;

namespace Microondas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MicrowaveController : ControllerBase
{
    private readonly IMicrowaveService _microwaveService;

    public MicrowaveController(IMicrowaveService microwaveService)
    {
        _microwaveService = microwaveService;
    }

    [HttpPost("start")]
    public IActionResult Start([FromQuery] int? timeInSeconds, [FromQuery] int? power)
    {
        try
        {
            _microwaveService.Machine.Start(timeInSeconds, power);
            return Ok(new
            {
                remainingTime = _microwaveService.Machine.RemainingTime,
                state = _microwaveService.Machine.State.ToString()
            });
        }
        catch (InvalidHeatingTimeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidPowerException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("pause")]
    public IActionResult Pause()
    {
        try
        {
            _microwaveService.Machine.Pause();
            return Ok(new
            {
                remainingTime = _microwaveService.Machine.RemainingTime,
                state = _microwaveService.Machine.State.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            remainingTime = _microwaveService.Machine.RemainingTime,
            totalTime = _microwaveService.Machine.TotalTime,
            power = _microwaveService.Machine.Power,
            state = _microwaveService.Machine.State.ToString(),
            heatingString = _microwaveService.Machine.HeatingString,
            display = _microwaveService.Machine.GetDisplay()
        });
    }

    [HttpPost("simulate-second")]
    public IActionResult SimulateSecond()
    {
        try
        {
            _microwaveService.SimulateSecond();
            return Ok(new
            {
                remainingTime = _microwaveService.Machine.RemainingTime,
                heatingString = _microwaveService.Machine.HeatingString,
                state = _microwaveService.Machine.State.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

