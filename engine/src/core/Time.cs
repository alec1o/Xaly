namespace XalyEngine;

public class Time
{
    public static float DeltaTime => (float)PreciseDeltaTime;
    public static double TimeScale { get; set; } = 1d;
    public static double PreciseDeltaTime { get; internal set; } = 0d;
    public static double TotalSeconds { get; internal set; } = 0d;
}