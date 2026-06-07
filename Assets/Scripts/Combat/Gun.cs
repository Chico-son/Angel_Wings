using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 500f;

    private void Update()
    {
        //Left Mouse button
        if (Input.GetMouseButtonDown(0))
        {
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
