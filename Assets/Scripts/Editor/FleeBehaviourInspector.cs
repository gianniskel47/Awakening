using UnityEngine;
using UnityEditor;
using BlazeAISpace;

namespace BlazeAISpace
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FleeBehaviour))]
    public class FleeBehaviourInspector : Editor
    {
        #region SERIALIZED PROPERTIES

        SerializedProperty distanceRun,

        moveSpeed,
        turnSpeed,

        moveAnim,
        moveAnimT,

        goToPosition,
        setPosition,
        showPosition,
        reachEvent,

        playAudio,
        alwaysPlayAudio,

        onStateEnter,
        onStateExit;

        #endregion

        #region EDITOR VARS

        FleeBehaviour script;
        FleeBehaviour[] scripts;
        int spaceBetween = 20;

        #endregion

        #region UNITY METHODS

        void SetScripts()
        {
            script = (FleeBehaviour) target;

            Object[] objs = targets;
            scripts = new FleeBehaviour[objs.Length];
            for (int i = 0; i < objs.Length; i++) {
                scripts[i] = objs[i] as FleeBehaviour;
            }
        }

        void OnEnable()
        {
            SetScripts();
            distanceRun = serializedObject.FindProperty("distanceRun");

            moveSpeed = serializedObject.FindProperty("moveSpeed");
            turnSpeed = serializedObject.FindProperty("turnSpeed");

            moveAnim = serializedObject.FindProperty("moveAnim");
            moveAnimT = serializedObject.FindProperty("moveAnimT");

            goToPosition = serializedObject.FindProperty("goToPosition");
            setPosition = serializedObject.FindProperty("setPosition");
            showPosition = serializedObject.FindProperty("showPosition");
            reachEvent = serializedObject.FindProperty("reachEvent");

            playAudio = serializedObject.FindProperty("playAudio");
            alwaysPlayAudio = serializedObject.FindProperty("alwaysPlayAudio");

            onStateEnter = serializedObject.FindProperty("onStateEnter");
            onStateExit = serializedObject.FindProperty("onStateExit");
        }

        protected virtual void OnSceneGUI()
        {
            if (script == null) return;

            if (script.showPosition) {
                DrawFleePosition();
            }
        }

        public override void OnInspectorGUI () 
        {
            EditorGUILayout.LabelField("Hover on any property below for insights", EditorStyles.helpBox);
            EditorGUILayout.Space(10);
            BlazeAIEditor.RefreshAnimationStateNames(script.blaze.anim);
            
            EditorGUILayout.LabelField("Distance Run", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(distanceRun);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Speeds", EditorStyles.boldLabel);
            BlazeAIEditor.CheckDisableWithRootMotion(script.blaze, moveSpeed);
            EditorGUILayout.PropertyField(turnSpeed);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
            BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Move Anim", "moveAnim", ref script.moveAnim, scripts);
            EditorGUILayout.PropertyField(moveAnimT);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Flee To Specific Location", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(goToPosition);
            if (script.goToPosition) {
                EditorGUILayout.PropertyField(setPosition);
                EditorGUILayout.PropertyField(showPosition);
                EditorGUILayout.PropertyField(reachEvent);
            }
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playAudio);
            if (script.playAudio) {
                EditorGUILayout.PropertyField(alwaysPlayAudio);
            }
            EditorGUILayout.Space(spaceBetween);
            
            
            EditorGUILayout.LabelField("State Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onStateEnter);
            EditorGUILayout.PropertyField(onStateExit);
            

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region EDITOR METHODS

        void DrawFleePosition()
        {
            EditorGUI.BeginChangeCheck();

            Vector3 currentPoint = script.setPosition;
            Vector3 newTargetPosition = Handles.PositionHandle(currentPoint, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Change Flee Position");
                script.setPosition = newTargetPosition;
            }
        }

        #endregion
    } 
}