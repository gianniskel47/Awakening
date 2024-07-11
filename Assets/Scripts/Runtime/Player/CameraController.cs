using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Transform camHolder;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float fieldOfViewMin = 5f;
    [SerializeField] private float fieldOfViewMax = 50f;
    [SerializeField] private float fieldOfViewIncreaseAmount = 1f;
    private float targetFielOfView = 50;

    public float rotationY;
    public float maxAngle;
    public float minAngle;
    public float mouseSensitivity;

    private bool isRotatingCam = false;
    private Vector2 mousePos;

    //TEST
    [SerializeField] Transform player;
    [SerializeField] float offsetZ;
    [SerializeField] float offsetY;


    private void Awake() {
      //  virtualCamera.m_Lens.FieldOfView = targetFielOfView;
    }

    private void OnEnable() {
        inputReader.ZoomEvent += InputReader_ZoomEvent;
        inputReader.RotateCamEvent += InputReader_RotateCamEvent;
        inputReader.RotateCamCanceledEvent += InputReader_RotateCamCanceledEvent;
        inputReader.LookEvent += InputReader_LookEvent;

        transform.eulerAngles = new Vector3(48,0,0);
        transform.localPosition = new Vector3(0, offsetY, offsetZ);
    }

    private void InputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
    }

    private void InputReader_RotateCamCanceledEvent()
    {
        isRotatingCam = false;
    }

    private void InputReader_RotateCamEvent()
    {
        isRotatingCam = true;
    }

    private void Update()
    {
      //  if (!IsOwner) return;

        if (isRotatingCam)
        {
            rotationY += mousePos.x * mouseSensitivity * Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
      //  transform.eulerAngles = new Vector3 (48, rotationY, 0);
    }

    private void InputReader_ZoomEvent(float arg0) {
  /*      if(arg0 < 0 ) {
            targetFielOfView += fieldOfViewIncreaseAmount;
        }
        if(arg0 > 0 ) {
            targetFielOfView -= fieldOfViewIncreaseAmount;
        }
        targetFielOfView = Mathf.Clamp(targetFielOfView, fieldOfViewMin, fieldOfViewMax);

        virtualCamera.m_Lens.FieldOfView = targetFielOfView;*/
    }

}
