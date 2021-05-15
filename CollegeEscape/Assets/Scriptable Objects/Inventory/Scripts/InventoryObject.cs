using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using UnityEditor;

public enum InterfaceType{
    Inventory,
    Equipment,
    Chest
}

[CreateAssetMenu(fileName="New Inventory", menuName="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
//, ISerializationCallbackReceiver
{
    public ItemDBObject dbObject;

    public InterfaceType interfaceType;

    public string savePath;

    public Inventory inventoryList;
    public InventorySlot[] GetSlots{
        get { return inventoryList.slotsInventory;}
    }

    public bool AddItem(Item _itemObject,int _amount){

        if(EmptySlotCount <=0){
            return false;
        }
        InventorySlot slotFound=FindItemInInventory(_itemObject);

        if(!dbObject.items[_itemObject.id].stackable || slotFound == null){
            SetEmptySlot(_itemObject,_amount);
            return true;
        }
        slotFound.AddAmount(_amount);
        return true;
        
    }

    public InventorySlot FindItemInInventory(Item _item){
        for(int i=0;i<GetSlots.Length;i++){
            if(_item.id == GetSlots[i].item.id)
                return GetSlots[i];
        }
        return null;
    }

    public int EmptySlotCount{
        get{
            int counter = 0;
            for(int i=0;i<GetSlots.Length;i++){
                if(GetSlots[i].item.id <= -1){
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot SetEmptySlot(Item item,int amount){
        for(int i=0;i<GetSlots.Length;i++){
            if(GetSlots[i].item.id<=-1){
                GetSlots[i].UpdateSlot(item,amount);
                return GetSlots[i];
            }
        }

        //when inventory is full

        return null;
    }

    public void SwapItemsInventory(InventorySlot slot1,InventorySlot slot2){

        if(slot2.CanPlaceInSlot(slot1.ItemObject) && slot1.CanPlaceInSlot(slot2.ItemObject)){
            InventorySlot aux=new InventorySlot(slot2.item,slot2.amount);
            slot2.UpdateSlot(slot1.item,slot1.amount);
            slot1.UpdateSlot(aux.item,aux.amount);
        }

        
    }

    public void RemoveItem(Item item){
        for(int i=0;i<GetSlots.Length;i++){
            if(GetSlots[i].item==item){
                GetSlots[i].UpdateSlot(null,0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save(){
        /*string saveData=JsonUtility.ToJson(this,true);
        BinaryFormatter binaryFormatter=new BinaryFormatter();

        //persistentDataPath - salveaza pe multiple dispozitive
        FileStream fileStream=File.Create(string.Concat(Application.persistentDataPath, savePath));
        binaryFormatter.Serialize(fileStream,saveData);
        fileStream.Close();
        */

        //schimbare adusa pentru a nu putea modifica fisierul de salvare
        IFormatter formatter=new BinaryFormatter();
        Stream stream=new FileStream(string.Concat(Application.persistentDataPath,savePath),FileMode.Create,FileAccess.Write);
        formatter.Serialize(stream,inventoryList);
        stream.Close();

    }

    [ContextMenu("Load")]
    public void Load(){
        /*if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            BinaryFormatter binaryFormatter=new BinaryFormatter();
            FileStream fileStream=File.Open(string.Concat(Application.persistentDataPath, savePath),FileMode.Open);
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(fileStream).ToString(),this);
            fileStream.Close();
        }*/

        IFormatter formatter=new BinaryFormatter();
        Stream stream=new FileStream(string.Concat(Application.persistentDataPath,savePath),FileMode.Open,FileAccess.Read);
        Inventory newinventoryList=(Inventory)formatter.Deserialize(stream);
        for(int i=0;i<GetSlots.Length;i++){
            GetSlots[i].UpdateSlot( newinventoryList.slotsInventory[i].item,
                                                        newinventoryList.slotsInventory[i].amount);
        }
        stream.Close();
    }

    [ContextMenu("Clear")]
    public void Clear(){
        inventoryList.Clear();
    }
/*
    public void OnBeforeSerialize(){
        
    }

    public void OnAfterDeserialize(){
        for(int i=0;i<inventoryList.itemsInventory.Count;i++){
            inventoryList.itemsInventory[i].itemObject=dbObject.getItemForId[inventoryList.itemsInventory[i].id];
        }
    }

*/
    
}

[System.Serializable]
public class Inventory{
      public InventorySlot[] slotsInventory=new InventorySlot[24];

    public void Clear(){
        for(int i=0;i<slotsInventory.Length;i++){
            slotsInventory[i].RemoveItem();
        }
    }
}

//we need to make a delegate (reference to a method or a function using to fire that method or function)
//the reason why we created slotDisplayed in InventorySlot class is cause when we pass the _slot, this will be a reference to slotDisplayed
public delegate void SlotUpdated(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
    public ItemCategories[] allowedItems=new ItemCategories[0];
       //pentru drag in equipment

    //If we dont mention this, save and load will fail because Unity cannot save a scriptable object 
    [System.NonSerialized]
    public UserInterface parent;

    //for passing the item stats to player, we need to make a reference to the object where the slot is on like 
    //itemsDisplayed dictionary, we need to find the Game Object using Inventory Slot, we will populate in static and dynamic interface
    [System.NonSerialized]
    public GameObject slotDisplayed;

    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;

    public Item item;
    public int amount;

    public ItemObject ItemObject{
        get{
            if(item.id>=0){
                return parent.inventory.dbObject.items[item.id];
            }
            return null;
        }
    }

    public InventorySlot(){
        UpdateSlot(new Item(),0);
        this.item=new Item();
        this.amount=0;
    }

    public InventorySlot(Item itemObject,int amount){
        UpdateSlot(itemObject,amount);
        this.item=itemObject;
        this.amount=amount;
    }

    public void UpdateSlot(Item itemObject,int amount){
        if(OnBeforeUpdate != null){
            OnBeforeUpdate.Invoke(this);
        }
        this.item=itemObject;
        this.amount=amount;
        if(OnAfterUpdate != null){
            OnAfterUpdate.Invoke(this);
        }
    }

    public void RemoveItem(){
        UpdateSlot(new Item(),0);
        item=new Item();
        amount=0;
    }

    public void AddAmount(int value){
        UpdateSlot(item,amount+=value);
    }

    public bool CanPlaceInSlot(ItemObject _itemObject){
        if(allowedItems.Length<=0 || _itemObject == null || _itemObject.data.id<0) return true;
        for(int i=0;i<allowedItems.Length;i++){
            if(_itemObject.category == allowedItems[i])
            return true;
        }
        return false;
    }
    
}
