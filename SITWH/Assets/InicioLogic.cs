using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class InicioLogic : MonoBehaviour, Player.INewactionmapActions
{
    public GameObject inicioPanel;
    public MonoBehaviour playerLogic;

    public GameObject objectToEnable1;
    public GameObject objectToEnable2;

    public GameObject optionsPanel;

    public float fadeDuration = 1.5f;

    Player input;
    bool isClosing;

    Graphic[] graphics;

    void Awake()
    {
        input = new Player();
        input.Newactionmap.SetCallbacks(this);
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        inicioPanel.SetActive(true);
        graphics = inicioPanel.GetComponentsInChildren<Graphic>(true);

        if (playerLogic != null)
            playerLogic.enabled = false;

        if (objectToEnable1 != null)
            objectToEnable1.SetActive(false);

        if (objectToEnable2 != null)
            objectToEnable2.SetActive(false);
    }

    public void PressStartButton()
    {
        if (isClosing) return;
        StartCoroutine(CloseStartPanel());
    }

    public void OnClosePanel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        PressStartButton();
    }

    IEnumerator CloseStartPanel()
    {
        isClosing = true;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeDuration);

            for (int i = 0; i < graphics.Length; i++)
            {
                Color c = graphics[i].color;
                c.a = a;
                graphics[i].color = c;
            }

            yield return null;
        }

        if (objectToEnable1 != null)
            objectToEnable1.SetActive(true);

        if (objectToEnable2 != null)
            objectToEnable2.SetActive(true);

        inicioPanel.SetActive(false);

        if (playerLogic != null)
            playerLogic.enabled = true;
    }

    public void OpenOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext context) { }
    public void OnLook(InputAction.CallbackContext context) { }
    public void OnGrab(InputAction.CallbackContext context) { }
    public void OnChoseNumber(InputAction.CallbackContext context) { }
    public void OnIncrement(InputAction.CallbackContext context) { }
    public void OnDecrement(InputAction.CallbackContext context) { }
    public void OnValid(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context) { }
}