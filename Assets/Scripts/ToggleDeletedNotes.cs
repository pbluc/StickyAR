using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDeletedNotes : MonoBehaviour
{

    public RawImage icon;
    public Texture restoreButton;
    public Texture acceptButton;

    public GameObject DeletedNotesButton;
    public GameObject leftScrollButton;
    public GameObject rightScrollButton; 

    public bool scrollOn = false;
    public int currentIndex = 0; 

    public GameObject addNoteButton;
    public GameObject deleteNoteButton;

    private Vector3 noteSpawnPosition;
    private Camera arCamera;
    private float distanceToCamera = 0.3f;
    private float noteY = 0.1f;
    private Vector3 noteScale = new Vector3(0.1f, 0.1f, 0.1f);
    private GameObject deleted;

    private void Start() {
        arCamera = Camera.main;
    }


    // Toggle delete restoring icon 
    public void ToggleDeleteButton(){
        if(scrollOn == false){
            icon.texture = acceptButton;
            scrollOn = true;
            addNoteButton.SetActive(false);
            deleteNoteButton.SetActive(false);
        }
        else{
            icon.texture = restoreButton;
            scrollOn = false;
            addNoteButton.SetActive(true);
            deleteNoteButton.SetActive(true);
            acceptDeletedNote();
        }
    }

    // Show current deleted notes 
    public void loadDeletedNotes(){
        
        if(OpenNote.currentSelectedNote == null){ 
            noteSpawnPosition = arCamera.transform.forward * distanceToCamera + arCamera.transform.position;
            noteSpawnPosition = new Vector3(noteSpawnPosition.x, noteSpawnPosition.y - noteY, noteSpawnPosition.z);
            deleted = DeleteNote.deletedNotes[currentIndex];

            if(DeleteNote.deletedNotes.Count == 0){
                DeletedNotesButton.SetActive(false);
            }

            deleted.transform.position = noteSpawnPosition;
            deleted.transform.Rotate(arCamera.transform.rotation.x,arCamera.transform.rotation.y,arCamera.transform.rotation.z);
            deleted.SetActive(true);
            deleted.transform.localScale = noteScale;
            OpenNote.currentSelectedNote = deleted;
        }
    }

    // Instantiate deleted note and remove from stack 
    public void acceptDeletedNote(){
        if(OpenNote.currentSelectedNote != null && DeleteNote.deletedNotes.Count > 0){
            OpenNote.currentSelectedNote.SetActive(false);
            noteSpawnPosition = arCamera.transform.forward * distanceToCamera + arCamera.transform.position;
            noteSpawnPosition = new Vector3(noteSpawnPosition.x, noteSpawnPosition.y - noteY, noteSpawnPosition.z);
            GameObject reloadNote = Instantiate(DeleteNote.deletedNotes[currentIndex]);
            reloadNote.transform.position = noteSpawnPosition;
            reloadNote.transform.Rotate(arCamera.transform.rotation.x,arCamera.transform.rotation.y,arCamera.transform.rotation.z);
            reloadNote.SetActive(true);
            reloadNote.transform.localScale = noteScale;
            OpenNote.currentSelectedNote = reloadNote;
            DeleteNote.deletedNotes.RemoveAt(currentIndex);
            currentIndex = 0;
            Debug.Log("New Deleted Note Count " + DeleteNote.deletedNotes.Count);
        }
    }

    // Shift index left 
    public void scrollLeft(){
        if (currentIndex != 0){
            currentIndex -= 1;
            OpenNote.currentSelectedNote.SetActive(false);
            OpenNote.currentSelectedNote = null;
            loadDeletedNotes();
        }
    }

    // Shift index right 
    public void scrollRight(){
        if (currentIndex < DeleteNote.deletedNotes.Count-1){
            currentIndex += 1;
            OpenNote.currentSelectedNote.SetActive(false);
            OpenNote.currentSelectedNote = null;
            loadDeletedNotes();
        }
    }

    // Activate buttons based on stack count 
    private void Update() {
        if(scrollOn){
            if(DeleteNote.deletedNotes.Count == 1){
                leftScrollButton.SetActive(false);
                rightScrollButton.SetActive(false);
            }
            else if(DeleteNote.deletedNotes.Count > 0 && currentIndex == 0){
                leftScrollButton.SetActive(false);
                rightScrollButton.SetActive(true);
            }
            else if(DeleteNote.deletedNotes.Count > 0 && currentIndex == DeleteNote.deletedNotes.Count - 1){
                leftScrollButton.SetActive(true);
                rightScrollButton.SetActive(false);
            }
            else if(DeleteNote.deletedNotes.Count > 0 && currentIndex > 0 && currentIndex < DeleteNote.deletedNotes.Count-1){
                leftScrollButton.SetActive(true);
                rightScrollButton.SetActive(true);
            }
        }
        else{
            leftScrollButton.SetActive(false);
            rightScrollButton.SetActive(false);
        }
    }
}
