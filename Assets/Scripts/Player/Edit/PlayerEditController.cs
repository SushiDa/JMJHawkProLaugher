using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerPlaceController))]
public class PlayerEditController : MonoBehaviour
{
    [System.Serializable]
    private class ObjectMapping
    {
        [HideInInspector]
        public int id = 0;
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
    private Vector2Int chooseObjectCount = new Vector2Int(1, 3);

    private PlayerPlaceController placeController;
    private readonly List<ObjectMapping> currentMappings = new List<ObjectMapping>();





    void Awake()
    {
        placeController = GetComponent<PlayerPlaceController>();
    }

    void OnEnable()
    {
        ClearMappings();
        CreateMappings();
        EnableChoose();
    }

    void OnDisable()
    {
        ClearMappings();
        chooseRoot.SetActive(false);
        placeRoot.SetActive(false);
    }

    private void EnableChoose()
    {
        chooseRoot.SetActive(true);
        placeRoot.SetActive(false);
    }

    private void EnablePlace()
    {
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
        }
    }





    public void PlayGame()
    {
        // TODO : switch from edit mode to game mode
    }

    public void Edit(int Id)
    {
        int index = currentMappings.FindIndex((ObjectMapping mapping) => mapping.id == Id);
        if (index < 0) return;
        placeController.Place(currentMappings[index].graphicPrefab);
        EnablePlace();
    }

    public void Choose()
    {
        placeController.Place(null);
        EnableChoose();
    }
}