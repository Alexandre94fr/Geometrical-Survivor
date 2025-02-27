using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyController : MonoBehaviour
{
    [Header(" External references :")]
    [SerializeField] S_Enemy _enemy;
    [SerializeField] Transform _enemyHealthBarTransform;

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
            (_enemyHealthBarTransform, nameof(_enemyHealthBarTransform)),
            (_targetTransform, nameof(_targetTransform))
        )) return;

        S_EnemyAttributes._OnMovementSpeedUpdateEvent += UpdateMovementSpeed;

        _enemyTransform = _enemy.transform;
    }

    void Update()
    {
        _movementDirection = GetMovementDirection(_targetTransform.position);

        UpdateEnemyOrientation(_movementDirection);

        Vector3 positionOffset = _movementSpeed * Time.deltaTime * _movementDirection;

        _enemyTransform.position += positionOffset;
        _enemyHealthBarTransform.position = new (_enemyTransform.position.x, _enemyTransform.position.y + (_enemyTransform.localScale.y / 2));
    }

    void UpdateMovementSpeed(S_Enemy p_enemy, int p_newMovementSpeed)
    {
        if (p_enemy != _enemy)
            return;

        _movementSpeed = p_newMovementSpeed;
    }

    Vector2 GetMovementDirection(Vector3 p_targetPosition)
    {
        return (p_targetPosition - transform.position).normalized;
    }

    void UpdateEnemyOrientation(Vector2 p_newOrientation)
    {
        float rotationAngle = Mathf.Atan2(p_newOrientation.y, p_newOrientation.x) * Mathf.Rad2Deg;

        _enemyTransform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90);
    }
}