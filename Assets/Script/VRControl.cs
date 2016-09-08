using UnityEngine;
using UnityEngine.VR;
using System.Collections;

public class VRControl : MonoBehaviour {
    void Update () {
        if (Input.GetKey(KeyCode.R))
        {
            InputTracking.Recenter();
        }
    }
}
