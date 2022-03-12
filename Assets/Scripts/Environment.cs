using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Lighting lighting = Lighting.Day;
    public GameObject lightsRoot;
    public GameObject player;

    [SerializeField]
    public EnvConfig[] envConfigs;

    private Dictionary<Lighting, EnvConfig> configs;

    private void OnValidate()
    {
        UpdateEnvironment();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateEnvironment();
    }

    public void LateUpdate()
    {
        lightsRoot.transform.position = player.transform.position + player.transform.up * 60;
        lightsRoot.transform.rotation = Quaternion.LookRotation(lightsRoot.transform.forward, player.transform.up);
    }

    private void ApplyLighting()
    {
        var config = configs[lighting];

        // First deactivate all lights
        foreach (var c in configs.Values)
        {
            c.light.gameObject.SetActive(false);
        }

        // Turn on the light corresponding to current lighting specs.
        config.light.gameObject.SetActive(true);
        RenderSettings.skybox = config.skybox;
        RenderSettings.fogColor = config.fogColor;
        RenderSettings.fogEndDistance = config.fogEndDistance;
    }

    // Update is called once per frame
    void UpdateEnvironment()
    {
        configs = new Dictionary<Lighting, EnvConfig>();
        foreach (var config in envConfigs)
        {
            configs.Add(config.lighting, config);
        }

        ApplyLighting();
    }
}

public enum Lighting
{
    Day, Night, Dusk
}

[System.Serializable]
public class EnvConfig
{
    public Lighting lighting;
    public Light light;
    public Color fogColor;
    public float fogEndDistance;
    public Material skybox;
}