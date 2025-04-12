namespace tut_05.Models;

public class Visit
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public string Note { get; set; }

    public Visit(int id, int animalId, string note)
    {
        Id = id;
        AnimalId = animalId;
        Note = note;
    }
}