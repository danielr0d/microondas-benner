using Microondas.Domain.Entities;
using Microondas.Domain.Localization;

namespace Microondas.Domain.Factories;

public class PredefinedProgramsFactory
{
    private static readonly List<HeatingProgram> DefaultPrograms = new()
    {
        new HeatingProgram
        {
            Name = "Popcorn",
            Food = "Popcorn",
            TimeInSeconds = 180,
            Power = 7,
            HeatingCharacter = '~',
            Instructions = "Listen for the popping sound to slow down, then stop the microwave.",
            IsCustom = false
        },
        new HeatingProgram
        {
            Name = "Milk",
            Food = "Milk",
            TimeInSeconds = 300,
            Power = 5,
            HeatingCharacter = '*',
            Instructions = "Be careful when heating liquids. Never leave unattended.",
            IsCustom = false
        },
        new HeatingProgram
        {
            Name = "Beef",
            Food = "Beef",
            TimeInSeconds = 840,
            Power = 4,
            HeatingCharacter = '#',
            Instructions = "Pause halfway through and stir the meat for even heating.",
            IsCustom = false
        },
        new HeatingProgram
        {
            Name = "Chicken",
            Food = "Chicken",
            TimeInSeconds = 480,
            Power = 7,
            HeatingCharacter = '+',
            Instructions = "Pause halfway through and rotate the chicken for uniform cooking.",
            IsCustom = false
        },
        new HeatingProgram
        {
            Name = "Beans",
            Food = "Beans",
            TimeInSeconds = 480,
            Power = 9,
            HeatingCharacter = '=',
            Instructions = "Leave the container uncovered to allow steam to escape.",
            IsCustom = false
        }
    };

    public static List<HeatingProgram> GetDefaultPrograms()
    {
        return DefaultPrograms.Select(p => new HeatingProgram
        {
            Name = p.Name,
            Food = p.Food,
            TimeInSeconds = p.TimeInSeconds,
            Power = p.Power,
            HeatingCharacter = p.HeatingCharacter,
            Instructions = p.Instructions,
            IsCustom = p.IsCustom
        }).ToList();
    }

    public static List<HeatingProgram> GetLocalizedPrograms(ILocalizationService localizationService)
    {
        var basePrograms = GetDefaultPrograms();
        var programKeyMap = new Dictionary<string, (string nameKey, string foodKey, string instrKey)>
        {
            { "Popcorn", ("program.popcorn", "program.popcorn_food", "program.popcorn_instructions") },
            { "Milk", ("program.milk", "program.milk_food", "program.milk_instructions") },
            { "Beef", ("program.beef", "program.beef_food", "program.beef_instructions") },
            { "Chicken", ("program.chicken", "program.chicken_food", "program.chicken_instructions") },
            { "Beans", ("program.beans", "program.beans_food", "program.beans_instructions") }
        };

        return basePrograms.Select(p =>
        {
            if (programKeyMap.TryGetValue(p.Name, out var keys))
            {
                return new HeatingProgram
                {
                    Name = p.Name,
                    DisplayName = localizationService.GetString(keys.nameKey),
                    Food = localizationService.GetString(keys.foodKey),
                    TimeInSeconds = p.TimeInSeconds,
                    Power = p.Power,
                    HeatingCharacter = p.HeatingCharacter,
                    Instructions = localizationService.GetString(keys.instrKey),
                    IsCustom = p.IsCustom
                };
            }
            return p;
        }).ToList();
    }
}

