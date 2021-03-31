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
            inventoryList.itemsInventory.Add(new InventorySlot(_itemObject.id,_itemObject,_amount));
            return;
        }

        for(int i=0;i<inventoryList.itemsInventory.Count;i++){
            if(inventoryList.itemsInventory[i].itemObject.id==_itemObject.id){
                inventoryList.itemsInventory[i].AddAmount(_amount);
                return;
            }
        }
            inventoryList.itemsInventory.Add(new InventorySlot(_itemObject.id,_itemObject,_amount));
        
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
        inventoryList=(Inventory)formatter.Deserialize(stream);
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
      public List<InventorySlot> itemsInventory=new List<InventorySlot>();

}

[System.Serializable]
public class InventorySlot
{
    public int id;
    public Item itemObject;
    public int amount;

    public InventorySlot(int id,Item itemObject,int amount){
        this.id=id;
        this.itemObject=itemObject;
        this.amount=amount;
    }

    public void AddAmount(int value){
        amount+=value;
    }
}