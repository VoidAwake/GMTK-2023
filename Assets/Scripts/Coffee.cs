using System.Collections.Generic;
    
public class Coffee
{
    public string style;
    public string milk;
    public string size;
    public string side;
    public string topping;
    public int questionAmount = 3;
    
    public static List<string> styles = new()
    {
        "Cappuccino",
        "Latte",
        "Mocha",
        "Flat White",
        "Espresso",
        "Ice Latte",
        "Ice Mocha",
        "Ice Flat White",
        "Ice Espresso",
        "Long Black",
        "Short Black",
        "Macchiato",
        "Affogato",
        "Ristretto",
        "Americano",
        "Piccolo",
        "Irish",
        "Vienna",
        "Turkish",
        "Greek",
        "Cortado",
        "Breve",
    };

    public static List<string> milks = new()
    {
        "Full Cream",
        "Skim",
        "Almond",
        "Oat",
        "Soy",
        "Coconut",
        "Lactose Free",
    };

    public static List<string> sizes = new()
    {
        "Small",
        "Regular",
        "Large",
        "Extra Large"
    };
    
    public static List<string> sides = new()
    {
        "Bread",
        "Amongus",
        "Fred",
        "Icecream",
        "Pancake",
        "Waffle",
        "Cookie",
        "Donut",
        "Cupcake",
        "Muffin",
        "Croissant",
        "Bagel",
        "Toast",
        "Biscuit",
        "Scone",
        "Pretzel",
        "Crumpet",
        "Pikelet",
        "Bun",
        "Roll",
        "Brioche",
        "Danish",
        "Strudel",
        "Pie",
        "Tart",
        "Cake",
        "Cheesecake",
        "Brownie",
    };
    
    public static List<string> toppings = new()
    {
        "Chocolate Shavings",
        "Sprinkles",
        "Cinnamon",
        "Caramel",
        "Honey",
        "Marshmallows"
    };
}
