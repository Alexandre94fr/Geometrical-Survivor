using System;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class S_EnemyAttributes : MonoBehaviour
{
    #region -= Events =-

    // Basic
    public static event Action<S_Enemy, string> _OnEnemyNameUpdateEvent;
    public static event Action<S_Enemy, Sprite> _OnEnemySpriteUpdateEvent;

    // Movement
    public static event Action<S_Enemy, int> _OnMovementSpeedUpdateEvent;

    // Combat
    public static event Action<S_Enemy, int> _OnMaxHealthPointsUpdateEvent;
    public static event Action<S_Enemy, int> _OnHealthPointsUpdateEvent;

    public static event Action<S_Enemy, int> _OnAttackDamageUpdateEvent;
    public static event Action<S_Enemy, float> _OnAttackCooldownTimeUpdateEvent;

    // Experience
    public static event Action<S_Enemy, int> _OnNanomachinesDroppedWhenKilledUpdateEvent;
    #endregion

    #region -= Getters / Setters =-

    #region - Basic -

    public string _Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_Name)}' to null or '' or ' '. The variable's value has NOT been changed.");
                return;
            }

            _name = value;

            _OnEnemyNameUpdateEvent?.Invoke(_enemy, _name);
        }
    }

    public Sprite _Sprite
    {
        get { return _sprite; }
        set
        {
            if (value == null)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_Sprite)}' to null. The variable's value has NOT been changed.");
                return;
            }

            _sprite = value;

            _OnEnemySpriteUpdateEvent?.Invoke(_enemy, _sprite);
        }
    }
    #endregion

    #region - Movement -

    public int _MovementSpeed
    {
        get { return _movementSpeed; }
        set
        {
            if (value < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_MovementSpeed)}' under 0. The variable's value has NOT been changed.");
                return;
            }

            _movementSpeed = value;

            _OnMovementSpeedUpdateEvent?.Invoke(_enemy, _movementSpeed);
        }
    }
    #endregion

    #region - Combat -

    #region Health points

    public int _MaxHealthPoints
    {
        get { return _maxHealthPoints; }
        set
        {
            if (value < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_MaxHealthPoints)}' under 0. The variable's value has NOT been changed.");
                return;
            }

            // To avoid having more HealthPoints than the MaxHealthPoints we set HealthPoints at MaxHealthPoints
            if (value < _HealthPoints)
                _HealthPoints = value;

            _maxHealthPoints = value;

            _OnMaxHealthPointsUpdateEvent?.Invoke(_enemy, _maxHealthPoints);
        }
    }

    public int _HealthPoints
    {
        get { return _healthPoints; }
        set
        {
            _healthPoints = value;

            if (_healthPoints <= 0)
            {
                _healthPoints = 0;
            }

            if (_healthPoints > _MaxHealthPoints)
                _healthPoints = _MaxHealthPoints;

            _OnHealthPointsUpdateEvent?.Invoke(_enemy, _healthPoints);
        }
    }
    #endregion

    #region Attack

    public int _AttackDamage
    {
        get { return _attackDamage; }
        set
        {
            if (value < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_AttackDamage)}' under 0. The variable's value has NOT been changed.");
                return;
            }

            _attackDamage = value;

            _OnAttackDamageUpdateEvent?.Invoke(_enemy, _attackDamage);
        }
    }

    public float _AttackCooldownTime
    {
        get { return _attackCooldownTime; }
        set
        {
            if (value < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_AttackCooldownTime)}' under 0. The variable's value has NOT been changed.");
                return;
            }

            _attackCooldownTime = value;

            _OnAttackCooldownTimeUpdateEvent?.Invoke(_enemy, _attackCooldownTime);
        }
    }
    #endregion

    #endregion

    #region - Experience -

    public int _NanomachinesDroppedWhenKilled
    {
        get { return _nanomachinesDroppedWhenKilled; }
        private set
        {
            _nanomachinesDroppedWhenKilled = value;

            if (_nanomachinesDroppedWhenKilled < 0)
            {
                Debug.LogError($"ERROR ! Someone tryied to set '{nameof(_NanomachinesDroppedWhenKilled)}' under 0. The variable's value has been set to 0.");
                _nanomachinesDroppedWhenKilled = 0;
            }

            _OnNanomachinesDroppedWhenKilledUpdateEvent?.Invoke(_enemy, _nanomachinesDroppedWhenKilled);
        }
    }
    #endregion

    #endregion

    [Header(" External references :")]
    [SerializeField] S_Enemy _enemy;
    [Space]

    #region -= Attributes - Private variables =-

    [Header(" Basic :")]
    [ReadOnlyInInspector] [SerializeField] string _name;
    [ReadOnlyInInspector] [SerializeField] Sprite _sprite;

    [Header(" Movement :")]
    [ReadOnlyInInspector] [SerializeField] int _movementSpeed;

    [Header(" Combat :")]
    [ReadOnlyInInspector] [SerializeField] int _maxHealthPoints;
    [ReadOnlyInInspector] [SerializeField] int _healthPoints;

    [Space]
    [ReadOnlyInInspector] [SerializeField] int _attackDamage;
    [ReadOnlyInInspector] [SerializeField] float _attackCooldownTime;

    [Header(" Experience :")]
    [ReadOnlyInInspector] [SerializeField] int _nanomachinesDroppedWhenKilled;
    #endregion

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_enemy, nameof(_enemy))
        )) return;

        InitializingPlayerAttributes(_enemy._EnemyStatistics);
    }

    void InitializingPlayerAttributes(S_EnemyProperties p_enemyProperties)
    {
        // Basic
        _Name = p_enemyProperties._EnemyProperties._Name;
        _Sprite = p_enemyProperties._EnemyProperties._Sprite;

        // Movement
        _MovementSpeed = p_enemyProperties._EnemyProperties._MovementSpeed;

        // Combat
        _MaxHealthPoints = p_enemyProperties._EnemyProperties._MaxHealthPoints;
        _HealthPoints = _MaxHealthPoints;

        _AttackDamage = p_enemyProperties._EnemyProperties._AttackDamage;
        _AttackCooldownTime = p_enemyProperties._EnemyProperties._AttackCooldownTime;

        // Experience
        _NanomachinesDroppedWhenKilled = p_enemyProperties._EnemyProperties._NanomachinesDroppedWhenKilled;
    }
}