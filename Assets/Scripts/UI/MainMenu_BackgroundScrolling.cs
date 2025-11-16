using UnityEngine;
using UnityEngine.UI;

public class MainMenu_BackgroundScrolling : MonoBehaviour
{
    [Header("Configuració")]
    [Tooltip("Velocitat del desplaçament (negatiu per moure cap a l'esquerra)")]
    [Range(0, 2f)]
    [SerializeField] private float scrollSpeed = 1;

    private Image backgroundImage;

    private Material material;
    private float offset = 0f;

    private void Start()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }

        if (backgroundImage == null)
        {
            Debug.LogError("No s'ha trobat cap Image! Assegura't que aquest script està en un objecte amb Image.");
            return;
        }

        material = new Material(backgroundImage.material);
        backgroundImage.material = material;
    }

    private void Update()
    {
        if (material != null)
        {
            offset += scrollSpeed * Time.deltaTime;

            material.mainTextureOffset = new Vector2(offset, 0);
        }
    }

    private void OnDestroy()
    {
        if (material != null)
        {
            Destroy(material);
        }
    }
}
