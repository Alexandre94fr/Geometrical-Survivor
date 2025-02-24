using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ActiveCapacityLauncher : MonoBehaviour
{
    [Header(" External references")]
    [SerializeField] Transform _launcherTransform;


    bool _canLaunchActiveCapacity = true;

    void Start()
    {
        if (!S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null,
            (_launcherTransform, nameof(_launcherTransform))
        )) return;
    }

    public void TryLaunchActiveCapacity(S_ActiveCapacityAttributes.ActiveCapacityStruct p_activeCapacityStruct)
    {
        if (!_canLaunchActiveCapacity)
            return;

        StartCoroutine(LaunchActiveCapacityTimer(p_activeCapacityStruct._CooldownTime));

        ModifyActiveCapacityAttributes(ref p_activeCapacityStruct);

        if (p_activeCapacityStruct._DoesInstantiateProjectile)
        {
            InstantiateProjectile(ref p_activeCapacityStruct);
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

    void ModifyActiveCapacityAttributes(ref S_ActiveCapacityAttributes.ActiveCapacityStruct p_activeCapacityStruct)
    {
        // TODO : Change active capacity struct values based on passive capacities
    }

    GameObject InstantiateProjectile(ref S_ActiveCapacityAttributes.ActiveCapacityStruct p_activeCapacityStruct)
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