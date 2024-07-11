using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] LayerMask aimLayers;

    CharacterController characterController;
    AnimationController animationController;

    HandleEquipedItems handleEquipedItems;

    [Header("SO")]
    [SerializeField] InputReader inputReader;
    [SerializeField] SO_GameObj soGameObj;

    [Header("MOVEMENT")]
    public float walkingSpeed;
    public bool isAiming = false;
    public float speed;
    private Vector2 move;
    private Vector2 mousePos;
    private Vector3 rotationTarget;

    [Header("CROUCH")]
    [SerializeField] private float crouchHeight = 1.3f;
    [SerializeField] private float speedWhileCrouching = 1f;
    public float crouchSpeed = 1f;
    [SerializeField] private float originalHeight;
    [SerializeField] private Vector3 crouchColliderCenter = new Vector3(0,.6f,0);
    [SerializeField] private Vector3 defaultColliderCenter;
    private Coroutine crouchCoroutine;
    public bool isCrouching = false;

    [Header("RUNNING")]
    public float runningSpeed;
    private bool isRunning = false;

    [Header("ATTACK")]
    [SerializeField] float attackCooldown;
    private float counter = 0;

    public float lastClickedTime;
    public float lastComboEnd;
    public int comboCounter;

    private void Start()
    {
        if (!IsOwner) return;     

        inputReader.MoveEvent += InputReader_MoveEvent;
        inputReader.LookEvent += InputReader_LookEvent;
        inputReader.AimEventPerformed += InputReader_AimEventPerformed;
        inputReader.AimEventCanceled += InputReader_AimEventCanceled;
        inputReader.CrouchEvent += InputReader_CrouchEvent;
        inputReader.RunEventPerformed += InputReader_RunEventPerformed;
        inputReader.RunEventCanceled += InputReader_RunEventCanceled;
        inputReader.InteractEvent += InputReader_InteractEvent;
        inputReader.ShowInventoryEvent += InputReader_ShowInventoryEvent;
        inputReader.HideInventoryEvent += InputReader_HideInventoryEvent;

        characterController = GetComponent<CharacterController>();
        animationController = GetComponentInChildren<AnimationController>();
        handleEquipedItems = GetComponent<HandleEquipedItems>();

        originalHeight = characterController.height;
        defaultColliderCenter = characterController.center;

      //  cameraHolder.SetActive(true);
    }

    #region EVENTS
    private void InputReader_RunEventCanceled()
    {
        isRunning = false;
    }

    private void InputReader_RunEventPerformed()
    {
        if (isCrouching) return;

        isRunning = true;
    }

    private void InputReader_CrouchEvent()
    {
        CrouchLogic();
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

    private void InputReader_MoveEvent(Vector2 arg0)
    {
        move = arg0;
        move.Normalize();
    }

    private void InputReader_HideInventoryEvent()
    {
        soGameObj.gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);
    }

    private void InputReader_ShowInventoryEvent()
    {
        soGameObj.gameObject.transform.GetChild(0).transform.gameObject.SetActive(true);
    }

    private void InputReader_InteractEvent()
    {
        if (!isAiming) return;


        Attack();
    }

    #endregion

    private void Update()
    {
        if (!IsOwner) return;        

        if (isAiming)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, 100f, aimLayers))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    rotationTarget = hit.transform.GetComponent<Transform>().position;
                }
                else
                {
                    rotationTarget = hit.point;
                }
            }

            MovePlayerWithAim();
        }
        else
        {
            MovePlayer();
        }

        if (handleEquipedItems.currentlyEquipedItem == null) return;
        //  if (handleEquipedItems.currentlyEquipedItem.weaponType == WeaponType.Bow) return;

         ExitAttack();             
    }

    public void Attack()
    {
        if(Time.time - lastComboEnd > 0.1f && comboCounter < 3/*handleEquipedItems.currentlyEquipedItem.soAttack.Count*/) //time for combos
        {
            CancelInvoke("EndCombo");

            if(Time.time - lastClickedTime >= 1.3f)  //time for attacks
            {
                animationController.AttackTrigger();

                animationController.PlayAttackAnim(comboCounter);
                comboCounter++;
                lastClickedTime = Time.time;

                if(comboCounter > 3)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    public void ExitAttack()
    {
        if (animationController.IsAttackEnded())
        {
            Invoke("EndCombo", 1f);
        }
    }

    public void EndCombo()
    {
        Debug.Log("KANE TO END");

        comboCounter = 0;
        lastComboEnd = Time.time;
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.08f);
        }

        if(move == Vector2.zero)
        {
            speed = 0f;
        }
        else
        {
            speed = isCrouching ? speedWhileCrouching : isRunning ? runningSpeed : walkingSpeed;
        }

        animationController.UpdateMovementAnimation();

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    public void MovePlayerWithAim()
    {
        var lookPos = rotationTarget - transform.position;
        lookPos.y = 0f;
        var rotation = Quaternion.LookRotation(lookPos);

        Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

        if(aimDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.08f);
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (move == Vector2.zero)
        {
            speed = 0f;
        }
        else
        {
            speed = isCrouching ? speedWhileCrouching : isRunning ? runningSpeed : walkingSpeed;
        }

        animationController.UpdateMovementAnimation();

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    #region CROUCH
    private void CrouchLogic()
    {
        if (!isCrouching)
        {
            if (crouchCoroutine != null)
            {
                StopCoroutine(crouchCoroutine);
            }
            crouchCoroutine = StartCoroutine(Crouch(originalHeight, crouchHeight, defaultColliderCenter, crouchColliderCenter));
        }
        else
        {
            if (crouchCoroutine != null)
            {
                StopCoroutine(crouchCoroutine);
            }
            crouchCoroutine = StartCoroutine(Crouch(crouchHeight, originalHeight, crouchColliderCenter, defaultColliderCenter));
        }
    }

    private IEnumerator Crouch(float startHeight, float endHeight, Vector3 startCenter, Vector3 endCenter)
    {
        isCrouching = true;

        characterController.height = endHeight;
        characterController.center = endCenter;

        isCrouching = endHeight != originalHeight;

        animationController.UpdateCrouchAnimation();

        yield return null;
    }
    #endregion

    public bool IsMovingBackward()
    {
        if (!isAiming) return false;

        Vector3 movementDirection = new Vector3(move.x, 0f, move.y).normalized;
        Vector3 forwardDirection = transform.forward;

        float dotProduct = Vector3.Dot(movementDirection, forwardDirection);
        return dotProduct < 0;
    }
}
