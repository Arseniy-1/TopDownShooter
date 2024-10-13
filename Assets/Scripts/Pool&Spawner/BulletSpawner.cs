using UnityEngine;

public abstract class BulletSpawner<T> : MonoBehaviour where T : Ammo
{
    [SerializeField] protected ObjectPool<T> Pool;

    public void Spawn(Transform transform)
    {
        CollectableObject obj = Pool.Get();
        obj.OnDestroyed += PlaceInPool;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.Activate();
    }

    public virtual void PlaceInPool(CollectableObject collectableObject)
    {
        collectableObject.OnDestroyed -= PlaceInPool;
        Pool.Release((T)collectableObject);
    }
}

public class EnemySpawner
{

}

