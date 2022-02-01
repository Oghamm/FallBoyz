using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private float moveSpeed = 0.00001f;

    private Camera mainCamera;
    private CamFollowSmooth followScript;

    #region Server

    [Command]
    private void CmdMove(Vector3 position)
    {
        //check if it's a valid position
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
        followScript = mainCamera.GetComponent<CamFollowSmooth>();
        followScript.target = this.transform;

    }


    [Client]
    private void Update()
    {
        if (!hasAuthority) { return; }

        if (!Input.GetMouseButtonDown(1)) { return; }
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        //Send the move position to server
        CmdMove(hit.point);

        /*if (isLocalPlayer) {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);
            agent.transform.position = agent.transform.position + movement;
        }*/

    }

    #endregion
}
