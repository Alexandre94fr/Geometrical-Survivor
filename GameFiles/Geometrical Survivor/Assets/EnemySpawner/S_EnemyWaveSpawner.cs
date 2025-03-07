using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class S_EnemyWaveSpawner : MonoBehaviour
{
    [Serializable]
    struct WaveProperties
    {
        public float _TimeBeforeNextWaveSpawn;

        [Space(20)]
        public bool _DoesSpawnAtPrecisePosition;
        public Vector2 _SpawnPosition;

        [Space(20)]
        public List<S_EnemyProperties> _SpawnedEnemies;
    }

    [Header(" Properties :")]
    [SerializeField] int _enemySpawnRangeFormPlayer = 50;

    [Space]
    [SerializeField] bool _isEnemyWaveSpawningInfinitely = true;
    [SerializeField] int _loopIntoLastXWave = 2;

    [Space]
    [SerializeField] List<WaveProperties> _wavesProperties;


    bool _isPlayerDead;
    Transform _playerTransform;


    void Start()
    {
        #region Securities 

        if (_enemySpawnRangeFormPlayer < 0)
        {
            Debug.LogError(
                $"ERROR ! The variable '{nameof(_enemySpawnRangeFormPlayer)}' is negative. \n" +
                "Enemy spawning canceled. \n"
            );

            return;
        }

        if (_isEnemyWaveSpawningInfinitely && _loopIntoLastXWave <= 0)
        {
            Debug.LogError(
                $"ERROR ! The variable '{nameof(_isEnemyWaveSpawningInfinitely)}' is at true, " +
                $"but the variable '{nameof(_loopIntoLastXWave)}' is equal or under 0.\n" +
                "Enemy spawning canceled. \n"
            );

            return;
        }

        if (_isEnemyWaveSpawningInfinitely && _loopIntoLastXWave > _wavesProperties.Count)
        {
            Debug.LogError(
                $"ERROR ! The variable '{nameof(_loopIntoLastXWave)}' is bigger than the number of waves. \n" +
                "Enemy spawning canceled. \n"
            );

            return;
        }
        #endregion

        _playerTransform = S_Player._Instance.transform;

        S_PlayerAttributes._OnHealthPointsUpdateEvent += OnPlayerLifeStateUpdate;

        StartCoroutine(StartSpawningEnemies());
    }

    void OnDestroy()
    {
        S_PlayerAttributes._OnHealthPointsUpdateEvent -= OnPlayerLifeStateUpdate;
    }

    void OnPlayerLifeStateUpdate(int p_newPlayerHealthPoints)
    {
        if (p_newPlayerHealthPoints <= 0)
            _isPlayerDead = true;
    }

    IEnumerator StartSpawningEnemies()
    {
        int waveNumber = 0;

        // Spawning all the enemy waves for the first time
        for (int i = 0; i < _wavesProperties.Count; i++)
        {
            if (_isPlayerDead)
                yield break;

            waveNumber = i;

            SpawnEnemyWave(_wavesProperties[i], waveNumber);

            // Wait for the new wave
            yield return new WaitForSeconds(_wavesProperties[i]._TimeBeforeNextWaveSpawn);
        }

        // Stopping the coroutine if it's not the infinite mode
        if (!_isEnemyWaveSpawningInfinitely)
            yield break;

        while (!_isPlayerDead)
        {
            // Will loop into the selected waves
            waveNumber -= _loopIntoLastXWave;
        
            for (int i = 0; i < _loopIntoLastXWave; i++)
            {
                if (_isPlayerDead)
                    yield break;

                waveNumber++;

                SpawnEnemyWave(_wavesProperties[waveNumber], waveNumber);

                yield return new WaitForSeconds(_wavesProperties[i]._TimeBeforeNextWaveSpawn);
            }
        }
    }

    Vector2 GetRandomWaveSpawnPosition()
    {
        // Getting a random angle in radians
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

        // Converting to Cartesian coordinates (halp), and returning it
        return new Vector2(
            _playerTransform.position.x + Mathf.Cos(randomAngle) * _enemySpawnRangeFormPlayer,
            _playerTransform.position.y + Mathf.Sin(randomAngle) * _enemySpawnRangeFormPlayer
        );
    }

    void SpawnEnemyWave(WaveProperties p_waveProperties, int p_waveNumber)
    {
        // Getting enemy wave spawn position
        Vector2 spawnPosition = p_waveProperties._SpawnPosition;

        if (!p_waveProperties._DoesSpawnAtPrecisePosition)
            spawnPosition = GetRandomWaveSpawnPosition();

        // Spawning the wave
        for (int j = 0; j < p_waveProperties._SpawnedEnemies.Count; j++)
        {
            #region Security

            if (p_waveProperties._SpawnedEnemies[j] == null)
            {
                Debug.LogError(
                    $"ERROR ! The {j} enemy of the {p_waveNumber} is null. \n " +
                    "The enemy will not be spawned. The wave spawning will continue. \n" // TODO Test
                );

                continue;
            }
            #endregion

            S_EnemySpawner._Instance.SpawnEnemy(p_waveProperties._SpawnedEnemies[j], spawnPosition);
        }
    }
}