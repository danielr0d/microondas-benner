namespace Microondas.Domain.DTOs;

public class HeatingProgramDto
{
    public string Name { get; set; } = string.Empty;
    public string Food { get; set; } = string.Empty;
    public int TimeInSeconds { get; set; }
    public int Power { get; set; }
    public char HeatingCharacter { get; set; }
    public string Instructions { get; set; } = string.Empty;
    public bool IsCustom { get; set; }
}

