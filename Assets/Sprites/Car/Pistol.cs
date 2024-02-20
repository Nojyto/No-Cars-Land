using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire = true;
    private float timer;
    public float timeBetweenFiring;
    public float allowedSideAngle = 60f;
    public float magSize = 20;
    public float currentMagSize;
    public float reloadTime = 5;
    private float reloadTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentMagSize = magSize;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        // Flip the sprite if the mouse is on the left side
        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }
        if (Input.GetMouseButton(0) && canFire && currentMagSize > 0)
        {
            canFire = false;
            currentMagSize -= 1;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        }
        else if (currentMagSize <= 0)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime)
            {
                currentMagSize = magSize;
                reloadTimer = 0;
            }
        }
    }
}
