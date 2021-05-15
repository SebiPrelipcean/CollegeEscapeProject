 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject itemObject;

    public void OnBeforeSerialize(){
#if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite=itemObject.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }

    public void OnAfterDeserialize(){

    }
}
