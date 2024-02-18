using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public int maxHealth = 2;
    public int currentHealth;
    //public float despawnTime = 4f;
    private Rigidbody2D rb;
    public float DamageTime;
    private Animator animator;
    private SpriteRenderer spriteRend;
    //public GameObject JoystickArea;
    public bool isPlayer, isBoss, isMob, isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DamageTime -= Time.deltaTime;
        //if (isDead && !isPlayer)
        //{
        //    despawnTime -= Time.deltaTime;
        //    GetComponent<Collider2D>().enabled = false;
        //}
        //if (despawnTime <= 0)
        //{
        //    Destroy(gameObject);
        //}
        if (DamageTime > 0)
        {
            spriteRend.color = Color.red;
        }
        else
        {
            spriteRend.color = Color.white;
        }


    }

    public void TakeDamage(int amount)
    {
        DamageTime = 0.15f;
        currentHealth -= amount;
        Debug.Log(currentHealth);

        switch (currentHealth)
        {
            case <= 0:
                if (isPlayer)
                {
                    isDead = true;
                    animator.SetBool("IsDead", true);
                    //JoystickArea.SetActive(false);
                    rb.constraints = RigidbodyConstraints2D.FreezePosition;
                }
                else if (isMob)
                {
                    isDead = true;
                    animator.SetBool("IsDead", true);
                    //rb.constraints = RigidbodyConstraints2D.FreezePosition;
                }
                else if (isBoss) { }
                break;
            default:
                if (isPlayer)
                {

                }
                break;
            
                

        }

        //if (isMob == true && currentHealth == maxHealth / 2)
        //    animator.SetTrigger("IsHalf");
    }
}
