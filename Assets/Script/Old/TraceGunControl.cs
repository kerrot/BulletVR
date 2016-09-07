using UnityEngine;
using System.Collections;

public class TraceGunControl : MonoBehaviour {
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float bulletWaitTime;    
    [SerializeField]
    private float time;

    private GameObject target;

    private AudioSource au;

    private bool spawn = false;

    void Start()
    {
        au = GetComponent<AudioSource>();
        BariaControl baria = GameObject.FindObjectOfType<BariaControl>();

        target = transform.FindChild("Target").gameObject;
        target.transform.parent = baria.transform;
        target.SetActive(false);
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        if (!spawn)
        {
            if (Time.time > time)
            {
                Spawn();
                spawn = true;
            }
        }
    }

    void Spawn()
    {
        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BulletControl b = obj.GetComponent<BulletControl>();
        if (b != null)
        {
            b.speed = bulletSpeed;
            b.WaitTime = bulletWaitTime;

            b.SetTarget(target);
            b.OnFire = FireAudio;
        }
    }

    void FireAudio()
    {
        au.Play();
    }
}
