using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ActiveCapacityLauncher : MonoBehaviour
{
    [Header(" External references")]
    [SerializeField] Transform _launcherTransform;
    [SerializeField] S_PlayerAttributes _playerAttributes; // TODO : This is a problem because Enemy don't have that

    [Header(" Properties :")]
    S_DefaultProjectile.ProjectileOwnerEnum _projectileOwner;

    bool _canLaunchActiveCapacity = true;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_launcherTransform, nameof(_launcherTransform)),
            (_playerAttributes, nameof(_playerAttributes))
        )) return;
    }

    public void TryLaunchActiveCapacity(S_ActiveCapacityProperties.ActiveCapacityStruct p_activeCapacityStruct)
    {
        if (!_canLaunchActiveCapacity)
            return;

        ModifyActiveCapacityAttributes(ref p_activeCapacityStruct);

        StartCoroutine(LaunchActiveCapacityTimer(p_activeCapacityStruct._CooldownTime));

        if (p_activeCapacityStruct._DoesInstantiateProjectile)
        {
            GameObject projectile = InstantiateProjectile(ref p_activeCapacityStruct);

            projectile.GetComponent<S_IProjectile>().LaunchProjectile(
                _projectileOwner,
                p_activeCapacityStruct._AttackLifetime,
                p_activeCapacityStruct._AttackReach,
                p_activeCapacityStruct._AttackDamage
            );
        }
        else
        {
            // NOT IMPLEMENTED (No ActiveCapacities does not use projectile)
        }
    }

    IEnumerator LaunchActiveCapacityTimer(float p_activeCapacityCooldown)
    {
        _canLaunchActiveCapacity = false;

        yield return new WaitForSeconds(p_activeCapacityCooldown);

        _canLaunchActiveCapacity = true;
    }

    void ModifyActiveCapacityAttributes(ref S_ActiveCapacityProperties.ActiveCapacityStruct p_activeCapacityStruct)
    {
        // Capacity properties
        p_activeCapacityStruct._AttackDamage += _playerAttributes._SumPassiveCapacityProperties._Damage;
        p_activeCapacityStruct._AttackReach += _playerAttributes._SumPassiveCapacityProperties._AttackReach;

        p_activeCapacityStruct._ArmingTime += _playerAttributes._SumPassiveCapacityProperties._ArmingTime;
        p_activeCapacityStruct._InvulnerabilityTime += _playerAttributes._SumPassiveCapacityProperties._InvulnerabilityTime;
        p_activeCapacityStruct._AttackLifetime += _playerAttributes._SumPassiveCapacityProperties._AttackLifetime;
        p_activeCapacityStruct._CooldownTime += _playerAttributes._SumPassiveCapacityProperties._CooldownTime;

        // In case the value goes under 0
        if (p_activeCapacityStruct._CooldownTime < 0)
            p_activeCapacityStruct._CooldownTime = 0;

        p_activeCapacityStruct._AttackStunningTime += _playerAttributes._SumPassiveCapacityProperties._StunningTime;
        p_activeCapacityStruct._AttackKnockback += _playerAttributes._SumPassiveCapacityProperties._AttackKnockback;
        p_activeCapacityStruct._SelfStunningTimeWhenSucceed += _playerAttributes._SumPassiveCapacityProperties._SelfStunningTimeWhenSucceed;
        p_activeCapacityStruct._SelfStunningTimeWhenFailed += _playerAttributes._SumPassiveCapacityProperties._SelfStunningTimeWhenFailed;
    }

    GameObject InstantiateProjectile(ref S_ActiveCapacityProperties.ActiveCapacityStruct p_activeCapacityStruct)
    {
        if (p_activeCapacityStruct._ProjectilePrefab == null)
        {
            Debug.LogError(
                $"ERROR ! The variable '{nameof(p_activeCapacityStruct._ProjectilePrefab)}' of the ScriptableObject '{p_activeCapacityStruct._Name}' is 'null'. " +
                $"This should not append, the method '{nameof(InstantiateProjectile)}' will return 'null'."
            );
            return null;
        }

        float angle = Mathf.Atan2(p_activeCapacityStruct._AttackDirection.y, p_activeCapacityStruct._AttackDirection.x) * Mathf.Rad2Deg;

        Quaternion projectileRotation = Quaternion.Euler(0, 0, _launcherTransform.eulerAngles.z + (angle - 90));

        // Will spawn the projectile at the player position and at the given orientation
        GameObject projectile = Instantiate(
            p_activeCapacityStruct._ProjectilePrefab,
            _launcherTransform.position,
            projectileRotation,
            transform
        );

        return projectile;
    }
}