using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSensors : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private float sphereRadius = .5f;
    [SerializeField] private float sphereOffset = 0f;
    [SerializeField] private LayerMask groundMask;
    [HideInInspector]
    public bool isGrounded;

/*    public delegate void PlayerEnterEvent(Transform player);

    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

    public event PlayerEnterEvent onPlayerEnter;
    public event PlayerExitEvent onPlayerExit;*/

    private void Update() {
        CheckForGround();
    }

    private void CheckForGround() {

        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + sphereOffset, transform.position.z);

        if (Physics.CheckSphere(spherePos, sphereRadius, groundMask, QueryTriggerInteraction.Ignore)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            onPlayerEnter?.Invoke(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            onPlayerExit?.Invoke(transform.position);
        }
    }*/

    private void OnDrawGizmos() {

        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + sphereOffset, transform.position.z);

        if (isGrounded) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(spherePos, sphereRadius);
    }
}
