public enum COFFEE_TYPE
{
    Cappuccino,
    Latte,
    Mocha,
    FlatWhite,
    Espresso,
    LongBlack,
    IcedLatte,
    IcedMocha
}

public enum COFFEE_SIZE
{
    Small,
    Medium,
    Large,
    ItsyBitsy,
    Middling,
    Stupendous
}

public enum MILK_TYPE
{
    None,
    FullCream,
    Skim,
    Almond,
    Oat,
    Soy
}

public class CoffeeOrder
{
    public COFFEE_TYPE coffeeType;
    public COFFEE_SIZE coffeeSize;
    public MILK_TYPE milkType;

    public CoffeeOrder(COFFEE_TYPE _coffeeType, COFFEE_SIZE _coffeeSize, MILK_TYPE _milkType)
    {
        coffeeType = _coffeeType;
        coffeeSize = _coffeeSize;
        milkType = _milkType;
    }
}
