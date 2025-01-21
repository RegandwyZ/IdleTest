using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Test);
    }

    private void Test()
    {
        Debug.Log("Test");
    }
}
