using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerLogic : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -25f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 50f;
    public float maxLookUp = 15f;
    public float maxLookDown = -15f;
    public float maxHorizontalAngle = 60f;

    [Header("Look Boundary")]
    public float screenLookThreshold = 0.3f;

    [Header("References")]
    public Transform cameraPivot;
    public Camera playerCamera;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation;
    private float yRotation;
    private float verticalVelocity;
    private float initialYRotation;
    private Vector2 extraLook;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialYRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        HandleLook();
        HandleExtraLookAtEdges();
        HandleMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 horizontalMove = transform.right * moveInput.x * moveSpeed;
        Vector3 forwardMove = transform.forward * moveInput.y * moveSpeed;
        Vector3 verticalMove = Vector3.up * verticalVelocity;

        controller.Move((horizontalMove + forwardMove + verticalMove) * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * 0.01f;
        float mouseY = lookInput.y * mouseSensitivity * 0.01f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookDown, maxLookUp);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation,
            initialYRotation - (maxHorizontalAngle * 0.5f),
            initialYRotation + (maxHorizontalAngle * 0.5f));

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void HandleExtraLookAtEdges()
    {
        if (playerCamera == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 mouseOffset = mousePosition - screenCenter;

        float horizontalRatio = mouseOffset.x / (Screen.width * 0.5f);
        float verticalRatio = mouseOffset.y / (Screen.height * 0.5f);

        extraLook = Vector2.zero;

        if (Mathf.Abs(horizontalRatio) > screenLookThreshold)
        {
            float extraAmount = (Mathf.Abs(horizontalRatio) - screenLookThreshold) / (1f - screenLookThreshold);
            extraLook.x = Mathf.Sign(horizontalRatio) * extraAmount * 10f;
        }

        if (Mathf.Abs(verticalRatio) > screenLookThreshold)
        {
            float extraAmount = (Mathf.Abs(verticalRatio) - screenLookThreshold) / (1f - screenLookThreshold);
            extraLook.y = Mathf.Sign(verticalRatio) * extraAmount * 10f;
        }

        if (extraLook != Vector2.zero)
        {
            ApplyExtraLook();
        }
    }

    void ApplyExtraLook()
    {
        float extraXRotation = xRotation + extraLook.y * 0.5f;
        float extraYRotation = yRotation + extraLook.x * 0.5f;

        extraXRotation = Mathf.Clamp(extraXRotation, maxLookDown * 1.2f, maxLookUp * 1.2f);
        extraYRotation = Mathf.Clamp(extraYRotation,
            initialYRotation - (maxHorizontalAngle * 0.6f),
            initialYRotation + (maxHorizontalAngle * 0.6f));

        cameraPivot.localRotation = Quaternion.Euler(extraXRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, extraYRotation, 0f);
    }

    void OnDrawGizmos()
    {
        if (cameraPivot != null)
        {
            float mainAngle = maxHorizontalAngle * 0.5f;
            float distance = 2f;

            Vector3 pivotPos = cameraPivot.position;
            Vector3 forwardDir = cameraPivot.forward;

            Gizmos.color = Color.yellow;
            Vector3 leftDir = Quaternion.Euler(0, -mainAngle, 0) * forwardDir;
            Vector3 rightDir = Quaternion.Euler(0, mainAngle, 0) * forwardDir;

            Gizmos.DrawLine(pivotPos, pivotPos + leftDir * distance);
            Gizmos.DrawLine(pivotPos, pivotPos + rightDir * distance);

            float angleStep = (mainAngle * 2) / 20;
            Vector3 prevPoint = pivotPos + Quaternion.Euler(0, -mainAngle, 0) * forwardDir * distance;

            for (int i = 1; i <= 20; i++)
            {
                float currentAngle = -mainAngle + (angleStep * i);
                Vector3 nextPoint = pivotPos + Quaternion.Euler(0, currentAngle, 0) * forwardDir * distance;
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }

            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            float extraAngle = maxHorizontalAngle * 0.3f;
            Vector3 extraLeftDir = Quaternion.Euler(0, -(mainAngle + extraAngle), 0) * forwardDir;
            Vector3 extraRightDir = Quaternion.Euler(0, mainAngle + extraAngle, 0) * forwardDir;

            Gizmos.DrawLine(pivotPos, pivotPos + extraLeftDir * distance * 0.8f);
            Gizmos.DrawLine(pivotPos, pivotPos + extraRightDir * distance * 0.8f);

            Gizmos.color = Color.cyan;
            Vector3 upDir = Quaternion.Euler(maxLookUp, 0, 0) * forwardDir;
            Vector3 downDir = Quaternion.Euler(maxLookDown, 0, 0) * forwardDir;

            Gizmos.DrawLine(pivotPos, pivotPos + upDir * distance);
            Gizmos.DrawLine(pivotPos, pivotPos + downDir * distance);

            Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
            Vector3 extraUpDir = Quaternion.Euler(maxLookUp * 1.2f, 0, 0) * forwardDir;
            Vector3 extraDownDir = Quaternion.Euler(maxLookDown * 1.2f, 0, 0) * forwardDir;

            Gizmos.DrawLine(pivotPos, pivotPos + extraUpDir * distance * 0.8f);
            Gizmos.DrawLine(pivotPos, pivotPos + extraDownDir * distance * 0.8f);
        }
    }

    public void ToggleCursor()
    {
        bool visible = !Cursor.visible;
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}