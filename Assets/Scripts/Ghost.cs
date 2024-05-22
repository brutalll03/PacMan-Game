
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement_Ghosts movement { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;
    public LayerMask layerMask;

    private void Awake()
    {
        movement = GetComponent<Movement_Ghosts>();
        frightened = GetComponent<GhostFrightened>();
        layerMask = GetComponent<Collider2D>().excludeLayers;
    }

    private void Start()
    {
        ResetState();

    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();
        frightened.Disable();
        GetComponent<Collider2D>().excludeLayers = layerMask;
        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PacMan"))
        {
            if (frightened.enabled)
            {
                GetComponent<Collider2D>().excludeLayers = layerMask | (1 << LayerMask.NameToLayer("PacMan"));
                GameManager.Instance.GhostEaten(this);
            }
            else
            {
                GameManager.Instance.PacmanEaten();
            }
        }
    }

}
