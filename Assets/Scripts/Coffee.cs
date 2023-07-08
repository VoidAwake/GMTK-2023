using System.Collections.Generic;
    
public class Coffee
{
    public string style;
    public string milk;
    public string size;
    
    public static List<string> styles = new()
    {
        "Cappuccino",
        "Latte",
        "Mocha",
        "FlatWhite",
        "Espresso",
        "IceLatte",
        "IceMocha"
    };

    public static List<string> milks = new()
    {
        "Full Cream",
        "Skim",
        "Almond",
        "Oat",
        "Soy"
    };

    public static List<string> sizes = new()
    {
        "Small",
        "Regular",
        "Large"
    };
}
