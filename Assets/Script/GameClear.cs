using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameClear : MonoBehaviour {
    [SerializeField]
    private string sceneName;

    void OnTriggerEnter(Collider other)
    {
        ShooterControl player = GameObject.FindObjectOfType<ShooterControl>();
        if (player != null)
        {
            player.SetState(ShooterControl.GameState.CLEAR);
            GetComponent<Animator>().SetTrigger("Clear");
        }
    }

    void ToNext()
    {
        SceneManager.LoadScene(sceneName);
    }
}
