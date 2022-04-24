using Terrain;

namespace Objectives
{
    public class BossObjective : Objective
    {
        private bool bossKilled;
        private ComputerAgent boss;


        public BossObjective(string title, string description, string name, ComputerAgent boss) : base(title, description, name)
        {
            this.boss = boss;
        }

        public override bool objectiveAchieved => bossKilled;


        public override string GetStatusText()
        {
            var enemies = bossKilled ? 1 : 0;
            return $"{enemies} remaining";
        }

        public override void UpdateObjective()
        {
            bossKilled = true;
        }

        public override void Activate(Planet planet)
        {
            planet.SpawnAgents(boss, () => planet.RandomPositionOnNavMesh(0.5f));
        }
    }
}