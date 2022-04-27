using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDetectingVerticalSurfaces : MonoBehaviour
{
    [SerializeField]
    private GameObject detectWallsButton;
    public GameObject verticalWallDetection;
    private DetectVerticalWallsWithRectangles verticalWallDetectionScript;
    
    void Awake()
    {
        verticalWallDetectionScript = verticalWallDetection.GetComponent<DetectVerticalWallsWithRectangles>();
    }

    public void StopDetection()
    {
        verticalWallDetectionScript.keepSearchingForVerticalSurfaces = false;
        gameObject.SetActive(false);
        detectWallsButton.SetActive(true);
    }
}
