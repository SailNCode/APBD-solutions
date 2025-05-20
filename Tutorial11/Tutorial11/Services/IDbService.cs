
using Tutorial11.DTOs;
using Tutorial11.DTOs.Response;
using Tutorial5.Models;

namespace Tutorial5.Services;

public interface IDbService
{
    Task<ICollection<Prescription>> GetAllPrescriptions();
    Task AddPrescription(PrescriptionRequestDTO prescriptionRequestDto);
    Task<PatientDetailsResponseDTO> GetPatientDetails(int id);
}