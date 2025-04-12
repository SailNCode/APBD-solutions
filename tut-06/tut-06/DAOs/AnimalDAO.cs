namespace tut_05.DAOs;
using static Database;
using tut_05.Models;

public class AnimalDAO
{
    public static Animal getAnimal(int id)
    {
        return AnimalsDatabase.FirstOrDefault(x => x.Id == id, null);
    }

    public static void RemoveById(int id)
    {
        AnimalsDatabase.RemoveAll(animal => animal.Id == id);
    }

    public static bool IsPresent(int id)
    {
        return AnimalsDatabase.Any(animal => animal.Id == id);
    }

    public static void AddAnimal(Animal animal)
    {
        AnimalsDatabase.Add(animal);
    } 

    public static List<Animal> GetAllAnimals()
    {
        return AnimalsDatabase.ToList();
    }

    public static List<Animal> GetAnimalsByName(string name)
    {
        return AnimalsDatabase.Where(animal => animal.Name == name).ToList();
    }
}