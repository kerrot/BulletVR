using UnityEngine;
using System.Collections;

public class VRButton : MonoBehaviour {
    [SerializeField]
    private GameObject ready;
    [SerializeField]
    private float second;

    private GameObject camera;
    private int layerMask;

    public delegate void ButtonEvent();
    public ButtonEvent OnPress;

    void Start()
    {
        Camera ca = Camera.main;
        if (ca == null)
        {
            ca = GameObject.FindObjectOfType<Camera>();
        }

        camera = ca.gameObject;
        layerMask = LayerMask.GetMask("UI");
    }

    void Update()
    {
        float step = (second > 0) ? Time.deltaTime / second : 1f;

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            VRButton b = hit.collider.gameObject.GetComponent<VRButton>();
            if (b != null)
            {
                Vector3 tmp = ready.transform.localScale;
                tmp.x += step;
                ready.transform.localScale = tmp;

                if (tmp.x >= 1)
                {
                    tmp.x = 0;
                    ready.transform.localScale = tmp;
                    if (OnPress != null)
                    {
                        OnPress();
                    }
                }
            }
        }
    }
}
