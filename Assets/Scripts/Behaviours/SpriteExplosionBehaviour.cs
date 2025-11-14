using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteExplosionBehaviour : MonoBehaviour
{
    [Header("Configuració")]
    [SerializeField] private int piecesX = 4; // Columnes
    [SerializeField] private int piecesY = 4; // Files
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float pieceLifetime = 3f;
    [SerializeField] private float gravityScale = 1f;

    [Header("Opcions")]
    [SerializeField] private bool fadeOut = true;
    [SerializeField] private bool rotateOnExplode = true;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Explode()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        Sprite originalSprite = spriteRenderer.sprite;
        Texture2D texture = originalSprite.texture;

        // Obtenim els límits del sprite
        Rect spriteRect = originalSprite.rect;
        float pieceWidth = spriteRect.width / piecesX;
        float pieceHeight = spriteRect.height / piecesY;

        // Creem cada tros
        for (int x = 0; x < piecesX; x++)
        {
            for (int y = 0; y < piecesY; y++)
            {
                CreatePiece(x, y, pieceWidth, pieceHeight, spriteRect, texture, originalSprite);
            }
        }

        // Desactivem o destruïm l'sprite original
        gameObject.SetActive(false);
    }

    void CreatePiece(int x, int y, float width, float height, Rect spriteRect, Texture2D texture, Sprite originalSprite)
    {
        // Creem un GameObject per cada tros
        GameObject piece = new GameObject($"Piece_{x}_{y}");

        // Calculem la posició del tros relatiu al centre del sprite
        float pieceSize = 1f / originalSprite.pixelsPerUnit;
        float offsetX = (x - piecesX / 2f + 0.5f) * width * pieceSize;
        float offsetY = (y - piecesY / 2f + 0.5f) * height * pieceSize;

        // Posicionem el tros a la posició correcta
        piece.transform.position = transform.position + new Vector3(offsetX, offsetY, 0);
        piece.transform.rotation = transform.rotation;
        piece.transform.localScale = transform.localScale;

        // Creem el sprite per aquest tros
        Rect pieceRect = new Rect(
            spriteRect.x + x * width,
            spriteRect.y + y * height,
            width,
            height
        );

        Sprite pieceSprite = Sprite.Create(
            texture,
            pieceRect,
            new Vector2(0.5f, 0.5f),
            originalSprite.pixelsPerUnit
        );

        // Afegim SpriteRenderer
        SpriteRenderer sr = piece.AddComponent<SpriteRenderer>();
        sr.sprite = pieceSprite;
        sr.material = spriteRenderer.material; // Utilitzem el mateix material (outline)
        sr.sortingLayerID = spriteRenderer.sortingLayerID;
        sr.sortingOrder = spriteRenderer.sortingOrder;

        // Afegim física
        Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        // Calculem la força d'explosió des del centre del sprite original
        Vector2 explosionCenter = transform.position;
        Vector2 piecePosition = piece.transform.position;
        Vector2 direction = (piecePosition - explosionCenter).normalized;

        // Si la direcció és zero (el tros està exactament al centre), donem una direcció aleatòria
        if (direction.magnitude < 0.01f)
        {
            direction = Random.insideUnitCircle.normalized;
        }

        // Apliquem la força d'explosió
        rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);

        // Rotació aleatòria
        if (rotateOnExplode)
        {
            rb.angularVelocity = Random.Range(-360f, 360f);
        }

        // Afegim component per destruir després
        PieceDestroyer destroyer = piece.AddComponent<PieceDestroyer>();
        destroyer.lifetime = pieceLifetime;
        destroyer.fadeOut = fadeOut;
    }
}

// Component per destruir els trossos després d'un temps
public class PieceDestroyer : MonoBehaviour
{
    public float lifetime = 3f;
    public bool fadeOut = true;

    private float timer = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (fadeOut && spriteRenderer != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
