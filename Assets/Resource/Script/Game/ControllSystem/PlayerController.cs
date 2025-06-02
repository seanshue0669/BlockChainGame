using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float speed = 0.1f;
    public float jumpforce = 10f;

    Rigidbody _playerRB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.S))
        {
            MoveUp();
        }
        MoveForward(speed);
    }

    [System.Obsolete]
    private void MoveForward(float speedFac)
    {
        Vector3 currentVelocity = _playerRB.velocity;
        _playerRB.velocity = new Vector3(
            currentVelocity.x,
            currentVelocity.y, 
            speedFac
        );
    }
    [System.Obsolete]
    private void MoveUp()
    {
        _playerRB.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
    }
}
