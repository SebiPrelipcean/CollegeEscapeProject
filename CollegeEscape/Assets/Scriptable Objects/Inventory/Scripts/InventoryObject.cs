using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using UnityEditor;


[CreateAssetMenu(fileName="New Inventory", menuName="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
//, ISerializationCallbackReceiver
{
    public ItemDBObject dbObject;

    public string savePath;

    public Inventory inventoryList;

    public bool AddItem(Item _itemObject,int _amount){

        if(EmptySlotCount <=0){
            return false;
        }
        InventorySlot slotFound=FindItemInInventory(_itemObject);

        if(!dbObject.getItemForId[_itemObject.id].stackable || slotFound == null){
            SetEmptySlot(_itemObject,_amount);
            return true;
        }
        slotFound.AddAmount(_amount);
        return true;
        
    }

    public InventorySlot FindItemInInventory(Item _item){
        for(int i=0;i<inventoryList.itemsInventory.Length;i++){
            if(_item.id == inventoryList.itemsInventory[i].item.id)
                return inventoryList.itemsInventory[i];
        }
        return null;
    }

    public int EmptySlotCount{
        get{
            int counter = 0;
            for(int i=0;i<inventoryList.itemsInventory.Length;i++){
                if(inventoryList.itemsInventory[i].item.id <= -1){
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot SetEmptySlot(Item item,int amount){
        for(int i=0;i<inventoryList.itemsInventory.Length;i++){
            if(inventoryList.itemsInventory[i].item.id<=-1){
                inventoryList.itemsInventory[i].UpdateSlot(item,amount);
                return inventoryList.itemsInventory[i];
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
        for(int i=0;i<inventoryList.itemsInventory.Length;i++){
            if(inventoryList.itemsInventory[i].item==item){
                inventoryList.itemsInventory[i].UpdateSlot(null,0);
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
        for(int i=0;i<inventoryList.itemsInventory.Length;i++){
            inventoryList.itemsInventory[i].UpdateSlot( newinventoryList.itemsInventory[i].item,
                                                        newinventoryList.itemsInventory[i].amount);
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
      public InventorySlot[] itemsInventory=new InventorySlot[24];

    public void Clear(){
        for(int i=0;i<itemsInventory.Length;i++){
            itemsInventory[i].RemoveItem();
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemCategories[] allowedItems=new ItemCategories[0];
       //pentru drag in equipment

    //If we dont mention this, save and load will fail because Unity cannot save a scriptable object 
    [System.NonSerialized]
    public UserInterface parent;
    public Item item;
    public int amount;

    public ItemObject ItemObject{
        get{
            if(item.id>=0){
                return parent.inventory.dbObject.getItemForId[item.id];
            }
            return null;
        }
    }

    public InventorySlot(){
        this.item=new Item();
        this.amount=0;
    }

    public InventorySlot(Item itemObject,int amount){
        this.item=itemObject;
        this.amount=amount;
    }

    public void UpdateSlot(Item itemObject,int amount){
        this.item=itemObject;
        this.amount=amount;
    }

    public void RemoveItem(){
        item=new Item();
        amount=0;
    }

    public void AddAmount(int value){
        amount+=value;
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
