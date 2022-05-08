using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteNote : MonoBehaviour
{
    public static List<GameObject> deletedNotes;
    public GameObject DeletedNotesButton;

    public static int currentIndex = 0; 

    void Start(){
        deletedNotes = new List<GameObject>();
        // arCamera = Camera.main;
    }
    
    public void deleteNote(){
        if(OpenNote.currentSelectedNote != null){
            
            OpenNote.currentSelectedNote.SetActive(false);
            
            // Add deleted note to list 
            deletedNotes.Add(OpenNote.currentSelectedNote);
            DeletedNotesButton.SetActive(true);

            OpenNote.currentSelectedNote = null;
            OpenNote.deletedNote = true;
        }
    }

}
