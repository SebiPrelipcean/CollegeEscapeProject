using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
