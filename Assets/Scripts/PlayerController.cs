using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveInputDeadZone;

    [SerializeField] private int leftInputID, rightInputID;
    [SerializeField] private float screenWidth;

    [SerializeField] private Vector2 lookInput;
    [SerializeField] private float cameraPitch;

    [SerializeField] private Vector2 moveTouchStartPosition;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector3 moveDirection;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float velocity;

    [SerializeField] private bool Perspective2D;
    [SerializeField] private bool isActive;
    [SerializeField] private GameObject otherPlayer;

    private void Start()
    {
        leftInputID = -1;
        rightInputID = -1;

        screenWidth = Screen.width / 2;

        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);
    }

    private void Update()
    {
        if (isActive)
        {
            GetTouchInput();

            if (rightInputID != -1)
                LookAround();

            Move();
        }
    }

    private void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x < screenWidth && leftInputID == -1)
                    {
                        leftInputID = t.fingerId;
                        moveTouchStartPosition = t.position;
                    }
                    else if (t.position.x > screenWidth && rightInputID == -1)
                        rightInputID = t.fingerId;

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == leftInputID)
                        leftInputID = -1;
                    else if (t.fingerId == rightInputID)
                        rightInputID = -1;

                    break;
                case TouchPhase.Moved:
                    if (t.fingerId == rightInputID)
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                    else if (t.fingerId == leftInputID)
                        moveInput = t.position - moveTouchStartPosition;

                    break;
                case TouchPhase.Stationary:
                    if (t.fingerId == rightInputID)
                        lookInput = Vector2.zero;

                    break;
            }

        }
    }

    private void LookAround()
    {
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        transform.Rotate(transform.up, lookInput.x);
    }

    private void Move()
    {
        if (characterController.velocity.x == 0 && velocity < 0)
            velocity = -.05f;
        else
            velocity += gravity * Time.deltaTime;

        Vector2 movementDirection;
        if (leftInputID == -1)
            movementDirection = Vector2.zero;
        else
            movementDirection = moveInput.normalized * moveSpeed * Time.deltaTime;
        Vector3 movement = transform.right * movementDirection.x + transform.forward * movementDirection.y;
        if (Perspective2D)
        {
            float temp = movement.z;
            movement.z = -movement.x;
            movement.x = temp;
            movement.x = velocity;
        }
        else
        {
            movement.y = velocity;
        }
        otherPlayer.transform.position = new Vector3(transform.position.x, otherPlayer.transform.position.y, transform.position.z);
        characterController.Move(movement);
    }

}
