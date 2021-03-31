using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject inventoryPrefab;

    public int x_start;
    public int y_start;
    public int x_space_between_items;
    public int y_space_between_items;
    public int nr_column;

    Dictionary<InventorySlot,GameObject> itemsDesplayed=new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay(){
        for(int i=0;i<inventory.inventoryList.itemsInventory.Count;i++){
            InventorySlot slot=inventory.inventoryList.itemsInventory[i];

            var obj=Instantiate(inventoryPrefab,Vector3.zero,Quaternion.identity,transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite=inventory.dbObject.getItemForId[slot.itemObject.id].uiDisplay;
            obj.GetComponent<RectTransform>().localPosition=GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text=slot.amount.ToString("n0");
            itemsDesplayed.Add(slot,obj);
        }
    }

    public void UpdateDisplay(){
        for(int i=0;i<inventory.inventoryList.itemsInventory.Count;i++){

            InventorySlot slot=inventory.inventoryList.itemsInventory[i];

            if(itemsDesplayed.ContainsKey(slot)){
                itemsDesplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text=slot.amount.ToString("n0");

            }else{
                var obj=Instantiate(inventoryPrefab,Vector3.zero,Quaternion.identity,transform);
                
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite=inventory.dbObject.getItemForId[slot.itemObject.id].uiDisplay;
                obj.GetComponent<RectTransform>().localPosition=GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text=slot.amount.ToString("n0");
                itemsDesplayed.Add(slot,obj);
            }
        }
    }

    public Vector3 GetPosition(int i){
            return new Vector3(x_start+(x_space_between_items*(i*nr_column)),y_start+(-y_space_between_items*(i/nr_column)),0f);
    }
}
