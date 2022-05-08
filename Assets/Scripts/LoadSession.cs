using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class LoadSession : MonoBehaviour
{
    public List<Workspace> savedWorkspaces;

    // Start is called before the first frame update
    void Start()
    {
        // Path of the file
        string path = Application.persistentDataPath + "/StickyAR Session Log.txt";
        // Check if a session log file exists
        if (File.Exists(path))
        {
            // Process the file and load workspaces here
            StreamReader reader = new StreamReader(path);

            savedWorkspaces = LoadWorkspaces(reader);
        }
    }

    private static List<Workspace> LoadWorkspaces(StreamReader reader)
    {
        List<Workspace> workspaces = new List<Workspace>();
        // Read and display lines from the file until the end of
        // the file is reached.
        while (reader.Peek() >= 0)
        {
            int workspaceId = int.Parse(reader.ReadLine());
            string workspaceName = reader.ReadLine();
            string creationDate = reader.ReadLine();

            List<StickyNote> savedStickyNotes = new List<StickyNote>();
            int numStickyNotes = int.Parse(reader.ReadLine());
            for (int i = 0; i < numStickyNotes; i++)
            {
                Vector3 stickyNotePosition = StringToVector3(reader.ReadLine());
                Quaternion stickyNoteOrientation = StringToQuaternion(reader.ReadLine());
                Vector3 stickyNoteScale = StringToVector3(reader.ReadLine());
                string stickyNoteMaterialName = reader.ReadLine();
                string stickyNoteText = reader.ReadLine();

                savedStickyNotes.Add(new StickyNote(stickyNotePosition, stickyNoteOrientation, stickyNoteScale, stickyNoteMaterialName, stickyNoteText));
            }

            workspaces.Add(new Workspace(workspaceId, workspaceName, creationDate, savedStickyNotes));
        }
        reader.Close();

        return workspaces;
    }

    private static Quaternion StringToQuaternion(string sQuaternion)
    {
        // Remove the parentheses
        if (sQuaternion.StartsWith("(") && sQuaternion.EndsWith(")"))
        {
            sQuaternion = sQuaternion.Substring(1, sQuaternion.Length - 2);
        }

        // Split the items
        string[] sArray = sQuaternion.Split(',');

        // Store as a Quaternion
        Quaternion result = new Quaternion(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]),
            float.Parse(sArray[3]));

        return result;
    }

    private static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // Split the items
        string[] sArray = sVector.Split(',');

        // Store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public class Workspace
    {
        public int id;
        public string name;
        public string creationDate;
        public List<StickyNote> stickyNotes;

        public Workspace(int id, string name, string creationDate, List<StickyNote> stickyNotes)
        {
            this.id = id;
            this.name = name;
            this.creationDate = creationDate;
            this.stickyNotes = stickyNotes;
        }

        public override string ToString()
        {
            return id + Environment.NewLine + name + Environment.NewLine + creationDate + Environment.NewLine;
        }
    }

    public class StickyNote
    {
        public Vector3 position;
        public Quaternion orientation;
        public Vector3 scale;
        public string materialName;
        public string text;

        public StickyNote(Vector3 position, Quaternion orientation, Vector3 scale, string materialName, string text)
        {
            this.position = position;
            this.orientation = orientation;
            this.scale = scale;
            this.materialName = materialName;
            this.text = text;
        }

        public override string ToString()
        {
            return position.ToString() + Environment.NewLine + orientation.ToString() + Environment.NewLine + scale.ToString() + Environment.NewLine + materialName + Environment.NewLine + text + Environment.NewLine;
        }
    }

}
