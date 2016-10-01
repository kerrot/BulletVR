using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BulletControl : MonoBehaviour {
    [SerializeField]
    private GameObject bullet_predict;
    [SerializeField]
    private float shiftSpeed;

    private ShooterControl shooter;
    private int layerNum;
    private Rigidbody rb;

    int hitIndex = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shooter = GameObject.FindObjectOfType<ShooterControl>();
        layerNum = LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        if (shooter.State == ShooterControl.GameState.BULLET_FLYING)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 tmp = bullet_predict.transform.localPosition;
            tmp.x += h * shiftSpeed;
            tmp.y += v * shiftSpeed;
            bullet_predict.transform.localPosition = tmp;

            if (Input.GetButton("Jump"))
            {
                bullet_predict.transform.localPosition = Vector3.zero;
                hitIndex = -1;
                shooter.SetState(ShooterControl.GameState.BULLET_MISS);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerNum)
        {
            ShooterControl.BounceRecord record = shooter.GetRecord(other.gameObject);
            transform.position = record.point;
            hitIndex = record.order;
            rb.velocity = record.reflect.normalized * rb.velocity.magnitude;
        }
        else if (other.gameObject.GetComponent<GameClear>() == null)
        {
            shooter.SetState(ShooterControl.GameState.BULLET_MISS);
        }
    }

    Vector3 ComputeMirror(Vector3 from, Vector3 hit, Vector3 normal)
    {
        Vector3 v = hit - from;
        Vector3 p = Vector3.Project(v, -normal);
        return from + p * 2;
    }

    Vector3 ComputeShiftPosition(Vector3 from, Vector3 hit, Vector3 normal, Vector3 target)
    {
        Vector3 v = hit - from;
        Vector3 p = Vector3.Project(v, -normal);
        Vector3 mirror = from + p * 2;

        Vector3 a = target - mirror;
        float length = p.magnitude / Mathf.Cos(Vector3.Angle(-p, a) * Mathf.Deg2Rad);
        return mirror + a.normalized * length;
    }

    Vector3 ComputeByIndex(int index)
    {
        Vector3 target = bullet_predict.transform.position;
        Vector3 from = shooter.gameObject.transform.position;

        if (index < hitIndex)
        {
            target = ComputeByIndex(index + 1);
        }

        if (index > 0)
        {
            for (int i = 0; i < index; ++i)
            {
                KeyValuePair<GameObject, ShooterControl.BounceRecord> tmp = shooter.GetRecordByOrder(i);

                from = ComputeMirror(from, tmp.Value.point, tmp.Key.transform.up);
            }
        }

        KeyValuePair<GameObject, ShooterControl.BounceRecord> record = shooter.GetRecordByOrder(index);

        return ComputeShiftPosition(from, record.Value.point, record.Key.transform.up, target);
    }

    public Vector3 ComputeGunTarget()
    {
        if (hitIndex < 0)
        {
            return bullet_predict.transform.position;
        }
        else if (hitIndex == 0)
        {
            KeyValuePair<GameObject, ShooterControl.BounceRecord> record = shooter.GetRecordByOrder(0);
            return ComputeShiftPosition(shooter.gameObject.transform.position, record.Value.point, record.Key.transform.up, bullet_predict.transform.position);
        }
        
        return ComputeByIndex(0);
    }
}
