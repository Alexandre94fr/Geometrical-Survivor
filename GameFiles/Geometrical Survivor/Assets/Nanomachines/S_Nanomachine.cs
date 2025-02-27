using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Nanomachine : MonoBehaviour
{
    [Header(" Properties :")]
    [ReadOnlyInInspector] public int _NanomachineAmount;

    bool _isColliding;

    void OnCollisionEnter2D(Collision2D p_collision2D)
    {
        if (_isColliding)
            return;

        _isColliding = true;

        Transform colliderTransform = p_collision2D.transform;

        if (colliderTransform.CompareTag("Player"))
        {
            colliderTransform.GetComponentInChildren<S_PlayerAttributes>().AddNanomachine(_NanomachineAmount);
            Destroy(gameObject);
        }
    }
}