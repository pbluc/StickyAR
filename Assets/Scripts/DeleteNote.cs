using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteNote : MonoBehaviour
{
    public Stack<GameObject> deletedNotes;
    public GameObject DeletedNotesButton;

    private Vector3 noteSpawnPosition;
    private Camera arCamera;
    private float distanceToCamera = 0.3f;
    private float noteY = 0.1f;
    private Vector3 noteScale = new Vector3(0.1f, 0.1f, 0.1f);
    private GameObject deleted;


    void Start(){
        deletedNotes = new Stack<GameObject>();
        arCamera = Camera.main;
    }

    
    public void deleteNote(){
        if(OpenNote.currentSelectedNote != null){
            
            Debug.Log("Deleted Note");
            OpenNote.currentSelectedNote.SetActive(false);
            
            deletedNotes.Push(OpenNote.currentSelectedNote);
            DeletedNotesButton.SetActive(true);

            OpenNote.currentSelectedNote = null;
            OpenNote.deletedNote = true;
        }
    }

    public void loadDeletedNotes(){
        
        if(OpenNote.currentSelectedNote == null){
                
                noteSpawnPosition = arCamera.transform.forward * distanceToCamera + arCamera.transform.position;
                noteSpawnPosition = new Vector3(noteSpawnPosition.x, noteSpawnPosition.y - noteY, noteSpawnPosition.z);
                deleted = deletedNotes.Pop();

                if(deletedNotes.Count == 0){
                    DeletedNotesButton.SetActive(false);
                }

                deleted.transform.position = noteSpawnPosition;
                deleted.transform.Rotate(arCamera.transform.rotation.x,arCamera.transform.rotation.y,arCamera.transform.rotation.z);
                deleted.SetActive(true);
                deleted.transform.localScale = noteScale;
                OpenNote.currentSelectedNote = deleted;
        }
    }

}
