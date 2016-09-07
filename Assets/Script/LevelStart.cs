using UnityEngine;
using System.Collections;

public class LevelStart : MonoBehaviour {

	void StartGamne()
    {
        ShooterControl game = GameObject.FindObjectOfType<ShooterControl>();
        if (game != null)
        {
            game.SetState(ShooterControl.GameState.BEFORE_FIRE);
        }
    }
}
