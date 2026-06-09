using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 500f;
    [SerializeField] private float fireRate = 0.1f;
    
    private float nextFireTime;

    private void Update()
    {
        if (Input.GetMouseButton(0) &&
        Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Fire();
        }
    }

    private void Fire()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Health health = hit.collider.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
