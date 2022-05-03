using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class OpenNote : MonoBehaviour
{
    private Camera arCamera;

    public static GameObject currentSelectedNote;
    public GameObject stickyNotePrefab;
    public GameObject deleteNoteButton;
    public GameObject acceptButton;
    public GameObject detectWallsButton;

    public static bool inStickyNoteMode = false;
    public static bool deletedNote = false;
    private Vector3 touchPosition;

    // new Note 
    private float distanceToCamera = 0.3f;
    private Vector3 noteSpawnPosition;
    private Vector3 noteScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 scaleAmount = new Vector3(0.0009f, 0.0009f, 0.0009f); 
    private float noteY = 0.1f;
    private Coroutine zoomCoroutine; 

    // Keyboard
    private UnityEngine.TouchScreenKeyboard keyboard;
    public string keyboardText = "Enter Text"; 

    private void Awake() {
        arCamera = Camera.main;
    }

    private void Start() {
        stickyNotePrefab.tag = "Sticky Note";
    }

    // Create new note infront of camera 
    public void addNewNote(){
        if(inStickyNoteMode)
        {
            deleteNoteButton.SetActive(true);

            // Create new note infront of camera 
            noteSpawnPosition = arCamera.transform.forward * distanceToCamera + arCamera.transform.position;
            noteSpawnPosition = new Vector3(noteSpawnPosition.x, noteSpawnPosition.y - noteY, noteSpawnPosition.z);
            currentSelectedNote = Instantiate(stickyNotePrefab, noteSpawnPosition, arCamera.transform.rotation);
            currentSelectedNote.SetActive(true);
            currentSelectedNote.transform.localScale = noteScale;
        }
    }

    // Open phone keyboard 
    void inputText(){
        if(currentSelectedNote != null){
            keyboard = TouchScreenKeyboard.Open(keyboardText, TouchScreenKeyboardType.Default);
            keyboard.active = true;
            Debug.Log("Opened Keyboard");
        }
    }

    // Place note on selected workspace 
    void placeNote(Vector3 pos, GameObject workspace, string ID){
        GameObject addedStickyNote = (GameObject)Instantiate(currentSelectedNote, pos, Quaternion.identity);
        addedStickyNote.tag = "Sticky Note";
        addedStickyNote.name = "NewNote" + ID;
        addedStickyNote.transform.parent = workspace.transform;
        addedStickyNote.transform.position = new Vector3(pos.x, pos.y, pos.z-0.015f);
        addedStickyNote.transform.rotation = currentSelectedNote.transform.rotation;
        currentSelectedNote.SetActive(false);
        currentSelectedNote = null;
        noteScale = new Vector3(0.1f, 0.1f, 0.1f);
        keyboardText = "Enter Text";
        deleteNoteButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(inStickyNoteMode){
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;
                bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                if(Physics.Raycast(ray, out RaycastHit raycastHit) && !isOverUI)
                {
                    GameObject selectedObject = raycastHit.collider.gameObject;
                    // Clicked on note --> Open keyboard
                    if(selectedObject == currentSelectedNote){
                        inputText();
                    }
                    else if (raycastHit.collider.tag == "Wall Workspace")
                    {
                        GameObject tappedWorkspace = raycastHit.collider.gameObject;
                        string workspaceID = tappedWorkspace.name.Substring(15);
                        // Finds and assigns the child named "Configuration"
                        GameObject stickyNoteWorkspaceConfig = GameObject.Find("Configuration " + workspaceID);

                        // If the child was not found
                        if (stickyNoteWorkspaceConfig == null)
                        {
                            // Create the child
                            stickyNoteWorkspaceConfig = new GameObject("Configuration " + workspaceID);
                            stickyNoteWorkspaceConfig.transform.position = tappedWorkspace.transform.position;
                        }
                        // Adds sticky note on the tapped workspace
                        placeNote(raycastHit.point, stickyNoteWorkspaceConfig, workspaceID);
                    }
                }
            }

            // Pinch to scale note 
            if(Input.touchCount >= 2 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began){
                zoomCoroutine = StartCoroutine(ZoomDetection()); 
            }

            // Move current note with camera 
            if(currentSelectedNote != null){
                Vector3 targetPosition = arCamera.transform.forward * distanceToCamera + arCamera.transform.position;
                targetPosition = new Vector3(targetPosition.x, targetPosition.y - noteY, targetPosition.z);

                Vector3 targetRotation = Quaternion.Euler(0, 2.0f, 0) * arCamera.transform.forward;
                Vector3 cameraAngle = arCamera.transform.eulerAngles; 
                currentSelectedNote.transform.position = new Vector3(targetPosition.x, currentSelectedNote.transform.position.y, targetPosition.z);
                currentSelectedNote.transform.LookAt(targetRotation);
                currentSelectedNote.transform.eulerAngles = cameraAngle;
            }

            // Update current sticky note text 
            if(keyboard != null && keyboard.active == true){
                keyboardText = keyboard.text;
                currentSelectedNote.GetComponentInChildren<Text>().text = keyboardText;
            }

            if(deletedNote){
                // Remove currently selected note if exists 
                if (currentSelectedNote != null){
                    currentSelectedNote.SetActive(false);
                    currentSelectedNote = null;
                }
                noteScale = new Vector3(0.1f, 0.1f, 0.1f);
                deletedNote = false;
            }
        }
        else{
            // StopCoroutine(zoomCoroutine);
        }

    }

    IEnumerator ZoomDetection(){
        while(Input.touchCount >= 2){
            var pos1 = Input.GetTouch(0).position;
            var pos2 = Input.GetTouch(1).position;
            var pos1next = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
            var pos2next = Input.GetTouch(01).position - Input.GetTouch(1).deltaPosition;

            var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1next, pos2next);
            if (zoom > 1.0f){
                noteScale += scaleAmount;
            }
            if (zoom < 1.0f){
                noteScale -= scaleAmount;
            }

            currentSelectedNote.transform.localScale = noteScale;
            yield return null;
        }
    }

}
