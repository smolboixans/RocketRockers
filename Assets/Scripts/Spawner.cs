using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float fireRate = 1.2f;

    float cd; //cooldown

    void Update() {
        if (!projectile) return;

        if (!(Game.instance.gameStart > 0f || Game.instance.gameOver)) {
        if (cd < 0) {
            cd = fireRate;
            float newX = Random.Range(-(transform.localScale.x/2), transform.localScale.x/2);
            Instantiate(projectile, transform.position + (Vector3.one * newX), transform.rotation);
        } else 
            cd -= Time.deltaTime;

        if (fireRate > 0.2f)
            fireRate -= Time.deltaTime * 0.02f;
        }
    }
}
