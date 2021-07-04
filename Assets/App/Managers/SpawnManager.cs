using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("System")]
    [SerializeField] private Database database;

    [Header("Collection List View")]
    [SerializeField] private CollectionUnit collectionUnitPrefab;
    [SerializeField] private Transform collectionListContentField;

    [Header("Collection View")]
    [SerializeField] private CollectionView collectionView;
    [SerializeField] private ElementUnit elementUnitPrefab;
    [SerializeField] private Transform collectionContentField;

    [Header("Element View")]
    [SerializeField] private ElementView elementView;


    public void BuildCollectionListView()
    {
        for(int i = 0; i < database.collections.Count; i++)
        {
            CollectionUnit newCollectionUnit = Instantiate(collectionUnitPrefab, collectionListContentField);
            newCollectionUnit.SetData(i, database.collections[i], this);
        }
    }

    public void BuildCollectionView(int collectionIndex)
    {
        collectionView.SetData(database.collections[collectionIndex].name);

        for(int i = 0; i < database.collections[collectionIndex].elements.Count; i++)
        {
            ElementUnit newElementUnit = Instantiate(elementUnitPrefab, collectionContentField);
            newElementUnit.SetData(database.collections[collectionIndex].elements[i], collectionIndex, i, this);
            collectionView.elementUnits.Add(newElementUnit.gameObject); 
        }
    }

    public void BuildElementView(int collectionIndex, int elementIndex)
    {
        elementView.SetData(database.collections[collectionIndex], database.collections[collectionIndex].elements[elementIndex], collectionIndex, elementIndex);
    }

}
