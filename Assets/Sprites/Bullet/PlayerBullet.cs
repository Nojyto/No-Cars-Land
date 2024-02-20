using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public ParticleSystem impactParticles;
    public float force;
    public float damage = 2;
    public float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        impactParticles.Stop();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x,direction.y).normalized*force;
        float rot = Mathf.Atan2(rotation.y,rotation.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot+90);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>3f){
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other){
        impactParticles.Play();
        if(other.gameObject.CompareTag("Enemy")){
            other.gameObject.GetComponent<EnemyTest>().TakeDamage(damage);
            Instantiate(impactParticles,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Obstacle")){
            Instantiate(impactParticles,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("EnemyBullet")){
            Instantiate(impactParticles,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
