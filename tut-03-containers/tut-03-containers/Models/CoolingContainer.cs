namespace tut_03_containers;

public class CoolingContainer: Container
{
    private static Dictionary<String, double> _possibleProducts = new Dictionary<String, double>
    {
        { "bananas", 13.3 },
        { "chocolate", 18.0 },
        { "fish", 2.0 },
        { "meat", -15.0 },
        { "ice cream", -18.0 },
        { "frozen pizza", -30.0 },
        { "cheese", 7.2 },
        { "sausages", 5.0 },
        { "butter", 20.5 },
        { "eggs", 19.0 }
    };

    private double _temperature;
    private string _productType;

    public CoolingContainer(double loadWeight, double netWeight, double maxWeight, double height, double depth,
        double temperature, string productType) :
        base(loadWeight, netWeight, maxWeight, height, depth, "C")
    {
        if (!_possibleProducts.ContainsKey(productType.ToLower()))
        {
            throw new ArgumentException($"Product type {productType} is not supported!");
        } else if (_possibleProducts[productType] < temperature) //Wydaje mi się to bardziej logiczne, niż w treści zadania
        {
            throw new ArgumentException($"Temperature {temperature} is too high!");
        }
        _temperature = temperature;
        _productType = productType;
    }

    public override string ToString()
    {
        return base.ToString() + ", temperature=" + _temperature + "\u00b0C, product type=" + _productType;
    }
}