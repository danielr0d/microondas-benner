using Microondas.Domain.Entities;

namespace Microondas.Domain.Interfaces;

public interface IProgramRepository
{
    Task<List<HeatingProgram>> GetAllCustomProgramsAsync();
    Task<HeatingProgram?> GetProgramByNameAsync(string name);
    Task AddProgramAsync(HeatingProgram program);
    Task UpdateProgramAsync(HeatingProgram program);
    Task DeleteProgramAsync(string name);
}

