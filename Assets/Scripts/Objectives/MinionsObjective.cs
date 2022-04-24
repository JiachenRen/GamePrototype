using Terrain;
using UnityEngine;

namespace Objectives
{
    public class MinionsObjective : Objective
    {
        private readonly ComputerAgent minion;
        private int minionsKilled;
        
        public override bool objectiveAchieved => minion.quantity == minionsKilled;

        
        public MinionsObjective(string title, string description, string name, ComputerAgent minion) : base(title, description, name)
        {
            this.minion = minion;
        }


        public override string GetStatusText()
        {
            return $"{minion.quantity - minionsKilled}/{minion.quantity} remaining.";
        }

        public override void UpdateObjective()
        {
            minionsKilled++;
        }

        public override void Activate(Planet planet)
        {
            planet.SpawnAgents(minion, () => planet.RandomPositionOnNavMesh(0.3f));
        }
    }
}