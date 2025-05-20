
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tutorial11.DTOs.Response;
using Tutorial5.Services;
using Tutorial9.Exceptions;

namespace Tutorial11.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IDbService _dbService;
    public PatientsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        try
        {
            PatientDetailsResponseDTO patientDetails = await _dbService.GetPatientDetails(id);
            return Ok(patientDetails);
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