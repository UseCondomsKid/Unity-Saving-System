using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Saveable))]
public class SaveableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        Saveable t = (Saveable)target;

        if (GUILayout.Button("Generate Id"))
        {
            t.GenerateId();
        }
    }
}
