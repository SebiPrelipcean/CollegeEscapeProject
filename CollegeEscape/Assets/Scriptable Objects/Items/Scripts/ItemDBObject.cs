using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item Database",menuName="Inventory System/Items/Database")]
public class ItemDBObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;
      public Dictionary<int,ItemObject> getItemForId=new Dictionary<int,ItemObject>();


    public void OnBeforeSerialize(){
        getItemForId=new Dictionary<int, ItemObject>();
    }

    public void OnAfterDeserialize(){
        for(int i=0;i<items.Length;i++){
            items[i].id=i;
            getItemForId.Add(i,items[i]);
        }
    }

}
