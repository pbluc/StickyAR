using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterStickyNoteMode : MonoBehaviour
{
    private Camera mainARCamera;

    public RawImage icon;

    public Texture highlightedIconTexture;
    public Texture unhighlightedIconTexture;

    public bool inStickyNoteMode = false;

    public GameObject stickyNotePrefab;
    public GameObject acceptButton;
    public GameObject deleteWorkspaceButton;
    public GameObject detectWallsButton;
    public GameObject verticalWallDetection;
    public GameObject workspaceSelection;
    public GameObject openNote;
    public GameObject deleteNoteButton;

    private DetectVerticalWallsWithRectangles verticalWallDetectionScript;
    private WorkspaceSelection workspaceSelectionScript;

    void Awake()
    {
        mainARCamera = Camera.main;

        verticalWallDetectionScript = verticalWallDetection.GetComponent<DetectVerticalWallsWithRectangles>();
        workspaceSelectionScript = workspaceSelection.GetComponent<WorkspaceSelection>();
    }

    public void ToggleStickyNoteInteractionMode()
    {
        if (inStickyNoteMode)
        {
            inStickyNoteMode = false;
            icon.texture = unhighlightedIconTexture;

            workspaceSelectionScript.canSelectWorkspaces = true;

            detectWallsButton.SetActive(true);
            deleteNoteButton.SetActive(false);
            openNote.SetActive(false);
            OpenNote.inStickyNoteMode = false;

        } else
        {
            inStickyNoteMode = true;
            icon.texture = highlightedIconTexture;

            verticalWallDetectionScript.keepSearchingForVerticalSurfaces = false;
            workspaceSelectionScript.canSelectWorkspaces = false;

            acceptButton.SetActive(false);
            deleteWorkspaceButton.SetActive(false);
            detectWallsButton.SetActive(false);
            openNote.SetActive(true);
            OpenNote.inStickyNoteMode = true;

            // Unhighlight all placed workspaces
            UnhighlightAllWorkspaces();
        }
    }

    private void UnhighlightAllWorkspaces()
    {
        GameObject[] placedWorkspaces = GameObject.FindGameObjectsWithTag("Wall Workspace");

        foreach (GameObject wallWorkspace in placedWorkspaces)
        {
            wallWorkspace.GetComponent<Outline>().enabled = false;
        }

        workspaceSelectionScript.selectedCount = 0;
    }
}
