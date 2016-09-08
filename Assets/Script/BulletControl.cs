using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BulletControl : MonoBehaviour {

    private ShooterControl shooter;
    private int layerNum;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shooter = GameObject.FindObjectOfType<ShooterControl>();
        layerNum = LayerMask.NameToLayer("Wall");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerNum)
        {
            ShooterControl.BounceRecord record = shooter.GetRecord(other.gameObject);
            transform.position = record.point;
            rb.velocity = record.reflect.normalized * rb.velocity.magnitude;
        }
        else if (other.gameObject.GetComponent<GameClear>() == null)
        {
            shooter.SetState(ShooterControl.GameState.BULLET_MISS);
        }
    }
}
