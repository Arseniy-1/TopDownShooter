using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetScaner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 150f;
    [SerializeField] private LayerMask _targetLayer;

    public List<ITarget> Scan()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _scanRadius);
        HashSet<ITarget> targets = new HashSet<ITarget>();

        foreach (Collider2D hit in hits)
            if (hit.TryGetComponent(out ITarget target) && (_targetLayer & (1 << hit.gameObject.layer)) != 0)
                targets.Add(target);

        return targets.ToList();
    }
}
