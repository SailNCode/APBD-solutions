using System.ComponentModel.DataAnnotations;

namespace Tutorial11.DTOs.Response;

public class PatientDetailsResponseDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PrescriptionResponseDTO> Prescriptions { get; set; }
}