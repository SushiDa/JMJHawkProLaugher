using System;

public static class GameEvents
{
    internal static Action IntroStart;
    internal static Action IntroEnd;
    internal static Action WaveIntroStart;
    internal static Action WaveIntroEnd;
    internal static Action NewWave;
    internal static Action EditItemStart;
    internal static Action EditItemEnd;
    internal static Action WaveStart;
    internal static Action<bool> WaveFinished; // Success
    internal static Action GameOver;

    internal static Action PlayerInteract;
    internal static Action<int, int, float, Trick> ScoreBonus; // score, multiplier, timeBonus, Trick
    internal static Action ComboBreak;
}