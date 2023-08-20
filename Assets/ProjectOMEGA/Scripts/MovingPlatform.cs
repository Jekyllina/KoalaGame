using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
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
        //if (rb.position.x >= maxX)
        //{            
        //    DirectionX *= -1;
        //}
        //else if (rb.position.x <= minX)
        //{            
        //    DirectionX *= -1;
        //}

        //Vector2 moveDirection = new((DirectionX * Speed), 0);
        //rb.velocity = moveDirection;

        if (rb.position.x >= maxX)
        {
            DirectionX *= -1;
        }
        else if (rb.position.x <= minX)
        {
            DirectionX *= -1;
        }

        Vector2 moveDirection = new((DirectionX * Speed), 0);
        
        transform.Translate(moveDirection);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && collision.transform.position.y > transform.position.y)
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
        else            
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
