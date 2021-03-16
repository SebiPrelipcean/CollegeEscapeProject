using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

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
        for(int i=0;i<inventory.inventoryList.Count;i++){
            var obj=Instantiate(inventory.inventoryList[i].itemObject.prefab,Vector3.zero,Quaternion.identity,transform);
            obj.GetComponent<RectTransform>().localPosition=GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text=inventory.inventoryList[i].amount.ToString("n0");
            itemsDesplayed.Add(inventory.inventoryList[i],obj);
        }
    }

    public void UpdateDisplay(){
        for(int i=0;i<inventory.inventoryList.Count;i++){
            if(itemsDesplayed.ContainsKey(inventory.inventoryList[i])){
                itemsDesplayed[inventory.inventoryList[i]].GetComponentInChildren<TextMeshProUGUI>().text=inventory.inventoryList[i].amount.ToString("n0");

            }else{
                var obj=Instantiate(inventory.inventoryList[i].itemObject.prefab,Vector3.zero,Quaternion.identity,transform);
                obj.GetComponent<RectTransform>().localPosition=GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text=inventory.inventoryList[i].amount.ToString("n0");
                itemsDesplayed.Add(inventory.inventoryList[i],obj);
            }
        }
    }

    public Vector3 GetPosition(int i){
            return new Vector3(x_start+(x_space_between_items*(i*nr_column)),y_start+(-y_space_between_items*(i/nr_column)),0f);
    }
}
