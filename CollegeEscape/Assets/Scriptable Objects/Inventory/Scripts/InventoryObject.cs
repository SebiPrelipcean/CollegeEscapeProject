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

    public void AddItem(Item _itemObject,int _amount){

        if(_itemObject.itemBuffs.Length>0){
            SetEmptySlot(_itemObject,_amount);
            return;
        }

        for(int i=0;i<inventoryList.itemsInventory.Length;i++){
            if(inventoryList.itemsInventory[i].id==_itemObject.id){
                inventoryList.itemsInventory[i].AddAmount(_amount);
                return;
            }
        }

        SetEmptySlot(_itemObject,_amount);
        
    }

    public InventorySlot SetEmptySlot(Item item,int amount){
        for(int i=0;i<inventoryList.itemsInventory.Length;i++){
            if(inventoryList.itemsInventory[i].id<=-1){
                inventoryList.itemsInventory[i].UpdateSlot(item.id,item,amount);
                return inventoryList.itemsInventory[i];
            }
        }

        //when inventory is full

        return null;
    }

    public void MoveItemInventory(InventorySlot slot1,InventorySlot slot2){
        InventorySlot aux=new InventorySlot(slot2.id,slot2.itemObject,slot2.amount);
        slot2.UpdateSlot(slot1.id,slot1.itemObject,slot1.amount);
        slot1.UpdateSlot(aux.id,aux.itemObject,aux.amount);
    }

    public void RemoveItem(Item item){
        for(int i=0;i<inventoryList.itemsInventory.Length;i++){
            if(inventoryList.itemsInventory[i].itemObject==item){
                inventoryList.itemsInventory[i].UpdateSlot(-1,null,0);
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
            inventoryList.itemsInventory[i].UpdateSlot(newinventoryList.itemsInventory[i].id,
                                                        newinventoryList.itemsInventory[i].itemObject,
                                                        newinventoryList.itemsInventory[i].amount);
        }
        stream.Close();
    }

    [ContextMenu("Clear")]
    public void Clear(){
        inventoryList=new Inventory();
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

}

[System.Serializable]
public class InventorySlot
{
    public int id=-1;
    public Item itemObject;
    public int amount;

    public InventorySlot(){
        this.id=-1;
        this.itemObject=null;
        this.amount=0;
    }

    public InventorySlot(int id,Item itemObject,int amount){
        this.id=id;
        this.itemObject=itemObject;
        this.amount=amount;
    }

    public void UpdateSlot(int id,Item itemObject,int amount){
        this.id=id;
        this.itemObject=itemObject;
        this.amount=amount;
    }

    public void AddAmount(int value){
        amount+=value;
    }
    
}
