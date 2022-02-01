using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowSmooth : MonoBehaviour
{

    public Transform target;
    Vector3 offsetCamera;

    [Range(0.01f, 1.0f)]
    [SerializeField] float smooth;

    void Start()
    {
        
    }

    void Update()
    {
        if(!target)
        {
            return;
        }

        offsetCamera = transform.position - target.position;
        Vector3 cameraPosition = target.position + offsetCamera;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, cameraPosition, smooth);
        transform.position = smoothPosition;
        transform.LookAt(target);
    }
}