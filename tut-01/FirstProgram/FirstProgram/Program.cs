// See https://aka.ms/new-console-template for more information

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Console.WriteLine("My name is Kamil");
        Console.WriteLine(CalcAverage(new int[]{1, 4, 5, 8}));
    }

    public static double CalcAverage(int[] numbers)
    {
        double sum = 0.0;
        foreach (var number in numbers)
        {
            sum += number;
        }
        return sum/numbers.Length;
    }
}