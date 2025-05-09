using UnityEngine;
using UnityEngine.UIElements;
public class MainUIController : MonoBehaviour
{
    private VisualElement _title;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find VE in root
        if (root != null)
        {
            _title = root.Q<VisualElement>("GameTitle");
        }


        // Start The looping animaton
        Invoke("LoopAnimation", 0.1f);
    }



    private void LoopAnimation()
    {

        _title.ToggleInClassList("titleScale");
        _title.RegisterCallback<TransitionEndEvent>(
            evt => _title.ToggleInClassList("titleScale")
        );
    }
}
