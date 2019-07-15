namespace Jerre.Weapons
{
    public interface IWeapon
    {
        bool Fire();
        bool Refill(int rounds);
        bool IsSpent();
    }
}
