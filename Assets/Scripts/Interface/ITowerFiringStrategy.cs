public interface ITowerFiringStrategy
{
    void Initialize(TowerData data);
    void Fire(EnemyNavAgent agent);
}