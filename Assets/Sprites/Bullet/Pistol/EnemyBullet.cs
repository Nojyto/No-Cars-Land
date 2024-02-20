using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    
    private GameObject player;
    private Rigidbody2D rb;
    public ParticleSystem impactParticles;
    public float force;
    private float bulletDamage = 0;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x,direction.y).normalized*force;
        float rot = Mathf.Atan2(-direction.y,-direction.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot+90);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>3){
            Destroy(gameObject);
        }
    }
    public void SetDamage(float damage){
        bulletDamage = damage;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Hit Player");
            other.gameObject.GetComponent<CarController>().TakeDamage(bulletDamage);
            Instantiate(impactParticles,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Obstacle")){
            Instantiate(impactParticles,transform.position,Quaternion.identity);
            Destroy(gameObject);
            Debug.Log("Hit Obstacle");
        }
        else if(other.gameObject.CompareTag("Bullet")){
            Instantiate(impactParticles,transform.position,Quaternion.identity);
            Destroy(gameObject);
            Debug.Log("Hit Bullet");
        }
        // else if(other.gameObject.CompareTag("Enemy")){
        //     Instantiate(impactParticles,transform.position,Quaternion.identity);
        //     Destroy(gameObject);
        //     Debug.Log("Hit Enemy");
        // }
    }
}
