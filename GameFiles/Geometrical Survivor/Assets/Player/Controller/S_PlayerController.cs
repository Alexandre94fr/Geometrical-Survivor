using System;
using UnityEngine;

public class S_PlayerController : MonoBehaviour
{
    public static Action<Vector2> _OnPlayerMoveInputEvent;
    public static Action<Vector2> _OnPlayerRotateEvent;

    [Header(" External references :")]
    [SerializeField] Transform _playerCameraTransform;
    [SerializeField] Transform _playerTransform;
    //[SerializeField] S_PlayerProperties _playerProperties;

    // TEMP: Will be replaced by _playerProperties
    [Header(" TEMP :")]
    [SerializeField] float _movementSpeed = 5.0f;
    
    Vector2 _movementDirection;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_playerTransform, nameof(_playerTransform)),
            (_playerCameraTransform, nameof(_playerCameraTransform))
        )) return;

        _OnPlayerMoveInputEvent += UpdateMovementDirection;
        _OnPlayerRotateEvent += UpdatePlayerOrientation;
    }

    void Update()
    {
        Vector3 positionOffset = _movementSpeed * Time.deltaTime * _movementDirection;

        _playerTransform.position += positionOffset;
        _playerCameraTransform.position = new(_playerTransform.position.x, _playerTransform.position.y, -10);
    }

    void UpdateMovementDirection(Vector2 p_newDirection)
    {
        _movementDirection = p_newDirection;
    }

    void UpdatePlayerOrientation(Vector2 p_newOrientation)
    {
        float rotationAngle = Mathf.Atan2(p_newOrientation.y, p_newOrientation.x) * Mathf.Rad2Deg;

        _playerTransform.rotation = Quaternion.Euler(0, 0, rotationAngle - 90);
    }
}