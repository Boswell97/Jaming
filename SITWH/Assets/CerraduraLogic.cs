using UnityEngine;
using UnityEngine.InputSystem;

public class CerraduraLogic : MonoBehaviour
{
    public GameObject[] canvasObjects;
    [SerializeField] private InputActionProperty closeAction;

    private bool isPlayerInTrigger = false;

    private void Start()
    {
        foreach (GameObject canvas in canvasObjects)
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }

        if (closeAction.action != null)
        {
            closeAction.action.Enable();
            closeAction.action.performed += OnClosePerformed;
        }
    }

    private void OnDestroy()
    {
        if (closeAction.action != null)
        {
            closeAction.action.performed -= OnClosePerformed;
        }
    }

    private void Update()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            foreach (GameObject canvas in canvasObjects)
            {
                if (canvas != null)
                {
                    canvas.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            CloseCanvas();
        }
    }

    public void CloseCanvas()
    {
        foreach (GameObject canvas in canvasObjects)
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }

    private void OnClosePerformed(InputAction.CallbackContext context)
    {
        if (isPlayerInTrigger)
        {
            CloseCanvas();
        }
    }
}