using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIGlitchEffect : MonoBehaviour
{
    [Header("Configuració del Glitch")]
    [Tooltip("Freqüència dels glitches (segons entre glitches)")]
    [Range(0.1f, 5f)]
    public float glitchFrequency = 1f;

    [Tooltip("Duració de cada glitch (segons)")]
    [Range(0.05f, 0.5f)]
    public float glitchDuration = 0.1f;

    [Tooltip("Intensitat del desplaçament")]
    [Range(0f, 50f)]
    public float offsetIntensity = 10f;

    [Header("Efectes Visuals")]
    public bool useColorGlitch = true;
    public Color glitchColor1 = new Color(1f, 0f, 0f, 1f);
    public Color glitchColor2 = new Color(0f, 1f, 1f, 1f);

    [Header("Opcions")]
    public bool startOnEnable = true;
    public bool randomGlitches = true;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Graphic graphic;
    private Color originalColor;
    private bool isGlitching = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        graphic = GetComponent<Graphic>();
        if (graphic != null)
        {
            originalColor = graphic.color;
        }
    }

    private void OnEnable()
    {
        isGlitching = false;

        if (startOnEnable)
        {
            StartGlitching();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetToOriginal();
    }

    public void StartGlitching()
    {
        if (!isGlitching)
        {
            StartCoroutine(GlitchRoutine());
        }
    }

    public void StopGlitching()
    {
        StopAllCoroutines();
        ResetToOriginal();
        isGlitching = false;
    }

    private IEnumerator GlitchRoutine()
    {
        isGlitching = true;

        while (isGlitching)
        {
            float waitTime = randomGlitches ?
                Random.Range(glitchFrequency * 0.5f, glitchFrequency * 1.5f) :
                glitchFrequency;

            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(PerformGlitch());
        }
    }

    private IEnumerator PerformGlitch()
    {
        //float elapsed = 0f;
        int glitchCount = Random.Range(1, 4);

        for (int i = 0; i < glitchCount; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-offsetIntensity, offsetIntensity),
                Random.Range(-offsetIntensity, offsetIntensity)
            );
            rectTransform.anchoredPosition = originalPosition + randomOffset;

            if (useColorGlitch && graphic != null)
            {
                Color glitchColor = Random.value > 0.5f ? glitchColor1 : glitchColor2;
                graphic.color = glitchColor;
            }

            yield return new WaitForSeconds(glitchDuration / glitchCount);

            ResetToOriginal();
            yield return new WaitForSeconds(0.02f);
        }

        ResetToOriginal();
    }

    private void ResetToOriginal()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = originalPosition;
        }

        if (graphic != null)
        {
            graphic.color = originalColor;
        }
    }

    public void TriggerSingleGlitch()
    {
        StartCoroutine(PerformGlitch());
    }
}