using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour
{
    internal int CurrentScore;
    internal int CurrentMultiplier;
    internal List<Trick> CurrentTricks;

    [SerializeField] internal float MaxComboTimer;
    [SerializeField] internal float ComboTimerIncrement;

    internal float CurrentComboTimer;

    private void Awake()
    {
        GameEvents.ScoreBonus += RegisterScoreBonus;
        GameEvents.NewWave += NewWave;
        GameEvents.WaveStart += StartWave;
        GameEvents.ComboBreak += ComboBreak;
    }

    private void OnDestroy()
    {
        GameEvents.ScoreBonus -= RegisterScoreBonus;
        GameEvents.NewWave -= NewWave;
        GameEvents.WaveStart -= StartWave;
        GameEvents.ComboBreak -= ComboBreak;
    }

    private void NewWave()
    {
        CurrentMultiplier = 1;
        CurrentTricks.Clear();
    }

    private void StartWave()
    {
        CurrentMultiplier = 1;
        CurrentTricks.Clear();
    }

    private void RegisterScoreBonus(int Score, int Multiplier, float TimeBonus, Trick trick)
    {
        if (CurrentTricks.Find(t => t.ItemSource == trick.ItemSource && t.Direction == trick.Direction).ItemSource != "")
        {
            CurrentMultiplier += Multiplier;
        }
        CurrentTricks.Add(trick);
        CurrentScore += Score * CurrentMultiplier;

        if(CurrentComboTimer > 0)
        {
            CurrentComboTimer = Mathf.Min(CurrentComboTimer + ComboTimerIncrement, MaxComboTimer);
        }
        else
        {
            CurrentComboTimer = MaxComboTimer;
        }
        
    }

    private void Update()
    {
        if(CurrentComboTimer > 0)
        {
            CurrentComboTimer -= Time.deltaTime;
            if(CurrentComboTimer <= 0)
            {
                GameEvents.ComboBreak?.Invoke();
            }
        }
    }

    private void ComboBreak()
    {
        CurrentMultiplier = 1;
        CurrentTricks.Clear();
        CurrentComboTimer = 0;

        AudioBridge.PlaySFX("ComboBreak");
    }

}

public struct Trick
{
    public string ItemSource;
    public PlayerDirection Direction;
}

public enum PlayerDirection
{
    UNKNOWN,
    FRONT,
    BACK,
    FOOT,
    HAND
}
