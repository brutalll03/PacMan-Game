using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement_Ghosts : MonoBehaviour
{
    public float speed = 3f;
    public LayerMask obstacleLayer;

    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2 currentDirection;
    private List<Vector2> savedDirections = new List<Vector2>();

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
        SendInRandomPosition();
    }

    private void SendInRandomPosition()
    {
        currentDirection = Vector2.zero;
        while (currentDirection == Vector2.zero)
        {
            currentDirection = GetRandomDirection();
        }
        MoveInDirection(currentDirection);
    }

    private void MoveInDirection(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
        CheckDoubleDirections();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            SendInRandomPosition();

        }
    }

    private void CheckWallCollisions()
    {
        RaycastHit2D upHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.up, .5f, obstacleLayer);
        upWall = upHit.collider != null;

        RaycastHit2D downHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.down, .5f, obstacleLayer);
        downWall = downHit.collider != null;

        RaycastHit2D leftHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.left, .5f, obstacleLayer);
        leftWall = leftHit.collider != null;

        RaycastHit2D rightHit = Physics2D.BoxCast(transform.position, Vector2.one * 0.4f, 0, Vector2.right, .5f, obstacleLayer);
        rightWall = rightHit.collider != null;
    }

    public Vector2 GetRandomDirection()
    {
        CheckWallCollisions();
        int range = Random.Range(0, directions.Length);
        Vector2 vector2 = directions[range];
        bool isValid = true;
        if (rightWall && vector2 == Vector2.right)
            isValid = false;
        if (leftWall && vector2 == Vector2.left)
            isValid = false;
        if (upWall && vector2 == Vector2.up)
            isValid = false;
        if (downWall && vector2 == Vector2.down)
            isValid = false;
        if (savedDirections.Contains(vector2))
            isValid = false;

        if (isValid == false)
        {
            if (rightWall == false && savedDirections.Contains(Vector2.right) == false)
                return Vector2.right;
            if (leftWall == false && savedDirections.Contains(Vector2.left) == false)
                return Vector2.left;
            if (upWall == false && savedDirections.Contains(Vector2.up) == false)
                return Vector2.up;
            if (downWall == false && savedDirections.Contains(Vector2.down) == false)
                return Vector2.down;

            savedDirections.Clear();
            return Vector2.zero;
        }

        return vector2;
    }

    public void CheckDoubleDirections()
    {
        if (savedDirections.Count > 2)
            savedDirections.RemoveAt(savedDirections.Count+1);

        savedDirections.Add(currentDirection);
    }
}




