using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField]
    private float driftFactor = 0.95f;

    [SerializeField]
    private float accelerationFactor = 30.0f;

    [SerializeField]
    private float turnFactor = 3.5f;

    [SerializeField]
    private float maxSpeed = 20;

    [SerializeField]
    private float carSpeed = 0;

    [SerializeField]
    private float carDamage = 1;

    [SerializeField]
    private bool isBraking;

    [SerializeField]
    private bool isTurbo;

    [SerializeField]
    private float health = 10f;

    [SerializeField]
    CircleCollider2D aggressionCollider;

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;
    private float upDownVelocity = 0;

    [SerializeField]
    private bool invincibility = false;
    private Rigidbody2D carRigidbody2D;
    private CameraShake cameraShake;
    private List<EnemyTest> enemiesInAggroRange = new List<EnemyTest>();

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        cameraShake = GameObject.Find("Virtual Camera").GetComponent<CameraShake>();
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            Death();
        }
        else
        {
            if (enemiesInAggroRange.Count > 0)
            {
                foreach (EnemyTest enemy in enemiesInAggroRange)
                {
                    enemy.Alerted();
                }
            }
            ApplyEngineForce();
            ThrottleUpwardsVelocity();
            ApplySteering();
            carSpeed = carRigidbody2D.velocity.magnitude;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyTest enemy = other.GetComponent<EnemyTest>();
            if (enemy != null && !enemiesInAggroRange.Contains(enemy))
            {
                enemiesInAggroRange.Add(enemy);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyTest enemy = other.GetComponent<EnemyTest>();
            if (enemy != null && enemiesInAggroRange.Contains(enemy))
            {
                enemiesInAggroRange.Remove(enemy);
            }
        }
    }

    void ApplyEngineForce()
    {
        //Calculates the speed at which we are going in accordance to the cars direction
        upDownVelocity = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        //Limits the max speed forward
        if (upDownVelocity > maxSpeed && accelerationInput > 0)
        {
            return;
        }
        //Limits the max speed of reversing to 50%
        if (upDownVelocity < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }
        //Limits the speed so the car cant move faster diagonally while accelerating
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        //Apply drag if no accelInput.
        if (accelerationInput == 0)
        {
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
            carRigidbody2D.drag = 0;

        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        float turningReq = (carRigidbody2D.velocity.magnitude / 8);
        turningReq = Mathf.Clamp01(turningReq);

        rotationAngle -= steeringInput * turnFactor * turningReq;
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    public void SetInputVector(Vector2 inputVector, bool braking, bool turbo)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
        isBraking = braking;
        isTurbo = turbo;
    }

    void ThrottleUpwardsVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity =
            transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (cameraShake != null)
            {
                //float carSpeed = carRigidbody2D.velocity.magnitude;
                cameraShake.ShakeCamera();
            }
        }
        if (collision.gameObject.CompareTag("Enemy") && carSpeed >= 10)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.GetComponent<EnemyTest>().HitByCar(carDamage * carSpeed * 0.1f);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && carSpeed >= 10)
        {
            //other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void TakeDamage(float damage)
    {
        if (invincibility)
        {
            return;
        }
        StartCoroutine(InvincibilityPeriod());
        health -= damage;
        Debug.Log("Hit");
    }

    IEnumerator InvincibilityPeriod()
    {
        invincibility = true;
        yield return new WaitForSeconds(0.5f);
        invincibility = false;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (accelerationInput < 0 && upDownVelocity > 0)
        {
            isBraking = true;
            return true;
        }
        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
        {
            return true;
        }
        return false;
    }

    private IEnumerator Death()
    {
        Debug.Log("You are dead");
        yield return new WaitForSeconds(3f);
    }
}
