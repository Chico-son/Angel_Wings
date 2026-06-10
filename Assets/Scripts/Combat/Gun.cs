using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 500f;
    [SerializeField] private float fireRate = 0.1f;

    [Header("Muzzle light")]
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private float muzzleLightDuration = 0.05f;

    [Header("References")]
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffectPrefab;

    [Header("Tracer")]
    [SerializeField] private LineRenderer tracerPrefab;
    [SerializeField] private float tracerDuration = 0.05f;
    [SerializeField] private Color tracerColor = new Color(1f, 0.8f, 0.2f, 1f);
    [SerializeField] private float tracerWidth = 0.15f;

    private float nextFireTime;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        if (gunBarrel == null) return;

        Vector3 origin = gunBarrel.position;
        Vector3 direction = gunBarrel.forward;
        Vector3 hitPoint = origin + direction * range;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, range))
        {
            hitPoint = hit.point;

            Health targetHealth = hit.collider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }

            SpawnImpactEffect(hit.point, hit.normal);
        }

        PlayMuzzleFlash();
        FlashMuzzleLight();
        SpawnTracer(origin, hitPoint);
    }

    private void FlashMuzzleLight()
    {
        if (muzzleLight == null) return;
        StartCoroutine(MuzzleLightRoutine());
    }

    private IEnumerator MuzzleLightRoutine()
    {
        muzzleLight.SetActive(true);
        yield return new WaitForSeconds(muzzleLightDuration);
        muzzleLight.SetActive(false);
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play(true);
        }
    }

    private void SpawnTracer(Vector3 start, Vector3 end)
    {
        if (tracerPrefab == null) return;

        LineRenderer tracer = Instantiate(tracerPrefab, Vector3.zero, Quaternion.identity);
        tracer.startColor = tracerColor;
        tracer.endColor = tracerColor;
        tracer.startWidth = tracerWidth;
        tracer.endWidth = tracerWidth;
        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);

        Destroy(tracer.gameObject, tracerDuration);
    }

    private void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        if (impactEffectPrefab == null) return;

        GameObject impact = Instantiate(
            impactEffectPrefab,
            position,
            Quaternion.LookRotation(normal)
        );
        impact.transform.localScale = Vector3.one * 20f;
        Destroy(impact, 1f);
    }
}