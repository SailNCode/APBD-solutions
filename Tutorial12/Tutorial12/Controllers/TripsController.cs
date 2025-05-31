using Microsoft.AspNetCore.Mvc;
using Tutorial12.DTOs;
using Tutorial12.Services;
using Tutorial9.Exceptions;

namespace Tutorial12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private IService _service;

    public TripsController(IService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(int? pageNum, int? pageSize)
    {
        TripsResponseDTO response = await _service.GetTripsInfo(pageNum, pageSize);
        return Ok(response);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(AssignClientTripRequestDTO requestDto, int idTrip)
    {
        try
        {
            var timestamp = DateTime.Now;
            await _service.RegisterClientForTrip(requestDto, idTrip, timestamp);
            return Ok();
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (InternalServerException e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
