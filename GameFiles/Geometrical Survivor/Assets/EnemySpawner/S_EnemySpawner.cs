using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemySpawner : MonoBehaviour
{
    public static S_EnemySpawner _Instance;

    public static Action<GameObject> _OnDespawnEnemyEvent;

    void Awake()
    {
        _Instance = S_Instantiator.ReturnInstance(this, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    void Start()
    {
        _OnDespawnEnemyEvent += DespawnEnemy;
    }

    void OnDestroy()
    {
        _OnDespawnEnemyEvent -= DespawnEnemy;
    }

    public void SpawnEnemy(S_EnemyProperties p_enemyProperties, Vector3 p_position, Transform p_parent = null)
    {
        #region Securities

        if (p_enemyProperties == null)
        {
            Debug.LogError(
                $"ERROR ! The given {nameof(S_EnemyProperties)} is null." +
                "The enemy spawn has been canceled."
            );

            return;
        }

        if (p_enemyProperties._EnemyProperties._Prefab == null)
        {
            Debug.LogError(
                $"ERROR ! The given {nameof(S_EnemyProperties)} '{p_enemyProperties._EnemyProperties._Name}' " +
                $"doesn't have a prefab set in. \n" +
                "The enemy spawn has been canceled."
            );

            return;
        }
        #endregion

        if (p_parent == null)
            p_parent = transform;

        // Creation of the enemy
        GameObject enemyGameObject = Instantiate(
            p_enemyProperties._EnemyProperties._Prefab,
            new(0, 0, 0),
            Quaternion.identity,
            p_parent
        );

        S_Enemy enemy = enemyGameObject.GetComponentInChildren<S_Enemy>();
        
        #region Security

        if (enemy == null )
        {
            Debug.LogError(
                $"ERROR ! The script '{nameof(S_Enemy)}' has not been founded insind the " +
                $"'{enemyGameObject}' instantiated enemy GameObject.\n" +
                "The enemy has been destroyed. The enemy spawn has been canceled."
            );

            Destroy(enemyGameObject);

            return;
        }
        #endregion

        enemy.transform.position = p_position;

        enemy._EnemyProperties = p_enemyProperties;
    }

    /// <summary>
    /// Will destroy the parent of the GameObject gave. Will cancel if the GameObject gave does not have the S_Enemy Script. </summary>
    void DespawnEnemy(GameObject p_enemyGameObject)
    {
        #region Securities

        if (p_enemyGameObject == null)
        {
            Debug.LogError(
                $"ERROR ! The given {nameof(GameObject)} is null. \n" +
                "The enemy despawn has been canceled."
            );

            return;
        }

        if (!p_enemyGameObject.TryGetComponent(out S_Enemy enemy))
        {
            Debug.LogError(
                $"ERROR ! The given {p_enemyGameObject} GameObject doesn't have a {nameof(S_Enemy)} Script. \n" +
                "The enemy despawn has been canceled."
            );

            return;
        }
        #endregion

        Destroy(p_enemyGameObject.transform.parent.gameObject);
    }
}