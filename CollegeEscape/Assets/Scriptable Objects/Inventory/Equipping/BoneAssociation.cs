using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneAssociation
{
    //We have the player who has an armature which has all the bones, we have to add the bottoms of set to our player

    //dictionary of our character bones
    public readonly Dictionary<int, Transform> boneDictionary = new Dictionary<int, Transform>();
    //array that holds bones
    private  readonly Transform[] boneTransforms = new Transform[67];

    private readonly Transform transform;

    public BoneAssociation(GameObject root){
        //root is the player
        this.transform = root.transform;
        TraverseBones(transform);
    }

    public Transform AddLimb(GameObject bonedObj, List<string> boneNames){
        Transform limb = ConfigureBonedObject(bonedObj.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);
        limb.SetParent(transform);
        return limb;
    }

    private Transform ConfigureBonedObject(SkinnedMeshRenderer skinnedMesh, List<string> boneNames){
        //add a new game object into the scene and return its transform
        var bonedObject = new GameObject().transform;

        var meshRenderer = bonedObject.gameObject.AddComponent<SkinnedMeshRenderer>();

        //var bones = skinnedMesh.bones;

        for(int i=0;i<boneNames.Count;i++){
            boneTransforms[i] = boneDictionary[boneNames[i].GetHashCode()];
        }

        meshRenderer.bones = boneTransforms;

        meshRenderer.sharedMesh = skinnedMesh.sharedMesh;
        meshRenderer.materials = skinnedMesh.sharedMaterials;

        return bonedObject;
    }

    private void TraverseBones(Transform trans){
        foreach ( Transform child in trans ){
            boneDictionary.Add(child.name.GetHashCode(), child);
            //make sure that we added all - heavy memory
            TraverseBones(child);
        }
    }


}
