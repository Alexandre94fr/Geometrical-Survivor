using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class S_Enemy : MonoBehaviour
{
    [Header(" Enemy's statistics :")]
    public S_EnemyProperties _EnemyStatistics;


    SpriteRenderer _spriteRenderer;

    bool _canAttack = true;
    bool _isCollidingPlayer;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_EnemyStatistics, nameof(_EnemyStatistics))
        )) return;

        S_EnemyAttributes._OnEnemySpriteUpdateEvent += UpdateSprite;
        S_EnemyAttributes._OnHealthPointsUpdateEvent += OnHealthUpdate;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D p_collision2D)
    {
        if (p_collision2D.transform.CompareTag("Player"))
        {
            _isCollidingPlayer = true;

            if (!_canAttack)
                return;
            
            StartCoroutine(StartAttackLoop(
                _EnemyStatistics._EnemyProperties._AttackCooldownTime,
                p_collision2D.gameObject.GetComponentInChildren<S_PlayerAttributes>(),
                _EnemyStatistics._EnemyProperties._AttackDamage
            ));
        }
    }

    void OnCollisionExit2D(Collision2D p_collision2D)
    {
        _isCollidingPlayer = false;
    }

    IEnumerator StartAttackLoop(float p_attackCooldownTime, S_PlayerAttributes p_playerAttributes, int p_attackDamage)
    {
        while (_isCollidingPlayer)
        {
            if (_canAttack)
            {
                StartCoroutine(LaunchAttackCooldown(_EnemyStatistics._EnemyProperties._AttackCooldownTime));

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

    void UpdateSprite(S_Enemy p_enemy, Sprite p_newSprite)
    {
        if (p_enemy != this)
            return;

        _spriteRenderer.sprite = p_newSprite;
    }

    void OnHealthUpdate(S_Enemy p_enemy, int p_healthPoint)
    {
        if (p_enemy != this)
            return;

        if (p_healthPoint <= 0)
            OnDeath();
    }

    void OnDeath()
    {
        S_NanomachinesManager._Instance.InstantiateNanomachineObject(transform.position, _EnemyStatistics._EnemyProperties._NanomachinesDroppedWhenKilled);

        Destroy(gameObject.transform.parent.gameObject);
    }
}