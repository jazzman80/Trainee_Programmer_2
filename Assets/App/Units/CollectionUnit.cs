using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine;

public class CollectionUnit : MonoBehaviour
{
    private SpawnManager spawnManager;
    private int index;
    [SerializeField] private TextMeshProUGUI collectionCaption;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image collectionImage;

    public void SetData(int _index, Collection data, SpawnManager _spawnManager)
    {
        spawnManager = _spawnManager;
        index = _index;
        collectionCaption.text = data.name;
        Davinci.get().load(data.backgroundImage).into(backgroundImage).start();
        Davinci.get().load(data.previewImage).into(collectionImage).start();
    }

    public void OnClick()
    {
        spawnManager.BuildCollectionView(index);
        GameEventMessage.SendEvent("Show Collection View");
    }

}
