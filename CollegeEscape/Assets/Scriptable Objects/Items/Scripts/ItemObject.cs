using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategories{
    FOOD,
    HAT,
    WEAPON,
    SHIRT,
    PANTS,
    SHOES,
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
    public Sprite uiDisplay;
    public ItemCategories category;
    [TextArea(15,20)]
    public string description;

    public bool stackable;
    public Item data=new Item();

    public Item createItem(){
        Item newItem=new Item(this);
        return newItem; 
    }
}

[System.Serializable]
public class Item{
    public int id = -1;
    public string name;
    public ItemBuff[] itemBuffs;

    public Item(){
        this.id=-1;
        this.name="";  
    }

    public Item(ItemObject itemObject){
        this.id=itemObject.data.id;
        this.name=itemObject.name;
        this.itemBuffs=new ItemBuff[itemObject.data.itemBuffs.Length];
        for(int i=0;i<itemBuffs.Length;i++){     
            itemBuffs[i]=new ItemBuff(itemObject.data.itemBuffs[i].minVal,itemObject.data.itemBuffs[i].maxVal);
            itemBuffs[i].attribute=itemObject.data.itemBuffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff : ModifiersInterface{
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

    public void AddValue(ref int _value){
        _value += value;
    }
}
