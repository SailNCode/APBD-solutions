using Microsoft.AspNetCore.Mvc;
using tut_05.DAOs;
using tut_05.Models;

namespace tut_05.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VisitsController : ControllerBase
{
    [HttpGet("{animalId}")]
    public IActionResult GetVisitsByAnimalId(int animalId)
    {
        return Ok(VisitDAO.GetVisitsByAnimalId(animalId));
    }

    [HttpPost]
    public IActionResult AddVisit(Visit visit)
    {
        if (VisitDAO.IsPresent(visit.Id))
        {
            return Conflict();
        }

        VisitDAO.AddVisit(visit);
        return Created();
    }
}