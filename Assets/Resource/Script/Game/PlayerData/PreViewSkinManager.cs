using UnityEngine;
using UnityEngine.SceneManagement;
public class PreViewSkinManager : MonoBehaviour
{
    //Singleton for GameDataManager
    private static PreViewSkinManager _instance;
    public static PreViewSkinManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PreViewSkinManager>();
                //Just for Safe
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("Manager");
                    _instance = go.AddComponent<PreViewSkinManager>();
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

    Material mat => targetRenderer.material;

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
            GameObject obj = GameObject.FindGameObjectWithTag("SkinTag");

            if (obj != null)
            {
                Debug.Log("[SkinManager] Find SkinTag¡G" + obj.name);

                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    targetRenderer = renderer;

                    Debug.Log("[SkinManager] Bnd Renderer and Apply texture¡I");
                }
                else
                {
                    Debug.LogWarning("[SkinManager] Find obj but no Renderer comp¡I");
                }
            }
            else
            {
                Debug.LogWarning("[SkinManager] Cant fnd tag 'SkinTag'¡I");
            }
        }
        else
        {
            Debug.Log("[SkinManager] Skip Find¡C");
        }
    }
    public void ApplySkinChange()
    {
        Debug.Log($"[ApplySkinChange] rarity: {skinSO.rarity}, wear: {skinSO.wear}");
        if(targetRenderer != null)
        {
            ApplyMainText(skinSO.rarity);
            ApplyParameter(skinSO.wear);
        }
    }

    void ApplyMainText(int rarity)
    {
        switch (rarity)
        {
            case 0:
                mat.SetTexture("_MainTex", normal);
                break;
            case 1:
                mat.SetTexture("_MainTex", rare);
                break;
            case 2:
                mat.SetTexture("_MainTex", epic);
                break;
            default:
                mat.SetTexture("_MainTex", deflaut);
                break;
        }
    }

    void ApplyParameter(float wear)
    {
        mat.SetFloat("_Wear", wear);
        mat.SetFloat("_Glossiness", 1 - wear);
    }
}
