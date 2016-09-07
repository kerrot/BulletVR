using UnityEngine;
using System.Collections;

public class NonHMDCamera : MonoBehaviour {
    [SerializeField]
    private GameObject HMD;

	void Start () {
        PlayerControl player = GameObject.FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            player.transform.parent = transform;
        }
        HMD.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
