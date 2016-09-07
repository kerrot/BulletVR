using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;

public class ShooterControl : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float bulletSpeed = 1f;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletCameraPos;
    [SerializeField]
    private GameObject gunCameraPos;
    [SerializeField]
    private GameObject gunPos;
    [SerializeField]
    private GameObject HMD;
    [SerializeField]
    private GameObject GameClear;

    private float x;
    private float y;
    private LineDrawControl line;
    private AudioSource au;
    private int layerMask;

    public enum GameState
    {
        START_ANIMATION,
        BEFORE_FIRE,
        BEFORE_FLYING,
        BULLET_FLYING,
        BULLET_MISS,
        CLEAR,
    };

    private GameState state = GameState.START_ANIMATION;

    void Start()
    {
        line = GetComponent<LineDrawControl>();
        au = GetComponent<AudioSource>();

        layerMask = LayerMask.GetMask("Wall");
    }

    void Update()
    {
        UpdateLaser();
        UpdateGun();
        UpdateCamera();
        VRReCenter();
        Fire();

        Rigidbody r = bullet.GetComponent<Rigidbody>();
        if (r.velocity != Vector3.zero)
        {
            bullet.transform.LookAt(bullet.transform.position + r.velocity);
        }
    }

    void UpdateGun()
    {
        if (state != GameState.BEFORE_FIRE && state != GameState.BULLET_FLYING)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        y += h;
        x += v;

        transform.rotation = Quaternion.Euler(new Vector3(x * speed, y * speed, 0));
    }

    void UpdateLaser()
    {
        List<Vector3> points = new List<Vector3>();
        List<GameObject> walls = new List<GameObject>();
        ComputeBounce(gunPos.transform.position, gunPos.transform.forward, points, walls);

        line.points = points.ToArray();
    }

    Vector3 ComputeBounce(Vector3 position, Vector3 direction, List<Vector3> points, List<GameObject> walls)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (!walls.Contains(hit.collider.gameObject))
            {
                points.Add(position);
                points.Add(hit.point);
                walls.Add(hit.collider.gameObject);

                Vector3 normal = hit.collider.gameObject.transform.up.normalized;
                Vector3 reflect = direction - 2 * Vector3.Dot(direction, normal) * normal;

                position = hit.point;
                direction = reflect;

                return ComputeBounce(hit.point, reflect, points, walls);
            }
        }

        points.Add(position);
        points.Add(position + direction * 10000);
        return direction;
    }

    void VRReCenter()
    {
        if (Input.GetKey(KeyCode.R))
        {
            InputTracking.Recenter();
        }
    }

    void Fire()
    {
        if (state == GameState.BEFORE_FIRE && Input.GetButton("Jump"))
        {
            bullet.transform.parent = null;
            SetHMDOrigin(bulletCameraPos);
            state = GameState.BEFORE_FLYING;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
            au.Play();
        }
//         else if (state == GameState.BULLET_FLYING)
//         {
//             state = GameState.BULLET_MISS;
//             HMD.transform.parent = gunCameraPos.transform;
//             Debug.Log(bulletCameraPos);
//         }
    }

    public void SetState(GameState s)
    {
        if (s == GameState.BEFORE_FIRE)
        {
            SetHMDOrigin(gunCameraPos);
        }

        if (s == GameState.CLEAR)
        {
            SetHMDOrigin(GameClear);
            GameClear.GetComponent<Animator>().SetTrigger("Clear");
        }

        state = s;
    }

    void SetHMDOrigin(GameObject obj)
    {
        HMD.transform.parent = obj.transform;
        HMD.transform.localPosition = Vector3.zero;
        HMD.transform.localRotation = Quaternion.identity;
    }

    void UpdateCamera()
    {
        if (state == GameState.BEFORE_FLYING)
        {
            Vector3 step = -HMD.transform.localPosition * Time.deltaTime;
            if (step == Vector3.zero || Vector3.Distance(HMD.transform.position, gunCameraPos.transform.position) <= step.magnitude)
            {
                SetState(GameState.BULLET_FLYING);
                return;
            }

            HMD.transform.position += step;
        }
    }
}
