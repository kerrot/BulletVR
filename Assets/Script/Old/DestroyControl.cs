using UnityEngine;
using System.Collections;

public class DestroyControl : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
