using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 15f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;


    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    public bool rightWall;
    public bool leftWall;
    public bool upWall;
    public bool downWall;

    private void Update()
    {
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, 0.8f, obstacleLayer);
        upWall = upHit.collider != null;

        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, obstacleLayer);
        downWall = downHit.collider != null;

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 0.8f, obstacleLayer);
        leftWall = leftHit.collider != null;

        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 0.8f, obstacleLayer);
        rightWall = rightHit.collider != null;

        if(rigidbody.velocity.normalized ==  Vector2.right && rightWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        if(rigidbody.velocity.normalized == Vector2.left && leftWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        if (rigidbody.velocity.normalized == Vector2.up && upWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        if (rigidbody.velocity.normalized == Vector2.down && downWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
        }


        // Check for arrow input and set direction based on the key
        if (Input.GetKeyUp(KeyCode.LeftArrow)) 
        {
            rigidbody.velocity = Vector2.left * speed;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rigidbody.velocity = Vector2.right * speed;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            rigidbody.velocity = Vector2.up * speed;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            rigidbody.velocity = Vector2.down * speed;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}