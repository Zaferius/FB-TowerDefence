public interface IAttackHandler
{
    void DoAttack(Tower targetTower);
    void InitializeFromDefinition(EnemyDefinition definition);
}