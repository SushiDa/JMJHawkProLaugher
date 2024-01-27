using UnityEngine;

[DisallowMultipleComponent]
public class ObjectMultiPart : MonoBehaviour
{
    [SerializeField]
    private GameObject nextGraphicPrefab;
    public GameObject NextGraphicPrefab => nextGraphicPrefab;

    private ObjectMultiPart linkedPrevious;
    public ObjectMultiPart LinkedPrevious {
        get { return linkedPrevious; }
        set { linkedPrevious = value; }
    }

    private ObjectMultiPart linkedNext;
    public ObjectMultiPart LinkedNext { 
        get { return linkedNext; }
        set { linkedNext = value; }
    }
}