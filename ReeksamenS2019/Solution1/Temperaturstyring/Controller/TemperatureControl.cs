using Temperaturstyring.Interfaces;

public class TemperatureControl
{
    private ILogFile _logFile;
    private IHeatRelay _heatRelay;
    private IThermometer _thermometer;
    
    //Constructor for dependency injection
    public TemperatureControl(IHeatRelay heatRelay, ILogFile logFile, IThermometer thermometer)
    {
        _heatRelay = heatRelay;
        _logFile = logFile;
        _thermometer = thermometer;
    }
    // Tilstandsmaskinens tilstande
    private enum State
    {
        On,
        Off
    }

    private State state;

    public bool IsHeatOn()
    {
        if (state == State.On)
        {
            return true;
        }

        return false;
    }

    public void TemperatureChanged(int temp)
    {
        switch (state)
        {
            // Principiel implementation ifølge tilstandsmaskinediagrammet
            // For tilstand Slukket, når der kommer en ny temperatur
            case State.Off:
                if (temp < 0)
                {
                    state = State.On;

                    _heatRelay.TurnOn();
                    using (var writer = File.AppendText(_logFile))
                    {
                        writer.WriteLine(DateTime.Now + $": Temperatur: {temp}. Varme tændt.");
                    }
                }
                break;

            case State.On:
                if (temp > 2)
                {
                    state = State.Off;
                    
                    _heatRelay.TurnOff();
                    
                    using (var writer = File.AppendText(_logFile))
                    {
                        writer.WriteLine(DateTime.Now + $": Temperatur: {temp}. Varme slukket.");
                    }
                }
                break;
        }
    }
}