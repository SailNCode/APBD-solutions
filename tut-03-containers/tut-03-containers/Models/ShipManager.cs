namespace tut_03_containers;

public class ShipManager
{
    private List<FreightShip> ships = new List<FreightShip>();

    public void AddShip(FreightShip ship)
    {
        ships.Add(ship);
    }

    public void RemoveShip(int shipId)
    {
        FreightShip? ship = ships.Find(x => x.Id == shipId);
        if (ship != null)
        {
            ships.Remove(ship);
        }
    }

    public FreightShip GetShip(int shipId)
    {
        FreightShip? ship = ships.Find(x => x.Id == shipId);
        if (ship == null)
        {
            throw new ArgumentException("The ship does not exist");
        }

        return ship;
    }
    public void MoveContainer(FreightShip srcShip, FreightShip dstShip, Container container)
    {
        if (!srcShip.Remove(container))
        {
            throw new Exception("Container on the ship does not exist!");
        }
        dstShip.Load(container);
    }
}