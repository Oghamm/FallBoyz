using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LifeMalus : NetworkBehaviour
{
    [SerializeField] private int RecoveryAmount = 50;
    private float timeBeActive = 3;
    private bool isActive = false;

    #region Server

    public override void OnStartClient()
    {
        StartCoroutine(ActiveTheBomb());
    }

    private IEnumerator ActiveTheBomb()
    {
        yield return new WaitForSeconds(timeBeActive);
        isActive = true;
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) { return; }

        if (!other.TryGetComponent<Health>(out Health health)) { return; }

        health.DealDamage(RecoveryAmount);

        DestroySelf();
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion
}
