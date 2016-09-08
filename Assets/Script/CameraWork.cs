using UnityEngine;
using System.Collections;

public class CameraWork : MonoBehaviour {

    public delegate void CameraEvent();
    public CameraEvent OnFollowEnd;

    private Vector3 cameraWorkStep;

    // Update is called once per frame
    void Update () {
        UpdateCamera();
    }

   

    void UpdateCamera()
    {
        if (cameraWorkStep == Vector3.zero || transform.localPosition.magnitude <= cameraWorkStep.magnitude)
        {
            transform.localPosition = Vector3.zero;
            if (OnFollowEnd != null)
            {
                OnFollowEnd();
            }
            return;
        }

        transform.localPosition += cameraWorkStep;
    }

    public void SetHMDOrigin(GameObject obj)
    {
        transform.parent = obj.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void SetCameraFollow(GameObject obj)
    {
        transform.parent = obj.transform;
        transform.localRotation = Quaternion.identity;
        cameraWorkStep = -transform.localPosition * Time.deltaTime;
    }
}
