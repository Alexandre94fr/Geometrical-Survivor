using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyController : MonoBehaviour
{
    [Header(" External references :")]
    [SerializeField] S_Enemy _enemy;

    [Space]
    [SerializeField] Transform _targetTransform; 
    // Because there is only one target possible (the player) we don't need to get it at runtime


    Transform _enemyTransform;
    int _movementSpeed;

    Vector2 _movementDirection;


    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_enemy, nameof(_enemy)),
            (_targetTransform, nameof(_targetTransform))
        )) return;

        S_EnemyAttributes._OnMovementSpeedUpdateEvent += UpdateMovementSpeed;

        _enemyTransform = _enemy.transform;
    }

    void Update()
    {
        _movementDirection = GetMovementDirection(_targetTransform.position);

        Vector3 positionOffset = _movementSpeed * Time.deltaTime * _movementDirection;

        _enemyTransform.position += positionOffset;
    }

    void UpdateMovementSpeed(S_Enemy p_enemy, int p_newMovementSpeed)
    {
        if (p_enemy != _enemy)
            return;

        _movementSpeed = p_newMovementSpeed;
    }

    void UpdateTarget(S_Enemy p_enemy, Transform p_newTarget)
    {
        if (p_enemy != _enemy)
            return;

        _targetTransform = p_newTarget;
    }

    Vector2 GetMovementDirection(Vector3 p_targetPosition)
    {
        return (p_targetPosition - transform.position).normalized;
    }
}