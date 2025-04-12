namespace tut_05.Models;

public class Animal : IEquatable<Animal>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public double Mass { get; set; }
    public string Coloration { get; set; }

    public Animal(int id, string name, string category, double mass, string coloration)
    {
        Id = id;
        Name = name;
        Category = category;
        Mass = mass;
        Coloration = coloration;
    }

    public bool Equals(Animal animal)
    {
        return Id == animal.Id;
    }
    
}