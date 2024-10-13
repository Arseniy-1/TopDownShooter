using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }

    private void Update()
    {
        HorizontalDirection = Input.GetAxis(Horizontal);
        VerticalDirection = Input.GetAxis(Vertical);
    }
}
