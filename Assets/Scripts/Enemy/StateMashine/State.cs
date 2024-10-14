using UnityEngine;

public abstract class State
{
    public abstract void OnUpdate();

    public abstract void Enter();

    public abstract void Exit();
}
