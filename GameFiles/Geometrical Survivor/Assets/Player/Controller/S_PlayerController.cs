using System;
using System.Collections;
using UnityEngine;

public class S_PlayerController : MonoBehaviour
{
    public static Action<Vector2> _OnPlayerMoveInputEvent;
    public static Action<Vector2> _OnPlayerRotateEvent;
    public static Action _OnActiveCapacityUseEvent;
    public static Action _OnActiveCapacityUnUseEvent;

    [Header(" External references :")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _playerCameraTransform;

    [Space]
    [SerializeField] S_ActiveCapacityLauncher _playerActiveCapacityLauncher;

    [Space]
    [SerializeField] S_PlayerProperties _playerStatistics;
    [SerializeField] S_PlayerAttributes _playerAttributes;

    bool _doesPlayerHoldingActiveCapacityLaunchingKey;
    int _movementSpeed;
    Vector2 _movementDirection;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_playerTransform, nameof(_playerTransform)),
            (_playerCameraTransform, nameof(_playerCameraTransform)),

            (_playerActiveCapacityLauncher, nameof(_playerActiveCapacityLauncher)),

            (_playerStatistics, nameof(_playerStatistics)),
            (_playerAttributes, nameof(_playerAttributes))
        )) return;

        _movementSpeed = _playerStatistics._MovementSpeed;

        S_PlayerAttributes._OnMovementSpeedUpdateEvent += UpdateMovementSpeed;

        _OnPlayerMoveInputEvent += UpdateMovementDirection;
        _OnPlayerRotateEvent += UpdatePlayerOrientation;
        _OnActiveCapacityUseEvent += StartTryLaunchActiveCapacity;
        _OnActiveCapacityUnUseEvent += StopTryLaunchActiveCapacity;
    }

    void Update()
    {
        Vector3 positionOffset = _movementSpeed * Time.deltaTime * _movementDirection;

        _playerTransform.position += positionOffset;
        _playerCameraTransform.position = new(_playerTransform.position.x, _playerTransform.position.y, -10);
    }

    void UpdateMovementSpeed(int p_newMovementSpeed)
    {
        _movementSpeed = p_newMovementSpeed;
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

    void StartTryLaunchActiveCapacity()
    {
        _doesPlayerHoldingActiveCapacityLaunchingKey = true;

        StartCoroutine(TryLaunchActiveCapactity());
    }

    void StopTryLaunchActiveCapacity()
    {
        _doesPlayerHoldingActiveCapacityLaunchingKey = false;
    }

    IEnumerator TryLaunchActiveCapactity()
    {
        while (_doesPlayerHoldingActiveCapacityLaunchingKey)
        {
            _playerActiveCapacityLauncher.TryLaunchActiveCapacity(_playerAttributes._EquippedActiveCapacity._ActiveCapacityProperties);

            yield return new WaitForEndOfFrame();
        }
    }
}