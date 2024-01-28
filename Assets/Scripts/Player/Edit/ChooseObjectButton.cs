using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class ChooseObjectButton : MonoBehaviour
{
    private int id = 0;
    public int ID {
        set { id = value; }
        private get { return id; }
    }

    private PlayerEditController playerEditController;
    public PlayerEditController PlayerEditController {
        get {
            if (playerEditController == null) playerEditController = FindObjectOfType<PlayerEditController>(includeInactive: true);
            return playerEditController;
        }
    }

    public void OnClick()
    {
        if (PlayerEditController == null) return;
        PlayerEditController.Edit(ID);
    }
}