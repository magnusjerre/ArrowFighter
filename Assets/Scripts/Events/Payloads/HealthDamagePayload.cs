namespace Jerre.Events
{
    public struct HealthDamagePayload
    {
        public int DamagedPlayerNumber;
        public int Damage;
        public int HealthLeft;

        public HealthDamagePayload(int damagePlayerNumber, int damage, int healthLeft)
        {
            DamagedPlayerNumber = damagePlayerNumber;
            Damage = damage;
            HealthLeft = healthLeft;
        }
    }
}
