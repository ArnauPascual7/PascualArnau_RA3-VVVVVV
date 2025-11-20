using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float _charactersDelay = 0.05f;

    private TextMeshProUGUI TextMeshPro;
    private string Text;

    private void Awake()
    {
        TextMeshPro = GetComponent<TextMeshProUGUI>();

        Text = TextMeshPro.text;
    }

    private void OnEnable()
    {
        StartCoroutine(MostrarTextLletraPerLletra());
    }

    public IEnumerator MostrarTextLletraPerLletra()
    {
        TextMeshPro.text = "";

        foreach (char caracter in Text)
        {
            TextMeshPro.text += caracter;
            yield return new WaitForSeconds(_charactersDelay);
        }
    }

    public void MostrarText(string nouText)
    {
        StopAllCoroutines();
        Text = nouText;
        StartCoroutine(MostrarTextLletraPerLletra());
    }

    public void MostrarTextComplet()
    {
        StopAllCoroutines();
        TextMeshPro.text = Text;
    }
}
