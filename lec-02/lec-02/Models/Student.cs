namespace lec_02.Models;

public class Student : Person
{
    private string _name;
    public string Name
    {
        get
        {
            return _name; 
        }
        set
        {
            _name = value;
        }
    }

    public override void SendMessage(string message)
    {
        base.SendMessage("Student: " + message);
    }
}