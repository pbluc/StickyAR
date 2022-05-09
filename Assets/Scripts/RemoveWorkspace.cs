using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWorkspace : MonoBehaviour
{
    private GameObject[] placedWorkspaces;

    public GameObject workspaceSelection;

    private WorkspaceSelection workspaceSelectionScript;

    // Start is called before the first frame update
    void Start()
    {
        workspaceSelectionScript = workspaceSelection.GetComponent<WorkspaceSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        placedWorkspaces = GameObject.FindGameObjectsWithTag("Wall Workspace");

        foreach (GameObject wallWorkspace in placedWorkspaces)
        {
            string workspaceID = wallWorkspace.name.Substring(15);
            GameObject stickyNoteWorkspaceConfig = GameObject.Find("Configuration " + workspaceID);
            
            //Scale Note Icons on the Minimap
            if (stickyNoteWorkspaceConfig != null)
            {
                Transform[] allChildren = stickyNoteWorkspaceConfig.GetComponentsInChildren<Transform>();
    
                foreach (Transform child in allChildren)
                {
                   GameObject icon = child.gameObject.transform.Find("noteIcon").gameObject;
                   icon.gameObject.transform.localScale = new Vector3(10/allChildren.Length, 10/allChildren.Length, 10/allChildren.Length);
                }
            }
        }

    }

    public void DeleteWorkspace()
    {
        foreach (GameObject wallWorkspace in placedWorkspaces)
        {
            bool selected = wallWorkspace.GetComponent<Outline>().enabled;
            if (selected)
            {
                string workspaceID = wallWorkspace.name.Substring(15);
                GameObject stickyNoteWorkspaceConfig = GameObject.Find("Configuration " + workspaceID);

                if (stickyNoteWorkspaceConfig != null)
                {
                    Destroy(stickyNoteWorkspaceConfig);
                }
                Destroy(wallWorkspace);
            }
        }
        workspaceSelectionScript.selectedCount = 0;
    }

}
