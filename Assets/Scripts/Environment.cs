using System;
using System.Collections.Generic;
using EventSystem;
using EventSystem.Events;
using UnityEngine;
using UnityEngine.UI;

public class Environment : RenderInEditor
{
    private Lighting lighting;
    public GameObject lightsRoot;
    public GameObject player;

    [SerializeField] public EnvConfig[] envConfigs;

    private Dictionary<Lighting, EnvConfig> configs;
    
    private void Start()
    {
        lighting = GameState.instance.lighting;
        UpdateEnvironment();
        
        EventManager.StartListening<LightingChangedEvent, Lighting>(OnLightingChanged);
    }

    private void OnLightingChanged(Lighting newLighting)
    {
        lighting = newLighting;
        ApplyLighting();
    }

    private void OnDestroy()
    {
        EventManager.StopListening<LightingChangedEvent, Lighting>(OnLightingChanged);
    }

    public void LateUpdate()
    {
        // For now, we just place the light directly above the player's head.
        if (player == null) return;
        var up = player.transform.up;
        lightsRoot.transform.position = player.transform.position + up * 60;
        lightsRoot.transform.rotation = Quaternion.LookRotation(lightsRoot.transform.forward, up);
    }

    protected override void OnEditorRender()
    {
        UpdateEnvironment();
    }

    public void ApplyLighting()
    {
        var config = configs[lighting];

        // First deactivate all lights
        foreach (var c in configs.Values) c.light.gameObject.SetActive(false);

        // Turn on the light corresponding to current lighting specs.
        config.light.gameObject.SetActive(true);
        RenderSettings.skybox = config.skybox;
        RenderSettings.fogColor = config.fogColor;
        RenderSettings.fogEndDistance = config.fogEndDistance;
    }

    // Update is called once per frame
    private void UpdateEnvironment()
    {
        configs = new Dictionary<Lighting, EnvConfig>();
        
        foreach (var config in envConfigs) configs.Add(config.lighting, config);

        ApplyLighting();
    }
}

public enum Lighting
{
    Day,
    Night,
    Dusk
}

[Serializable]
public class EnvConfig
{
    public Lighting lighting;
    public Light light;
    public Color fogColor;
    public float fogEndDistance;
    public Material skybox;
}