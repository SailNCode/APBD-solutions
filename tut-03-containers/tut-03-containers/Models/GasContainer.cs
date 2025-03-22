namespace tut_03_containers;

public class GasContainer(double loadWeight, double netWeight, double maxWeight, double height, double depth, double pressure) :
    Container(loadWeight, netWeight, maxWeight, height, depth, "G"),
    IHazardNotifier
{
    private double _pressure = pressure;
    public override string ToString()
    {
        return base.ToString() + ", pressure=" + _pressure + "atm";
    }

    public override double RemoveLoad()
    {
        double removedLoad = _loadWeight * 0.95;
        _loadWeight -= removedLoad;
        return removedLoad;
    }

    public void SendWarning()
    {
        Console.WriteLine("Danger detected in GasContainer: " + SerialNumber);
    }
}