using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWorkspaceVisibility : MonoBehaviour
{
    public RawImage icon;

    public Texture workspacesVisible;
    public Texture workspacesInvisible;

    public bool workspaceVisible = true;
    private GameObject[] workspaces;

    public void ToggleVisibility()
    {
        if (workspaceVisible)
        {
            workspaceVisible = false;
            icon.texture = workspacesInvisible;
        } else
        {
            workspaceVisible = true;
            icon.texture = workspacesVisible;
        }
        workspaces = GameObject.FindGameObjectsWithTag("Wall Workspace");

        foreach (GameObject workspace in workspaces)
        {
            workspace.GetComponent<MeshRenderer>().enabled = workspaceVisible;
        }

    }
}
