using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        GameEvents.IntroEnd += StartWaveIntro;
        GameEvents.WaveIntroEnd += StartEditMode;
        GameEvents.EditItemEnd += StartWave;
        GameEvents.WaveFinished += CheckGameEnded;
    }

    private void OnDestroy()
    {
        GameEvents.IntroEnd += StartWaveIntro;
        GameEvents.WaveIntroEnd -= StartEditMode;
        GameEvents.EditItemEnd -= StartWave;
        GameEvents.WaveFinished -= CheckGameEnded;
    }

    private void Start()
    {
        // PlayMusicHere
        if(GameEvents.IntroStart != null)
        {
            GameEvents.IntroStart();
        }
        else
        {
            StartWaveIntro();
        }
    }

    private void StartWaveIntro()
    {
        GameEvents.NewWave?.Invoke();
        if (GameEvents.WaveIntroStart != null)
        {
            GameEvents.EditItemStart();
        }
        else
        {
            StartWave();
        }
    }

    private void StartEditMode()
    {
        if (GameEvents.EditItemStart != null)
        {
            GameEvents.EditItemStart();
        }
        else
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        GameEvents.WaveStart?.Invoke();
    }

    private void CheckGameEnded(bool success)
    {
        if(success)
        {
            // Display
            StartWaveIntro();
        }
        else
        {
            GameEvents.GameOver?.Invoke();
        }
    }
}
