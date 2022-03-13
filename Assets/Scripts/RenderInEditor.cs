using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RenderInEditor : MonoBehaviour
{
    public bool renderingEnabled = true;
    
    [Tooltip("Names of children that are generated and should be removed at each editor update.")]
    public String[] generatedChildren;

    private void Awake()
    {
        DestroyGeneratedObjects();
    }

    private void OnValidate()
    {
        if (!renderingEnabled || Application.isPlaying) return;
        
        EditorApplication.delayCall += () =>
        {
            if (Application.isPlaying) return;
            if (gameObject != null && gameObject.activeInHierarchy)
            {
                DestroyGeneratedObjects();
                OnEditorRender();
            }
        };
    }

    protected void DestroyGeneratedObjects()
    {
        foreach (var obj in FindObjectsToDestroy())
        {
            DestroyImmediate(obj);
        }
    }

    protected List<GameObject> FindObjectsToDestroy()
    {
        var objectsToDestroy = new List<GameObject>();
        foreach (Transform t in transform)
        {
            if (generatedChildren.Contains(t.gameObject.name))
            {
                objectsToDestroy.Add(t.gameObject);
            }
        }

        return objectsToDestroy;
    }

    protected virtual void OnEditorRender()
    {
        
    }
}