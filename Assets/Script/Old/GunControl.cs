using UnityEngine;
using System.Collections;

public class GunControl : MonoBehaviour {
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float spawnTime = 1f;

    private AudioSource au;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        au = GetComponent<AudioSource>();
    }

    void Spawn()
    {
        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BulletControl b = obj.GetComponent<BulletControl>();
        if (b != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                b.SetTarget(player.transform.position);
            }
            b.OnFire = FireAudio;
        }
    }

    void FireAudio()
    {
        au.Play();
    }
}
