using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] private PlayerMover _mover;

    private bool _facingRight = true;

    private void FixedUpdate()
    {
        CorrectFlip();
    }

    private void CorrectFlip(float horizontalSpeed)
    {
        if (!_facingRight && _mover.HorizontalSpeed > 0)
            Flip();
        else if (_facingRight && _mover.HorizontalSpeed < 0)
            Flip();
    }

    //убрать зависимость от mover

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}