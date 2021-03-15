using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategories{
    FOOD,
    EQUIPMENT,
    DEFAULT
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemCategories category;
    [TextArea(15,20)]
    public string description;
}
