using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    [SerializeField]
    private float health = 3f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletPos;

    [SerializeField]
    private float knockbackForceMultiplier = 1f;

    private Transform target;
    private IWeapon currentWeapon;
    private float shootingRange = 12f;
    private bool alerted = false;
    private bool isShooting = false;
    private float attackCooldown;
    private float currentCooldown;
    private Animator anim;
    private Vector2 originalPosition;
    private bool isFollowing = false;
    private float alertTimer = 3f;
    private float currentTimer;
    private bool dead;
    private bool canShoot = true;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        currentWeapon = GetComponentInChildren<IWeapon>();
        attackCooldown = currentWeapon.getCooldownForWeapon();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        originalPosition = transform.position;
        currentCooldown = 0;
        currentTimer = alertTimer;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void ShootOrMove()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        Vector3 direction = (target.position - transform.position).normalized;
        if (distanceToPlayer > shootingRange)
        {
            anim.SetBool("isMoving", true);
            FlipSprite(direction.x);
            agent.SetDestination(target.position);
            isFollowing = true;
        }
        else
        {
            anim.SetBool("isMoving", false);
            Vector2 currentPos = transform.position;
            // Stop following and start shooting logic
            isFollowing = false;
            FlipSprite(direction.x);
            agent.SetDestination(currentPos);
            Shoot();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (health <= 0)
        {
            if (rb != null)
                rb.isKinematic = true;
            if (col != null)
                col.enabled = false;
            Death();
        }
        else if (alerted)
        {
            currentTimer -= Time.deltaTime;
            currentCooldown -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                alerted = false;
                currentTimer = alertTimer;
            }
            else
            {
                //Checks whether the enemy is near the player, if yes he shoots, otherwise it moves closer
                ShootOrMove();
            }
        }
        else if (isFollowing && !isShooting)
        {
            agent.SetDestination(originalPosition);
            isFollowing = false;
            anim.SetBool("isMoving", true);
            if (transform.position.x < originalPosition.x)
            {
                FlipSprite(1f); // Assuming the original facing direction is right
            }
            else
            {
                FlipSprite(-1f); // Assuming the original facing direction is left
            }
        }
        else if (
            Mathf.Abs(originalPosition.x - transform.position.x) <= 0.25f
            && Mathf.Abs(originalPosition.y - transform.position.y) <= 0.25f
        )
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void FlipSprite(float directionX)
    {
        if (directionX > 0) // Moving right
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (directionX < 0) // Moving left
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    private void Shoot()
    {
        if (currentCooldown <= 0 && canShoot)
        {
            if (currentWeapon == null)
            {
                Debug.Log("Current weapon is null");
            }
            else
            {
                anim.SetTrigger("Shoot");
                currentWeapon.ShootWeapon(bulletPos, bulletPrefab);
                currentCooldown = attackCooldown;
            }
        }

        currentCooldown -= Time.deltaTime;
    }

    public void Alerted()
    {
        alerted = true;
        currentTimer = alertTimer;
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(ShootAfterHit(1f));
        anim.SetTrigger("Hit");
        health -= damage;
        alerted = true;
        StartCoroutine(HitBloodFlow());

        float knockbackForce = damage * knockbackForceMultiplier;
        Vector2 knockbackDirection = (transform.position - target.position).normalized;
        Knockback(knockbackDirection, knockbackForce, 0.3f);
    }

    public void HitByCar(float damage)
    {
        StartCoroutine(ShootAfterHit(3f));
        anim.SetTrigger("Hit");
        health -= damage;
        alerted = true;
        StartCoroutine(HitBloodFlow());

        float knockbackForce = damage * knockbackForceMultiplier;
        Vector2 knockbackDirection = (transform.position - target.position).normalized;
        Knockback(knockbackDirection, knockbackForce, 1.2f);
    }

    IEnumerator ShootAfterHit(float waitTime)
    {
        canShoot = false;
        yield return new WaitForSeconds(waitTime);
        canShoot = true;
    }

    private void Knockback(Vector2 direction, float force, float forceMultip)
    {
        if (rb != null)
        {
            agent.isStopped = true;
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * force * forceMultip, ForceMode2D.Impulse);
        }
        StartCoroutine(ResetRigidbody2D(rb, 3));
    }

    IEnumerator ResetRigidbody2D(Rigidbody2D rb, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Reset velocity after knockback
        }
        agent.isStopped = false;
    }

    private void Death()
    {
        agent.isStopped = true;
        anim.SetBool("isDead", true);
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator HitBloodFlow()
    {
        if (health <= 0)
        {
            yield return null;
        }
        //Constant blood flow
        yield return new WaitForSeconds(5f);
    }

    private IEnumerator DeathAnimation()
    {
        //Let out blood

        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
