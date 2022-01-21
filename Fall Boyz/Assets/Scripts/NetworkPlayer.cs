using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] TMP_Text DisplayNameText = null;
    [SerializeField] MeshRenderer meshRenderer = null;
    [SerializeField] Health health = null;
    [SerializeField] [SyncVar(hook = nameof(HandleDisplayNameUpdated))] private string displayName = "Missing Name";
    [SerializeField] [SyncVar(hook = nameof(HandleColorUpdated))] private Color color = Color.white;

    private Camera mainCamera;

    private void ServerHandleOnDie()
    {

    }

    #region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Command]
    private void CmdSetName(string name)
    {
        if (name.Length < 2 | name.Length > 20) { return; }
        
        ClientPrintNewName(name);
        SetDisplayName(name);
    }

    [Command]
    private void CmdSayMyName(GameObject target, string name)
    {
        if (!target.TryGetComponent<NetworkIdentity>(out NetworkIdentity targetIdentity)) { return; }

        TargetSayMyName(targetIdentity.connectionToClient, name);
    }

    [Server]
    public void SetColor(Color newColor)
    {
        color = newColor;
    }

    [Server]
    private void ServerHandleOnPlayerDied()
    {
        //NetworkServer.Destroy(gameObject);
        connectionToClient.Disconnect();
    }

    #endregion

    #region Client

    [ContextMenu("Set New Name")]
    private void SetNewName()
    {
        CmdSetName("Alex");
    }

    [TargetRpc]
    private void TargetSayMyName(NetworkConnection target, string name)
    {
        Debug.Log($"Your name is {name}");
    }

    [ClientRpc]
    public void ClientSayCongrat()
    {
        Debug.Log("Congrats !");
    }

    [ClientRpc]
    public void ClientPrintNewName(string name)
    {

    }

    public override void OnStartClient()
    {
        Debug.Log($"[Client] OnStartClient {NetworkClient.connection.connectionId}");
        health.ServerOnPlayerDied += ServerHandleOnPlayerDied;
    }

    public override void OnStopClient()
    {
        health.ServerOnPlayerDied -= ServerHandleOnPlayerDied;
    }

    private void HandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        DisplayNameText.text = newDisplayName;
    }

    private void HandleColorUpdated(Color oldColor, Color newColor)
    {
        meshRenderer.material.color = newColor;
    }

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    [Client]
    void Update()
    {
        if (!hasAuthority) { return; }

        if (!Input.GetMouseButtonDown(0)) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        if (!hit.transform.parent) { return; }

        //Send the move gameobject that we hit to server and our name
        CmdSayMyName(hit.transform.parent.gameObject, displayName);
    }

    #endregion
}
