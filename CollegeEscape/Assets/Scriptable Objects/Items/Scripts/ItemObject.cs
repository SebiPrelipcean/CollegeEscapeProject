using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategories{
    FOOD,
    EQUIPMENT,
    DEFAULT
}

public enum Attributes{
    STAMINA,
    STRENGTH,
    AGILITY,
    INTELLECT
}
public abstract class ItemObject : ScriptableObject
{
    public int id;
    public Sprite uiDisplay;
    public ItemCategories category;
    [TextArea(15,20)]
    public string description;
    public ItemBuff[] itemBuffs;

    public Item createItem(){
        Item newItem=new Item(this);
        return newItem; 
    }
}

[System.Serializable]
public class Item{
    public int id;
    public string name;
    public ItemBuff[] itemBuffs;

    public Item(ItemObject itemObject){
        this.id=itemObject.id;
        this.name=itemObject.name;
        this.itemBuffs=new ItemBuff[itemObject.itemBuffs.Length];
        for(int i=0;i<itemBuffs.Length;i++){     
            itemBuffs[i]=new ItemBuff(itemObject.itemBuffs[i].minVal,itemObject.itemBuffs[i].maxVal);
            itemBuffs[i].attribute=itemObject.itemBuffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff{
    public Attributes attribute;
    public int minVal;
    public int maxVal;

    public int value;

    public ItemBuff(int minVal,int maxVal){
        this.minVal=minVal;
        this.maxVal=maxVal;
        generateValue();
    }

    public void generateValue(){
        //UnityEngine deoarece folosim System.Serializable
        value=UnityEngine.Random.Range(minVal,maxVal);
    }
}
