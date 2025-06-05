
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinManager : MonoBehaviour
{
    private static SkinManager _instance;
    public static SkinManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SkinManager>();
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("Manager");
                    if (go == null)
                    {
                        go = new GameObject("Manager");
                    }
                    _instance = go.AddComponent<SkinManager>();
                }
            }
            return _instance;
        }
    }

    public Renderer targetRenderer;

    public Texture deflaut;
    public Texture normal;
    public Texture rare;
    public Texture epic;

    [SerializeField]
    public SkinSO skinSO;

    Material mat => targetRenderer != null ? targetRenderer.material : null;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ApplySkinChange();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedFindAndApply());
    }

    private System.Collections.IEnumerator DelayedFindAndApply()
    {
        yield return null;
        FindMat();
        ApplySkinChange();
    }

    private void FindMat()
    {
        if (targetRenderer == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");

            if (obj != null)
            {
                Debug.Log("[SkinManager] Found Player with tag.");
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    targetRenderer = renderer;
                    Debug.Log("[SkinManager] Renderer assigned from Player.");
                }
                else
                {
                    Debug.LogWarning("[SkinManager] Player object found but has no Renderer.");
                }
            }
            else
            {
                Debug.LogWarning("[SkinManager] No GameObject with tag 'Player' found.");
            }
        }
    }

    public void ApplySkinChange()
    {
        if (targetRenderer == null || mat == null)
        {
            Debug.LogWarning("[SkinManager] Cannot apply skin: targetRenderer or material is null.");
            return;
        }

        Debug.Log($"[SkinManager] ApplySkinChange: rarity = {skinSO.rarity}, wear = {skinSO.wear}");

        ApplyMainTex(skinSO.rarity);
        ApplyParameters(skinSO.wear);
        ApplyExtraTextures();
    }

    void ApplyMainTex(int rarity)
    {
        Texture selected = rarity switch
        {
            0 => normal,
            1 => rare,
            2 => epic,
            _ => deflaut
        };

        if (selected == null)
        {
            Debug.LogError($"[SkinManager] Missing texture for rarity {rarity}. Applying white texture.");
            selected = Texture2D.whiteTexture;
        }
        else
        {
            Debug.Log($"[SkinManager] MainTex assigned: {selected.name}");
        }

        mat.SetTexture("_MainTex", selected);
    }

    void ApplyParameters(float wear)
    {
        mat.SetFloat("_Wear", wear);
        mat.SetFloat("_Glossiness", 1 - wear);
    }

    void ApplyExtraTextures()
    {
        ApplySafeTexture("_DirtyTex", "Mat/Textures/dirty");
        ApplySafeTexture("_MaskTex", "Mat/Textures/mask");
    }

    void ApplySafeTexture(string propertyName, string path)
    {
        Texture tex = Resources.Load<Texture2D>(path);
        if (tex == null)
        {
            Debug.LogError($"[SkinManager] Missing texture at path: Resources/{path}. Using white fallback.");
            tex = Texture2D.whiteTexture;
        }
        else
        {
            Debug.Log($"[SkinManager] Texture loaded for {propertyName}: {tex.name}");
        }
        mat.SetTexture(propertyName, tex);
    }

}
