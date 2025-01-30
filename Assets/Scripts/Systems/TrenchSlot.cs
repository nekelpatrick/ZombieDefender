using UnityEngine;

public class TrenchSlot : MonoBehaviour
{
 [SerializeField] private Transform unitAnchor;
 public bool IsOccupied { get; private set; }
 public GameObject CurrentSoldier { get; private set; }

 public bool PlaceUnit(GameObject soldierPrefab)
 {
  if (IsOccupied) return false;

  CurrentSoldier = Instantiate(soldierPrefab, unitAnchor.position, unitAnchor.rotation);
  CurrentSoldier.transform.SetParent(transform);
  IsOccupied = true;
  return true;
 }
}