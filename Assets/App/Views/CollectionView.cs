using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    public List<GameObject> elementUnits = new List<GameObject>();

    public void SetData(string headerText)
    {
        header.text = headerText;

        for(int i = 0; i < elementUnits.Count; i++)
        {
            Destroy(elementUnits[i]);
        }

        elementUnits.Clear();
    }
}
