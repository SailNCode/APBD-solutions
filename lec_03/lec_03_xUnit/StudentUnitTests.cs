using lec_03;

namespace lec_03_xUnit;

public class StudentUnitTests
{
    [Fact]
    public void CalculateAverage_WithPositiveGrades_ReturnsCorrectAverage()
    {
        //Arrange
        var grades = new List<double>(){0,3,6};
        var student = new Student("John", "Doe", grades);
        //Act
        double averageGrade = student.CalculateAverage();

        //Assert
        Assert.Equal(3, averageGrade);
    }

    [Fact]
    public void CalculateAverage_WithEmptyGradeList_ThrowsDivideByZeroException()
    {
        //Arrange
        List<double> grades = new List<double>(); //empty list
        Student student = new Student("John", "Doe", grades);
        
        //Act & Assert
        Assert.Throws<DivideByZeroException>(() => student.CalculateAverage());
    }
}