using Tutorial5.Models;

namespace Tutorial11.DTOs.Response;

public class PrescriptionResponseDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentResponseDTO> Medicaments { get; set; } = new List<MedicamentResponseDTO>();
    public DoctorResponseDTO? Doctor { get; set; }
    
}