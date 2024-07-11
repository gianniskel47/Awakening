using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStateManager : NetworkBehaviour
{
    [Header("Other Scripts")]
    public PlayerSensors playerSensors;
    public PlayerVitals playerVitals;
    [Tooltip("This is the starting state")]
    [SerializeField] private PlayerState currentState;
    [Header("PlayerStates")]
    public IdleState idleState;
    public WalkState walkState;
    public RunState runState;
    public CrouchState clouchState;
    public CrouchIdleState crouchIdleState;
    public CrouchWalkState crouchWalkState;
    public AirborneState airborneState;

    [SerializeField] private InputReader inputReader;
    public Animator playerAnimator;
    [SerializeField] float animationBlendSpeed;
    public Rigidbody playerRigitbody;
    public CharacterController playerController;

    [SerializeField] SO_GameObj soGameObj;

    public Vector2 moveInput {  get; private set; }
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 moveAmmount;

    private float elapsedTime = 0f;

    [SerializeField] GameObject cameraHolder;

    private void OnEnable() {

        //Deffault collider values
        playerController.center = new Vector3(0, .9f, 0);
        playerController.height = 1.9f;
    }

    private void InputReader_CrouchEvent() {
        if (isRunning) return;

        isCrouching = !isCrouching;
    }

    private void InputReader_RunEventCanceled() {
        isRunning = false;
    }

    private void InputReader_RunEventPerformed() {
        isRunning = true;
    }

    private void InputReader_MoveEvent(Vector2 arg0) {
        moveInput = arg0;
        moveInput.Normalize();
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            inputReader.MoveEvent += InputReader_MoveEvent;
            inputReader.RunEventPerformed += InputReader_RunEventPerformed;
            inputReader.RunEventCanceled += InputReader_RunEventCanceled;
            inputReader.CrouchEvent += InputReader_CrouchEvent;


            moveAmmount = transform.position;

            playerVitals.OnTired += PlayerVitals_OnTired;

            playerAnimator = GetComponentInChildren<Animator>();

            currentState.Enter(this);

          //  gameObject.tag = "Player";

            cameraHolder.SetActive(true);
        }
       
    }

    private void PlayerVitals_OnTired(object sender, System.EventArgs e) {
        isRunning = false ;
    }

    private void Update() {
        currentState.Do(this);

        if(currentState == runState) {
            playerVitals.CurrentStamina = -10 * Time.deltaTime;
        }
        if(currentState == walkState) {
            playerVitals.CurrentStamina = 2 * Time.deltaTime;
        }
        if(currentState == idleState) {
            playerVitals.CurrentStamina = 5 * Time.deltaTime;
        }
        //print(playerVitals.CurrentStamina);

    }
    private void FixedUpdate() {
        currentState.FixedDo(this);

        //rotate body
        Debug.Log("WALKINGGG");
       // playerVisual.rotation = Quaternion.Euler(0, 0, 0);
        float rotateSpeed = 5f;
        transform.forward = Vector3.Slerp(transform.forward,moveDir , Time.deltaTime * rotateSpeed);
    }

    public void SwitchState(PlayerState state) {
        currentState.Exit(this);

        currentState = state;

        state.Enter(this);
    }
    
    public void SetRigitbody() {
        SetRigitbodyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetRigitbodyServerRpc() {
        playerRigitbody.isKinematic = true;
        SetRigitbodyClientRpc();
    }

    [ClientRpc]
    private void SetRigitbodyClientRpc() {
        playerRigitbody.isKinematic = true;
    }

    public void ChangeLocomotion(string floatName, float finalNumber)
    {
        StopAllCoroutines();
        StartCoroutine(LerpValue(floatName, finalNumber));
    }

    private IEnumerator LerpValue(string floatName, float finalNumber)
    {
        elapsedTime = 0;

        while (elapsedTime < animationBlendSpeed)
        {
            playerAnimator.SetFloat(floatName, Mathf.Lerp(playerAnimator.GetFloat(floatName), finalNumber, elapsedTime/animationBlendSpeed));

            elapsedTime += Time.deltaTime;

            yield return null;            
        }
        // Make sure we got there

        playerAnimator.SetFloat(floatName, finalNumber);

        yield return null;
    }

}
