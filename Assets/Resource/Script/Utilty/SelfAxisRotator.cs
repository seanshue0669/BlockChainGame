using UnityEngine;

public class SelfAxisRotator : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [Header("Rotation Settings")]
    public Axis rotationAxis = Axis.Y;  
    public float speed = 90f;            

    void Update()
    {
        Vector3 axisVector = Vector3.up;

        switch (rotationAxis)
        {
            case Axis.X:
                axisVector = Vector3.right;
                break;
            case Axis.Y:
                axisVector = Vector3.up;
                break;
            case Axis.Z:
                axisVector = Vector3.forward;
                break;
        }

        transform.Rotate(axisVector, speed * Time.deltaTime, Space.Self);
    }
}
