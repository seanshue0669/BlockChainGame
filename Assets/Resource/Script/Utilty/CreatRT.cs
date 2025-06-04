using UnityEngine;
using UnityEngine.UI;

public class CreatRT : MonoBehaviour
{
    [Header("RenderTexture")]
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Camera targetCamera;

    void Start()
    {
        if (targetCamera != null && renderTexture != null)
        {
            targetCamera.targetTexture = renderTexture;
        }
    }
}
