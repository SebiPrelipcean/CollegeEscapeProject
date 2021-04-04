using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
public abstract class UserInterface : MonoBehaviour
{
    public Player player;
    public InventoryObject inventory;


    public Dictionary<GameObject,InventorySlot> itemsDesplayed=new Dictionary<GameObject,InventorySlot>();

    void Start()
    {
        for(int i=0;i<inventory.inventoryList.itemsInventory.Length;i++){
            inventory.inventoryList.itemsInventory[i].parent=this;
        }
        CreateSlots();
        AddEvent(gameObject,EventTriggerType.PointerEnter,delegate{OnEnterInterface(gameObject);});
        AddEvent(gameObject,EventTriggerType.PointerExit,delegate{OnExitInterface(gameObject);});
            
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

    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj,EventTriggerType type,UnityAction<BaseEventData> action){
        EventTrigger trigger=obj.GetComponent<EventTrigger>();
        var eventTrigger=new EventTrigger.Entry();
        eventTrigger.eventID=type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj){
        player.mouseItem.hoverObj=obj;
        if(itemsDesplayed.ContainsKey(obj)){
            player.mouseItem.hoverItem=itemsDesplayed[obj];
        }
    }

    public void OnExit(GameObject obj){
        player.mouseItem.hoverObj=null;
        player.mouseItem.hoverItem=null;
    }

    public void OnEnterInterface(GameObject obj){
        player.mouseItem.ui=obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj){
        player.mouseItem.ui=null;
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
        player.mouseItem.obj=mouseObject;
        player.mouseItem.item=itemsDesplayed[obj];
    }

    public void OnEndDrag(GameObject obj){

        var itemOnMouse=player.mouseItem;
        var mouseHoverItem=itemOnMouse.hoverItem;
        var mouseHoverObj=itemOnMouse.hoverObj;
        var getItemObject=inventory.dbObject.getItemForId;

        if(itemOnMouse.ui!=null){
            if(mouseHoverObj){
                if(mouseHoverItem.CanPlaceInSlot(getItemObject[itemsDesplayed[obj].id]) && (mouseHoverItem.itemObject.id<=-1 || (mouseHoverItem.itemObject.id>=0 && itemsDesplayed[obj].CanPlaceInSlot(getItemObject[mouseHoverItem.itemObject.id]))))
                    inventory.MoveItemInventory(itemsDesplayed[obj], mouseHoverItem.parent.itemsDesplayed[itemOnMouse.hoverObj]);
            }
        }else{
            inventory.RemoveItem(itemsDesplayed[obj].itemObject);
        }
        Destroy(itemOnMouse.obj);
        player.mouseItem.item=null;
    }

    public void OnDrag(GameObject obj){
        if(player.mouseItem.obj!=null){
            player.mouseItem.obj.GetComponent<RectTransform>().position=Input.mousePosition;
        }
    }

}

public class MouseItem{
    public UserInterface ui;
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}

