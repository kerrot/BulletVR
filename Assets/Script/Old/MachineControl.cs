using UnityEngine;
using System.Collections;

public class MachineControl : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private float anglespeed;

    void Update ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float cv = Input.GetAxis("CameraVertical");

        Vector3 hoffset = transform.right.normalized * speed * h;
        Vector3 voffset = -transform.forward.normalized * speed * v;

        transform.Rotate(new Vector3(0, cv * anglespeed, 0));

        transform.position += hoffset + voffset;
    }
}
