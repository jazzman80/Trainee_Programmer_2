using System.Collections.Generic;
using UnityEngine;

public class Collection
{
    public int id;
    public string name;
    public string backgroundImage;
    public string previewImage;
    public List<CollectionElement> elements = new List<CollectionElement>();
}
