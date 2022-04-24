using System;
using EventSystem;
using EventSystem.Events;
using Terrain;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ObjectiveTracker : MonoBehaviour
{
    public Planet planet;

    public TMP_Text objectiveText;

    public GameObject victoryCard;

    public GameObject defeatCard;

    private int enemiesToKill;

    private int enemiesKilled;

    private void Start()
    {
        foreach (var agent in planet.agentPrototypes)
        {
            var computerAgent = agent.GetComponent<ComputerAgent>();
            enemiesToKill += computerAgent.quantity;
        }
        
        UpdateObjectiveText();
        EventManager.StartListening<AgentDeathEvent, ComputerAgent>(OnAgentDeath);
        EventManager.StartListening<VictoryEvent>(OnVictory);
        EventManager.StartListening<DefeatEvent>(OnDefeat);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<AgentDeathEvent, ComputerAgent>(OnAgentDeath);
        EventManager.StopListening<VictoryEvent>(OnVictory);
        EventManager.StopListening<DefeatEvent>(OnDefeat);
    }

    private void OnAgentDeath(ComputerAgent agent)
    {
        enemiesKilled++;
        UpdateObjectiveText();
        if (enemiesKilled == enemiesToKill)
        {
            EventManager.TriggerEvent<VictoryEvent>();
        }
    }

    private void UpdateObjectiveText()
    {
        objectiveText.text = $"{enemiesToKill - enemiesKilled}/{enemiesToKill} remaining.";
    }

    private void OnVictory()
    {
        victoryCard.SetActive(true);
        PauseThenExit();
    }

    private void OnDefeat()
    {
        Debug.Log("Defeat");
        defeatCard.SetActive(true);
        PauseThenExit();
    }

    private void PauseThenExit()
    {
        GameState.Pause();
        Invoke(nameof(ExitToMainMenu), 2);
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
