// See https://aka.ms/new-console-template for more information

using tut_03_containers;

internal class Program
{
    public static void Main(string[] args)
    {
        ShipManager shipManager = new ShipManager();
        
        Console.WriteLine("Creating containers...");
        Container container = new Container(120, 10, 300,  10, 15,"N");
        LiquidContainer liquidContainer = new LiquidContainer(120, 10, 300, 10, 15, true);
        GasContainer gasContainer = new GasContainer(120, 10, 300, 10, 15, 8);
        CoolingContainer coolingContainer = new CoolingContainer(120, 10, 300, 10, 15, 12, "bananas");
        Console.WriteLine(container);
        Console.WriteLine(liquidContainer);
        Console.WriteLine(gasContainer);
        Console.WriteLine(coolingContainer);

        
        Console.WriteLine("Unloading container...");
        liquidContainer.RemoveLoad();
        Console.WriteLine(liquidContainer);
        
        Console.WriteLine("Loading container...");
        liquidContainer.Load(280);
        Console.WriteLine(liquidContainer);
        
        List<Container> containers = new List<Container>
        {
            liquidContainer,
            gasContainer,
            coolingContainer
        };
        FreightShip ship1 = new FreightShip(3, 4, 3);
        FreightShip ship2 = new FreightShip(8, 1, 1);
        shipManager.AddShip(ship1);
        Console.WriteLine(ship1);
        shipManager.AddShip(ship2);
        
        
        Console.WriteLine("Loading multiple containers on the ship...");
        ship1.Load(containers);
        Console.WriteLine(ship1.ToDetailedString());
        try
        {
            Console.WriteLine("Loading single container on the ship...");
            ship1.Load(new Container(120, 10, 300, 10, 15, "G"));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine(ship1.ToDetailedString());
        
        Console.WriteLine("Removing container from the ship...");
        ship1.Remove(liquidContainer);
        
        Console.WriteLine(ship1.ToDetailedString());

        Console.WriteLine("Replacing with new container...");
        GasContainer newGasContainer = new GasContainer(200, 10, 300, 10, 15, 3);
        ship1.Swap(gasContainer, newGasContainer);
        
        Console.WriteLine(ship1.ToDetailedString());

        Console.WriteLine("Moving container from ship with id 1 to one with id 2...");
        FreightShip tmp1 = shipManager.GetShip(1);
        FreightShip tmp2 = shipManager.GetShip(2);
        shipManager.MoveContainer(tmp1, tmp2, newGasContainer);
        
        Console.WriteLine(tmp1.ToDetailedString());
        Console.WriteLine(tmp2.ToDetailedString());
        
        Console.WriteLine("Printing basic information about ship...");
        
        Console.WriteLine(ship1);
        
        Console.WriteLine("Printing detailed information about ship...");
        Console.WriteLine(ship1.ToDetailedString());

        


    }
}