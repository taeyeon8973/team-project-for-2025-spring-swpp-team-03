using UnityEngine;

public interface IMovementStrategy
{
    void Initialize(EnemyMoving enemy, Vector3 startPosition);
    void Move(EnemyMoving enemy, Rigidbody rb);
    string GetStrategyName();
} 