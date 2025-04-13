using Microsoft.AspNetCore.Mvc;
using tut_05.DAOs;
using tut_05.Models;

namespace tut_05.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController: ControllerBase
{
    [HttpGet]
    public IActionResult GetAnimals(string? name)
    {
        if (name == null)
        {
            return Ok(AnimalDAO.GetAllAnimals());
        }
        return Ok(AnimalDAO.GetAnimalsByName(name));
    }

    [HttpGet("{id}")]
    public IActionResult GetAnimal(int id)
    {
        Animal animal = AnimalDAO.getAnimal(id);
        if (animal == null)
        {
            return NotFound();
        }
        return Ok(animal);
    }

    [HttpPost]
    public IActionResult AddAnimal(Animal animal)
    {
        if (AnimalDAO.IsPresent(animal.Id))
        {
            return Conflict();
        }
        
        AnimalDAO.AddAnimal(animal);
        return Created();
    }
    [HttpPut("{id}")]
    public IActionResult UpdateAnimal(Animal animal, int id)
    {
        if (!AnimalDAO.IsPresent(id))
        {
            return NotFound();
        }
        
        AnimalDAO.RemoveById(id);
        AnimalDAO.AddAnimal(animal);
        return Ok();
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        if (!AnimalDAO.IsPresent(id))
        {
            return NotFound();
        }
        AnimalDAO.RemoveById(id);
        return Ok();
    }
}