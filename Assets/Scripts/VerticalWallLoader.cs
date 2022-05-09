using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class VerticalWallLoader : MonoBehaviour
{
    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;
    private Camera arCamera;

    [SerializeField]
    private GameObject acceptButtonObject;
    private Button acceptButton;
    [SerializeField]
    private GameObject detectWallsButton;
    [SerializeField]
    private GameObject stickyMode;
    [SerializeField]
    private GameObject wallWorkspace;
    private ARPlane wallPlane;
    private Pose wallPose;

    private Vector3 touchPosition;
    public static bool keepSearchingForVerticalSurfaces = true;

    public static int numWorkspaces = 1;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake() {
        arCamera = GetComponent<ARSessionOrigin>().camera;
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Start() {
        acceptButton = GameObject.Find("AcceptButton").GetComponent<Button>();
        acceptButton.onClick.AddListener(stopVertical);
    }

    // Stop detecting vertical surfaces 
    void stopVertical(){
        // Debug.Log("Accept Button Pressed");
        keepSearchingForVerticalSurfaces = false;
        acceptButtonObject.SetActive(false);
        detectWallsButton.SetActive(true);
        stickyMode.SetActive(true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(keepSearchingForVerticalSurfaces){
            arPlaneManager.enabled = true;
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began){
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;

                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes) && isOverUI == false){
                    // Retrieves the first plane trackable detected
                    ARRaycastHit firstHit = hits[0];
                    wallPlane = (ARPlane) firstHit.trackable;

                    wallPose = firstHit.pose;
                    
                    acceptButtonObject.SetActive(true);

                    // Resizes the wall workspace indicator to fit the size and dimensions of the AR Plane being detected
                    Vector2 wallPlaneSize = wallPlane.size;
                    Vector3 wallScale = new Vector3(wallPlaneSize.x, 1f, wallPlaneSize.y);

                    // Place new workspace and keep detecting vertical surfaces until user has chosen all workspaces
                    wallWorkspace.SetActive(true);
                    GameObject addedWorkspace = Instantiate(wallWorkspace);
                    addedWorkspace.transform.position = wallPose.position;
                    addedWorkspace.transform.rotation = wallPlane.transform.rotation;
                    addedWorkspace.transform.localScale = wallScale * 0.1f;
                    addedWorkspace.name = "Wall Workspace " + numWorkspaces;
                    numWorkspaces += 1;
                    wallWorkspace.SetActive(false);
                }
            }
        }
        // Disable all AR Planes running 
        else{
            foreach (var plane in arPlaneManager.trackables){
                plane.gameObject.SetActive(false);
            }
            arPlaneManager.enabled = false;
        }
        
    }
}