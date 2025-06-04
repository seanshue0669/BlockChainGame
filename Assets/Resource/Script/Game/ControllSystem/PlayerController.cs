using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float minforwardSpeed = 2f;
    public float maxforwardSpeed = 5f;
    public float slideSpeed = 0.2f;
    public float jumpforce = 3f;
    public float lateralSpeed = 3f;
    public float smoothingFactor = 15f;

    [Header("Current speed")]
    [SerializeField]
    private float forwardSpeed = 2f;

    private bool isGrounded = false;
    private bool jumpRequested = false;
    private float groundBufferTime = 0.2f; 
    private float lastGroundedTime = 0f;
    private float moveXInput = 0f;
    private Rigidbody _playerRB;

    private float _startTimeStrap;
    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        _startTimeStrap = Time.time;

    }
    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.S))
        {
            jumpRequested = true;
        }

        moveXInput = 0f;
        if (UnityEngine.Input.GetKey(KeyCode.A)) moveXInput = -slideSpeed;
        if (UnityEngine.Input.GetKey(KeyCode.D)) moveXInput = slideSpeed;
        float t = 1f - Mathf.Exp(-(Time.time- _startTimeStrap) / smoothingFactor);
        forwardSpeed = Mathf.Lerp(minforwardSpeed, maxforwardSpeed, t);
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        bool canJump = isGrounded || (Time.time - lastGroundedTime <= groundBufferTime);

        if (canJump && jumpRequested)
        {
            MoveUp();
            isGrounded = false;
            jumpRequested = false;
        }

        Move(moveXInput, forwardSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            lastGroundedTime = Time.time;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    [System.Obsolete]
    private void Move(float xInput, float zSpeed)
    {
        Vector3 currentVelocity = _playerRB.velocity;

        _playerRB.velocity = new Vector3(
            xInput * lateralSpeed,     
            currentVelocity.y,         
            zSpeed                     
        );
    }
    [System.Obsolete]
    private void MoveUp()
    {
        if (isGrounded)
        {
            Vector3 currentVelocity = _playerRB.velocity;

            _playerRB.velocity = new Vector3(
                currentVelocity.x,
                jumpforce,
                currentVelocity.z
            );
        }
    }
}
