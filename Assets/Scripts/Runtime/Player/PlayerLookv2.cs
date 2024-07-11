using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerLookv2 : NetworkBehaviour
{
    [SerializeField] InputReader inputReader;
    public Vector2 mousePos;
    public bool isAiming = false;

    public Transform headBone; // Assign the head bone in the inspector
    public Transform playerBody; // Assign the player's body transform in the inspector
    public float rotationSpeed = 5f; // Speed of the smooth rotation

    private Quaternion initialHeadRotation; // Store the initial rotation of the head
    private Quaternion targetHeadRotation; // Target rotation for the head
    private Quaternion targetBodyRotation; // Target rotation for the body

    public float headMaxAngle = 15f; // Maximum angle the head can rotate

    [SerializeField] LayerMask floorLayer;

    public override void OnNetworkSpawn()
    {
        inputReader.LookEvent += InputReader_LookEvent;
        inputReader.AimEventPerformed += InputReader_AimEventPerformed;
        inputReader.AimEventCanceled += InputReader_AimEventCanceled;

        // Store the initial rotations of the head and chest
        initialHeadRotation = headBone.rotation;

        // Initialize target rotations
        targetHeadRotation = initialHeadRotation;
    }

    private void InputReader_AimEventCanceled()
    {
        isAiming = false;
    }

    private void InputReader_AimEventPerformed()
    {
        isAiming = true;
    }

    private void InputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        if (isAiming) // Right mouse button
        {
            // Cast a ray from the mouse position into the world
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
            {
                Vector3 targetPosition = hit.point;
                UpdateTargetRotations(targetPosition);
            }
        }
        else
        {
            // Smoothly rotate back to the initial rotations
            UpdateDefaultRotations();
        }

        // Smoothly rotate towards the target rotations
        headBone.rotation = Quaternion.Slerp(headBone.rotation, targetHeadRotation, Time.deltaTime * rotationSpeed);
        playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetBodyRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateTargetRotations(Vector3 targetPosition)
    {
        // Calculate direction from head to target
        Vector3 direction = targetPosition - headBone.position;
        direction.y = 0; // Keep the direction strictly horizontal

        // Calculate the target rotation in world space
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Get the player's current rotation around the Y-axis
        float playerYRotation = playerBody.eulerAngles.y;

        // Update the target rotations for head and chest
        targetHeadRotation = CalculateClampedRotation(targetRotation, playerYRotation, -90f, 90f);
        targetBodyRotation = CalculateClampedRotation(targetRotation, playerYRotation, -360, 360);
    }

    private Quaternion CalculateClampedRotation(Quaternion targetRotation, float playerYRotation, float minY, float maxY)
    {
        // Convert the target rotation to global Euler angles
        Vector3 globalEulerAngles = targetRotation.eulerAngles;

        // Calculate the relative Y rotation
        float relativeYRotation = Mathf.Repeat(globalEulerAngles.y - playerYRotation + 180f, 360f) - 180f;

        // Clamp the relative Y rotation
        relativeYRotation = Mathf.Clamp(relativeYRotation, minY, maxY);

        // Calculate the final Y rotation in global space
        float finalYRotation = playerYRotation + relativeYRotation;

        // Return the final rotation quaternion
        return Quaternion.Euler(0, finalYRotation, 0);
    }

    private void UpdateDefaultRotations()
    {
        // Get the player's current rotation around the Y-axis
        float playerYRotation = playerBody.eulerAngles.y;

        // Calculate the default rotations based on player facing direction
        targetHeadRotation = Quaternion.Euler(0, playerYRotation, 0) * initialHeadRotation;
        targetBodyRotation = playerBody.rotation;
    }
}
