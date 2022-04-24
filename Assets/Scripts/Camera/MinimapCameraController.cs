using System.Collections.Generic;
using Terrain;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MinimapCameraController : MonoBehaviour
{
    public GameObject player;
    public Planet planet;
    public RawImage directionIndicator;
    public GameObject enemyIndicatorPrototype;
    public Canvas canvas;
    public GameObject compass;
    public float elevation;
    public List<GameObject> enemyIndicators;

    // Either orient to the north or according to player's orientation
    public bool northUp;

    private void Start()
    {
        enemyIndicators = new List<GameObject>();
        enemyIndicatorPrototype.SetActive(false);
    }

    private void LateUpdate()
    {
        // Update camera
        var playerPos = player.transform.position;
        transform.position = playerPos + player.transform.up * elevation;
        var camUp = northUp ? Vector3.up : player.transform.forward;
        transform.LookAt(playerPos, camUp);

        // Update player direction indicator
        var playerUp = player.transform.up;
        var indicatorAngle = Vector3.SignedAngle(
            Vector3.ProjectOnPlane(player.transform.forward, playerUp),
            Vector3.ProjectOnPlane(transform.up, playerUp),
            playerUp
        );
        directionIndicator.rectTransform.rotation = Quaternion.identity;
        if (northUp)
            directionIndicator.rectTransform.Rotate(new Vector3(0, 0, indicatorAngle));

        // Update compass orientation
        compass.transform.rotation = Quaternion.identity;
        if (!northUp)
        {
            var angle = Vector3.SignedAngle(
                Vector3.ProjectOnPlane(Vector3.up, playerUp),
                Vector3.ProjectOnPlane(player.transform.forward, playerUp),
                playerUp
            );
            compass.transform.Rotate(0, 0, angle);
        }

        // Update closest enemy indicator
        var enemies = planet.GetAgents();

        while (enemyIndicators.Count < enemies.Count)
        {
            var newIndicator = Instantiate(enemyIndicatorPrototype, canvas.transform);
            newIndicator.SetActive(true);
            enemyIndicators.Add(newIndicator);
        }

        while (enemyIndicators.Count > enemies.Count)
        {
            var indicator = enemyIndicators[0];
            enemyIndicators.RemoveAt(0);
            Destroy(indicator);
        }
        
        for (var i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            var indicator = enemyIndicators[i];
            var enemyToPlayer = Vector3.ProjectOnPlane(enemy.transform.position - playerPos, playerUp);
            indicator.transform.rotation = Quaternion.identity;
            var reference = northUp
                ? Vector3.ProjectOnPlane(camUp, playerUp)
                : Vector3.ProjectOnPlane(player.transform.forward, playerUp);
            var closestIndicatorAngle = -Vector3.SignedAngle(reference, enemyToPlayer, playerUp);
            indicator.transform.Rotate(new Vector3(0, 0, closestIndicatorAngle));
        }
    }

    public void ToggleNorthUp()
    {
        northUp = !northUp;
    }
}