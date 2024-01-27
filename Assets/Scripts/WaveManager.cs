using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] internal float WaveStartTimer;

    internal int WaveNumber;
    internal float CurrentWaveTimer;

    bool WaveStarted;

    private void Awake()
    {
        GameEvents.ScoreBonus += RegisterScoreBonus;
        GameEvents.WaveFinished += FinishWave;
        GameEvents.NewWave += NewWave;
        GameEvents.WaveStart += StartWave;
    }

    private void OnDestroy()
    {
        GameEvents.ScoreBonus -= RegisterScoreBonus;
        GameEvents.WaveFinished -= FinishWave;
        GameEvents.NewWave -= NewWave;
        GameEvents.WaveStart -= StartWave;
    }

    private void NewWave()
    {
        WaveNumber++;
        WaveStarted = false;
        //TODO timer en fonction de la wave courante
        CurrentWaveTimer = WaveStartTimer;
    }

    private void StartWave()
    {
        WaveStarted = true;
    }

    private void RegisterScoreBonus(int Score, int Multiplier, float TimeBonus, Trick trick)
    {
        CurrentWaveTimer = Mathf.Min(WaveStartTimer, CurrentWaveTimer + TimeBonus);
    }

    // Update is called once per frame
    void Update()
    {
        if (!WaveStarted || CurrentWaveTimer <= 0) return;

        CurrentWaveTimer -= Time.deltaTime;
        if (CurrentWaveTimer <= 0) GameEvents.WaveFinished?.Invoke(false);
    }

    private void FinishWave(bool success)
    {
        WaveStarted = false;
        CurrentWaveTimer = -1;
    }

}
