using UnityEngine;

public class Weapon : MonoBehaviour
{
    // This script handles the players weapon system, it is attached to the ship's firepoints

    public float damage; // damage of the weapons
    public float range; // range of the weapons 
    public float fireRate; // firerate of the weapons

    public Transform firePoint; // location of the firepoint
    public ParticleSystem muzzleFlash; // effect for the mussle flash
    public GameObject impactEffect; // effect for the impact
    public TrailRenderer laserEffect; // effect for the laser beam
    private float nextTimeToFire = 0f; // float for setting the rate of fire

    Ray laser; // raycasyt for the weapon
    RaycastHit hit; // raycast hit for the weapon

    // Update is called once per frame
    void Update()
    {
        // Check to make sure ship input is allowed. This stops input being sent to the ship while the splash screen is up.
        if (EventManager.acceptShipInput == true)
        {
            // Fires the weapon if them Unity fire button is down and the fire cooldown is complete
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

    }

    // Function that handles the weapons firing and effect on target
    void Shoot()
    {
        // Bugfix to keep the weapons angled along the plane of y, even when ship is listing due to rigidbody forces. 
        Vector3 firingSolution = firePoint.forward;
        firingSolution.y = 0f;
        firePoint.forward = firingSolution;

        // Setting up the laser firing coordinates.
        laser.origin = firePoint.position;
        laser.direction = firePoint.forward;

        // Play shooting effect
        muzzleFlash.Play();
        // Play shooting sound
        FindObjectOfType<AudioManager>().Play("PlayerWeapon");

        var laserBeam = Instantiate(laserEffect, firePoint.position, Quaternion.identity); // Generate an instance of the laserbeam effect
        laserBeam.AddPosition(firePoint.position); // Set the start position of the beam

        laserBeam.transform.position = firePoint.position + (firePoint.forward * range); // Set the destination of the beam

        // Checking for hits
        if (Physics.Raycast(laser, out hit, range))
        {
            EnemyHandler target = hit.transform.GetComponent<EnemyHandler>(); // Getting the handler of the enemy that has been hit

            // If the target is not invalid
            if (target != null)
            {
                target.TakeDamage(damage); // send damage amount to its script
            }

            laserBeam.transform.position = firePoint.position + (firePoint.forward * hit.distance); // Updates the destination of the beam to match the hit location

            GameObject impactObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // generate the impact effect on target

            Destroy(impactObject, 1f); // clean up impact effect
        }
    }
}
