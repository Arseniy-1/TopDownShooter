using UnityEngine;

public class CameraFolover : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _stiffness = 0.125f;

    private Vector3 _endPosition;
    private Vector3 _smoothPosition;

    private void Update()
    {
        if (_player != null)
        {
            _endPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
            _smoothPosition = Vector3.Lerp(transform.position, _endPosition, _stiffness);
            transform.position = _smoothPosition;
        }
    }
}
