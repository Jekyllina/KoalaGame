using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float MaxX = 4;
    public float MinX = 4;
    public float Speed = 3;

    Rigidbody2D rb;
    float maxX;
    float minX;
    int DirectionX = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxX = rb.position.x + MaxX;
        minX = rb.position.x - MinX;       
    }    
    
    void FixedUpdate()
    {
        if(!GetComponent<HealthManager>().isDead)
        {
            if (rb.position.x >= maxX)
            {
                DirectionX *= -1;
            }
            else if (rb.position.x <= minX)
            {
                DirectionX *= -1;
            }

            Vector2 moveDirection = new((DirectionX * Speed), 0);
            rb.velocity = moveDirection;

            if (rb.velocity.x < -.1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
            }
            else if (rb.velocity.x > .1f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
            }
        }

        if (GetComponent<HealthManager>().isDead)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == 6)
        {
            if (!GetComponent<HealthManager>().isDead)
                other.collider.gameObject.GetComponent<PlayerController>().isHit = true;
        }
    }
}
