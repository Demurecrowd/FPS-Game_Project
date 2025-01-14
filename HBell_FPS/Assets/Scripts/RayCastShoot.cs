﻿using UnityEngine;
using System.Collections;

public class RayCastShoot : MonoBehaviour
{
    FlashLight flashlight;

    
    public int gunDamage = 1;
    public float fireRate = 0.01f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;


    void Start()
    {
        flashlight = GetComponent<FlashLight>();
        laserLine = GetComponent<LineRenderer>();

        gunAudio = GetComponent<AudioSource>();

        fpsCam = GetComponentInParent<Camera>();
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && flashlight.BatteryLife >= flashlight.maxValue / 10f)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                //ShootableBox health = hit.collider.GetComponent<ShootableBox>();

                //if (health != null)
                //{
                //    health.Damage(gunDamage);
                //}

                IShootable health = hit.collider.GetComponent<IShootable>();
                print(health);
                if (health != null)
                {
                    health.ShotReaction( hit, gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }

            flashlight.Fired();
        }
    }


    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }


   
}