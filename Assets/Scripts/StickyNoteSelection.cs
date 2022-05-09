using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNoteSelection : MonoBehaviour
{
    public int selectedCount = 0;

    private Camera mainARCamera;

    public GameObject changeTextureMenu;

    public bool canSelectStickyNotes = true;

    // Start is called before the first frame update
    void Start()
    {
        mainARCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSelectStickyNotes)
        {
            // Check to see if user taps on workspace
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray raycast = mainARCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                if (Physics.Raycast(raycast, out raycastHit) && !isOverUI)
                {
                    if (raycastHit.collider.tag == "Sticky Note")
                    {
                        GameObject tappedStickyNote = raycastHit.collider.gameObject;
                        bool selected = tappedStickyNote.GetComponent<Outline>().enabled;
                        if (selected)
                        {
                            selectedCount -= 1;
                            // Removes highlight outline on sticky note prefab instance 
                            tappedStickyNote.GetComponent<Outline>().enabled = false;
                            
                            GameObject icon = tappedStickyNote.transform.Find("noteIcon").gameObject;
                            icon.GetComponent<Renderer> ().material.color = Color.red;
                        }
                        else
                        {
                            selectedCount += 1;
                            // Adds highlight outline on sticky note prefab instance 
                            tappedStickyNote.GetComponent<Outline>().enabled = true;
                            GameObject icon = tappedStickyNote.transform.Find("noteIcon").gameObject;
                            icon.GetComponent<Renderer> ().material.color = Color.yellow;
                        }
                    }
                }
            }

            if (selectedCount > 0)
            {
                changeTextureMenu.SetActive(true);
            }
            else
            {
                changeTextureMenu.SetActive(false);
            }
        }
    }
}
