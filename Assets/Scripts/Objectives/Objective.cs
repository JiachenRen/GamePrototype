using Terrain;
using UnityEngine;

namespace Objectives
{
    public abstract class Objective
    {
        public string title;
        public string description;
        public string name;

        public abstract bool objectiveAchieved
        {
            get;
        }

        public Objective(string title, string description, string name)
        {
            this.title = title;
            this.description = description;
            this.name = name;
        }

        public abstract string GetStatusText();

        public abstract void UpdateObjective();

        public abstract void Activate(Planet planet);
    }
}