using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState State = GameState.INTRO;
    [SerializeField] GameObject EditModeObject;
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
        State = GameState.INTRO;
        // PlayMusicHere
        if (GameEvents.IntroStart != null)
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
        if (State != GameState.INTRO) return;
        State = GameState.WAVEINTRO;
        GameEvents.NewWave?.Invoke();
        if (GameEvents.WaveIntroStart != null)
        {
            GameEvents.WaveIntroStart();
        }
        else
        {
            StartEditMode();
        }
    }

    private void StartEditMode()
    {
        if (State != GameState.WAVEINTRO) return;
        State = GameState.EDIT;
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
        if (State != GameState.EDIT) return;
        State = GameState.GAMING;
        EditModeObject.SetActive(false);
        GameEvents.WaveStart?.Invoke();
    }

    private void CheckGameEnded(bool success)
    {
        if (State != GameState.GAMING) return;

        if (success)
        {
            State = GameState.INTRO;
            // Display
            StartWaveIntro();
        }
        else
        {
            State = GameState.GAMEOVER;
            GameEvents.GameOver?.Invoke();
        }
    }

    private enum GameState
    {
        INTRO,
        WAVEINTRO,
        EDIT,
        GAMING,
        GAMEOVER
    }
}


