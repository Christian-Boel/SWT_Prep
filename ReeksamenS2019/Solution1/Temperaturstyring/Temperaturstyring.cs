class TemperaturStyring
{
    private string logFile = "logfile.txt"; // Navnet på systemets log-fil

    // Tilstandsmaskinens tilstande
    private enum State
    {
        Slukket,
        Tændt
    }

    private State state;

    public bool IsHeatOn()
    {
        // Mangler kode her
    }

    public void TemperatureChanged(int temp)
    {
        switch (state)
        {
            // Principiel implementation ifølge tilstandsmaskinediagrammet
            // For tilstand Slukket, når der kommer en ny temperatur
            case State.Slukket:
                if (temp < 0)
                {
                    state = State.Tændt;

                    varmeRelæ.TurnOn();
                    using (var writer = File.AppendText(logFile))
                    {
                        writer.WriteLine(DateTime.Now + ": Temperatur: {0}. Varme tændt.", temp);
                    }
                }
                break;

            case State.Tændt:
                // Mere kode her
                break;
        }
    }


}