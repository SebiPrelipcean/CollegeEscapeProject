using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    
    //public MouseItem mouseItem=new MouseItem();
    public void OnTriggerEnter(Collider collider){
        var item=collider.GetComponent<GroundItem>();
        if(item){
            Item _item=new Item(item.itemObject);
            if(inventory.AddItem(_item,1)){
                Destroy(collider.gameObject);
            }
            
        }
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            inventory.Save();
            equipment.Save();
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            inventory.Load();
            equipment.Load();
        }
    }

    private void OnApplicationQuit(){
        inventory.inventoryList.Clear();
        equipment.inventoryList.Clear();
    }
}
