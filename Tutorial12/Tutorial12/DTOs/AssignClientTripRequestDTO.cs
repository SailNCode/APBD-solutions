using System.ComponentModel.DataAnnotations;

namespace Tutorial12.DTOs;

public class AssignClientTripRequestDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    public string Telephone { get; set; }
    [Length(11,11)]
    public string Pesel { get; set; }
    public int IdTrip { get; set; }
    public string TripName { get; set; }
    public DateTime? PaymentDate { get; set; }
}