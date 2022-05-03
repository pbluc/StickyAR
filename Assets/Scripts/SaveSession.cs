using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSession : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Write current session information to mobile device
    public void SaveCurrentSession()
    {
       // Path of the file
        string path = Application.persistentDataPath + "/StickyAR Session Log.txt";

        // Information about the session
        string fileContents = LoadFileContents();

        // Create file if it doesn't exist
        if (!File.Exists(path))
        {
            string createText = fileContents + Environment.NewLine;
            // Write contents containing information about session to file
            File.WriteAllText(path, createText);
        }

        // Append some extra text
        string appendText = "This is extra text" + Environment.NewLine;
        File.AppendAllText(path, appendText);
    }

    public string LoadFileContents()
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
                    // TODO: Add workspace information to file contents

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

                    // TODO: Write creation date of workspace

                    for (int i = 0; i < numStickyNotes; i++)
                    {
                        GameObject stickyNote = stickyNoteWorkspaceConfig.transform.GetChild(i).gameObject;

                        contents += stickyNote.transform.position.ToString() + Environment.NewLine;
                        contents += stickyNote.transform.rotation.ToString() + Environment.NewLine;
                        contents += stickyNote.transform.localScale.ToString() + Environment.NewLine;

                        // TODO: Write material and text of sticky note to contents
                    }

                }
            }

            contents += "###" + Environment.NewLine;
        }
        return contents;
    }
}
