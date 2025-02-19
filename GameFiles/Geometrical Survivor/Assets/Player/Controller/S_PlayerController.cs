using System;
using UnityEngine;

public class S_PlayerController : MonoBehaviour
{
    public static Action<Vector2> _OnPlayerMoveInputEvent;

    [SerializeField] Transform _playerTransform;
    //[SerializeField] S_PlayerProperties _playerProperties;

    // TEMP: Will be replaced by _playerProperties
    [SerializeField] float _movementSpeed = 5.0f;
    
    Vector2 _movementDirection;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_playerTransform, nameof(_playerTransform))
        )) return;

        _OnPlayerMoveInputEvent += UpdateMovementDirection;
    }

    void Update()
    {
        Vector3 positionOffset = _movementSpeed * Time.deltaTime * _movementDirection;

        _playerTransform.position += positionOffset;
    }

    void UpdateMovementDirection(Vector2 p_newDirection)
    {
        _movementDirection = p_newDirection;
    }
}