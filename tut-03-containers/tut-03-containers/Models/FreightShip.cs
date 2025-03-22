namespace tut_03_containers;

public class FreightShip
{
    private static int nextId = 1;
    public int Id {get;}
    private double MaxSpeed { get; }
    private int MaxNContainers { get; }
    private double MaxLoadInTons { get; }
    private List<Container> _containers = new List<Container>();
    public FreightShip(double maxSpeed, int maxNContainers, double maxLoadInTons)
    {
        Id = nextId++;
        MaxSpeed = maxSpeed;
        MaxNContainers = maxNContainers;
        MaxLoadInTons = maxLoadInTons;
    }

    public override string ToString()
    {
        return $"Freight ship {Id} (speed={MaxSpeed}, maxContainerNum={MaxNContainers}, maxWeight={MaxLoadInTons})";
    }

    public string ToDetailedString()
    {
        string result = ToString() + "\n";
        foreach (Container container in _containers)
        {
            result += "\t"+container.ToString() + "\n";
        }

        return result;
    }
    
    private Container GetContainer(String serialNumber)
    {
        Container? match = _containers.Find(container => container.SerialNumber == serialNumber);
        if (match == null)
        {
            throw new ArgumentException($"Container with serial number {serialNumber} is not on the ship!");
        }
        return match;
    }

    private double GetLoadWeight()
    {
        return _containers.Sum(container => container.GetTotalWeight());
    }

    public void Load(Container container)
    {
        if (GetLoadWeight() + container.GetTotalWeight() > MaxLoadInTons * 1000)
        {
            throw new ArgumentException("Container weighs too much!");
        }
        if (_containers.Count >= MaxNContainers)
        {
            throw new ArgumentException("Limit of contaiers reached!");
        }
        _containers.Add(container);
    }

    public void Load(List<Container> containers)
    {
        double totalNewWeight = containers.Sum(container => container.GetTotalWeight());
        if (GetLoadWeight() + totalNewWeight > MaxLoadInTons * 1000)
        {
            throw new ArgumentException("Loading containers aborted: they weight too much!");
        }
        containers.ForEach(container => Load(container));
    }

    public bool Remove(Container container)
    {
        return _containers.Remove(container);
    }

    public void Swap(Container oldContainer, Container newContainer)
    {
        if (!_containers.Remove(oldContainer))
        {
            throw new Exception("Container on the ship does not exist!");
        }
        _containers.Add(newContainer);
    }
}