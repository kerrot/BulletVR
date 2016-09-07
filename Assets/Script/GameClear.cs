using UnityEngine;
using System.Collections;

public class GameClear : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        ShooterControl player = GameObject.FindObjectOfType<ShooterControl>();
        if (player != null)
        {
            player.SetState(ShooterControl.GameState.CLEAR);
        }
    }
}
