using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;


    public Dictionary<GameObject,InventorySlot> itemsDesplayed=new Dictionary<GameObject,InventorySlot>();

    void Start()
    {
        for(int i=0;i<inventory.GetSlots.Length;i++){
            inventory.GetSlots[i].parent=this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject,EventTriggerType.PointerEnter,delegate{OnEnterInterface(gameObject);});
        AddEvent(gameObject,EventTriggerType.PointerExit,delegate{OnExitInterface(gameObject);});
            
    }

    private void OnSlotUpdate(InventorySlot slot){

        if(slot.item.id>=0){
                slot.slotDisplayed.transform.GetChild(0).GetComponentInChildren<Image>().sprite=slot.ItemObject.uiDisplay;
                slot.slotDisplayed.transform.GetChild(0).GetComponentInChildren<Image>().color=new Color(1,1,1,1);
                slot.slotDisplayed.GetComponentInChildren<TextMeshProUGUI>().text=(slot.amount==1) ? "" : slot.amount.ToString("n0");
            }else{
                slot.slotDisplayed.transform.GetChild(0).GetComponentInChildren<Image>().sprite=null;
                slot.slotDisplayed.transform.GetChild(0).GetComponentInChildren<Image>().color=new Color(1,1,1,1);
                slot.slotDisplayed.GetComponentInChildren<TextMeshProUGUI>().text="";
            }

    }

    /*void Update()
    {
        itemsDesplayed.UpdateSlotDisplay();
    }*/

    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj,EventTriggerType type,UnityAction<BaseEventData> action){
        EventTrigger trigger=obj.GetComponent<EventTrigger>();
        var eventTrigger=new EventTrigger.Entry();
        eventTrigger.eventID=type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj){
        MouseInfo.slotHoverOver=obj;
    }

    public void OnExit(GameObject obj){
        MouseInfo.slotHoverOver=null;
    }

    public void OnEnterInterface(GameObject obj){
        MouseInfo.interfaceMouseIsOver=obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj){
        MouseInfo.interfaceMouseIsOver=null;
    }

    public void OnBeginDrag(GameObject obj){
        MouseInfo.draggedTempItem=CreateTempItem(obj);
    }

    

    public GameObject CreateTempItem(GameObject obj){

        GameObject result = null;
        if(itemsDesplayed[obj].item.id >= 0){
            result = new GameObject();
            //rectTransform
            var rt = result.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(30,30);//dimensiuni sprite
            result.transform.SetParent(transform.parent);

            var image = result.AddComponent<Image>();
            image.sprite = itemsDesplayed[obj].ItemObject.uiDisplay;
            image.raycastTarget = false;
        }

        return result;
    }

    public void OnEndDrag(GameObject obj){


         Destroy(MouseInfo.draggedTempItem);

         if(MouseInfo.interfaceMouseIsOver == null){
             itemsDesplayed[obj].RemoveItem();
             return;
         }

         if(MouseInfo.slotHoverOver){
             InventorySlot mouseHoverSlotData=MouseInfo.interfaceMouseIsOver.itemsDesplayed[MouseInfo.slotHoverOver];
             inventory.SwapItemsInventory(itemsDesplayed[obj],mouseHoverSlotData);
         }

    }

    public void OnDrag(GameObject obj){
        if(MouseInfo.draggedTempItem!=null){
            MouseInfo.draggedTempItem.GetComponent<RectTransform>().position=Input.mousePosition;
        }
    }



}

public static class MouseInfo{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject draggedTempItem;
    public static GameObject slotHoverOver;
}

//Extension method for our dictionary because we want to make a class for that specific dictionary
public static class ExtensionMethods{
    public static void UpdateSlotDisplay(this Dictionary<GameObject,InventorySlot> _itemsDesplayed){

        //we need to make a callback for updating the slot display, so instead of doing a foreach loop for every single slot, we'll wait 
        //for a callback to fire saying the slot was updated and then we will run that code
        foreach(KeyValuePair<GameObject,InventorySlot> slot in _itemsDesplayed){
            
            //If the current slot represents an item
            if(slot.Value.item.id>=0){
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite=slot.Value.ItemObject.uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color=new Color(1,1,1,1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text=(slot.Value.amount==1) ? "" : slot.Value.amount.ToString("n0");
            }else{
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite=null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color=new Color(1,1,1,1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text="";
            }
        }
    }
}
