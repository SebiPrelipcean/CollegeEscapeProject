using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item Database",menuName="Inventory System/Items/Database")]
public class ItemDBObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;

    public void OnBeforeSerialize(){
    }

    public void OnAfterDeserialize(){
        UpdateID();
    }

    [ContextMenu("Update ID's")]
    public void UpdateID(){
        for(int i=0;i<items.Length;i++){
            if(items[i].data.id != i){
                items[i].data.id = i;
            }
        }
    }

}
