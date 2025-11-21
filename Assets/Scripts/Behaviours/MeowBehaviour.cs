using UnityEngine;

public class MeowBehaviour : MonoBehaviour
{
    private SpriteRenderer _sr;

    [Header("MIAU")]
    [Tooltip("miau?")]
    [SerializeField] private GameObject meow;

    private void Awake()
    {
        _sr = meow.GetComponent<SpriteRenderer>();
    }

    public void Meow()
    {
        meow.SetActive(true);

        AudioManager.Instance.PlayClip(AudioClipType.Meow);
    }

    public void SetDirection(Vector2 direction)
    {
        if (direction.x != 0)
        {
            if (direction.x > 0)
            {
                meow.transform.localPosition = new Vector3(0.3f, meow.transform.localPosition.y, meow.transform.localPosition.z);
            }
            else
            {
                meow.transform.localPosition = new Vector3(-0.3f, meow.transform.localPosition.y, meow.transform.localPosition.z);
            }

            _sr.flipX = direction.x < 0;
        }
    }

    public void FlipY()
    {
        meow.transform.localPosition = new Vector3(meow.transform.localPosition.x, -meow.transform.localPosition.y, meow.transform.localPosition.z);

        bool flip = !_sr.flipY;
        _sr.flipY = flip;
    }
}
