using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine;

public class ElementUnit : MonoBehaviour
{
    public SpawnManager spawnManager;

    [SerializeField] private Image elementImage;
    [SerializeField] private Toggle favorite;
    private int collectionIndex;
    private int elementIndex;

    public void SetData(CollectionElement data, int _collectionIndex, int _elementIndex, SpawnManager _spawnManager)
    {
        spawnManager = _spawnManager;
        collectionIndex = _collectionIndex;
        elementIndex = _elementIndex;
        favorite.isOn = data.isInFavorites;

        Davinci.get().load(data.imagePath).into(elementImage).start();
        elementImage.preserveAspect = true;
    }

    public void OnClick()
    {
        spawnManager.BuildElementView(collectionIndex, elementIndex);
        GameEventMessage.SendEvent("Show Element View");
    }
}
