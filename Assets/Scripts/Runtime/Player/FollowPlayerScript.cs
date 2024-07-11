using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] InputReader inputReader;
   // [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private bool isRotatingCam = false;

    private float rotationY;
    public float mouseSensitivity;

    private void Start()
    {
        inputReader.RotateCamEvent += InputReader_RotateCamEvent;
        inputReader.RotateCamCanceledEvent += InputReader_RotateCamCanceledEvent;
    }

    private void InputReader_RotateCamCanceledEvent()
    {
        isRotatingCam = false;
        StartCoroutine(LerpCoroutine());
    }

    private void InputReader_RotateCamEvent()
    {
        StopAllCoroutines();
        isRotatingCam = true;
    }

    private void Update()
    {
        if (isRotatingCam)
        {
            rotationY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -360, 360);
        }
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;

        transform.position = playerTransform.position;
        transform.eulerAngles = new Vector3 (0, rotationY, 0);
    }

    private IEnumerator LerpCoroutine()
    {
        float elapsedTime = 0;
        float waitTime = 2f;

        while (elapsedTime < waitTime)
        {
            rotationY = Mathf.Lerp(rotationY, 0, elapsedTime/waitTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        rotationY = 0;
        yield return null;
    }

    public void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
    }
}
