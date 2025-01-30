using UnityEngine;
using System.Collections;
using MoreMountains.TopDownEngine;


public class SoldierCharacter : MonoBehaviour
{
 [Header("Combat")]
 [SerializeField] private float detectionRadius = 5f;
 [SerializeField] private float fireRate = 1f;
 [SerializeField] private float damagePerShot = 20f;

 [Header("References")]
 [SerializeField] private Transform weaponPivot;
 [SerializeField] private LayerMask enemyLayer;

 private Transform currentTarget;
 private Coroutine shootingCoroutine;

 private void Awake()
 {
  var detectionCollider = gameObject.AddComponent<SphereCollider>();
  detectionCollider.radius = detectionRadius;
  detectionCollider.isTrigger = true;
 }

 private void OnTriggerEnter(Collider other)
 {
  if (!currentTarget && (enemyLayer.value & (1 << other.gameObject.layer)) != 0)
  {
   currentTarget = other.transform;
   shootingCoroutine = StartCoroutine(ShootingRoutine());
  }
 }

 private IEnumerator ShootingRoutine()
 {
  while (currentTarget != null)
  {
   if (currentTarget.gameObject.activeInHierarchy)
   {
    FaceTarget();
    FireShot();
   }
   yield return new WaitForSeconds(1f / fireRate);
  }
 }

 private void FaceTarget()
 {
  Vector3 direction = currentTarget.position - weaponPivot.position;
  weaponPivot.rotation = Quaternion.LookRotation(direction);
 }

 private void FireShot()
 {
  if (Physics.Raycast(weaponPivot.position, weaponPivot.forward,
      out RaycastHit hit, detectionRadius, enemyLayer))
  {
   // Access the TopDown Engine's Health component correctly
   if (hit.collider.TryGetComponent<MoreMountains.TopDownEngine.Health>(out var health))
   {
    // TopDown Engine's Damage method parameters:
    health.Damage(
        damagePerShot,          // float damage
        this.gameObject,        // GameObject instigator
        0.1f,                   // float flickerDuration
        0f,                     // float invincibilityDuration
        Vector3.zero,           // Vector3 damageDirection
        null                    // List<TypedDamage> typedDamages (optional)
    );
   }
  }
 }
}