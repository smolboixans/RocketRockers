using System;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float velocity;
    [SerializeField] private float lifetime = 7;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = velocity * transform.up;
    }

    void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime < 0) Destroy(this.gameObject);
    }
}
