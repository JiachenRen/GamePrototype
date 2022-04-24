namespace Potion
{
    public class PowerPotion : Potion
    {
        public float damageIncrease = 1;
        
        public override void Apply(Agent agent)
        {
            damageIncrease += 1;
        }
    }
}