using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class S_DefaultProjectile : MonoBehaviour, S_IProjectile
{
    public enum ProjectileOwnerEnum
    {
        Player,
        Enemy
    }

    Transform _transform;

    ProjectileOwnerEnum _projectileOwner;
    float _movementSpeed;
    int _projectileDamage;

    bool _isProjectileLaunched = false;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (!_isProjectileLaunched)
            return;

        Vector3 positionOffset = _movementSpeed * Time.deltaTime * _transform.up;

        _transform.position += positionOffset;
    }

    void OnCollisionEnter2D(Collision2D p_collision2D)
    {
        Transform colliderTransform = p_collision2D.transform;

        if (_projectileOwner == ProjectileOwnerEnum.Player && colliderTransform.CompareTag("Enemy"))
        {
            colliderTransform.GetComponentInChildren<S_EnemyAttributes>()._HealthPoints -= _projectileDamage;

            SelfDestroy();
        }
        else if (_projectileOwner == ProjectileOwnerEnum.Enemy && colliderTransform.CompareTag("Player"))
        {
            colliderTransform.GetComponentInChildren<S_PlayerAttributes>()._HealthPoints -= _projectileDamage;

            SelfDestroy();
        }
        else if (colliderTransform.CompareTag("Obstacle"))
        {
            SelfDestroy();
        }
        else
        {
            Debug.LogWarning($"WARNING ! The Script '{this}' hasn't planned a collision with the '{p_collision2D.gameObject.name}' GameObject.");
        }
    }

    public void LaunchProjectile(ProjectileOwnerEnum p_projectileOwner, float p_projectileLifetime, float p_projectileRange, int p_projectileDamage)
    {
        if (p_projectileLifetime == 0)
        {
            Debug.LogError(
                $"ERROR ! The given '{nameof(p_projectileLifetime)}' insind the '{gameObject.name}' GameObject is equal to zero. " +
                "That will cause a division by zero."
            );
            return;
        }

        _projectileOwner = p_projectileOwner;
        _movementSpeed = p_projectileRange / p_projectileLifetime;
        _projectileDamage = p_projectileDamage;

        gameObject.layer = GetLayerIntFormProjectileOwner(_projectileOwner);

        StartCoroutine(LifetimeTimer(p_projectileLifetime));

        _isProjectileLaunched = true;
    }

    int GetLayerIntFormProjectileOwner(ProjectileOwnerEnum p_projectileOwner)
    {
        int layerIndex = LayerMask.NameToLayer(p_projectileOwner.ToString() + "Projectile");

        if (layerIndex == -1)
        {
            Debug.LogError($"ERROR ! The given Layer '{p_projectileOwner + "Projectile"}' does not exist ! Please create it in Project Settings -> Tags and Layers.");
        }

        return layerIndex;
    }

    IEnumerator LifetimeTimer(float p_lifetime)
    {
        yield return new WaitForSeconds(p_lifetime);

        SelfDestroy();
    }

    void SelfDestroy()
    {
        _isProjectileLaunched = false;

        Destroy(gameObject);
    }
}