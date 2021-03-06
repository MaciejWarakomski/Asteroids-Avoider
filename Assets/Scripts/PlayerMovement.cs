using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;

    private Vector3 movementDirection;


    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    private void ProcessInput()
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

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        var viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }
        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }
        transform.position = newPosition;
    }

    private void RotateToFaceVelocity()
    {
        if (rb.velocity == Vector3.zero) { return; }
        var targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (movementDirection == Vector3.zero) { return; }
        rb.AddForce(movementDirection * forceMagnitude * Time.fixedDeltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }
}
