using UnityEngine;
public class Idle : State
{
    public Idle(Flipper flipper, Rigidbody2D rigidbody2D, Animator animator)
    {

    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    //public override void Initialize(Flipper flipper, Rigidbody2D rigidbody2D, Animator animator)
    //{
    //    throw new System.NotImplementedException();
    //}

    public override void OnUpdate() => Stay();

    private void Stay()
    {
    }
}
