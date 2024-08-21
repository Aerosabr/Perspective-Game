using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Camera Controls
    private Transform cameraTransform;
    [SerializeField]
    private float cameraSensitivity; //Speed at which camera moves around
    private float cameraPitch; //Angle camera is facing in 1st person view

    //Character Movement
    private CharacterController characterController;
    [SerializeField]
    private float moveSpeed;
    private float gravity = -1f;
    private float velocity; //Vertical movement of player

    //Touchscreen Variables
    private int leftInputID;
    private int rightInputID;
    private float screenWidth;

    //Right Finger Variables
    private Vector2 lookInput; //Direction right finger is moving
    private float jumpTime;

    //Left Finger Variables
    private Vector2 moveTouchStartPosition;
    private Vector2 moveInput;
    private Vector3 moveDirection;

    public bool Perspective2D;
    public bool isActive;

    private void Start()
    {
        cameraTransform = transform.GetChild(0);
        characterController = GetComponent<CharacterController>();
        leftInputID = -1;
        rightInputID = -1;

        screenWidth = Screen.width / 2;
    }

    private void Update()
    {
        GetTouchInput();
        if (isActive)
        {
            if (Perspective2D && transform.position.x < -20)
                CheckpointManager.instance.GoCheckpoint();
            else if (!Perspective2D && transform.position.y < -17)
                CheckpointManager.instance.GoCheckpoint();
            else
            {
                if (rightInputID != -1 && !Perspective2D)
                    LookAround();

                Move();
            }
        }
    }

    public void SwitchPerspective()
    {
        Debug.Log("Pressed");
        if (Perspective2D)
            StartCoroutine(CameraMove(Quaternion.Euler(0, 180, 0), new Vector3(0, 0.75f, 0)));
        else
            StartCoroutine(CameraMove(Quaternion.Euler(90, 90, 0), new Vector3(0.6f, 20, 0)));
    }

    private IEnumerator CameraMove(Quaternion rotation, Vector3 position)
    {
        characterController.Move(Vector3.zero);
        isActive = false;
        Vector3 startPosition = transform.GetChild(0).transform.localPosition;
        Vector3 targetPosition = position;
        Quaternion startRotation = transform.GetChild(0).transform.rotation;
        Quaternion endRotation = rotation;
        float duration = 2f;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime / duration;
            transform.GetChild(0).transform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            transform.GetChild(0).transform.rotation = Quaternion.Lerp(startRotation, endRotation, progress);
            yield return null;
        }

        Perspective2D = !Perspective2D;
        StageManager.instance.ChangePerspective(Perspective2D);
        isActive = true;
    }

    private void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                return;

            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x < screenWidth && leftInputID == -1)
                    {
                        leftInputID = t.fingerId;
                        moveTouchStartPosition = t.position;
                    }
                    else if (t.position.x > screenWidth && rightInputID == -1)
                    {
                        rightInputID = t.fingerId;
                        jumpTime = Time.time;
                    }

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == leftInputID)
                        leftInputID = -1;
                    else if (t.fingerId == rightInputID)
                    {
                        rightInputID = -1;
                        if (Time.time - jumpTime <= 0.2f && lookInput == Vector2.zero)
                            Jump();
                    }
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

    private void Jump()
    {
        if (Perspective2D)
        {
            if (characterController.velocity.x != 0)
                return;
        }
        else if (!characterController.isGrounded)
            return;

        velocity += .3f;
    }

    private void LookAround()
    {
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        transform.Rotate(transform.up, lookInput.x);
    }

    private void Move()
    {
        Vector2 movementDirection;
        if (leftInputID == -1)
            movementDirection = Vector2.zero;
        else
            movementDirection = moveInput.normalized * moveSpeed * Time.deltaTime;
        Vector3 movement = transform.right * movementDirection.x + transform.forward * movementDirection.y;
        if (Perspective2D)
        {
            if (characterController.velocity.x == 0 && velocity < 0)
                velocity = -.05f;
            else
                velocity += gravity * Time.deltaTime;
            float temp = movement.z;
            movement.z = movement.x;
            movement.x = temp;
            movement.x = velocity;
            
        }
        else
        {
            if (characterController.velocity.y == 0 && velocity < 0)
                velocity = -.05f;
            else
                velocity += gravity * Time.deltaTime;
            movement.y = velocity;
            
        } 

        characterController.Move(movement);
    }
}
