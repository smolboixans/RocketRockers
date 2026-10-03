using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float speed;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = speed * transform.up;
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag != "Player" && collision.collider.tag != "Bounds") {
            Destroy(collision.collider.gameObject);
            Game.instance.AddScore(5);
        }

        Destroy(gameObject);
    }
}
