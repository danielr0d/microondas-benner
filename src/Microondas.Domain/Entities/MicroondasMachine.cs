using Microondas.Domain.Enums;
using Microondas.Domain.Exceptions;

namespace Microondas.Domain.Entities;

public class MicroondasMachine
{
    private const int MinimumTime = 1;
    private const int MaximumTime = 120;
    private const int DefaultPower = 10;
    private const int QuickStartTime = 30;
    private const char DefaultHeatingCharacter = '.';

    private int _totalTime;
    private int _remainingTime;
    private int _power;
    private MachineState _state;
    private string _heatingString;
    private HeatingProgram? _currentProgram;
    private char _currentHeatingCharacter;

    public MicroondasMachine()
    {
        _totalTime = 0;
        _remainingTime = 0;
        _power = DefaultPower;
        _state = MachineState.Stopped;
        _heatingString = string.Empty;
        _currentProgram = null;
        _currentHeatingCharacter = DefaultHeatingCharacter;
    }

    public int TotalTime => _totalTime;
    public int RemainingTime => _remainingTime;
    public int Power => _power;
    public MachineState State => _state;
    public string HeatingString => _heatingString;
    public HeatingProgram? CurrentProgram => _currentProgram;

    public void Start(int? timeInSeconds = null, int? power = null)
    {
        if (_state == MachineState.Heating)
        {
            AddTime(QuickStartTime);
            return;
        }

        if (_state == MachineState.Paused)
        {
            _state = MachineState.Heating;
            return;
        }

        int time = timeInSeconds ?? QuickStartTime;
        int powerValue = power ?? DefaultPower;

        ValidateHeatingTime(time);
        ValidatePower(powerValue);

        _totalTime = time;
        _remainingTime = time;
        _power = powerValue;
        _state = MachineState.Heating;
        _heatingString = string.Empty;
        _currentProgram = null;
        _currentHeatingCharacter = DefaultHeatingCharacter;
    }

    public void StartProgram(HeatingProgram program)
    {
        if (_state == MachineState.Heating && _currentProgram != null)
            return;

        if (_state == MachineState.Paused && _currentProgram != null)
        {
            _state = MachineState.Heating;
            return;
        }

        _currentProgram = program;
        _totalTime = program.TimeInSeconds;
        _remainingTime = program.TimeInSeconds;
        _power = program.Power;
        _currentHeatingCharacter = program.HeatingCharacter;
        _state = MachineState.Heating;
        _heatingString = string.Empty;
    }

    public void Pause()
    {
        if (_state == MachineState.Stopped)
        {
            _heatingString = string.Empty;
            _currentProgram = null;
            return;
        }

        if (_state == MachineState.Paused)
        {
            _state = MachineState.Stopped;
            _remainingTime = 0;
            _heatingString = string.Empty;
            _currentProgram = null;
            _currentHeatingCharacter = DefaultHeatingCharacter;
            return;
        }

        _state = MachineState.Paused;
    }

    public void SimulateSecond()
    {
        if (_state != MachineState.Heating)
            return;

        int charCount = _currentProgram != null ? 1 : _power;
        _heatingString += new string(_currentHeatingCharacter, charCount);
        _remainingTime--;

        if (_remainingTime <= 0)
        {
            _state = MachineState.Stopped;
            _remainingTime = 0;
            _heatingString += " - Heating completed";
        }
    }

    public string GetDisplay()
    {
        if (_state == MachineState.Stopped && _remainingTime == 0)
            return "00:00";

        int minutes = _remainingTime / 60;
        int seconds = _remainingTime % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void AddTime(int seconds)
    {
        if (_currentProgram != null)
            return;

        _remainingTime += seconds;
        _totalTime += seconds;

        if (_remainingTime > MaximumTime)
            _remainingTime = MaximumTime;
    }

    private void ValidateHeatingTime(int timeInSeconds)
    {
        if (timeInSeconds < MinimumTime || timeInSeconds > MaximumTime)
            throw new InvalidHeatingTimeException($"Heating time must be between {MinimumTime} and {MaximumTime} seconds.");
    }

    private void ValidatePower(int power)
    {
        if (power < 1 || power > 10)
            throw new InvalidPowerException("Power must be between 1 and 10.");
    }
}


