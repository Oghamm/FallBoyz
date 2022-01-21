using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SyncVar(hook = nameof(HandleHealthUpdated))] private int currentHealth;

    public event Action ServerOnPlayerDied;

    #region Server
    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) { return; }

        currentHealth-= damageAmount;

        if (currentHealth > 0) { return; }

        ServerOnPlayerDied?.Invoke();
    }
    [Server]
    public void Recovery(int healthPoint)
    {
        currentHealth += healthPoint;
    }

    #endregion

    #region Client

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        Debug.Log($"My health is now {newHealth}");
    }
    #endregion
    
}
