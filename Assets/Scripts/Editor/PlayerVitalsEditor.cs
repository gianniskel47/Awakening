using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerVitals))]
public class PlayerVitalsEditor : Editor
{
    private PlayerVitals playerVitals;

    private void OnEnable() {
        playerVitals = (PlayerVitals)target;
    }
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var value =  EditorGUILayout.FloatField("Damage to take",10f);
        if (GUILayout.Button("Test Take Damage")) {
            playerVitals.TakeDamage(value,playerVitals.OwnerClientId);
        }

    }
}
