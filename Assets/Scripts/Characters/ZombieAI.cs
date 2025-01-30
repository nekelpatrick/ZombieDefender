using UnityEngine;
using UnityEngine.AI;
using MoreMountains.TopDownEngine;

[RequireComponent(typeof(Character))]
public class ZombieAI : MonoBehaviour
{
 [Header("Settings")]
 [SerializeField] private float moveSpeed = 2f;
 [SerializeField] private float damageOnReachEnd = 50f;
 [SerializeField] private Transform trenchTarget;

 private NavMeshAgent _navMeshAgent;
 private Health _health;

 private void Awake()
 {
  _navMeshAgent = GetComponent<NavMeshAgent>();
  _health = GetComponent<Health>();
  _health.OnDeath += HandleDeath;

  _navMeshAgent.speed = moveSpeed;
 }

 private void Start()
 {
  if (trenchTarget != null)
  {
   _navMeshAgent.SetDestination(trenchTarget.position);
  }
 }

 private void OnCollisionEnter(Collision collision)
 {
  if (collision.gameObject.CompareTag("Trench"))
  {
   // TopDown Engine's Damage method parameters:
   // float damage, GameObject instigator, float flickerDuration, 
   // float invincibilityDuration, Vector3 damageDirection
   _health.Damage(
       damageOnReachEnd,
       gameObject,       // Instigator
       0.1f,            // Flicker duration
       0f,              // Invincibility duration
       Vector3.zero     // Damage direction
   );
  }
 }

 private void HandleDeath()
 {
  _health.OnDeath -= HandleDeath;
  Destroy(gameObject);
 }
}