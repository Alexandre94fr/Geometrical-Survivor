using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class S_Enemy : MonoBehaviour
{
    [Header(" Properties :")]
    public S_EnemyProperties _EnemyProperties;

    [Header(" Internal references :")]
    public S_EnemyAttributes _EnemyAttributes;
    public S_EnemyController _EnemyController;

    SpriteRenderer _spriteRenderer;

    bool _canAttack = true;
    bool _isCollidingPlayer;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_EnemyProperties, nameof(_EnemyProperties)),

            (_EnemyAttributes, nameof(_EnemyAttributes)),
            (_EnemyController, nameof(_EnemyController))
        )) return;

        S_EnemyAttributes._OnEnemySpriteUpdateEvent += OnSpriteUpdate;
        S_EnemyAttributes._OnEnemySpriteColorUpdateEvent += OnSpriteColorUpdate;
        S_EnemyAttributes._OnHealthPointsUpdateEvent += OnHealthUpdate;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnDestroy()
    {
        S_EnemyAttributes._OnEnemySpriteUpdateEvent -= OnSpriteUpdate;
        S_EnemyAttributes._OnEnemySpriteColorUpdateEvent -= OnSpriteColorUpdate;
        S_EnemyAttributes._OnHealthPointsUpdateEvent -= OnHealthUpdate;
    }

    void OnCollisionEnter2D(Collision2D p_collision2D)
    {
        if (p_collision2D.transform.CompareTag("Player"))
        {
            _isCollidingPlayer = true;

            if (!_canAttack)
                return;
            
            StartCoroutine(StartAttackLoop(
                _EnemyProperties._EnemyProperties._AttackCooldownTime,
                p_collision2D.gameObject.GetComponentInChildren<S_PlayerAttributes>(),
                _EnemyProperties._EnemyProperties._AttackDamage
            ));
        }
    }

    void OnCollisionExit2D(Collision2D p_collision2D)
    {
        _isCollidingPlayer = false;
    }

    #region Enemy update methods

    void OnSpriteUpdate(S_Enemy p_enemy, Sprite p_newSprite)
    {
        if (p_enemy != this)
            return;

        _spriteRenderer.sprite = p_newSprite;
    }

    void OnSpriteColorUpdate(S_Enemy p_enemy, Color p_newSpriteColor)
    {
        if (p_enemy != this)
            return;

        _spriteRenderer.color = p_newSpriteColor;
    }

    void OnHealthUpdate(S_Enemy p_enemy, int p_healthPoint)
    {
        if (p_enemy != this)
            return;

        if (p_healthPoint <= 0)
            OnDeath();
    }
    #endregion

    IEnumerator StartAttackLoop(float p_attackCooldownTime, S_PlayerAttributes p_playerAttributes, int p_attackDamage)
    {
        while (_isCollidingPlayer)
        {
            if (_canAttack)
            {
                StartCoroutine(LaunchAttackCooldown(_EnemyProperties._EnemyProperties._AttackCooldownTime));

                Attack(p_playerAttributes, p_attackDamage);
            }
                
            yield return null;
        }
    }

    IEnumerator LaunchAttackCooldown(float p_attackCooldownTime)
    {
        _canAttack = false;

        yield return new WaitForSeconds(p_attackCooldownTime);

        _canAttack = true;
    }

    void Attack(S_PlayerAttributes p_playerAttributes, int p_attackDamage)
    {
        p_playerAttributes._HealthPoints -= p_attackDamage;
    }

    void OnDeath()
    {
        S_NanomachinesManager._Instance.InstantiateNanomachineObject(transform.position, _EnemyProperties._EnemyProperties._NanomachinesDroppedWhenKilled);

        S_EnemySpawner._OnDespawnEnemyEvent?.Invoke(gameObject);
    }
}