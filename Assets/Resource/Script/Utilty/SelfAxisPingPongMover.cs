using UnityEngine;

public class SelfAxisPingPongMover : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [Header("Move Settings")]
    public Axis moveAxis = Axis.Y;
    public float amplitude = 1f; 
    public float speed = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, amplitude * 2f) - amplitude;

        Vector3 move = Vector3.zero;
        switch (moveAxis)
        {
            case Axis.X:
                move = new Vector3(offset, 0, 0);
                break;
            case Axis.Y:
                move = new Vector3(0, offset, 0);
                break;
            case Axis.Z:
                move = new Vector3(0, 0, offset);
                break;
        }

        transform.localPosition = startPosition + move;
    }
}
