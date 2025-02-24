using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class S_DefaultProjectile : MonoBehaviour, S_IProjectile
{
    Transform _transform;

    float _movementSpeed;

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

    void OnTriggerEnter2D(Collider2D p_collision2D)
    {
        Transform colliderTransform = p_collision2D.transform;

        if (colliderTransform.CompareTag("Enemy"))
        {
            // TODO : Deal damage to the enemy

            SelfDestroy();
        }
        else if (colliderTransform.CompareTag("Obstacle"))
        {
            SelfDestroy();
        }
    }

    public void LaunchProjectile(float p_projectileLifetime, float p_projectileRange)
    {
        if (p_projectileLifetime == 0)
        {
            Debug.LogError(
                $"ERROR ! The given '{nameof(p_projectileLifetime)}' insind the '{gameObject.name}' GameObject is equal to zero. " +
                "That will cause a division by zero."
            );
            return;
        }

        _movementSpeed = p_projectileRange / p_projectileLifetime;

        StartCoroutine(LifetimeTimer(p_projectileLifetime));

        _isProjectileLaunched = true;
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