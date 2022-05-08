using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkspaceSelection : MonoBehaviour
{
    public int selectedCount = 0;

    private Camera mainARCamera;

    public GameObject deleteWorkspaceButton;

    public bool canSelectWorkspaces = true;

    // Start is called before the first frame update
    void Start()
    {
        mainARCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSelectWorkspaces)
        {
            // Check to see if user taps on workspace
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray raycast = mainARCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                if (Physics.Raycast(raycast, out raycastHit) && !isOverUI)
                {
                    if (raycastHit.collider.tag == "Wall Workspace")
                    {
                        GameObject tappedWorkspace = raycastHit.collider.gameObject;
                        bool selected = tappedWorkspace.GetComponent<Outline>().enabled;
                        if (selected)
                        {
                            selectedCount -= 1;
                            // Removes highlight outline on workspace prefab instance 
                            tappedWorkspace.GetComponent<Outline>().enabled = false;
                            GameObject icon = tappedWorkspace.transform.Find("WorkspaceIcon").gameObject;
                            icon.GetComponent<Renderer> ().material.color = Color.blue;
                        }
                        else
                        {
                            selectedCount += 1;
                            // Adds highlight outline on workspace prefab instance 
                            tappedWorkspace.GetComponent<Outline>().enabled = true;
                            GameObject icon = tappedWorkspace.transform.Find("WorkspaceIcon").gameObject;
                            icon.GetComponent<Renderer> ().material.color = Color.magenta;
                        }
                    }
                }
            }

            if (selectedCount > 0)
            {
                deleteWorkspaceButton.SetActive(true);
            }
            else
            {
                deleteWorkspaceButton.SetActive(false);
            }
        }
    }
}
