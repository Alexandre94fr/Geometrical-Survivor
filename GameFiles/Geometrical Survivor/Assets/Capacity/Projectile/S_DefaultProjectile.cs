using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
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

        _isProjectileLaunched = false;

        Destroy(gameObject);
    }
}