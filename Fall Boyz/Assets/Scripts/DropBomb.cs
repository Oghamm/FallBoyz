using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DropBomb : NetworkBehaviour
{

    [SerializeField] private GameObject bombPrefab = null;

    #region Server

    [Command]
    private void CmdDropBomb()
    {
        GameObject instance = Instantiate(
            bombPrefab,
            connectionToClient.identity.transform.position + Vector3.back,
            Quaternion.identity);

        NetworkServer.Spawn(instance, connectionToClient);
    }

    #endregion

    #region Client

    [Client]
    private void Update()
    {
        if (!hasAuthority) { return; }

        if (!Input.GetKeyDown(KeyCode.Space)) { return; }

        CmdDropBomb();
    }

    #endregion
}
