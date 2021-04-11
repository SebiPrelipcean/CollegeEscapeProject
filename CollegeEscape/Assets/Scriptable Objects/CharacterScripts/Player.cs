using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    public Attribute[] attributes;

    //give values
    private void Start(){
        for(int i=0 ; i < attributes.Length ; i++){
            attributes[i].SetParent(this);
        }

        for(int i=0 ; i < equipment.GetSlots.Length ; i++){
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot inventorySlot){
        if(inventorySlot == null){
            return;
        }

        //first we will create a way to know what type of interface we are (enum in InventoryObject)
        switch(inventorySlot.parent.inventory.interfaceType){
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", inventorySlot.ItemObject," on ", inventorySlot.parent.inventory.interfaceType, ", Allowed Items: ", string.Join(", ",inventorySlot.allowedItems)));
                for(int i=0;i<inventorySlot.item.itemBuffs.Length;i++){
                    for(int j=0;j<attributes.Length;j++){
                        //check if the item buffs attribute type is the same as the attribute type of our player
                        if(attributes[j].attributeType == inventorySlot.item.itemBuffs[i].attribute){
                            attributes[j].modInt.RemoveModifier(inventorySlot.item.itemBuffs[i]);
                        }
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot inventorySlot){

        if(inventorySlot.ItemObject == null)
            return;

        switch(inventorySlot.parent.inventory.interfaceType){
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:

                print(string.Concat("Placed ", inventorySlot.ItemObject," on ", inventorySlot.parent.inventory.interfaceType, ", Allowed Items: ", string.Join(", ",inventorySlot.allowedItems)));
                
                //pass the item stats to the player when these are removed and added

                for(int i=0;i<inventorySlot.item.itemBuffs.Length;i++){
                    for(int j=0;j<attributes.Length;j++){
                        //check if the item buffs attribute type is the same as the attribute type of our player
                        if(attributes[j].attributeType == inventorySlot.item.itemBuffs[i].attribute){
                            attributes[j].modInt.AddModifier(inventorySlot.item.itemBuffs[i]);
                        }
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }

    }
    
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

    public void AttributeModified(Attribute attribute){
        Debug.Log(string.Concat(attribute.attributeType, " was updated! Value is now: ", attribute.modInt.GetModifiedValue));
    }

    private void OnApplicationQuit(){
        inventory.Clear();
        equipment.Clear();
    }
}

//store the type of int and modified integer and a link to the parent
[System.Serializable]
public class Attribute{
    [System.NonSerialized]
    public Player parent;
    public Attributes attributeType;
    public ModifiableInteger modInt;//value

    public void SetParent(Player parent){
        this.parent = parent;
        this.modInt = new ModifiableInteger(AttributeModified);  //constructor
    }

    public void AttributeModified(){
            //we will make a fire that will callback the function inside ModifiableInteger by passing this method to constructor
            parent.AttributeModified(this);
        }
}
