using tut_05.Models;

namespace tut_05;

public class Database
{
    public static List<Animal> AnimalsDatabase = new List<Animal>()
    {
        new Animal(1, "Lion", "Mammal", 190.5, "Golden"),
        new Animal(2, "Penguin", "Bird", 15.2, "Black and White"),
        new Animal(3, "Python", "Reptile", 75.0, "Green"),
        new Animal(4, "Elephant", "Mammal", 5400.0, "Gray"),
        new Animal(5, "Frog", "Amphibian", 0.3, "Green"),
        new Animal(7, "Lion", "Mammal", 200, "Golden")
    };

    public static List<Visit> VisitsDatabase = new List<Visit>()
    {
        new Visit(1, 2, "Wing broken"),
        new Visit(2, 2, "Everything ok"),
        new Visit(3, 1, "Troots weirdly")
    };
}