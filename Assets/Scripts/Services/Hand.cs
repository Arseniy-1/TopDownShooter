using UnityEngine;

public class Hand : MonoBehaviour
{
    public void SpotTarget(Vector3 targetPosition)
    {
        var direction = targetPosition - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
