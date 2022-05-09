using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SaveSession : MonoBehaviour
{

    // Write current session information to mobile device
    public void SaveCurrentSession()
    {
       // Path of the file
        string path = Application.persistentDataPath + "/StickyAR Session Log.txt";

        // Information about the session
        string fileContents = LoadFileContents();

        string createText = fileContents + Environment.NewLine;
        // Write contents containing information about session to file
        File.WriteAllText(path, createText);

        // TODO: Display confirmation of session having been saved
    }

    private string LoadFileContents()
    {
        string contents = "";

        GameObject[] allWorkspaces = GameObject.FindGameObjectsWithTag("Wall Workspace");

        foreach (GameObject workspace in allWorkspaces)
        {
            string workspaceID = workspace.name.Substring(15);
            // Finds and assigns the child named "Configuration"
            GameObject stickyNoteWorkspaceConfig = GameObject.Find("Configuration " + workspaceID);

            // If the child was found
            if (stickyNoteWorkspaceConfig != null)
            {
                // Check to see if config contains sticky notes
                int numStickyNotes = stickyNoteWorkspaceConfig.transform.childCount;
                if (numStickyNotes > 0)
                {
                    // Add workspace information to file contents

                    // Write the unique ID of the workspace
                    contents += workspace.transform.GetChild(1).name + Environment.NewLine;

                    // Write the name (default or user given) of the workspace
                    if (workspace.transform.GetChild(0).name == "Name")
                    {
                        contents += workspace.name + Environment.NewLine;
                    }
                    else
                    {
                        contents += workspace.transform.GetChild(0).name + Environment.NewLine;
                    }

                    // Write creation date of workspace
                    contents += workspace.transform.GetChild(3).name + Environment.NewLine;

                    // Write number of sticky notes on workspace
                    contents += numStickyNotes + Environment.NewLine;

                    // Write information about the sticky notes
                    for (int i = 0; i < numStickyNotes; i++)
                    {
                        GameObject stickyNote = stickyNoteWorkspaceConfig.transform.GetChild(i).gameObject;
                        // Write the position, rotation, and localScale of sticky note to contents
                        contents += stickyNote.transform.position.ToString() + Environment.NewLine;
                        contents += stickyNote.transform.rotation.ToString() + Environment.NewLine;
                        contents += stickyNote.transform.localScale.ToString() + Environment.NewLine;
                        // Write the name of the material on sticky note to contents
                        contents += stickyNote.GetComponent<Renderer>().material.name + Environment.NewLine;
                        // Write text of sticky note to contents
                        Text stickyNoteText = stickyNote.transform.Find("StickyNoteCanvas/StickyNoteText").gameObject.GetComponent<Text>();
                        contents += stickyNoteText.text + Environment.NewLine;
                    }

                }
            }
        }
        return contents;
    }
}
