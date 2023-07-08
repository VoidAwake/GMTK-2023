using System;

public static class EnumExtensions
{
    public static T GetRandom<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(new Random().Next(values.Length));
    }
}
