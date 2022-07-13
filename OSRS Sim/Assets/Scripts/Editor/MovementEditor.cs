using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Movement))]
public class MovementEditor : Editor
{
    private SerializedProperty runEnergyRegen;

    protected virtual void OnEnable()
    {
        runEnergyRegen = serializedObject.FindProperty("runEnergyRegen");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();
    }
}
