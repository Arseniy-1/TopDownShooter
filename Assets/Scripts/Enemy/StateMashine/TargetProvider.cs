public class TargetProvider
{
    public ITarget Target { get; private set; }

    public void SetTarget(ITarget target)
    {
        Target = target;
    }
}
