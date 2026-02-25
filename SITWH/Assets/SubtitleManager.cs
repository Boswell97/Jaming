using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public GameObject panel;
    public TextMeshProUGUI subtitleText;
    Coroutine routine;

    void Awake()
    {
        Instance = this;
    }

    public void Show(string text, float duration)
    {
        panel.gameObject.SetActive(true);

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowRoutine(text, duration));
    }

    IEnumerator ShowRoutine(string text, float duration)
    {
        subtitleText.text = text;
        subtitleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        subtitleText.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }
}