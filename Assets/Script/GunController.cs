using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Animator gunAnim;
    [SerializeField] private Transform gun;
    [SerializeField] private float gunDistance = 1.0f;

    private bool gunFacingRight = true;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    public int currentBullet;
    public int maxBullet=30;


    private void Start()
    {
        Relode();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        // help to find the angle of the mouse pointer and adjust in that direction
        gun.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0, 0);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot(direction);
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Relode();
        
        }

        GunFlipController(mousePos);
    }

    private void GunFlipController(Vector3 mousePos)
    {
        
        if (mousePos.x < gun.position.x && gunFacingRight)
        {
            GunFlip();

        }
        else if (mousePos.x > gun.position.x && !gunFacingRight) { GunFlip(); }
    }

    private void GunFlip()
    {
        gunFacingRight = !gunFacingRight;
        gun.localScale = new Vector3(gun.localScale.x, gun.localScale.y * -1, gun.localScale.z);
    }

    public void Shoot(Vector3 direction) 
        {

            if (currentBullet <= 0)
        {
            return;
        }
            gunAnim.SetTrigger("Shoot");

            currentBullet--;

            GameObject newBullet = Instantiate(bulletPrefab,gun.position,Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;  
            Destroy(newBullet,4);

        }
    private void Relode()
    {
        currentBullet = maxBullet;
    }
}
