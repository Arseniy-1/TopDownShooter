using UnityEngine;

public class Attacker : State
{
    private TargetProvider _targetProvider;

    public Attacker(Flipper flipper, Animator animator, TargetProvider targetProvider)
    {
        _targetProvider = targetProvider;
    }

    public override void Enter()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void Exit()
    {
    }

}
