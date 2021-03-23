using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName="New Inventory", menuName="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public List<InventorySlot> inventoryList=new List<InventorySlot>();

    private ItemDBObject dbObject;

    public string savePath;

    private void OnEnable(){
#if UNITY_EDITOR
        this.dbObject=(ItemDBObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset",typeof(ItemDBObject));
#else
        this.dbObject=Resources.Load<ItemDBObject>("Database");
#endif
    }

    public void AddItem(ItemObject _itemObject,int _amount){


        for(int i=0;i<inventoryList.Count;i++){
            if(inventoryList[i].itemObject==_itemObject){
                inventoryList[i].AddAmount(_amount);
                break;
            }
        }
            inventoryList.Add(new InventorySlot(dbObject.getIdForItem[_itemObject],_itemObject,_amount));
        
    }

    public void Save(){
        string saveData=JsonUtility.ToJson(this,true);
        BinaryFormatter binaryFormatter=new BinaryFormatter();

        //persistentDataPath - salveaza pe multiple dispozitive
        FileStream fileStream=File.Create(string.Concat(Application.persistentDataPath, savePath));
        binaryFormatter.Serialize(fileStream,saveData);
        fileStream.Close();
    }

    public void Load(){
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            BinaryFormatter binaryFormatter=new BinaryFormatter();
            FileStream fileStream=File.Open(string.Concat(Application.persistentDataPath, savePath),FileMode.Open);
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(fileStream).ToString(),this);
            fileStream.Close();
        }
    }

    public void OnBeforeSerialize(){
        
    }

    public void OnAfterDeserialize(){
        for(int i=0;i<inventoryList.Count;i++){
            inventoryList[i].itemObject=dbObject.getItemForId[inventoryList[i].id];
        }
    }


    
}

[System.Serializable]
public class InventorySlot
{
    public int id;
    public ItemObject itemObject;
    public int amount;

    public InventorySlot(int id,ItemObject itemObject,int amount){
        this.id=id;
        this.itemObject=itemObject;
        this.amount=amount;
    }

    public void AddAmount(int value){
        amount+=value;
    }
}

