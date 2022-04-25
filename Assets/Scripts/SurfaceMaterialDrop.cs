using System;
using System.Linq;
using EventSystem;
using EventSystem.Events;
using TMPro;
using UnityEngine;

public class SurfaceMaterialDrop : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public Material[] materials;

    public void Start()
    {
        var materialNames = materials.Select(x => x.name).ToArray();
        dropdown.options = materials.Select(x => new TMP_Dropdown.OptionData(x.name)).ToList();
        dropdown.value = Array.IndexOf(materialNames, GameState.instance.surfaceMaterial.name);
        dropdown.onValueChanged.AddListener(delegate(int val)
        {
            GameState.instance.surfaceMaterial = materials[val];
            EventManager.TriggerEvent<SurfaceMaterialChangedEvent, Material>(materials[val]);
        });
    }
}