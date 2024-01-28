using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] internal float WaveStartTimer;
    [SerializeField] internal PlayerGameController PlayerPrefab;
    [SerializeField] internal Transform PlayerSpawnPosition;

    [SerializeField] private TMP_Text PromptText;

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

        // Spawn all items
        foreach (var spawner in FindObjectsOfType<ItemPreview>(true))
        {
            spawner.gameObject.SetActive(true);
        }

        foreach (var item in FindObjectsOfType<AbstractItem>(true))
        {
            Destroy(item.gameObject);
        }
        var player = FindObjectOfType<PlayerGameController>(true);

        if(player != null)
            Destroy(player.gameObject);

        //TODO timer en fonction de la wave courante
        CurrentWaveTimer = WaveStartTimer;
    }

    private void StartWave()
    {
        // Coroutine de départ ici
        WaveStarted = true;
        // Spawn Player
        Instantiate<PlayerGameController>(PlayerPrefab, PlayerSpawnPosition.position, PlayerSpawnPosition.rotation);

        // Spawn all items
        foreach(var spawner in FindObjectsOfType<ItemPreview>(true))
        {
            spawner.SpawnItem();
            spawner.gameObject.SetActive(false);
        }


        StartCoroutine(WaveStartRoutine());
    }

    private IEnumerator WaveStartRoutine()
    {
        PromptText.text = "READY ...";
        yield return new WaitForSeconds(.5f);
        PromptText.text = "3";
        yield return new WaitForSeconds(.5f);
        PromptText.text = "2";
        yield return new WaitForSeconds(.5f);
        PromptText.text = "1";
        yield return new WaitForSeconds(.5f);
        PromptText.text = "GO !";
        yield return new WaitForSeconds(.5f);
        PromptText.text = "";
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

        var player = FindObjectOfType<PlayerGameController>(true);

        if(player != null)
        {
            player.GetComponent<PlayerProximityInteract>().CancelActiveItem(true, ItemCancelTypes.NONE);
            player.InputHub.CanMove = false;
            player.InputHub.CanJump = false;
            player.InputHub.CanRotate = false;
        }
    }

}
