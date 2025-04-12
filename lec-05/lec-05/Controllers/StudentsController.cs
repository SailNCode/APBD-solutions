using lec_05.Models;
using Microsoft.AspNetCore.Mvc;

namespace lec_05.Controllers;

[ApiController]
[Route("students")]
public class StudentsController : ControllerBase
{
    private static List<Student> students = new List<Student>(
        new Student[]
        {
            new Student("Michał", "Dupa", "s28762"),
            new Student("Izabela", "Rozaniela", "s27455")
        });

    [HttpGet]
    public IActionResult GetStudents()
    {
        return Ok(students);
    }
    [HttpGet("{id}")]
    public IActionResult GetStudent(string id)
    {
        return Ok(students[int.Parse(id)]);
    }
}