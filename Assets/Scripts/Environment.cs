using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Environment : RenderInEditor
{
    public Lighting lighting = Lighting.Day;
    public GameObject lightsRoot;
    public GameObject player;

    public TMPro.TMP_Dropdown myDrop;

    [SerializeField] public EnvConfig[] envConfigs;

    private Dictionary<Lighting, EnvConfig> configs;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateEnvironment();
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
        if(myDrop.value == 0) lighting = Lighting.Day;
        else if (myDrop.value == 1) lighting = Lighting.Night;
        else if (myDrop.value == 2) lighting = Lighting.Dusk;

        Debug.Log("Applying lighting");
        
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