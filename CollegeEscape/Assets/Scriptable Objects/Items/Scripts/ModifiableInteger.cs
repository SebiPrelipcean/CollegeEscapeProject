using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//callback
public delegate void ModifiedEvent(); 

[System.Serializable]
public class ModifiableInteger
{
    [SerializeField] private int baseValue;
    public int GetBaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }

    [SerializeField] private int modifiedValue;
    public int GetModifiedValue { get { return modifiedValue; } private set { modifiedValue = value;}}

    public List<ModifiersInterface> modifiers = new List<ModifiersInterface>();

    public event ModifiedEvent modifiedEvent;

    public ModifiableInteger(ModifiedEvent method = null){
        modifiedValue = GetBaseValue;
        
        if(method != null){
            modifiedEvent += method;
        }
    }

    public void EnterEvent(ModifiedEvent method){
        modifiedEvent += method;
    }

    public void ExitEvent(ModifiedEvent method){
        modifiedEvent -= method;
    }

    public void UpdateModifiedValue(){
        var addVal = 0;

        for(int i=0;i < modifiers.Count; i++){
            modifiers[i].AddValue(ref addVal);
        }

        GetModifiedValue = baseValue + addVal;

        if(modifiedEvent != null){
            modifiedEvent.Invoke();
        }
    }

    public void AddModifier(ModifiersInterface _modifier){
        modifiers.Add(_modifier);
        UpdateModifiedValue();
    }

    public void RemoveModifier(ModifiersInterface _modifier){
        modifiers.Remove(_modifier);
        UpdateModifiedValue();
    }
}
