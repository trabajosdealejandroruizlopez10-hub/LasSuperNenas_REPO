using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public Camera cam;

    [Header("Shooting Settings")]
    public float range = 100f;
    public int damage = 1;
    public float fireRate = 0.2f;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            
            
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

           
            Debug.DrawLine(cam.transform.position, hit.point, Color.red, 0.5f);
        }
        else
        {
            Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * range, Color.yellow, 0.5f);
        }
    }
}