using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class TracePlayerCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Collider>().isTrigger = true;
	}

    void OnTriggerEnter(Collider other)
    {
        BulletControl b = other.GetComponent<BulletControl>();
        if (b != null)
        {
            PlayerControl player = GameObject.FindObjectOfType<PlayerControl>();
            if (player != null)
            {
                b.SetVelocity(player.transform.position - b.gameObject.transform.position);
            }
        }
    }
}
