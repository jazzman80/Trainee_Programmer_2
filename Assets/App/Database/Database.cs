using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour
{
    public List<Collection> collections = new List<Collection>();

    public void BuildDatabase(List<CollectionListResponse> collectionListResponse, List<CollectionElementResponse> collectionElementResponse, NetworkManager networkManager)
    {
        //build collections list
        for (int i = 0; i < collectionListResponse.Count; i++)
        {
            Collection newCollection = new Collection();
            newCollection.id = collectionListResponse[i].id;
            newCollection.name = collectionListResponse[i].name;
            newCollection.backgroundImage = collectionListResponse[i].backgroundImage;
            newCollection.previewImage = collectionListResponse[i].previewImage;

            collections.Add(newCollection);
        }

        //build collections
        for(int i = 0; i < collectionElementResponse.Count; i++)
        {
            CollectionElement newElement = new CollectionElement();
            newElement.imagePath = collectionElementResponse[i].photo;
            newElement.description = Format(collectionElementResponse[i].description);
            newElement.price = collectionElementResponse[i].price;
            newElement.rating = collectionElementResponse[i].rating;
            newElement.isInFavorites = collectionElementResponse[i].favorite;
            newElement.index = collectionElementResponse[i].id;

            //add element to collection
            for(int j = 0; j < collections.Count; j++)
            {
                if(collections[j].id == collectionElementResponse[i].sets_id)
                {
                    collections[j].elements.Add(newElement);
                    break;
                }
            }
        }
    }

    private string Format(string description)
    {
        return description.Replace("</style>", "</color></size>").Replace("<style=\"HelveticaBold\">", "<size=100><color=black>").
            Replace("<style=\"CollectionItemMark\">", "<size=70><color=black>").Replace("<style=\"Normal\">", "<size=50><color=black>").
            Replace("<style=\"CollectionItemDescriptionPreTitle\">", "<size=50><color=grey>");
    }

    public void UpdateFav(int collectionIndex, int elementIndex, bool _isInFavorites)
    {
        if (collections.Count!=0) collections[collectionIndex].elements[elementIndex].isInFavorites = _isInFavorites;
    }

}
