using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //public string paramSpeedName;
    [Header("Params")]
    public float moveSpeed = 2;
    public float jumpForce = 2;
    [Range(0f, 1f)]
    public float nextJumpScale = 0.5f;
    public int maxJumps = 2;

    [Header("Fall damage")]
    [SerializeField] private float fallDamageStart = 15f;
    [SerializeField] private float fallDamageKill = 30f;

    [Header("Check Settings")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundMask;

    [Header("Animations")]

    [SerializeField] private string speedAnimationParameter;
    [SerializeField] private AnimationCurve hitStunCurve;
    public bool IsDamaged;
    public float DamageStunTime = 0.2f;
    public Sprite DamageSprite;

    private Rigidbody2D rb;
    private PlayerCharacter pc;
    public int jumps = 0;
    public bool isGrounded;
    private bool wasGrounded;
    public float fallTime = 0;
    public bool isDead;
    private bool canPlay;
    private float running = 0;
    private float speed;
    private float damageTime = 0;
    private float comboTime = 0;
    private int comboCount = 0;
    private Vector2 startPos = Vector2.zero;

    #region Unity Methods

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCharacter>();
        canPlay = true;
        speed = moveSpeed;
        startPos = transform.position;
    }
    
    void Update()
    {
        if (transform.position.y < -5.0f)
        {
            transform.position = startPos;
            //isDead = true;
        }

        if (canPlay)
        {
            Move();
            CheckFallDamage();            
        }

        if (IsDamaged)
        {
            transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = DamageSprite;
            pc.SkinManager.currentSkin.GetAnimator().enabled = false;
            canPlay = false;

            //DA INSERIRE logica per non ricevere danni

            damageTime += Time.deltaTime;
            
            if(damageTime >= DamageStunTime)
            {
                IsDamaged = false;
                pc.SkinManager.currentSkin.GetAnimator().enabled = true;
                damageTime = 0;
                canPlay = true;
            }
        }

        if (isDead)
        {
            pc.SkinManager.currentSkin.GetAnimator().SetTrigger("IsDead");
            canPlay = false;
            isDead = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == 7 && !isGrounded)
        {
            rb.velocity = Vector2.up * (jumpForce + 2);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Platform")
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.tag == "Platform")
        {
            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<ParticleSystem>().Play();

            StartCoroutine(DestroyThis(collision.gameObject));            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Crate" && isGrounded)
        {
            pc.SkinManager.currentSkin.GetAnimator().SetBool("IsPushing", true);
            moveSpeed = speed - 2;
        }

        if(InputManager.HORIZONTALMOVE == 0 || !isGrounded)
            pc.SkinManager.currentSkin.GetAnimator().SetBool("IsPushing", false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Crate")
        {
            pc.SkinManager.currentSkin.GetAnimator().SetBool("IsPushing", false);
            moveSpeed = speed;
        }
    }
    #endregion

    IEnumerator DestroyThis(GameObject obj)
    {       
        yield return new WaitForSeconds(1f);
        Destroy(obj);
    }

    private void CheckFallDamage()
    {
        //if (!wasGrounded && isGrounded)
        //{
        //    if (rb.velocity.y < -fallDamageStart)
        //    {
        //        // Apply fall damage
        //        float damagePerc = Mathf.Abs(rb.velocity.y + fallDamageStart) / fallDamageKill;
        //        float damage = pc.GetMaxHealth() * damagePerc;
        //        Debug.Log($"Damage: {damage}");
        //        pc.GetDamage(damage);
        //    }
        //    else { Debug.Log("It's not getting damage"); }
        //}        

        if (rb.velocity.y < 0)
            fallTime += Time.deltaTime;

        if(isGrounded)
        {
            if (fallTime > 0.7f)
            {                
                float damage = fallTime * 80;
                Debug.Log($"Damage: {damage}");
                pc.GetDamage(damage);
            }

            fallTime = 0;
        }
    }

    void Move()
    {
        // Sets move direction
        var x = InputManager.HORIZONTALMOVE * moveSpeed;

        Vector2 moveDirection = new(x, rb.velocity.y);

        // Apply movement
        rb.velocity = moveDirection;

        if (rb.velocity.x != 0)
            running += Time.deltaTime;
        else
            running = 0;

        //if (running >= 1.5)
        //    moveSpeed = speed + 3;
        //else if (running < 1.5 && running > 0)
        //    moveSpeed = speed;

        if(isGrounded)
            pc.SkinManager.currentSkin.GetAnimator().SetFloat(speedAnimationParameter, running);

        // Flip sprite - NOT YET. We don't know how the animation should work
        // Note 2: you can also put the X scale to -1
        if (rb.velocity.x < -.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
        }
        else if (rb.velocity.x > .1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
        }

        wasGrounded = isGrounded;
        isGrounded = IsGrounded();

        // Perform jump
        if (isGrounded)
        {
            jumps = 0;
        }

        pc.SkinManager.currentSkin.GetAnimator().SetBool("IsGrounded", isGrounded);

        if (isGrounded && InputManager.JUMPBUTTON)
        {
            Jump();
        }

        if (!isGrounded && InputManager.JUMPBUTTON && jumps < maxJumps)
            Jump();

        if (isGrounded && InputManager.ATTACKBUTTON && comboCount <= 2)
            Attack();
    }

    void Jump()
    {        
        rb.velocity = Vector2.up * jumpForce * (Mathf.Pow(nextJumpScale, jumps));
        pc.SkinManager.currentSkin.GetAnimator().SetTrigger("IsJumping");
        //pc.SkinManager.currentSkin.Jump();
        jumps++;        
    }

    void Attack()
    {
        if (comboCount == 0 || comboCount == 1)
        {
            pc.SkinManager.currentSkin.GetAnimator().SetTrigger("comboAttack");
        }
        else if (comboCount == 2)
            comboCount = 0;

        comboTime += Time.deltaTime;
        
        if (/*comboTime >= 0.5f && comboTime <= 0.9f*/ hitStunCurve.Evaluate(comboTime) >= 1)
        {
            comboCount++;
            comboTime = 0;
        }
        else
        {
            comboCount = 0;
            comboTime = 0;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask);
    }
}
