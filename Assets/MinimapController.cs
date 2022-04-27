using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public GameObject mainCamera;
    
    private Vector3 offset;

    void Start ()
    {
        offset = transform.position - mainCamera.transform.position;
    }
    void LateUpdate ()
    {
        transform.position = mainCamera.transform.position + offset;

        var euler = mainCamera.transform.rotation.eulerAngles;
        var rot = Quaternion.Euler( 90, (euler.y), 0);
        transform.localRotation = rot;
    }
}
