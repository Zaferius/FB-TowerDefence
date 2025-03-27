using System;
public interface IHealth
{
    void TakeDamage(int amount);
    float Current { get; }
    float Max { get; }
    
    event Action<float, float> OnHealthChanged;
    event Action OnDeath;
    
    void SetMaxHealth(float value);
}