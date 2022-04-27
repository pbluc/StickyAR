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
