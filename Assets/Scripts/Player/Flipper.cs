using UnityEngine;

public class Flipper : MonoBehaviour
{
    private bool _facingRight = true;

    public void CorrectFlip(float horizontalSpeed)
    {
      if (!_facingRight && horizontalSpeed > 0)
          Flip();
      else if (_facingRight && horizontalSpeed < 0)
          Flip();
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}