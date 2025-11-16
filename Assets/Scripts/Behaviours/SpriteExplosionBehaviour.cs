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

        Rect spriteRect = originalSprite.rect;
        float pieceWidth = spriteRect.width / piecesX;
        float pieceHeight = spriteRect.height / piecesY;

        for (int x = 0; x < piecesX; x++)
        {
            for (int y = 0; y < piecesY; y++)
            {
                CreatePiece(x, y, pieceWidth, pieceHeight, spriteRect, texture, originalSprite);
            }
        }

        gameObject.SetActive(false);
    }

    void CreatePiece(int x, int y, float width, float height, Rect spriteRect, Texture2D texture, Sprite originalSprite)
    {
        GameObject piece = new GameObject($"Piece_{x}_{y}");

        float pieceSize = 1f / originalSprite.pixelsPerUnit;
        float offsetX = (x - piecesX / 2f + 0.5f) * width * pieceSize;
        float offsetY = (y - piecesY / 2f + 0.5f) * height * pieceSize;

        piece.transform.position = transform.position + new Vector3(offsetX, offsetY, 0);
        piece.transform.rotation = transform.rotation;
        piece.transform.localScale = transform.localScale;

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

        SpriteRenderer sr = piece.AddComponent<SpriteRenderer>();
        sr.sprite = pieceSprite;
        sr.material = spriteRenderer.material; // Utilitzem el mateix material (outline)
        sr.sortingLayerID = spriteRenderer.sortingLayerID;
        sr.sortingOrder = spriteRenderer.sortingOrder;

        Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        Vector2 explosionCenter = transform.position;
        Vector2 piecePosition = piece.transform.position;
        Vector2 direction = (piecePosition - explosionCenter).normalized;

        if (direction.magnitude < 0.01f)
        {
            direction = Random.insideUnitCircle.normalized;
        }

        rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);

        if (rotateOnExplode)
        {
            rb.angularVelocity = Random.Range(-360f, 360f);
        }

        PieceDestroyer destroyer = piece.AddComponent<PieceDestroyer>();
        destroyer.lifetime = pieceLifetime;
        destroyer.fadeOut = fadeOut;
    }
}

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
