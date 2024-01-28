using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFastDebug : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI scoretext;
    [SerializeField]
    private TextMeshProUGUI Combotext;
    PlayerScoring player;



    private void Start()
    {
        player = FindAnyObjectByType<PlayerScoring>();
    }

    private void Update()
    {
        scoretext.text = player.CurrentScore.ToString("000");
        Combotext.text = player.CurrentMultiplier.ToString("0");
    }
}
