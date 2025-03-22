using tut_03_containers.Exceptions;

namespace tut_03_containers;

public class Container
{
    protected static int nextId = 1;
    public string SerialNumber { get; }
    protected double _loadWeight;
    protected double _netWeight;
    protected double _maxWeight;
    protected double _height;
    protected double _depth;
    public Container(double loadWeight, double netWeight, double maxWeight, double height, double depth, string category)
    {
        if (netWeight > loadWeight)
        {
            throw new ArgumentException("Net mass must be smaller than mass.");
        }
        if (loadWeight> maxWeight)
        {
            throw new ArgumentException("Total mass must be smaller equal max weight.");
        }
        _loadWeight = loadWeight;
        _netWeight = netWeight;
        _maxWeight = maxWeight;
        _height = height;
        _depth = depth;
        SerialNumber = String.Format("KON-{0}-{1}", category, nextId++);
    }

    public override string ToString()
    {
        return String.Format($"{SerialNumber}: mass={_loadWeight}kg, net_mass={_netWeight}kg, " +
                             $"max_weight={_maxWeight}kg, height={_height}cm, depth={_depth}cm");
    }

    public double GetTotalWeight()
    {
        return _loadWeight + _netWeight;
    }
    public virtual double RemoveLoad()
    {
        double weightRemoved = _loadWeight;
        _loadWeight = 0;
        return weightRemoved;
    }

    public virtual void Load(double loadWeight)
    {
        if (loadWeight + _loadWeight > _maxWeight)
        {
            double exceededWeight = (loadWeight + _loadWeight) - _maxWeight;
            throw new OverfillException(exceededWeight);
        }
        _loadWeight += loadWeight;
    }
}