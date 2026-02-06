using UnityEngine;
using UnityEngine.InputSystem;

public class CursorDragDrop : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Sprite cursorSprite;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color grabColor = Color.green;
    [SerializeField] private Vector2 cursorOffset = new Vector2(0f, -100f);

    [Header("Grab Settings")]
    [SerializeField] private float grabDistance = 5f;
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private LayerMask grabMask = -1;

    [Header("Hold Settings")]
    [SerializeField] private float holdDistance = 2f;
    [SerializeField] private float smoothFollowSpeed = 10f;
    [SerializeField] private Vector3 holdRotationOffset = Vector3.zero;

    private Camera mainCamera;
    private SpriteRenderer cursorRenderer;
    private GameObject grabbedObject;
    private Rigidbody grabbedRigidbody;
    private Vector2 screenCenter;
    private Vector3 currentHoldPosition;
    private bool isGrabbing;
    private bool canGrab;
    private Collider hoveredObject;

    void Awake()
    {
        // Buscar la cámara principal de forma segura
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            // Buscar cualquier cámara en la escena
            Camera[] cameras = FindObjectsOfType<Camera>();
            if (cameras.Length > 0)
            {
                mainCamera = cameras[0];
                Debug.Log("Cámara asignada: " + mainCamera.name);
            }
            else
            {
                Debug.LogError("No se encontró ninguna cámara en la escena!");
                enabled = false; // Desactivar el script si no hay cámara
                return;
            }
        }

        // Crear el GameObject para el cursor
        GameObject cursorObject = new GameObject("CursorSprite");
        cursorRenderer = cursorObject.AddComponent<SpriteRenderer>();
        cursorRenderer.sprite = cursorSprite;
        cursorRenderer.sortingOrder = 999;

        // Ocultar el cursor del sistema
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("mainCamera es null en Start!");
            enabled = false;
            return;
        }

        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        UpdateCursorPosition();
    }

    void Update()
    {
        // Verificar si la cámara sigue siendo válida
        if (mainCamera == null)
        {
            Debug.LogWarning("Cámara perdida, buscando de nuevo...");
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        UpdateCursorPosition();
        CheckForGrabbableObjects();
        HandleGrabbedObject();
        UpdateCursorAppearance();
    }

    void UpdateCursorPosition()
    {
        if (mainCamera == null || cursorRenderer == null) return;

        try
        {
            // Posicionar el cursor en el centro de la pantalla con offset
            Vector3 screenPos = new Vector3(
                screenCenter.x + cursorOffset.x,
                screenCenter.y + cursorOffset.y,
                mainCamera.nearClipPlane + 0.1f
            );

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
            cursorRenderer.transform.position = worldPos;

            // Mantener el sprite mirando hacia la cámara
            cursorRenderer.transform.LookAt(mainCamera.transform);
            cursorRenderer.transform.Rotate(0, 180, 0);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error en UpdateCursorPosition: " + e.Message);
        }
    }

    void CheckForGrabbableObjects()
    {
        if (isGrabbing || mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance, grabMask))
        {
            hoveredObject = hit.collider;
            canGrab = hit.collider.GetComponent<Rigidbody>() != null;
        }
        else
        {
            hoveredObject = null;
            canGrab = false;
        }
    }

    void HandleGrabbedObject()
    {
        if (!isGrabbing || grabbedObject == null || mainCamera == null) return;

        // Calcular la posición objetivo
        Vector3 targetPosition = mainCamera.transform.position +
                                mainCamera.transform.forward * holdDistance;

        // Suavizar el movimiento
        currentHoldPosition = Vector3.Lerp(
            currentHoldPosition,
            targetPosition,
            smoothFollowSpeed * Time.deltaTime
        );

        // Mover el objeto agarrado
        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.MovePosition(currentHoldPosition);

            if (holdRotationOffset != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.Euler(holdRotationOffset);
                grabbedRigidbody.MoveRotation(targetRotation);
            }

            grabbedRigidbody.linearVelocity = Vector3.zero;
            grabbedRigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            grabbedObject.transform.position = currentHoldPosition;
        }
    }

    void UpdateCursorAppearance()
    {
        if (cursorRenderer == null) return;

        if (isGrabbing)
        {
            cursorRenderer.color = grabColor;
        }
        else if (canGrab)
        {
            cursorRenderer.color = hoverColor;
        }
        else
        {
            cursorRenderer.color = normalColor;
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isGrabbing)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }
    }

    void TryGrabObject()
    {
        if (!canGrab || hoveredObject == null || mainCamera == null) return;

        grabbedObject = hoveredObject.gameObject;
        grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();

        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.useGravity = false;
            grabbedRigidbody.linearDamping = 10f;
            grabbedRigidbody.angularDamping = 10f;

            currentHoldPosition = grabbedObject.transform.position;
        }

        isGrabbing = true;
        Debug.Log("Objeto agarrado: " + grabbedObject.name);
    }

    void ReleaseObject()
    {
        if (grabbedObject == null) return;

        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.useGravity = true;
            grabbedRigidbody.linearDamping = 0f;
            grabbedRigidbody.angularDamping = 0.05f;

            if (mainCamera != null)
            {
                Vector3 throwDirection = mainCamera.transform.forward;
                grabbedRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }

            Debug.Log("Objeto lanzado con fuerza: " + throwForce);
        }

        grabbedObject = null;
        grabbedRigidbody = null;
        isGrabbing = false;
        hoveredObject = null;
    }

    void OnDrawGizmos()
    {
        if (mainCamera == null) return;

        Gizmos.color = canGrab ? Color.green : Color.red;
        Vector3 rayStart = mainCamera.transform.position;
        Vector3 rayEnd = rayStart + mainCamera.transform.forward * grabDistance;
        Gizmos.DrawLine(rayStart, rayEnd);

        if (isGrabbing && grabbedObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(currentHoldPosition, 0.2f);
        }
    }

    public void SetCursorSprite(Sprite newSprite)
    {
        cursorSprite = newSprite;
        if (cursorRenderer != null)
        {
            cursorRenderer.sprite = newSprite;
        }
    }

    void OnDestroy()
    {
        if (cursorRenderer != null && cursorRenderer.gameObject != null)
        {
            Destroy(cursorRenderer.gameObject);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}