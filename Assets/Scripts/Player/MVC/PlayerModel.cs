using UnityEngine;

public class PlayerModel
{
    private readonly Health _health;

    private float _speed;

    public PlayerModel()
    {
        _health = new Health();
        _speed = 5;
    }

    public float Speed => _speed;

    public void TakeDamage(int amount)
    {
        _health.TakeDamage(amount);
    }
}
