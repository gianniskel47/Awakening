using UnityEditor;
using UnityEngine;

namespace BlazeAISpace 
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CoverShooterBehaviour))]
    public class CoverShooterBehaviourInspector : Editor
    {
        #region SERIALIZED PROPERTIES

        SerializedProperty moveSpeed,
        turnSpeed,
        idleAnim,
        moveAnim,
        idleMoveT,
        targetLocationUpdateTime,

        equipWeapon,
        equipAnim,
        equipDuration,
        onEquipEvent,

        unEquipAnim,
        unEquipDuration,
        onUnEquipEvent,
        equipAnimT,

        onStateEnter,
        onStateExit,

        distanceFromEnemy,
        attackDistance,
        layersCheckOnAttacking,
        shootingAnim,
        shootingAnimT,
        shootEvent,
        shootEvery,
        singleShotDuration,
        delayBetweenEachShot,
        totalShootTime,

        firstSightDecision,
        coverBlownDecision,
        attackEnemyCover,

        braveMeter,
        changeCoverFrequency,
        noCoverShootChance,

        callOthers,
        callRadius,
        showCallRadius,
        agentLayersToCall,
        callPassesColliders,
        callOthersTime,
        receiveCallFromOthers,

        moveForwards,
        moveForwardsToDistance,
        moveForwardsSpeed,
        moveForwardsAnim,

        moveBackwards,
        moveBackwardsDistance,
        moveBackwardsSpeed,
        moveBackwardsAnim,
        moveBackwardsAttack,

        forwardAndBackAnimT,

        strafe,
        strafeSpeed,
        strafeTime,
        strafeWaitTime,
        leftStrafeAnim,
        rightStrafeAnim,
        strafeAnimT,
        strafeLayersToAvoid,

        searchLocationRadius,
        timeToStartSearch,
        searchPoints,
        searchPointAnim,
        pointWaitTime,
        endSearchAnim,
        endSearchAnimTime,
        searchAnimsT,
        playAudioOnSearchStart,
        playAudioOnSearchEnd,

        returnPatrolAnim,
        returnPatrolAnimT,
        returnPatrolTime,
        playAudioOnReturnPatrol,

        playAudioOnChase,
        alwaysPlayOnChase,

        playAudioDuringShooting,
        alwaysPlayDuringShooting,

        playAudioOnMoveToShoot,
        alwaysPlayOnMoveToShoot;

        #endregion

        #region EDITOR VARIABLES

        bool displayshootEvents = true;
        int spaceBetween = 20;

        string[] tabs = {"General", "Attack", "Attack-Idle", "Call Others", "Search & Return", "Audios & Events"};
        int tabSelected = 0;
        int tabIndex = -1;

        CoverShooterBehaviour script;
        CoverShooterBehaviour[] scripts;

        #endregion

        #region UNITY METHODS

        void SetScripts()
        {
            script = (CoverShooterBehaviour) target;

            Object[] objs = targets;
            scripts = new CoverShooterBehaviour[objs.Length];
            for (int i = 0; i < objs.Length; i++) {
                scripts[i] = objs[i] as CoverShooterBehaviour;
            }
        }

        void OnEnable()
        {
            SetScripts();
            GetSelectedTab(); 

            moveSpeed = serializedObject.FindProperty("moveSpeed");
            turnSpeed = serializedObject.FindProperty("turnSpeed");
            idleAnim = serializedObject.FindProperty("idleAnim");
            moveAnim = serializedObject.FindProperty("moveAnim");
            idleMoveT = serializedObject.FindProperty("idleMoveT");
            targetLocationUpdateTime = serializedObject.FindProperty("targetLocationUpdateTime");


            equipWeapon = serializedObject.FindProperty("equipWeapon");
            equipAnim = serializedObject.FindProperty("equipAnim");
            equipDuration = serializedObject.FindProperty("equipDuration");
            equipAnimT = serializedObject.FindProperty("equipAnimT");
            onEquipEvent = serializedObject.FindProperty("onEquipEvent");
    
            unEquipAnim = serializedObject.FindProperty("unEquipAnim");
            unEquipDuration = serializedObject.FindProperty("unEquipDuration");
            onUnEquipEvent = serializedObject.FindProperty("onUnEquipEvent");


            onStateEnter = serializedObject.FindProperty("onStateEnter");
            onStateExit = serializedObject.FindProperty("onStateExit");


            distanceFromEnemy = serializedObject.FindProperty("distanceFromEnemy");
            attackDistance = serializedObject.FindProperty("attackDistance");
            layersCheckOnAttacking = serializedObject.FindProperty("layersCheckOnAttacking");
            shootingAnim = serializedObject.FindProperty("shootingAnim");
            shootingAnimT = serializedObject.FindProperty("shootingAnimT");
            shootEvent = serializedObject.FindProperty("shootEvent");
            shootEvery = serializedObject.FindProperty("shootEvery");
            singleShotDuration = serializedObject.FindProperty("singleShotDuration");
            delayBetweenEachShot = serializedObject.FindProperty("delayBetweenEachShot");
            totalShootTime = serializedObject.FindProperty("totalShootTime");


            firstSightDecision = serializedObject.FindProperty("firstSightDecision");
            coverBlownDecision = serializedObject.FindProperty("coverBlownDecision");
            attackEnemyCover = serializedObject.FindProperty("attackEnemyCover");


            braveMeter = serializedObject.FindProperty("braveMeter");
            changeCoverFrequency = serializedObject.FindProperty("changeCoverFrequency");
            noCoverShootChance = serializedObject.FindProperty("noCoverShootChance");


            callOthers = serializedObject.FindProperty("callOthers");
            callRadius = serializedObject.FindProperty("callRadius");
            showCallRadius = serializedObject.FindProperty("showCallRadius");
            agentLayersToCall = serializedObject.FindProperty("agentLayersToCall");
            callPassesColliders = serializedObject.FindProperty("callPassesColliders");
            callOthersTime = serializedObject.FindProperty("callOthersTime");
            receiveCallFromOthers = serializedObject.FindProperty("receiveCallFromOthers");


            moveForwards = serializedObject.FindProperty("moveForwards");
            moveForwardsToDistance = serializedObject.FindProperty("moveForwardsToDistance");
            moveForwardsSpeed = serializedObject.FindProperty("moveForwardsSpeed");
            moveForwardsAnim = serializedObject.FindProperty("moveForwardsAnim");


            moveBackwards = serializedObject.FindProperty("moveBackwards");
            moveBackwardsDistance = serializedObject.FindProperty("moveBackwardsDistance");
            moveBackwardsSpeed = serializedObject.FindProperty("moveBackwardsSpeed");
            moveBackwardsAnim = serializedObject.FindProperty("moveBackwardsAnim");
            forwardAndBackAnimT = serializedObject.FindProperty("forwardAndBackAnimT");
            moveBackwardsAttack = serializedObject.FindProperty("moveBackwardsAttack");


            strafe = serializedObject.FindProperty("strafe");
            strafeSpeed = serializedObject.FindProperty("strafeSpeed");
            strafeTime = serializedObject.FindProperty("strafeTime");
            strafeWaitTime = serializedObject.FindProperty("strafeWaitTime");
            leftStrafeAnim = serializedObject.FindProperty("leftStrafeAnim");
            rightStrafeAnim = serializedObject.FindProperty("rightStrafeAnim");
            strafeAnimT = serializedObject.FindProperty("strafeAnimT");
            strafeLayersToAvoid = serializedObject.FindProperty("strafeLayersToAvoid");


            searchLocationRadius = serializedObject.FindProperty("searchLocationRadius");
            timeToStartSearch = serializedObject.FindProperty("timeToStartSearch");
            searchPoints = serializedObject.FindProperty("searchPoints");
            searchPointAnim = serializedObject.FindProperty("searchPointAnim");
            pointWaitTime = serializedObject.FindProperty("pointWaitTime");
            endSearchAnim = serializedObject.FindProperty("endSearchAnim");
            endSearchAnimTime = serializedObject.FindProperty("endSearchAnimTime");
            searchAnimsT = serializedObject.FindProperty("searchAnimsT");
            playAudioOnSearchStart = serializedObject.FindProperty("playAudioOnSearchStart");
            playAudioOnSearchEnd = serializedObject.FindProperty("playAudioOnSearchEnd");


            returnPatrolAnim = serializedObject.FindProperty("returnPatrolAnim");
            returnPatrolAnimT = serializedObject.FindProperty("returnPatrolAnimT");
            returnPatrolTime = serializedObject.FindProperty("returnPatrolTime");
            playAudioOnReturnPatrol = serializedObject.FindProperty("playAudioOnReturnPatrol");


            playAudioOnChase = serializedObject.FindProperty("playAudioOnChase");
            alwaysPlayOnChase = serializedObject.FindProperty("alwaysPlayOnChase");

            playAudioDuringShooting = serializedObject.FindProperty("playAudioDuringShooting");
            alwaysPlayDuringShooting = serializedObject.FindProperty("alwaysPlayDuringShooting");

            playAudioOnMoveToShoot = serializedObject.FindProperty("playAudioOnMoveToShoot");
            alwaysPlayOnMoveToShoot = serializedObject.FindProperty("alwaysPlayOnMoveToShoot");
        }
        
        public override void OnInspectorGUI () 
        {
            DrawToolbar();
            BlazeAIEditor.RefreshAnimationStateNames(script.blaze.anim);
            EditorGUILayout.LabelField("Hover on any property below for insights", EditorStyles.helpBox);
            EditorGUILayout.Space(10);
            
            tabIndex = -1;

            switch (tabSelected)
            {
                case 0:
                    DrawGeneralTab(script);
                    break;
                case 1:
                    DrawAttackTab();
                    break;
                case 2:
                    DrawAttackIdleTab(script);
                    break;
                case 3:
                    DrawCallOthersTab(script);
                    break;
                case 4:
                    DrawSearchAndReturnTab(script);
                    break;
                case 5:
                    DrawAudiosAndEventsTab(script);
                    break;
            }

            EditorPrefs.SetInt("BlazeShooterTabSelected", tabSelected);
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region DRAW

        void DrawToolbar()
        {   
            GUILayout.BeginHorizontal();
            
            foreach (var item in tabs) {
                tabIndex++;

                if (tabIndex == 3) {
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space(0.2f);
                    GUILayout.BeginHorizontal();
                }

                if (tabIndex == tabSelected) {
                    // selected button
                    GUILayout.Button(item, BlazeAIEditor.ToolbarStyling(true), GUILayout.MinWidth(105), GUILayout.Height(40));
                }
                else {
                    // unselected buttons
                    if (GUILayout.Button(item, BlazeAIEditor.ToolbarStyling(false), GUILayout.MinWidth(105), GUILayout.Height(40))) {
                        // this will trigger when a button is pressed
                        tabSelected = tabIndex;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        void DrawGeneralTab(CoverShooterBehaviour script)
        {
            EditorGUILayout.LabelField("Speeds", EditorStyles.boldLabel);
            BlazeAIEditor.CheckDisableWithRootMotion(script.blaze, moveSpeed);
            EditorGUILayout.PropertyField(turnSpeed);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Animations", EditorStyles.boldLabel);
            BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Idle Anim", "idleAnim", ref script.idleAnim, scripts);
            BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Move Anim", "moveAnim", ref script.moveAnim, scripts);
            EditorGUILayout.PropertyField(idleMoveT);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Chase Location Update", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(targetLocationUpdateTime);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Equip Weapon", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(equipWeapon);
            if (script.equipWeapon) 
            {
                EditorGUILayout.Space();
                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Equip Anim", "equipAnim", ref script.equipAnim, scripts);
                EditorGUILayout.PropertyField(equipDuration);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(equipAnimT);
                EditorGUILayout.Space(spaceBetween);
                
                EditorGUILayout.PropertyField(onEquipEvent);
                EditorGUILayout.Space(5);

                EditorGUILayout.LabelField("Unequip weapon", EditorStyles.boldLabel);
                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Un Equip Anim", "unEquipAnim", ref script.unEquipAnim, scripts);
                EditorGUILayout.PropertyField(unEquipDuration);
                EditorGUILayout.Space(spaceBetween);

                EditorGUILayout.PropertyField(onUnEquipEvent);
            }
        }

        void DrawAttackTab()
        {
            EditorGUILayout.LabelField("Distances", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(distanceFromEnemy);
            EditorGUILayout.PropertyField(attackDistance);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Friendly-Fire Layers", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(layersCheckOnAttacking);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Shoot Animation", EditorStyles.boldLabel);
            BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Shooting Anim", "shootingAnim", ref script.shootingAnim, scripts);
            EditorGUILayout.PropertyField(shootingAnimT);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Timing", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(shootEvery);
            EditorGUILayout.PropertyField(singleShotDuration);
            EditorGUILayout.PropertyField(delayBetweenEachShot);
            EditorGUILayout.PropertyField(totalShootTime);
            EditorGUILayout.Space(spaceBetween);
            

            displayshootEvents = EditorGUILayout.Toggle("Display Attack Events", displayshootEvents);
            if (displayshootEvents) {
                EditorGUILayout.PropertyField(shootEvent);
                EditorGUILayout.Space();
            }
            else {
                EditorGUILayout.Space(spaceBetween);
            }
            

            EditorGUILayout.LabelField("Decisions", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(firstSightDecision);
            EditorGUILayout.PropertyField(coverBlownDecision);
            EditorGUILayout.PropertyField(attackEnemyCover);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("Braveness", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(braveMeter);
            EditorGUILayout.Space(spaceBetween);
            
            EditorGUILayout.LabelField("Change Cover", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(changeCoverFrequency);
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("No Cover Found Shoot Chance", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(noCoverShootChance);
        }

        void DrawAttackIdleTab(CoverShooterBehaviour script)
        {
            EditorGUILayout.LabelField("Move Forwards", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(moveForwards);
            if (script.moveForwards) {
                EditorGUILayout.PropertyField(moveForwardsToDistance);
                BlazeAIEditor.CheckDisableWithRootMotion(script.blaze, moveForwardsSpeed);
                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Move Forwards Anim", "moveForwardsAnim", ref script.moveForwardsAnim, scripts);
            }
            EditorGUILayout.Space(spaceBetween);

            if (script.moveForwards && script.moveBackwards) {
                EditorGUILayout.LabelField("For best behaviour, [Move Backwards Distance] is locked to be always less than [Move Forwards To Distance] - This is because both Move Forwards and Backwards are enabled.", BlazeAIEditor.BoxStyle());
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Move Backwards", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(moveBackwards);
            if (script.moveBackwards) {
                EditorGUILayout.PropertyField(moveBackwardsDistance);
                BlazeAIEditor.CheckDisableWithRootMotion(script.blaze, moveBackwardsSpeed);
                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Move Backwards Anim", "moveBackwardsAnim", ref script.moveBackwardsAnim, scripts);
                EditorGUILayout.PropertyField(moveBackwardsAttack);
            }
            EditorGUILayout.Space(spaceBetween);


            EditorGUILayout.LabelField("Animation T of Moving Forwards/Backwards", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(forwardAndBackAnimT);
            EditorGUILayout.Space(spaceBetween);


            EditorGUILayout.LabelField("Strafing", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(strafe);
            if (script.strafe) {
                BlazeAIEditor.CheckDisableWithRootMotion(script.blaze, strafeSpeed);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(strafeTime);
                EditorGUILayout.PropertyField(strafeWaitTime);
                EditorGUILayout.Space();

                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Left Strafe Anim", "leftStrafeAnim", ref script.leftStrafeAnim, scripts);
                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Right Strafe Anim", "rightStrafeAnim", ref script.rightStrafeAnim, scripts);
                EditorGUILayout.PropertyField(strafeAnimT);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(strafeLayersToAvoid);
            }
        }

        void DrawCallOthersTab(CoverShooterBehaviour script)
        {
            EditorGUILayout.LabelField("Call Others", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(callOthers);
            if (script.callOthers) {
                EditorGUILayout.Space();
                
                EditorGUILayout.PropertyField(callRadius);
                EditorGUILayout.PropertyField(showCallRadius);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(agentLayersToCall);
                EditorGUILayout.PropertyField(callPassesColliders);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(callOthersTime);
            }
            EditorGUILayout.PropertyField(receiveCallFromOthers);
        }

        void DrawSearchAndReturnTab(CoverShooterBehaviour script)
        {
            EditorGUILayout.LabelField("Searching Location", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(searchLocationRadius);
            if (script.searchLocationRadius) {
                EditorGUILayout.PropertyField(timeToStartSearch);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(searchPoints);
                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Search Point Anim", "searchPointAnim", ref script.searchPointAnim, scripts);
                EditorGUILayout.PropertyField(pointWaitTime);
                EditorGUILayout.Space();

                BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "End Search Anim", "endSearchAnim", ref script.endSearchAnim, scripts);
                EditorGUILayout.PropertyField(endSearchAnimTime);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(searchAnimsT);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(playAudioOnSearchStart);
                EditorGUILayout.PropertyField(playAudioOnSearchEnd);

                return;
            }
            
            EditorGUILayout.Space(spaceBetween);
            EditorGUILayout.LabelField("Returning To Patrol (Alert State)", EditorStyles.boldLabel);
            BlazeAIEditor.DrawPopupProperty(script.blaze.anim, "Return Patrol Anim", "returnPatrolAnim", ref script.returnPatrolAnim, scripts);
            EditorGUILayout.PropertyField(returnPatrolAnimT);
            EditorGUILayout.PropertyField(returnPatrolTime);
            EditorGUILayout.PropertyField(playAudioOnReturnPatrol);
        }

        void DrawAudiosAndEventsTab(CoverShooterBehaviour script)
        {
            EditorGUILayout.LabelField("Audios", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(playAudioOnChase);
            if (script.playAudioOnChase) {
                EditorGUILayout.PropertyField(alwaysPlayOnChase);
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(playAudioDuringShooting);
            if (script.playAudioDuringShooting) {
                EditorGUILayout.PropertyField(alwaysPlayDuringShooting);
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(playAudioOnMoveToShoot);
            if (script.playAudioOnMoveToShoot) {
                EditorGUILayout.PropertyField(alwaysPlayOnMoveToShoot);
            }
            EditorGUILayout.Space(spaceBetween);

            EditorGUILayout.LabelField("State Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onStateEnter);
            EditorGUILayout.PropertyField(onStateExit);
        }

        void GetSelectedTab()
        {
            if (EditorPrefs.HasKey("BlazeShooterTabSelected")) {
                tabSelected = EditorPrefs.GetInt("BlazeShooterTabSelected");
            }
            else {
                tabSelected = 0;
            }   
        }

        #endregion
    }
}