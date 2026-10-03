using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private InputActionReference move, shoot;
    [SerializeField] private float boostPower = 5;
    [SerializeField] private float turnPower = 5;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private GameObject projectile;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (Game.instance.gameOver) return;

        Vector2 inpDir = move.action.ReadValue<Vector2>();

        if (inpDir != Vector2.zero)
        {
            //rotate to side
            rb.MoveRotation(rb.rotation - (inpDir.x * turnPower));
            //boost forward
            rb.AddForce(transform.up * boostPower * inpDir.y);
            if (!ps.isPlaying) ps.Play();
        } else
            if (ps.isPlaying) ps.Stop();

        //shooting mechanic
        if (shoot.action.triggered) Instantiate(projectile, transform.position + transform.up, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //game over    
        Game.instance.EndGame();
    }
}
