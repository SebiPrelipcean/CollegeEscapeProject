using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{

    public GameObject inventoryPrefab;

    public int x_start;
    public int y_start;
    public int x_space_between_items;
    public int y_space_between_items;
    public int nr_column;

    public override void CreateSlots(){
        itemsDesplayed=new Dictionary<GameObject, InventorySlot>();

        for(int i=0;i<inventory.GetSlots.Length;i++){
            //respawn
            var obj=Instantiate(inventoryPrefab,Vector3.zero,Quaternion.identity,transform);
            obj.GetComponent<RectTransform>().localPosition=GetPosition(i);

            AddEvent(obj,EventTriggerType.PointerEnter,delegate{OnEnter(obj);});
            AddEvent(obj,EventTriggerType.PointerExit,delegate{OnExit(obj);});
            AddEvent(obj,EventTriggerType.BeginDrag,delegate{OnBeginDrag(obj);});
            AddEvent(obj,EventTriggerType.EndDrag,delegate{OnEndDrag(obj);});
            AddEvent(obj,EventTriggerType.Drag,delegate{OnDrag(obj);});

            inventory.GetSlots[i].slotDisplayed = obj;

            itemsDesplayed.Add(obj,inventory.GetSlots[i]);
        }
    }

    private Vector3 GetPosition(int i){
            return new Vector3(x_start + (x_space_between_items * (i % nr_column)), y_start + (-y_space_between_items * (i / nr_column)), 0f);
    }
}
