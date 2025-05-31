using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tutorial12.Services;
using Tutorial9.Exceptions;

namespace Tutorial12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController: ControllerBase
{
    private IService _service;

    public ClientsController(IService service)
    {
        _service = service;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {   
        try
        {
            await _service.DeleteClient(idClient);
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
    }

}