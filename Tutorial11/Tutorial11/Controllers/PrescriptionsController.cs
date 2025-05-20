using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial11.DTOs;
using Tutorial5.Data;
using Tutorial5.Models;
using Tutorial5.Services;
using Tutorial9.Exceptions;

namespace Tutorial5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public PrescriptionsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetAllPrescriptions()
        // {
        //     return Ok(await _dbService.GetAllPrescriptions());
        // }
        [HttpPost]
        public async Task<IActionResult> AddPrescription(PrescriptionRequestDTO prescriptionRequestDto)
        {
            try
            {
                await _dbService.AddPrescription(prescriptionRequestDto);
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
}
