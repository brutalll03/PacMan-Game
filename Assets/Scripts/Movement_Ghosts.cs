using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Ghosts : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask obstacleLayer;

    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2 currentDirection;

    public float speedMultiplier = 1f;
    public Vector2 initialDirection;

    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
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

    private void Start()
    {
        currentDirection = GetRandomDirection();
    }

    private void Update()
    {
        MoveInDirection(currentDirection);
    }

    private void MoveInDirection(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);

        RaycastHit2D upHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.up, .3f, obstacleLayer);
        upWall = upHit.collider != null;

        RaycastHit2D downHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.down, .3f, obstacleLayer);
        downWall = downHit.collider != null;

        RaycastHit2D leftHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.left, .3f, obstacleLayer);
        leftWall = leftHit.collider != null;

        RaycastHit2D rightHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.right, .3f, obstacleLayer);
        rightWall = rightHit.collider != null;

        if (rigidbody.velocity.normalized == Vector2.right && rightWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (currentDirection == directions[4] && !rightWall)
            {
                rigidbody.velocity = Vector2.right * speed;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                rigidbody.rotation = 0;
                currentDirection = GetRandomDirection();
            }
        }

        if (rigidbody.velocity.normalized == Vector2.left && leftWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (currentDirection == directions[3] && !leftWall)
            {
                rigidbody.velocity = Vector2.left * speed;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                rigidbody.rotation = 180;
                currentDirection = GetRandomDirection();
            }
        }

        if (rigidbody.velocity.normalized == Vector2.up && upWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (currentDirection == directions[1] && !upWall)
            {
                rigidbody.velocity = Vector2.up * speed;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                rigidbody.rotation = 90;
                currentDirection = GetRandomDirection();
            }
        }

        if (rigidbody.velocity.normalized == Vector2.down && downWall)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.None;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (currentDirection == directions[2] && !downWall)
            {
                rigidbody.velocity = Vector2.down * speed;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                rigidbody.rotation = 270;
                currentDirection = GetRandomDirection();
            }
        }


    }

    Vector2 GetRandomDirection()
    {
        return directions[Random.Range(0, directions.Length)];
    }
}




