using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine.UI;

public class ElementView : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Database database;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private Image productImage;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI rating;
    [SerializeField] private UIToggle favorite;
    private int index;
    private int collectionIndex;
    private int elementIndex;

    public void SetData(Collection collectionData, CollectionElement elementData, int _collectionIndex, int _elementIndex)
    {
        collectionIndex = _collectionIndex;
        elementIndex = _elementIndex;

        header.text = collectionData.name;

        Davinci.get().load(elementData.imagePath).into(productImage).start();
        productImage.preserveAspect = true;

        description.text = elementData.description;
        price.text = "Цена<br><size=100><b>" + elementData.price + "</b></size> руб.";
        rating.text = "Рейтинг<br><size=100><b>" + elementData.rating + "<sprite index=0></b></size>";
        favorite.IsOn = elementData.isInFavorites;
        index = elementData.index;
    }

    public void Favorite()
    {
        networkManager.FavoriteElement(index);
        database.UpdateFav(collectionIndex, elementIndex, true);
    }

    public void UnFavorite()
    {
        networkManager.UnFavoriteElement(index);
        database.UpdateFav(collectionIndex, elementIndex, false);
    }

    public void OnBackButtonClick()
    {
        spawnManager.BuildCollectionView(collectionIndex);
    }
}
