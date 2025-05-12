using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //public float speed;
    //private Rigidbody2D rb;

    void Awake()
    {
        Destroy(gameObject, 4.0f);
        //rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null) return;

        ChestManager chest = collision.GetComponent<ChestManager>();
        if(chest != null)
        {
            chest.TakeDamage(1);
        }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            GameObject playerObject = GameObject.Find("Player");
            enemy.TakeDamage(1, playerObject.transform.position);
        }

        Destroy(gameObject);
    }
}
