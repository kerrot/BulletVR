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
    private Vector3 startRotation;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletCameraPos;
    [SerializeField]
    private GameObject gunCameraPos;
    [SerializeField]
    private GameObject HMD;
    [SerializeField]
    private GameObject GameClear;
    [SerializeField]
    private float h;
    [SerializeField]
    private float v;

    private float x;
    private float y;
    private float bx;
    private float by;

    private LineDrawControl line;
    private AudioSource au;
    private int layerMask;

    private CameraWork cw;

    public struct BounceRecord
    {
        public Vector3 point;
        public Vector3 reflect;
    }

    Dictionary<GameObject, BounceRecord> records = new Dictionary<GameObject, BounceRecord>();

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
        cw = HMD.GetComponent<CameraWork>();
        cw.OnFollowEnd += CameraWorkEnd;
    }

    void Update()
    {
        UpdateLaser();
        UpdateGun();
        Fire();

        Rigidbody r = bullet.GetComponent<Rigidbody>();
        if (r.velocity != Vector3.zero)
        {
            bullet.transform.LookAt(bullet.transform.position + r.velocity);
        }
    }

    void UpdateGun()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (state == GameState.BEFORE_FIRE || state == GameState.BULLET_FLYING)
        {
            y += h;
            x -= v;

            transform.rotation = Quaternion.Euler(startRotation) * Quaternion.Euler(new Vector3(x * speed, y * speed, 0));
        }
    }

    void UpdateLaser()
    {
        if (state == GameState.CLEAR)
        {
            line.points = null;
        }
        else
        {
            records.Clear();

            List<Vector3> points = new List<Vector3>();
            List<GameObject> walls = new List<GameObject>();
            ComputeBounce(transform.position, transform.forward, points, walls);

            line.points = points.ToArray();
        }
    }

    Vector3 ComputeBounce(Vector3 position, Vector3 direction, List<Vector3> points, List<GameObject> walls)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (!walls.Contains(hit.collider.gameObject))
            {
                walls.Add(hit.collider.gameObject);

                Vector3 normal = hit.collider.gameObject.transform.up.normalized;
                Vector3 reflect = direction - 2 * Vector3.Dot(direction, normal) * normal;


                points.Add(position);
                points.Add(hit.point);

                position = hit.point;
                direction = reflect;

                records.Add(hit.collider.gameObject, new BounceRecord() { point = hit.point, reflect = reflect });

                return ComputeBounce(hit.point, reflect, points, walls);
            }
        }

        points.Add(position);
        points.Add(position + direction * 100000);
        return direction;
    }

    public BounceRecord GetRecord(GameObject obj)
    {
        if (records.ContainsKey(obj))
        {
            return records[obj];
        }

        return new BounceRecord();
    }

    void Fire()
    {
        if (Input.GetButton("Jump"))
        {
            if (state == GameState.BEFORE_FIRE)
            {
                bullet.transform.parent = null;
                cw.SetCameraFollow(bulletCameraPos);

                state = GameState.BEFORE_FLYING;
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
                au.Play();
            }
            else if (state == GameState.BULLET_FLYING)
            {
                SetState(GameState.BULLET_MISS);
            }
        }
    }

    public void SetState(GameState s)
    {
        switch (s)
        {
            case GameState.BEFORE_FIRE:
                {
                    cw.SetHMDOrigin(gunCameraPos);
                }
                break;
            case GameState.BULLET_MISS:
                {
                    bullet.transform.parent = transform;
                    bullet.transform.localPosition = Vector3.zero;
                    bullet.transform.localRotation = Quaternion.identity;
                    bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    cw.SetCameraFollow(gunCameraPos);
                }
                break;
            case GameState.BULLET_FLYING:
                {
                    bx = 0;
                    by = 0;
}
                break;
            case GameState.CLEAR:
                {
                    cw.SetHMDOrigin(GameClear);
                }
                break;
        }

        state = s;
    }

    void CameraWorkEnd()
    {
        switch (state)
        {
            case GameState.BEFORE_FLYING:
                SetState(GameState.BULLET_FLYING);
                break;
            case GameState.BULLET_MISS:
                SetState(GameState.BEFORE_FIRE);
                break;
        }
    }

    void OnDestroy()
    {
        cw.OnFollowEnd -= CameraWorkEnd;
    }
}
