namespace Microondas.Domain.Entities;

public class HeatingProgram
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Food { get; set; } = string.Empty;
    public int TimeInSeconds { get; set; }
    public int Power { get; set; }
    public char HeatingCharacter { get; set; }
    public string Instructions { get; set; } = string.Empty;
    public bool IsCustom { get; set; } = false;

    public override bool Equals(object? obj)
    {
        return obj is HeatingProgram program &&
               Name == program.Name &&
               Food == program.Food;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Food);
    }
}


