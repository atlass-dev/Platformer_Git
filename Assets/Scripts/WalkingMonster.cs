using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    private float speed = 1f;
    private Vector3 dir;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        dir = transform.right;
        lives = 5;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.3f + transform.right * dir.x * 0.595f, 0.01f);

        if (colliders.Length > 0 && colliders[0].gameObject != Hero.Instance.gameObject) dir *= -1f;
        if (colliders.Length > 0 && colliders[0].gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            dir *= -1f;
        }
        sprite.flipX = dir.x < 0.0f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir,speed* Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.3f + transform.right * dir.x * 0.58f, 0.01f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }
    public override void Die()
    {
        Destroy(this.gameObject);
        gameObject.tag = "enemy_dead";
        LevelController.Instance.EnemiesCount();
    }
}
