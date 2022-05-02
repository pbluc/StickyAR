using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetWorkspaceName : MonoBehaviour
{
    public GameObject setWorkspaceNameInputField;
    private GameObject selectedWorkspace;

    public InputField workspaceName;

    public GameObject workspaceSelection;

    private WorkspaceSelection workspaceSelectionScript;

    private bool setInputFieldInitialText = false;

    void Awake()
    {
        workspaceSelectionScript = workspaceSelection.GetComponent<WorkspaceSelection>();
    }

    void Start()
    {
        workspaceName.onEndEdit.AddListener(delegate { ChangeWorkspaceName(workspaceName); });
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure only one workspace is selected at a time to change its name
        if (workspaceSelectionScript.selectedCount == 1)
        {
            if (!setInputFieldInitialText)
            {
                workspaceName.text = GetNameCurrentlySelectedWorkspace();
                setInputFieldInitialText = true;
            }
            setWorkspaceNameInputField.SetActive(true);
        }
        else
        {
            setWorkspaceNameInputField.SetActive(false);
            setInputFieldInitialText = false;
        }
    }

    public string GetNameCurrentlySelectedWorkspace()
    {
        GameObject[] placedWorkspaces = GameObject.FindGameObjectsWithTag("Wall Workspace");

        foreach (GameObject wallWorkspace in placedWorkspaces)
        {
            bool selected = wallWorkspace.GetComponent<Outline>().enabled;
            if (selected)
            {
                selectedWorkspace = wallWorkspace;
                if (selectedWorkspace.transform.GetChild(0).name == "Name")
                {
                    return wallWorkspace.name;
                } else
                {
                    return selectedWorkspace.transform.GetChild(0).name;
                }
                
            }
        }
        return "";
    }

    void ChangeWorkspaceName(InputField workspaceNameInput)
    {
        selectedWorkspace.transform.GetChild(0).name = workspaceNameInput.text;
    }

}
