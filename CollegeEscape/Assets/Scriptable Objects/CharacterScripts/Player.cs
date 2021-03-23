using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public void OnTriggerEnter(Collider collider){
        var item=collider.GetComponent<Item>();
        if(item){
            inventory.AddItem(item.item,1);
            Destroy(collider.gameObject);
        }
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            inventory.Save();
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            inventory.Load();
        }
    }

    private void OnApplicationQuit(){
        inventory.inventoryList.Clear();
    }
}
