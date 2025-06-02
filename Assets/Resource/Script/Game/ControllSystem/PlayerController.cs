using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float forwardSpeed = 0.1f;
    public float slideSpeed = 0.5f;
    public float jumpforce = 10f;
    public float lateralSpeed = 3f;


    private bool isGrounded = false;
    private float groundBufferTime = 0.5f; 
    private float lastGroundedTime = 0f;
    private Rigidbody _playerRB;


    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
    }
    [System.Obsolete]
    void FixedUpdate()
    {
        bool canJump = isGrounded || (Time.time - lastGroundedTime <= groundBufferTime);

        if (canJump && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.S)))
        {
            MoveUp();
            isGrounded = false;
        }


        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -slideSpeed;
        if (Input.GetKey(KeyCode.D)) moveX = slideSpeed;

        Move(moveX, forwardSpeed);
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
        _playerRB.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
    }
}
