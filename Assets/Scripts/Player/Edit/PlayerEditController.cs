using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerPlaceController))]
public class PlayerEditController : MonoBehaviour
{
    [System.Serializable]
    private class ObjectMapping
    {
        [HideInInspector]
        public int id = 0;
        [HideInInspector]
        public GameObject uiObject;

        public GameObject graphicPrefab;
        public GameObject uiPrefab;

        public ObjectMapping Copy()
        {
            return new ObjectMapping {
                id = System.Guid.NewGuid().GetHashCode(),
                graphicPrefab = graphicPrefab,
                uiPrefab = uiPrefab
            };
        }
    }





    [Header("Action Maps")]
    [SerializeField]
    private string editActionMap = "EditMode";
    [SerializeField]
    private string gameActionMap = "General";

    [Header("Global Roots")]
    [SerializeField]
    private GameObject editGlobalRoot;
    [SerializeField]
    private GameObject gameGlobalRoot;

    [Header("Settings")]
    [SerializeField]
    private GameObject chooseRoot;
    [SerializeField]
    private GameObject placeRoot;
    [SerializeField]
    private ObjectMapping[] objectsMappings = new ObjectMapping[0];

    [Header("Choose Settings")]
    [SerializeField]
    private RectTransform chooseUIRoot;
    [SerializeField]
    private Vector2Int chooseObjectCount = new Vector2Int(2, 3);

    private PlayerPlaceController placeController;
    private readonly List<ObjectMapping> currentMappings = new List<ObjectMapping>();
    private int currentEditingId = 0;


    private PlayerInputHub inputHub;
    public PlayerInputHub InputHub { 
        get {
            if (inputHub == null) inputHub = FindObjectOfType<PlayerInputHub>();
            return inputHub;
        }
    }




    void Awake()
    {
        placeController = GetComponent<PlayerPlaceController>();
    }

    void OnEnable()
    {
        ClearMappings();
        CreateMappings();
        EnableChoose();
        if (InputHub != null) InputHub.PlayerInputWrapper.SetActionMap(editActionMap);
    }

    void OnDisable()
    {
        ClearMappings();
        chooseRoot.SetActive(false);
        placeRoot.SetActive(false);
        if (InputHub != null) InputHub.PlayerInputWrapper.SetActionMap(gameActionMap);
    }

    private void EnableChoose()
    {
        placeController.enabled = false;
        chooseRoot.SetActive(true);
        placeRoot.SetActive(false);
    }

    private void EnablePlace()
    {
        placeController.enabled = true;
        placeRoot.SetActive(true);
        chooseRoot.SetActive(false);
    }





    private void ClearMappings()
    {
        currentMappings.Clear();
        for (int i = chooseUIRoot.childCount - 1; i >= 0; --i) {
            Destroy(chooseUIRoot.GetChild(i).gameObject);
        }
    }

    private void CreateMappings()
    {
        // Loop x Time randomly to create random Mappings
        int mappingCount = Random.Range(chooseObjectCount.x, chooseObjectCount.y);
        for (int i = 0; i < mappingCount; ++i) {

            // Choose random mapping and duplicate it
            int index = Random.Range(0, objectsMappings.Length);
            currentMappings.Add(objectsMappings[index].Copy());

            // Instantiate Mapping UI Button, and assign him his ID
            GameObject uiObject = Instantiate(currentMappings[i].uiPrefab, chooseUIRoot);
            uiObject.GetComponent<ChooseObjectButton>().ID = currentMappings[i].id;
            if (i == 0) EventSystem.current.SetSelectedGameObject(uiObject);
            currentMappings[i].uiObject = uiObject;
        }
    }





    public void Edit(int Id)
    {
        InputHub.ReadJump(purge: true);
        int index = currentMappings.FindIndex((ObjectMapping mapping) => mapping.id == Id);
        if (index < 0) return;
        placeController.Place(currentMappings[index].graphicPrefab);
        currentEditingId = Id;
        EnablePlace();
    }

    public void Choose()
    {
        placeController.Place(null);
        if (currentMappings.Count <= 0) PlayGame();
        else EnableChoose();
    }

    public void ConsumeEditing()
    {
        int index = currentMappings.FindIndex((ObjectMapping mapping) => mapping.id == currentEditingId);
        if (currentEditingId == 0 || index < 0) return;
        if (currentMappings[index].uiObject != null) Destroy(currentMappings[index].uiObject);
        currentMappings.RemoveAt(index);
        currentEditingId = 0;
    }

    private void PlayGame()
    {
        editGlobalRoot.SetActive(false);
        gameGlobalRoot.SetActive(true);
    }
}