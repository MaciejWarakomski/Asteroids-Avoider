using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;

    private Vector3 movementDirection;


    void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            var worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            movementDirection = transform.position - worldPosition;
            movementDirection.z = 0f;
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if(movementDirection == Vector3.zero) { return; }
        rb.AddForce(movementDirection * forceMagnitude * Time.fixedDeltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }
}
