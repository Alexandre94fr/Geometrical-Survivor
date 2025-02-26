using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class S_Enemy : MonoBehaviour
{
    [Header(" Enemy's statistics :")]
    public S_EnemyProperties _EnemyStatistics;


    SpriteRenderer _spriteRenderer;


    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_EnemyStatistics, nameof(_EnemyStatistics))
        )) return;

        S_EnemyAttributes._OnEnemySpriteUpdateEvent += UpdateSprite;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    void UpdateSprite(S_Enemy p_enemy, Sprite p_newSprite)
    {
        if (p_enemy != this)
            return;

        _spriteRenderer.sprite = p_newSprite;
    }
}