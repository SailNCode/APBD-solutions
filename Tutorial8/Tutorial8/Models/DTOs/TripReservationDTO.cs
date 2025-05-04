namespace Tutorial8.Models.DTOs;

public class TripReservationDTO: TripDTO
{
    public int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
}