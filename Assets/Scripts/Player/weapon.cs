using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform sideFirePoint;
    public Transform headFirePoint;
    public GameObject bulletPrefab;
    public Animator animator;
    public float shootIntervalTime = .5f;

    public bool isShootingUp = false;
    private float animationDuration;
    private float shootingAnimationDuration = 0.8f;
    private float timeLastShoot = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica si el jugador quiere disparar arriba
        bool wasShootingUp = isShootingUp;
        isShootingUp = Input.GetKey(KeyCode.W);
        if (isShootingUp != wasShootingUp)
        {
            animator.SetBool("IsShootingUp", isShootingUp);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animationDuration = Time.time + shootingAnimationDuration;
            if (!animator.GetBool("IsShooting"))
            {
                animator.SetBool("IsShooting", true);
            }
            Shoot(isShootingUp ? headFirePoint : sideFirePoint);
        }
        // Comprueba si el jugador mantiene el boton de disparo
        else if (Input.GetButton("Fire1"))
        {
            if (!animator.GetBool("IsShooting"))
            {
                animator.SetBool("IsShooting", true);
            }
            if (Time.time >= animationDuration)
            {
                animationDuration = Time.time + shootingAnimationDuration;
                Shoot(isShootingUp ? headFirePoint : sideFirePoint);
            }
        }
        else
        {
            if (animator.GetBool("IsShooting"))
            {
                Invoke("StopShooting", .5f);
            }
        }
    }

    void Shoot(Transform firePoint)
    {

        if (Time.time >= timeLastShoot)
        {
            timeLastShoot = Time.time + shootIntervalTime;

            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletInstance.GetComponent<Bullet>();
            if (bullet != null)
            {
                // Configura la dirección de la bala
                bullet.SetDirection(firePoint == sideFirePoint);
            }
        }
    }
    void StopShooting()
    {
        animator.SetBool("IsShooting", false);
    }
}