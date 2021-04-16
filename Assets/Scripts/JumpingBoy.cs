using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingBoy : Unit
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 15f;
    private bool isGrounded = false;
    private bool isImmortal = false;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private Bullet bullet;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        bullet = Resources.Load<Bullet>("Bullet");
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if(isGrounded) State = CharState.idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }

    private void Run()
    {
        if(isGrounded) State = CharState.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        if (!isGrounded) State = CharState.jump;
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 spawnPos = transform.position; spawnPos.y += 0.6f;
        Bullet newBullet = Instantiate(bullet, spawnPos, bullet.transform.rotation) as Bullet;


        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1f : 1f);
    }

    public override void ReceiveDamage()
    {
        if (!isImmortal)
        {
            lives--;

            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * 3f, ForceMode2D.Impulse);

            if (sprite.flipX)
                rb.AddForce(transform.right * 4.5f, ForceMode2D.Impulse);
            else
                rb.AddForce(transform.right * -4.5f, ForceMode2D.Impulse);
            StartCoroutine(DamageColor());

            isImmortal = true;

            Debug.Log(lives);
        }
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        if(!isGrounded) State = CharState.jump;
    }

    private IEnumerator DamageColor()
    {
        sprite.color = new Color(0.99609375f, 0.54296875f, 0.54296875f);
        yield return new WaitForSeconds(0.25f);
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(1);
        sprite.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.4f);
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.4f);
        sprite.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.4f);
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.4f);
        sprite.color = new Color(1f, 1f, 1f, 1f);
        isImmortal = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();

        if (unit)
            ReceiveDamage();
    }
}

public enum CharState
{
    idle,
    run,
    jump
}
