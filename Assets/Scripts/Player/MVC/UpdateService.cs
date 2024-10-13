using System;

public class UpdateService
{
    public event Action Updated = delegate { };

    public void OnUpdate()
    {
        Updated.Invoke();
    }
}
