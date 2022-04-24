namespace Potion
{
    public class HealingPotion : Potion
    {
        public float healAmount = 30;
        
        public override void Apply(Agent agent)
        {
            agent.currentHealth += healAmount;
        }
    }
}