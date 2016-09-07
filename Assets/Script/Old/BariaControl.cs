using UnityEngine;
using System.Collections;

public class BariaControl : MonoBehaviour {
    [SerializeField]
    private float slowSpeed;
    [SerializeField]
    private float slowTime;
    [SerializeField]
    private GameObject turtorial;
    [SerializeField]
    private GameObject aim;
    [SerializeField]
    private GameObject ready;

    private float slowStartTime;


    private bool first = true;
    private float tmpTimeScale;

    private float pauseTime;
    private float resumeTime;
    private bool pause;

    void Update()
    {
        if (!pause)
        {
            if (Time.realtimeSinceStartup - slowStartTime > slowTime)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }

        if (turtorial.activeSelf)
        {
            PlayerControl player = GameObject.FindObjectOfType<PlayerControl>();

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, aim.transform.position - player.transform.position, out hit))
            {
                if (hit.collider.gameObject.name == "OK")
                {
                    Vector3 tmp = ready.transform.localScale;
                    tmp.x += 0.02f;
                    ready.transform.localScale = tmp;

                    if (tmp.x >= 1)
                    {
                        turtorial.SetActive(false);
                        Time.timeScale = tmpTimeScale;
                        aim.SetActive(false);
                        resumeTime = Time.realtimeSinceStartup;

                        slowStartTime += resumeTime - pauseTime;

                        pause = false;
                    }
                }
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        BulletControl b = other.GetComponent<BulletControl>();
        if (b != null)
        {
            SlowMotion();
            if (first)
            {
                turtorial.SetActive(true);
                tmpTimeScale = Time.timeScale;
                Time.timeScale = 0;
                aim.SetActive(true);
                pauseTime = Time.realtimeSinceStartup;
                pause = true;

                first = false;
            }
        }
    }

    public void SlowMotion()
    {
        Time.timeScale = slowSpeed;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        slowStartTime = Time.realtimeSinceStartup;
    }
}
