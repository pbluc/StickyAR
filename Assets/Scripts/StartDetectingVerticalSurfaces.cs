using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDetectingVerticalSurfaces : MonoBehaviour
{
    [SerializeField]
    private GameObject acceptButton;
    public GameObject verticalWallDetection; 

    private DetectVerticalWallsWithRectangles verticalWallDetectionScript;

    void Awake()
    {
        verticalWallDetectionScript = verticalWallDetection.GetComponent<DetectVerticalWallsWithRectangles>();
    }

    public void StartDetection()
    {
        verticalWallDetectionScript.keepSearchingForVerticalSurfaces = true;
        gameObject.SetActive(false);
        acceptButton.SetActive(true);
    }
}
