using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformUp : MonoBehaviour
{
    public float MaxY = 4;
    public float MinY = 4;
    public float Speed = 3;

    Rigidbody2D rb;
    float maxY;
    float minY;
    int DirectionY = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxY = rb.position.y + MaxY;
        minY = rb.position.y - MinY;       
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

        if (rb.position.y >= maxY)
        {
            DirectionY *= -1;
        }
        else if (rb.position.y <= minY)
        {
            DirectionY *= -1;
        }

        Vector2 moveDirection = new(0, (DirectionY * Speed));
        
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
