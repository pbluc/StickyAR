using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTexture : MonoBehaviour
{
    private GameObject[] placedStickyNotes;

    public GameObject stickyNoteSelection;

    private StickyNoteSelection stickyNoteSelectionScript;

    public Material Material;

    // Start is called before the first frame update
    void Start()
    {
        stickyNoteSelectionScript = stickyNoteSelection.GetComponent<StickyNoteSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        placedStickyNotes = GameObject.FindGameObjectsWithTag("Sticky Note");
    }

    public void ChangeStickyNoteTexture()
    {
        foreach (GameObject stickyNote in placedStickyNotes)
        {
            bool selected = stickyNote.GetComponent<Outline>().enabled;
            if (selected)
            {
                stickyNote.GetComponent<MeshRenderer>().material = Material;
                stickyNote.GetComponent<Outline>().enabled = false;
            }
        }
        stickyNoteSelectionScript.selectedCount = 0;
    }
}