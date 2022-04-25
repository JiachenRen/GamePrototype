using System;
using System.Linq;
using EventSystem;
using EventSystem.Events;
using TMPro;
using UnityEngine;

public class LightingDrop : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private readonly Lighting[] lightingConfigs =
    {
        Lighting.Day, Lighting.Night, Lighting.Dusk
    };

    public void Start()
    {
        dropdown.options = lightingConfigs.Select(x => new TMP_Dropdown.OptionData(x.ToString())).ToList();
        dropdown.value = Array.IndexOf(lightingConfigs, GameState.instance.lighting);
        dropdown.onValueChanged.AddListener(delegate(int val)
        {
            GameState.instance.lighting = lightingConfigs[val];
            EventManager.TriggerEvent<LightingChangedEvent, Lighting>(lightingConfigs[val]);
        });
    }
}