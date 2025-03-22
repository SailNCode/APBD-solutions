namespace tut_03_containers.Exceptions;

public class OverfillException(double exceededWeight): Exception
{
    private double _exceededWeight = exceededWeight;

    public override string ToString()
    {
        return "Weight exceeded by: " + _exceededWeight;
    }
}