using System.Collections.Generic;
using EventSystem;
using EventSystem.Events;
using Terrain;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Objectives
{
    public class ObjectiveTracker : MonoBehaviour
    {
        public Planet planet;

        public TMP_Text objectiveStatusText;

        public TMP_Text objectiveDescText;

        public TMP_Text objectiveTitleText;

        public GameObject victoryCard;

        public GameObject defeatCard;

        public ComputerAgent[] agents;

        private List<Objective> objectives;

        private Objective currentObjective => objectives[0];

        private void Start()
        {
            objectives = new List<Objective>
            {
                new MinionsObjective("Objective", "Eliminate the filthy devils", "MinionsObjective", agents[0]),
                new BossObjective("Objective", "Defeat the boss", "BossObjective", agents[1])
            };

            UpdateObjectiveText();
            EventManager.StartListening<PlanetInitializedEvent>(OnPlanetInitialized);
            EventManager.StartListening<ObjectiveUpdateEvent>(OnObjectiveUpdate);
            EventManager.StartListening<VictoryEvent>(OnVictory);
            EventManager.StartListening<DefeatEvent>(OnDefeat);
        }

        private void OnDestroy()
        {
            EventManager.StopListening<PlanetInitializedEvent>(OnPlanetInitialized);
            EventManager.StopListening<ObjectiveUpdateEvent>(OnObjectiveUpdate);
            EventManager.StopListening<VictoryEvent>(OnVictory);
            EventManager.StopListening<DefeatEvent>(OnDefeat);
        }

        private void OnPlanetInitialized()
        {
            currentObjective.Activate(planet);
        }

        private void OnObjectiveUpdate()
        {
            currentObjective.UpdateObjective();
            if (currentObjective.objectiveAchieved)
            {
                objectives.RemoveAt(0);
                if (objectives.Count > 0)
                {
                    currentObjective.Activate(planet);
                }
            }
            if (objectives.Count == 0)
            {
                // No more objectives
                EventManager.TriggerEvent<VictoryEvent>();
            }
            UpdateObjectiveText();
        }

        private void UpdateObjectiveText()
        {
            objectiveTitleText.text = currentObjective.title;
            objectiveDescText.text = currentObjective.description;
            objectiveStatusText.text = currentObjective.GetStatusText();
        }

        private void OnVictory()
        {
            victoryCard.SetActive(true);
            PauseThenExit();
        }

        private void OnDefeat()
        {
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
}
