using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class DetectVerticalWallsWithRectangles : MonoBehaviour
{
    [SerializeField]
    private GameObject acceptButton;
    public GameObject wallIndicator;
    public GameObject wallWorkspacePrefab;

    public ARPlaneManager arPlaneManager;
    public ARRaycastManager raycastManager;
    public ARSessionOrigin arOrigin;

    public Camera mainARCamera;

    private Pose wallPose;

    private bool wallPoseIsValid = false;
    public bool keepSearchingForVerticalSurfaces = true;

    public static int numWorkspaces = 1;

    void Start()
    {
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
    }

    void Update()
    {
        if (keepSearchingForVerticalSurfaces)
        {
            UpdateWallPose();
            UpdateWallIndicator();

            // Check to see if user taps on a detected workspace placement
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray raycast = mainARCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                if (Physics.Raycast(raycast, out raycastHit) && !isOverUI)
                {
                    if (raycastHit.collider.name == "Wall Workspace Indicator")
                    {
                        acceptButton.SetActive(true);
                        // Place workspace and keep detecting vertical surfaces until user has chosen all workspaces
                        GameObject addedWorkspace = (GameObject) Instantiate(wallWorkspacePrefab, wallIndicator.transform.position, wallIndicator.transform.rotation);
                        addedWorkspace.transform.localScale = wallIndicator.transform.localScale;
                        addedWorkspace.name = "Wall Workspace " + numWorkspaces;
                        addedWorkspace.transform.GetChild(1).name = addedWorkspace.GetInstanceID().ToString();
                        addedWorkspace.transform.GetChild(3).name = DateTime.Now.ToString("MM dd yyyy"); ;
                        numWorkspaces += 1;
                    }
                }
            }
        } else
        {
            wallIndicator.SetActive(false);
        }
    }

    private void UpdateWallIndicator()
    {
        // If a valid vertical surface was found 
        if (wallPoseIsValid)
        {
            // Makes the wall workspace indicator visible and sets its position and rotation to that of the vertical surface found
            wallIndicator.SetActive(true);
            Vector3 wallPoseEulerAngles = wallPose.rotation.eulerAngles;
            wallPoseEulerAngles.x = 0f;
            Quaternion wallPoseRotation = Quaternion.Euler(wallPoseEulerAngles);

            //wallIndicator.transform.SetPositionAndRotation(wallPose.position, wallPoseRotation);
            wallIndicator.transform.position = wallPose.position;
            if (Application.platform == RuntimePlatform.Android)
            {
                wallIndicator.transform.rotation = wallPoseRotation;
            } else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                wallIndicator.transform.rotation = wallPose.rotation;
            }
            
            //wallWorkspacePrefab.transform.SetPositionAndRotation(wallIndicator.transform.position, wallIndicator.transform.rotation);
        } else
        {
            // If no valid vertical surface found, makes wall workspace indicator invisible 
            wallIndicator.SetActive(false);
        }
    }

    private void UpdateWallPose()
    {
        // Raycasts from the center of the phone display screen
        Vector3 screenCenter = mainARCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        // Populates list with trackables of type vertical plane that were hit by the raycast
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        // Checks to see if we've detected any planes 
        wallPoseIsValid = hits.Count > 0;
    
        if (wallPoseIsValid)
        {
            // Retrieves the first plane trackable detected
            ARRaycastHit firstHit = hits[0];
            ARPlane wallPlane = (ARPlane)firstHit.trackable;

            if (wallPlane.alignment == PlaneAlignment.Vertical)
            {
                // Sets the pose of the wall workspace indicator to that of the first plane trackable hit
                wallPose = firstHit.pose;

                // Resizes the wall workspace indicator to fit the size and dimensions of the AR Plane being detected
                Vector2 wallPlaneSize = wallPlane.size;
                Vector3 wallScale = new Vector3(wallPlaneSize.x, 1f, wallPlaneSize.y);

                wallIndicator.transform.localScale = wallScale * 0.1f;
                //wallWorkspacePrefab.transform.localScale = wallIndicator.transform.localScale;
            } else
            {
                wallPoseIsValid = false;
            }
        }
    }

}
