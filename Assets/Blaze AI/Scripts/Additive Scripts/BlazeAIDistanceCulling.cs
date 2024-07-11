using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlazeAISpace;

[AddComponentMenu("Blaze AI/Additive Scripts/Blaze AI Distance Culling")]
public class BlazeAIDistanceCulling : MonoBehaviour
{
    [Tooltip("Automatically get the game camera.")]
    public bool autoCatchCamera = true;

    [Tooltip("The player or camera to calculate the distance between it and the AIs.")]
    public Transform cameraOrPlayer;

    [Min(0), Tooltip("If an AI distance is more than this set value then the it will get culled.")]
    public float distanceToCull = 30;
    
    [Range(0, 30), Tooltip("Run the cycle every set frames. The bigger the number, the better it is for performance but less accurate.")]
    public int cycleFrames = 7; 
    [Range(0, 10), Tooltip("This sets how many frames to rest before moving to the next calculation in the loop. This helps spread the distance culling calculations across several frames which improves performance.")]
    public int restFrames = 2;

    [Tooltip("If set to true, the culling will be disabling Blaze only and playing the idle animation set in the [Anim To Play On Cull] property in Blaze inspector. When within range again, Blaze will re-enable.")]
    public bool disableBlazeOnly;


    #region SYSTEM VARIABLES

    public static BlazeAIDistanceCulling instance;

    List<BlazeAI> agentsList = new List<BlazeAI>();
    int framesPassed = 0;
    
    int restFramesPassed = 0;
    bool isLooping = false;

    #endregion

    #region UNITY & SYSTEM METHODS

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }

        if (autoCatchCamera) {
            cameraOrPlayer = Camera.main.transform;
            return;
        }

        if (cameraOrPlayer == null) {
            Debug.LogWarning("No camera has been set in the camera property in the Blaze AI Distance Culling component.");
        }

        if (restFrames < 0) {
            restFrames = 0;
        }

        if (cycleFrames < 0) {
            cycleFrames = 0;
        }
    }

    public virtual void Update()
    {
        // prevent continuation if no camera set
        if (cameraOrPlayer == null) {
            return;
        }

        if (isLooping) return;

        // increment the frames for the cycle
        if (framesPassed < cycleFrames) {
            framesPassed++;
            return;
        }

        framesPassed = 0;

        if (!isLooping) {
            StartCoroutine("RunCulling");
        }
    }

    public virtual IEnumerator RunCulling()
    {
        isLooping = true;

        int max = agentsList.Count;
        for (int i=0; i<max; i++) 
        {
            if (i > 0) {
                while (restFramesPassed < restFrames) {
                    restFramesPassed++;
                    yield return null;
                }
            }

            restFramesPassed = 0;
            
            if (i > agentsList.Count-1) {
                break;
            }

            BlazeAI blaze = agentsList[i];
            if (blaze == null) {
                agentsList.RemoveAt(i);
                continue;
            }

            // calculate the distance using sqr magnitude since it's faster than Vector3.Distance()
            float agentDistance = (blaze.transform.position - cameraOrPlayer.position).sqrMagnitude;

            // if distance is larger than set -> cull the AI
            if (agentDistance > distanceToCull * distanceToCull) 
            {
                if (disableBlazeOnly) {
                    if (!blaze.enabled) {
                        continue;
                    }

                    blaze.enabled = false;
                    
                    if (blaze.state != BlazeAI.State.death) {
                        PlayCullAnim(blaze);
                    }

                    continue;
                }

                if (!blaze.gameObject.activeSelf) {
                    continue;
                }

                blaze.gameObject.SetActive(false);
                continue;
            }
            

            // reaching this point means distance is less than set -> re-enable AI
            
            
            if (disableBlazeOnly) 
            {
                if (!blaze.enabled) {
                    blaze.enabled = true;
                    continue;
                }
            }

            if (!blaze.gameObject.activeSelf) {
                blaze.gameObject.SetActive(true);
                PlayCullAnim(blaze);
            }
        }

        isLooping = false;
    }

    #endregion

    #region APIs

    // add agent to the list of culling
    public virtual void AddAgent(BlazeAI agent)
    {
        if (agentsList.Contains(agent)) {
            return;
        }

        agentsList.Add(agent);
    }

    // remove agent from the list of culling
    public virtual void RemoveAgent(BlazeAI agent)
    {
        if (!agentsList.Contains(agent)) {
            return;
        }

        agentsList.Remove(agent);
    }

    // check if passed agent is in the list of culling
    public bool CheckAgent(BlazeAI agent)
    {
        if (agentsList.Contains(agent)) {
            return true;
        }

        return false;
    }

    // play the cull animation
    void PlayCullAnim(BlazeAI blaze)
    {
        if (blaze.animToPlayOnCull.Length > 0 && blaze.animToPlayOnCull != null) {
            blaze.animManager.Play(blaze.animToPlayOnCull, 0.25f);
        }
    }

    #endregion
}