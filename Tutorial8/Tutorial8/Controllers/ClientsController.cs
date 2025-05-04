using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private IClientService _clientService;
    private ITripsService _tripsService;
    public ClientsController(IClientService clientService, ITripsService tripsService)
    {
        _clientService = clientService;
        _tripsService = tripsService;
    }
    
    /*
     * Returns all trip to which client is registered
     */
    [HttpGet("{id}/trips")]
    public async Task<IActionResult> GetTripsByClientId(int id)
    {
        Console.WriteLine(id);
        if (!await _clientService.IsClientPresent(id))
        {
            return NotFound();
        }

        List<TripReservationDTO> tripReservations = await _tripsService.GetTripReservationsByClientId(id);

        if (tripReservations.IsNullOrEmpty())
        {
            return NotFound();
        }

        return Ok(tripReservations);
    }
    
    /*
     * Adds client to db. Beforehand, email is checked to contain '@'
     */
    [HttpPost]
    public async Task<IActionResult> CreateClient(ClientDTO client)
    {

        if (!client.Email.Contains("@"))
        {
            return BadRequest("Invalid email, include '@'");
        }
        if (await _clientService.AddClient(client))
        {
            return Created();
        }
        return BadRequest();

    }
    /*
     * Registers client to a specific trip. Checks whether their ids are valid and whether trip is not full yet
     */
    [HttpPut("{id}/trips/{tripId}")]
    public async Task<IActionResult> RegisterClientForTrip(int id, int tripId)
    {
        if (! await _clientService.IsClientPresent(id))
        {
            return NotFound("Client id absent");
        }

        if (!await _tripsService.IsTripPresent(tripId))
        {
            return NotFound("Trip id absent");
        }

        if (await _tripsService.IsTripFull(tripId))
        {
            return BadRequest("Trip already full");
        }
        if (await _tripsService.RegisterClientForTrip(id, tripId))
        {
            return NoContent();
        }

        return BadRequest("Client already registered for trip");
    }

    /*
     * Removes client from trip. Checks whether such association is indeed present
     */
    [HttpDelete("{id}/trips/{tripId}")]
    public async Task<IActionResult> RemoveClientFromTrip(int id, int tripId)
    {
        if (! await _tripsService.IsClientRegisteredForTrip(id, tripId))
        {
            return BadRequest("Client is not registered for such trip!");
        }

        if (await _tripsService.RemoveClientFromTrip(id, tripId))
        {
            return NoContent();
        }

        return BadRequest();

    }
}