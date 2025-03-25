public interface IHealth
{
    void TakeDamage(int amount);
    float Current { get; }
    float Max { get; }
    
    event System.Action<float, float> OnHealthChanged;
}