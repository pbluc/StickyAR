using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteNote : MonoBehaviour
{
    public void deleteNote(){
        if(OpenNote.currentSelectedNote != null){
            Debug.Log("Deleted Note");
            OpenNote.currentSelectedNote.SetActive(false);
            OpenNote.currentSelectedNote = null;
            OpenNote.deletedNote = true;
        }
    }

}
