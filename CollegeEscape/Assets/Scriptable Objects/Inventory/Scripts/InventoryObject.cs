using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Inventory", menuName="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> inventoryList=new List<InventorySlot>();

    public void AddItem(ItemObject _itemObject,int _amount){

        bool hasItem=false;

        for(int i=0;i<inventoryList.Count;i++){
            if(_itemObject==inventoryList[i].itemObject){
                inventoryList[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if(hasItem==false){
            inventoryList.Add(new InventorySlot(_itemObject,_amount));
        }

    }


    
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject itemObject;
    public int amount;

    public InventorySlot(ItemObject _itemObject,int _amount){
        itemObject=_itemObject;
        amount=_amount;
    }

    public void AddAmount(int value){
        amount+=value;
    }
}

