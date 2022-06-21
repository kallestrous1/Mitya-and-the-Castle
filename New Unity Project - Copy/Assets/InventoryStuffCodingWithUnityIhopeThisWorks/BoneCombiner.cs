using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCombiner 
{
    public readonly Dictionary<int, Transform> RootBoneDictionary = new Dictionary<int, Transform>();
    private readonly Transform[] boneTransforms = new Transform[14];

    private readonly Transform playerTransform;

    public BoneCombiner(GameObject rootObj)
    {
        playerTransform = rootObj.transform;
        TraverseHierarchy(playerTransform);
    }

    public Transform AddLimb(GameObject bonedObj)
    {
        Transform limb= ProcessBonedObject(bonedObj.GetComponentInChildren<SkinnedMeshRenderer>());
        limb.SetParent(playerTransform);
        return playerTransform;
    }

    private Transform ProcessBonedObject(SkinnedMeshRenderer renderer)
    {
        var bonedObjectTransform = new GameObject().transform;  
        var meshRenderer = bonedObjectTransform.gameObject.AddComponent<SkinnedMeshRenderer>();
        var bones = renderer.bones;

        for (int i = 0; i < bones.Length; i++)
        {
            boneTransforms[i] = RootBoneDictionary[bones[i].name.GetHashCode()];
        }

        meshRenderer.bones = boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.sharedMaterial = renderer.sharedMaterial;

        return bonedObjectTransform; 
    }

    private void TraverseHierarchy(Transform transform)
    {
        foreach (Transform child in transform)
        {
            RootBoneDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierarchy(child);
        }
    }
}
