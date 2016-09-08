using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonChangeScene : MonoBehaviour {
    [SerializeField]
    private VRButton button;
    [SerializeField]
    private string sceneName;

    void Start()
    {
        if (button != null)
        {
            button.OnPress = () => SceneManager.LoadScene(sceneName);
        }
    }

    void OnDestroy()
    {
        button.OnPress = null;
    }
}
