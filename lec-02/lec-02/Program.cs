// See https://aka.ms/new-console-template for more information

using lec_02.Models;

public class Program
{
    public static void Main(string[] args)
    {
        Person s = new Student();
        s.SendMessage("Hello World!");
    }
}