using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tutorial11.DTOs;
using Tutorial11.DTOs.Response;
using Tutorial5.Data;
using Tutorial5.Models;
using Tutorial9.Exceptions;

namespace Tutorial5.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Prescription>> GetAllPrescriptions()
    {
        return await _context.Prescriptions.ToListAsync();
    }

    public async Task AddPrescription(PrescriptionRequestDTO prescriptionRequestDto)
    {
        if (await _context.Patients.Where(p => p.IdPatient == prescriptionRequestDto.Patient.IdPatient).CountAsync() == 0)
        {
            await AddPatient(prescriptionRequestDto.Patient);
        }

        foreach (var medic in prescriptionRequestDto.Medicaments)
        {
            if (await _context.Medicaments.Where(p => p.IdMedicament == medic.IdMedicament).CountAsync() == 0)
            {
                throw new NotFoundException("Such medicament doesn't exist");
            }
        }

        if (prescriptionRequestDto.Medicaments.Count == 0)
        {
            throw new BadRequestException("No medicament on the prescription");
        }
        if (prescriptionRequestDto.Medicaments.Count > 10)
        {
            throw new BadRequestException("Prescription can't hold more than 10 medicaments");
        }
        if (prescriptionRequestDto.DueDate < prescriptionRequestDto.Date)
        {
            throw new BadRequestException("Dates are incorrenct: Due date < Date");
        }
        
        await using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var prescription = new Prescription
                {
                    Date = prescriptionRequestDto.Date,
                    DueDate = prescriptionRequestDto.DueDate,
                    IdPatient = prescriptionRequestDto.Patient.IdPatient,
                    IdDoctor = prescriptionRequestDto.IdDoctor,
                };

                await _context.Prescriptions.AddAsync(prescription);
                await _context.SaveChangesAsync();

                var prescriptionMedicaments = prescriptionRequestDto.Medicaments.Select(m => new PrescriptionMedicament
                {
                    IdMedicament = m.IdMedicament,
                    IdPrescription = prescription.IdPrescription,
                    Dose = m.Dose,
                    Details = m.Description
                }).ToList();

                _context.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    private async Task AddPatient(PatientRequestDTO dto)
    {
        await _context.Patients.AddAsync(new Patient()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Birthdate = dto.BirthDate
        });
    }

    public async Task<PatientDetailsResponseDTO> GetPatientDetails(int id)
    {   
        var result = await _context.Patients.Where(p => p.IdPatient == id).Select(pat => new PatientDetailsResponseDTO()
        {
            IdPatient = pat.IdPatient,
            FirstName = pat.FirstName,
            LastName = pat.LastName,
            BirthDate = pat.Birthdate,
            Prescriptions = pat.Prescriptions.Select(p => new PrescriptionResponseDTO()
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Medicaments = p.PrescriptionMedicaments.Select(m => new MedicamentResponseDTO()
                {
                    IdMedicament = m.IdMedicament,
                    Name = m.Medicament.Name,
                    Dose = m.Dose,
                    Description = m.Medicament.Description

                }).ToList(),
                Doctor = new DoctorResponseDTO()
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName
                }
            }).OrderBy(p => p.DueDate).ToList()
        }).FirstOrDefaultAsync();
        if (result == null)
        {
            throw new NotFoundException("Patient does not exist");
        }
        return result;
    }
}