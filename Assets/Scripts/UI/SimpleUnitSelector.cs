using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Add this namespace

public class SimpleUnitSelector : MonoBehaviour
{
 [System.Serializable]
 public class UnitButton
 {
  public Button Button;
  public GameObject UnitPrefab;
  public Sprite UnitIcon;
 }

 [SerializeField] private UnitButton[] unitButtons;
 [SerializeField] private LayerMask trenchSlotLayer;

 private GameObject selectedUnitPrefab;
 private Camera mainCam;

 private void Awake()
 {
  mainCam = Camera.main;
  InitializeButtons();
 }

 private void InitializeButtons()
 {
  foreach (UnitButton button in unitButtons)
  {
   button.Button.image.sprite = button.UnitIcon;
   button.Button.onClick.AddListener(() => SelectUnit(button.UnitPrefab));
  }
 }

 private void SelectUnit(GameObject prefab)
 {
  selectedUnitPrefab = prefab;
  StartCoroutine(PlacementRoutine());
 }

 private IEnumerator PlacementRoutine()
 {
  while (selectedUnitPrefab != null)
  {
   if (Input.GetMouseButtonDown(0))
   {
    TryPlaceUnit();
   }
   yield return null;
  }
 }

 private void TryPlaceUnit()
 {
  Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
  if (Physics.Raycast(ray, out RaycastHit hit, 100f, trenchSlotLayer))
  {
   if (hit.collider.TryGetComponent<TrenchSlot>(out var slot))
   {
    if (slot.PlaceUnit(selectedUnitPrefab))
    {
     selectedUnitPrefab = null;
    }
   }
  }
 }
}