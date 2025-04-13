namespace tut_05.DAOs;
using static Database;
using tut_05.Models;
public class VisitDAO
{
    public static List<Visit> GetVisitsByAnimalId(int animalId)
    {
        return VisitsDatabase.Where(visit => visit.AnimalId == animalId).ToList();
    }

    public static bool IsPresent(int visitId)
    {
        return VisitsDatabase.Any(visit => visit.Id == visitId);
    }

    public static void AddVisit(Visit visit)
    { 
        VisitsDatabase.Add(visit);
    }
}