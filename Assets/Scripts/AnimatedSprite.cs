
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0];
    public Sprite[] spritesDeath = new Sprite[0];
    public float animationTime = 0.25f;
    public bool loop = true;
    int count;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    bool isDying;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    public void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    public void Advance()
    {
        if (isDying)
            return;
        if (spriteRenderer == null)
            return;
        if (!spriteRenderer.enabled)
        {
            return;
        }

        animationFrame++;

        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        animationFrame = -1;

    }

    public void Death()
    {
        isDying = true;
        InvokeRepeating(nameof(DeathAnim), animationTime, animationTime);
    }

    private void DeathAnim()
    {
        
        if (spriteRenderer == null)
            return;
        if (!spriteRenderer.enabled)
        {
            return;
        }

        animationFrame++;

        if (animationFrame >= spritesDeath.Length && loop)
        {
            animationFrame = 0;
            count++;
            print(count);
        }

        if (animationFrame >= 0 && animationFrame < spritesDeath.Length)
        {
            spriteRenderer.sprite = spritesDeath[animationFrame];

        }
        if (count >= 1)
        {
            Restart();
            GameManager.Instance.Reset();
        }

    }
}