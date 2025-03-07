﻿using UnityEngine;
using System.Collections;

public class MovePlat2 : MonoBehaviour {
    private Transform pointStart;
    public Transform pointInter;
    public Transform pointEnd;

    public bool isAlt = false;
    public float speed = 5;

    private Vector3 directionInter;
    private Vector3 directionFinal;

    private Rigidbody m_rb;
    private Rigidbody m_rbOther;
    private bool toggle = true;
    private bool triggered = false;

    private float origDistInter;
    private float origDistFinal;

    void Start() {
        m_rb = GetComponent<Rigidbody>();
        pointStart = this.transform;
        directionInter = (pointInter.position - transform.position).normalized * speed;
        origDistInter = Vector3.Distance(this.transform.position, pointInter.position);

        origDistFinal = Vector3.Distance(this.transform.position, pointEnd.position);
        directionFinal = (pointEnd.position - transform.position).normalized * speed;
    }

    void Update() {
        matchVel();
        if (!isAlt)
            movetoPoints(directionInter, pointInter, origDistInter);
        else movetoPoints(directionFinal, pointEnd, origDistFinal);
    }

    void movetoPoints(Vector3 direction, Transform point, float originalDistance) {
        if (Vector3.Distance(this.transform.position, point.position) > 1f && toggle) {
            m_rb.velocity = direction;
        } else {
            m_rb.velocity = new Vector3(0, 0, 0);
            toggle = false;
        }
        if (!toggle) {
            m_rb.velocity = -direction;
            if (Vector3.Distance(this.transform.position, point.position) > originalDistance) {
                m_rb.velocity = new Vector3(0, 0, 0);
                toggle = true;
            }
        }
    }

    void matchVel() {

        if (triggered) {
            Vector3 CurrentLocalVelocity = this.transform.TransformDirection(m_rb.velocity);

            m_rbOther.AddRelativeForce(CurrentLocalVelocity);
            //m_rbOther.MovePosition(this.transform.position);
        } 
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.CompareTag("Player")) {
            m_rbOther = coll.gameObject.GetComponent<Rigidbody>();
            isAlt = true;
        }
    }
    void OnCollisionStay(Collision coll) {
        if (coll.gameObject.CompareTag("Player")) {
            triggered = true;
        }
    }
    void OnCollisionExit(Collision coll) {
        if (coll.gameObject.CompareTag("Player")) {
            m_rbOther.velocity = new Vector3(0,0,0);
            triggered = false;
        }
    }
}
