namespace ProjectCleanSword.Scripts.Interfaces;

public interface IDamageable
{
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
    
    void Damage(int amount) { }
    void Death() { }
}