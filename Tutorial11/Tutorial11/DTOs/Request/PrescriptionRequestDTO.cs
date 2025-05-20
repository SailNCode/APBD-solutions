using Tutorial5.Models;

namespace Tutorial11.DTOs;

public class PrescriptionRequestDTO
{
    public PatientRequestDTO Patient { get; set; }
    public int IdDoctor { get; set; }
    public ICollection<MedicamentRequestDTO> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}