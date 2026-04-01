using Microondas.Domain.Entities;
using Microondas.Domain.Interfaces;
using System.Text.Json;

namespace Microondas.Infrastructure.Repositories;

public class JsonProgramRepository : IProgramRepository
{
    private readonly string _filePath;
    private readonly string _directoryPath;

    public JsonProgramRepository(string? baseDirectory = null)
    {
        _directoryPath = Path.Combine(baseDirectory ?? AppContext.BaseDirectory, "Data");
        _filePath = Path.Combine(_directoryPath, "programs.json");
    }

    public async Task<List<HeatingProgram>> GetAllCustomProgramsAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
                return new List<HeatingProgram>();

            var json = await File.ReadAllTextAsync(_filePath);
            var programs = JsonSerializer.Deserialize<List<HeatingProgram>>(json) ?? new List<HeatingProgram>();
            return programs.Where(p => p.IsCustom).ToList();
        }
        catch
        {
            return new List<HeatingProgram>();
        }
    }

    public async Task<HeatingProgram?> GetProgramByNameAsync(string name)
    {
        var programs = await GetAllCustomProgramsAsync();
        return programs.FirstOrDefault(p => p.Name == name);
    }

    public async Task AddProgramAsync(HeatingProgram program)
    {
        program.IsCustom = true;
        var programs = await GetAllCustomProgramsAsync();

        if (programs.Any(p => p.Name == program.Name))
            throw new InvalidOperationException($"Program '{program.Name}' already exists.");

        programs.Add(program);
        await SaveProgramsAsync(programs);
    }

    public async Task UpdateProgramAsync(HeatingProgram program)
    {
        program.IsCustom = true;
        var programs = await GetAllCustomProgramsAsync();
        var index = programs.FindIndex(p => p.Name == program.Name);

        if (index == -1)
            throw new InvalidOperationException($"Program '{program.Name}' not found.");

        programs[index] = program;
        await SaveProgramsAsync(programs);
    }

    public async Task DeleteProgramAsync(string name)
    {
        var programs = await GetAllCustomProgramsAsync();
        programs.RemoveAll(p => p.Name == name);
        await SaveProgramsAsync(programs);
    }

    private async Task SaveProgramsAsync(List<HeatingProgram> programs)
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        var json = JsonSerializer.Serialize(programs, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }
}

