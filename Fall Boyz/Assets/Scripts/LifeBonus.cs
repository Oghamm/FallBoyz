using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LifeBonus : NetworkBehaviour
{
    [SerializeField] private int RecoveryAmount = 10;

    #region Server

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Health>(out Health health)) { return; }

        health.Recovery(RecoveryAmount);

        DestroySelf();
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion
}
