using Microondas.Domain.Entities;
using Microondas.Domain.Exceptions;

namespace Microondas.Infrastructure.Services;

public interface IMicrowaveService
{
    MicroondasMachine Machine { get; }
    void SimulateSecond();
}

public class MicrowaveService : IMicrowaveService
{
    public MicroondasMachine Machine { get; }

    public MicrowaveService()
    {
        Machine = new MicroondasMachine();
    }

    public void SimulateSecond()
    {
        Machine.SimulateSecond();
    }
}

