using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DebugGameLoop : MonoBehaviour
{
    [SerializeField] TMP_Text Text;
    [SerializeField] TMP_Text Counter;

    private void Awake()
    {
        Text.text = "";
        Counter.text = "";
        GameEvents.IntroStart += Intro;
        GameEvents.WaveIntroStart += WaveIntro;
        GameEvents.GameOver += GameOver;
    }

    private void OnDestroy()
    {
        GameEvents.IntroStart -= Intro;
        GameEvents.WaveIntroStart -= WaveIntro;
        GameEvents.GameOver -= GameOver;
    }

    private async void Intro()
    {
        await DebugGameState("WELCOME JMJ !", EndIntro);
    }

    private void EndIntro()
    {
        GameEvents.IntroEnd?.Invoke();
    }

    private async void WaveIntro()
    {
        WaveManager wave = FindObjectOfType<WaveManager>();
        await DebugGameState("Wave " + wave.WaveNumber, EndWaveIntro);
    }

    private void EndWaveIntro()
    {
        GameEvents.WaveIntroEnd?.Invoke();
    }

    private async void Edit()
    {
        await DebugGameState("Edit", EndEdit);
    }

    private void EndEdit()
    {
        GameEvents.EditItemEnd?.Invoke();
    }

    private void GameOver()
    {
        Text.text = "Game Over";
    }

    private async Task DebugGameState(string text, Action callback)
    {
        Text.text =text;
        Counter.text = "3";
        await Task.Delay(750);
        Counter.text = "2";
        await Task.Delay(750);
        Counter.text = "1";
        await Task.Delay(750);
        Counter.text = "";
        Text.text = "";
        callback();
    }

}
