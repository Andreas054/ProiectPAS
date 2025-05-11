using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //public float speed;
    //private Rigidbody2D rb;

    void Start()
    {
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
        Destroy(gameObject);
    }
}
