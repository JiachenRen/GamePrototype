using System;
using Terrain;

namespace Audio
{
    [Serializable]
    public struct AudioSourceInfo
    {
        public AudioActor audioActor;

        public AudioAction audioAction;

        public TerrainType terrain;
        

        public AudioSourceInfo(AudioActor audioActor, AudioAction audioAction, TerrainType terrain)
        {
            this.audioActor = audioActor;
            this.audioAction = audioAction;
            this.terrain = terrain;
        }

        public override string ToString()
        {
            return $"{audioActor} {audioAction} {terrain}";
        }
    }
}