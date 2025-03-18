namespace lec_02.Models;

public class Person
{
    public virtual void SendMessage(string message)
    {
        Console.WriteLine(message);
    }
}