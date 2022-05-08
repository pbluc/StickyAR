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
    // Start is called before the first frame update
    void Start()
    {
        // Path of the file
        string path = Application.persistentDataPath + "/StickyAR Session Log.txt";
        // Read the text directly from the file
        StreamReader reader = new StreamReader(path);
        string fileContents = reader.ReadToEnd();
        reader.Close();

        string[] lines = fileContents.Split("\n"[0]);
        List<Workspace> savedWorkspaces = LoadWorkspaces(lines);
    }

    public List<Workspace> LoadWorkspaces(string[] lines)
    {
        List<Workspace> workspaces = new List<Workspace>();

        // TODO: Parse through lines to populate workspace objects

        return workspaces;
    }

    public class Workspace
    {
        public int id;
        public string name;
        public string creationDate;
        public List<StickyNote> stickyNotes;

        public Workspace(int id, string name, string creationDate)
        {
            this.id = id;
            this.name = name;
            this.creationDate = creationDate;
        }

        public override string ToString()
        {
            return id + Environment.NewLine + name + Environment.NewLine + creationDate + Environment.NewLine;
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

}
