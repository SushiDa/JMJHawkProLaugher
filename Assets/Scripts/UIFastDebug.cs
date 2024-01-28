using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFastDebug : MonoBehaviour
{
    // Start is called before the first frame update
    TMP_Text DebugText;
    WaveManager wave;
    PlayerScoring player;

    private void Awake()
    {
        DebugText = GetComponent<TMP_Text>();
        wave = FindObjectOfType<WaveManager>();
        player = FindObjectOfType<PlayerScoring>();
    }

    private void Update()
    {
        DebugText.text = "Score : " + player.CurrentScore.ToString("000") + " | "+ player.CurrentTricks.Count +" tricks<br>Combo : x"+ player.CurrentMultiplier.ToString("0") + " | "+ player.CurrentComboTimer.ToString("0.0") +"s<br>Wave : "+ wave.WaveNumber +" | "+ wave.CurrentWaveTimer +"s";
    }
}
