using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPreview : MonoBehaviour
{
    [SerializeField] private AbstractItem ItemPrefab;
    [SerializeField] internal string Title;
    internal ItemPreview NextItem;

    internal bool IsNextMultipart;
    // Start is called before the first frame update
    internal void SpawnItem(bool force = false, AbstractItem parentItem = null)
    {
        if (!IsNextMultipart || force)
        {
            var item = Instantiate(ItemPrefab, transform.position, transform.rotation);
            if(parentItem != null)
            {
                parentItem.LinkedItem = item;
            }
            if (NextItem != null)
            {
                NextItem.SpawnItem(force: true, parentItem: item);

            }
        }
    }
}
