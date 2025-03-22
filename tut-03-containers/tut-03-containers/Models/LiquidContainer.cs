namespace tut_03_containers;

public class LiquidContainer:
    Container, IHazardNotifier
{    
    private double _liquidVolumeLimit;
    private bool _hasHazardousLoad;
    public bool HasHazardousLoad
    {
        get { return _hasHazardousLoad; }
        set
        {
            _hasHazardousLoad = value;
            _liquidVolumeLimit = _hasHazardousLoad ? _maxWeight * 0.5 : _maxWeight * 0.9;
            
        }
    }
    public LiquidContainer(double loadWeight, double netWeight, double maxWeight, double height, double depth,
        bool hasHazardousLoad) : base(loadWeight, netWeight, maxWeight, height, depth, "L")
    {
        HasHazardousLoad = hasHazardousLoad;
    }

    public override string ToString()
    {
        return base.ToString() + ", hasHazardousLoad: " + HasHazardousLoad;
    }

    public void SendWarning()
    {
        Console.WriteLine("Danger detected in LiquidContainer: " + SerialNumber);
    }

    public override void Load(double loadWeight)
    {
        if (_loadWeight + loadWeight > _liquidVolumeLimit)
        {
            SendWarning();
        }
        base.Load(loadWeight);
    }
}