namespace lec_05.Models;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IndexNumber { get; set;}

    public Student(string firstName, string lastName, string indexNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        IndexNumber = indexNumber;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}