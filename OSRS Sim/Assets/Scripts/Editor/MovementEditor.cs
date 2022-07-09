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

        Movement movement = (Movement)target;

        runEnergyRegen.floatValue = EditorGUILayout.Slider("Run Energy Regen", runEnergyRegen.floatValue, 0, 100);

        serializedObject.ApplyModifiedProperties();
    }
}
