using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject inventoryPrefab;

    public MouseItem mouseItem=new MouseItem();

    public int x_start;
    public int y_start;
    public int x_space_between_items;
    public int y_space_between_items;
    public int nr_column;

    Dictionary<GameObject,InventorySlot> itemsDesplayed=new Dictionary<GameObject,InventorySlot>();

    void Start()
    {
        CreateSlots();
    }

    void Update()
    {
        UpdateSlots();
    }

    public void UpdateSlots(){
        foreach(KeyValuePair<GameObject,InventorySlot> slot in itemsDesplayed){
            
            //If the current slot represents an item
            if(slot.Value.id>=0){
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite=inventory.dbObject.getItemForId[slot.Value.itemObject.id].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color=new Color(1,1,1,1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text=(slot.Value.amount==1) ? "" : slot.Value.amount.ToString("n0");
            }else{
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite=null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color=new Color(1,1,1,1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text="";
            }
        }
    }

    public void CreateSlots(){
        itemsDesplayed=new Dictionary<GameObject, InventorySlot>();

        for(int i=0;i<inventory.inventoryList.itemsInventory.Length;i++){
            //respawn
            var obj=Instantiate(inventoryPrefab,Vector3.zero,Quaternion.identity,transform);
            obj.GetComponent<RectTransform>().localPosition=GetPosition(i);

            AddEvent(obj,EventTriggerType.PointerEnter,delegate{OnEnter(obj);});
            AddEvent(obj,EventTriggerType.PointerExit,delegate{OnExit(obj);});
            AddEvent(obj,EventTriggerType.BeginDrag,delegate{OnBeginDrag(obj);});
            AddEvent(obj,EventTriggerType.EndDrag,delegate{OnEndDrag(obj);});
            AddEvent(obj,EventTriggerType.Drag,delegate{OnDrag(obj);});

            itemsDesplayed.Add(obj,inventory.inventoryList.itemsInventory[i]);
        }
    }

    public void AddEvent(GameObject obj,EventTriggerType type,UnityAction<BaseEventData> action){
        EventTrigger trigger=obj.GetComponent<EventTrigger>();
        var eventTrigger=new EventTrigger.Entry();
        eventTrigger.eventID=type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public Vector3 GetPosition(int i){
            return new Vector3(x_start+(x_space_between_items*(i*nr_column)),y_start+(-y_space_between_items*(i/nr_column)),0f);
    }

    public void OnEnter(GameObject obj){
        mouseItem.hoverObj=obj;
        if(itemsDesplayed.ContainsKey(obj)){
            mouseItem.hoverItem=itemsDesplayed[obj];
        }
    }

    public void OnExit(GameObject obj){
        mouseItem.hoverObj=null;
        mouseItem.hoverItem=null;
    }

    public void OnBeginDrag(GameObject obj){
        var mouseObject=new GameObject();
        //rectTransform
        var rt=mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta=new Vector2(30,30);//dimensiuni sprite
        mouseObject.transform.SetParent(transform.parent);
        //verificam daca este un item in slot
        if(itemsDesplayed[obj].id >= 0){
            var image=mouseObject.AddComponent<Image>();
            image.sprite=inventory.dbObject.getItemForId[itemsDesplayed[obj].id].uiDisplay;
            image.raycastTarget=false;
        }
        mouseItem.obj=mouseObject;
        mouseItem.item=itemsDesplayed[obj];
    }

    public void OnEndDrag(GameObject obj){
        if(mouseItem.hoverObj){
            inventory.MoveItemInventory(itemsDesplayed[obj],itemsDesplayed[mouseItem.hoverObj]);
        }else{
            inventory.RemoveItem(itemsDesplayed[obj].itemObject);
        }
        Destroy(mouseItem.obj);
        mouseItem.item=null;
    }

    public void OnDrag(GameObject obj){
        if(mouseItem.obj!=null){
            mouseItem.obj.GetComponent<RectTransform>().position=Input.mousePosition;
        }
    }

}

public class MouseItem{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}
