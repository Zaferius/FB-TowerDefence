using UnityEngine;
using UnityEngine.AI;

public class RunnerBehavior : IEnemyBehavior
{
    private readonly NavMeshAgent _agent;
    private readonly Transform _target;
    private readonly Transform _self;

    public RunnerBehavior(NavMeshAgent agent, Transform target, Transform self)
    {
        _agent = agent;
        _target = target;
        _self = self;
    }

    public void Tick()
    {
        if (_agent.remainingDistance < 0.5f)
        {
            Object.Destroy(_self.gameObject);
        }
    }
}