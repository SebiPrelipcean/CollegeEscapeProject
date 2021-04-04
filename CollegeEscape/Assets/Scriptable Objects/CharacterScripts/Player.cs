using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    
    public MouseItem mouseItem=new MouseItem();
    public void OnTriggerEnter(Collider collider){
        var item=collider.GetComponent<GroundItem>();
        if(item){
            inventory.AddItem(new Item(item.itemObject),1);
            Destroy(collider.gameObject);
        }
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            inventory.Save();
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            inventory.Load();
        }
    }

    private void OnApplicationQuit(){
        inventory.inventoryList.itemsInventory=new InventorySlot[24];
    }
}
