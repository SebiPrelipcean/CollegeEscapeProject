using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item Database",menuName="Inventory System/Items/Database")]
public class ItemDBObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;
    public Dictionary<ItemObject,int> getIdForItem=new Dictionary<ItemObject, int>();

      public Dictionary<int,ItemObject> getItemForId=new Dictionary<int,ItemObject>();


    public void OnBeforeSerialize(){
        
    }

    public void OnAfterDeserialize(){
        getIdForItem=new Dictionary<ItemObject, int>();
        getItemForId=new Dictionary<int, ItemObject>();
        for(int i=0;i<items.Length;i++){
            getIdForItem.Add(items[i],i);
            getItemForId.Add(i,items[i]);
        }
    }

}
