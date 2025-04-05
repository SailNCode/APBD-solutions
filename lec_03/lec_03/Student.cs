namespace lec_03;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<double> Grades { get; set; }

    public Student(string firstName, string lastName, List<double> grades)
    {
        FirstName = firstName;
        LastName = lastName;
        Grades = grades;
    }

    public double CalculateAverage()
    {
        if (Grades.Count == 0)
            throw new DivideByZeroException();
        return Grades.Sum() / Grades.Count;
    }
}