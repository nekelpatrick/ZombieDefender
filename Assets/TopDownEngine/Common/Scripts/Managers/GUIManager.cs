using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.EventSystems;

namespace MoreMountains.TopDownEngine
{
	[AddComponentMenu("TopDown Engine/Managers/GUI Manager")]
	public class GUIManager : MMSingleton<GUIManager>
	{
		// Add this new serialized field for your toolbar
		[Tooltip("Your custom toolbar")]
		public GameObject YourToolbar;

		// Keep existing fields
		public Canvas MainCanvas;
		public GameObject HUD;
		public MMProgressBar[] HealthBars;
		public MMProgressBar[] DashBars;
		public AmmoDisplay[] AmmoDisplays;
		public GameObject PauseScreen;
		public GameObject DeathScreen;
		public CanvasGroup Buttons;
		public CanvasGroup Arrows;
		public CanvasGroup Joystick;
		public Text PointsText;
		public string PointsTextPattern = "000000";

		protected float _initialJoystickAlpha;
		protected float _initialButtonsAlpha;
		protected bool _initialized = false;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void InitializeStatics()
		{
			_instance = null;
		}

		protected override void Awake()
		{
			base.Awake();
			Initialization();
		}

		protected virtual void Initialization()
		{
			if (_initialized) return;

			if (Joystick != null) _initialJoystickAlpha = Joystick.alpha;
			if (Buttons != null) _initialButtonsAlpha = Buttons.alpha;

			_initialized = true;
		}

		protected virtual void Start()
		{
			SetHUDActive(true);
			RefreshPoints();
			SetPauseScreen(false);
			SetDeathScreen(false);
		}

		// Fixed SetHUDActive method
		public virtual void SetHUDActive(bool state)
		{
			if (HUD != null)
			{
				HUD.SetActive(state);
			}
			if (YourToolbar != null)
			{
				YourToolbar.SetActive(state);
			}
			if (PointsText != null)
			{
				PointsText.enabled = state;
			}
		}


		/// <summary>
		/// Sets the avatar active or inactive
		/// </summary>
		/// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
		public virtual void SetAvatarActive(bool state)
		{
			if (HUD != null)
			{
				HUD.SetActive(state);
			}
		}

		/// <summary>
		/// Called by the input manager, this method turns controls visible or not depending on what's been chosen
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		/// <param name="movementControl">Movement control.</param>
		public virtual void SetMobileControlsActive(bool state, InputManager.MovementControls movementControl = InputManager.MovementControls.Joystick)
		{
			Initialization();

			if (Joystick != null)
			{
				Joystick.gameObject.SetActive(state);
				if (state && movementControl == InputManager.MovementControls.Joystick)
				{
					Joystick.alpha = _initialJoystickAlpha;
				}
				else
				{
					Joystick.alpha = 0;
					Joystick.gameObject.SetActive(false);
				}
				if (YourToolbar != null)
				{
					YourToolbar.SetActive(state);
					YourToolbar.GetComponent<CanvasGroup>().alpha = state ? 1 : 0;
				}
			}

			if (Arrows != null)
			{
				Arrows.gameObject.SetActive(state);
				if (state && movementControl == InputManager.MovementControls.Arrows)
				{
					Arrows.alpha = _initialJoystickAlpha;
				}
				else
				{
					Arrows.alpha = 0;
					Arrows.gameObject.SetActive(false);
				}
			}

			if (Buttons != null)
			{
				Buttons.gameObject.SetActive(state);
				if (state)
				{
					Buttons.alpha = _initialButtonsAlpha;
				}
				else
				{
					Buttons.alpha = 0;
					Buttons.gameObject.SetActive(false);
				}
			}
		}

		/// <summary>
		/// Sets the pause screen on or off.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the pause.</param>
		public virtual void SetPauseScreen(bool state)
		{
			if (PauseScreen != null)
			{
				PauseScreen.SetActive(state);
				EventSystem.current.sendNavigationEvents = state;
			}
		}

		/// <summary>
		/// Sets the death screen on or off.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the pause.</param>
		public virtual void SetDeathScreen(bool state)
		{
			if (DeathScreen != null)
			{
				DeathScreen.SetActive(state);
				EventSystem.current.sendNavigationEvents = state;
			}
		}

		/// <summary>
		/// Sets the jetpackbar active or not.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the pause.</param>
		public virtual void SetDashBar(bool state, string playerID)
		{
			if (DashBars == null)
			{
				return;
			}

			foreach (MMProgressBar jetpackBar in DashBars)
			{
				if (jetpackBar != null)
				{
					if (jetpackBar.PlayerID == playerID)
					{
						jetpackBar.gameObject.SetActive(state);
					}
				}
			}
		}

		/// <summary>
		/// Sets the ammo displays active or not
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		/// <param name="playerID">Player I.</param>
		public virtual void SetAmmoDisplays(bool state, string playerID, int ammoDisplayID)
		{
			if (AmmoDisplays == null)
			{
				return;
			}

			foreach (AmmoDisplay ammoDisplay in AmmoDisplays)
			{
				if (ammoDisplay != null)
				{
					if ((ammoDisplay.PlayerID == playerID) && (ammoDisplayID == ammoDisplay.AmmoDisplayID))
					{
						ammoDisplay.gameObject.SetActive(state);
					}
				}
			}
		}

		/// <summary>
		/// Sets the text to the game manager's points.
		/// </summary>
		public virtual void RefreshPoints()
		{
			if (PointsText != null)
			{
				PointsText.text = GameManager.Instance.Points.ToString(PointsTextPattern);
			}
		}

		/// <summary>
		/// Updates the health bar.
		/// </summary>
		/// <param name="currentHealth">Current health.</param>
		/// <param name="minHealth">Minimum health.</param>
		/// <param name="maxHealth">Max health.</param>
		/// <param name="playerID">Player I.</param>
		public virtual void UpdateHealthBar(float currentHealth, float minHealth, float maxHealth, string playerID)
		{
			if (HealthBars == null) { return; }
			if (HealthBars.Length <= 0) { return; }

			foreach (MMProgressBar healthBar in HealthBars)
			{
				if (healthBar == null) { continue; }
				if (healthBar.PlayerID == playerID)
				{
					healthBar.UpdateBar(currentHealth, minHealth, maxHealth);
				}
			}

		}

		/// <summary>
		/// Updates the dash bars.
		/// </summary>
		/// <param name="currentFuel">Current fuel.</param>
		/// <param name="minFuel">Minimum fuel.</param>
		/// <param name="maxFuel">Max fuel.</param>
		/// <param name="playerID">Player I.</param>
		public virtual void UpdateDashBars(float currentFuel, float minFuel, float maxFuel, string playerID)
		{
			if (DashBars == null)
			{
				return;
			}

			foreach (MMProgressBar dashbar in DashBars)
			{
				if (dashbar == null) { return; }
				if (dashbar.PlayerID == playerID)
				{
					dashbar.UpdateBar(currentFuel, minFuel, maxFuel);
				}
			}
		}

		/// <summary>
		/// Updates the (optional) ammo displays.
		/// </summary>
		/// <param name="magazineBased">If set to <c>true</c> magazine based.</param>
		/// <param name="totalAmmo">Total ammo.</param>
		/// <param name="maxAmmo">Max ammo.</param>
		/// <param name="ammoInMagazine">Ammo in magazine.</param>
		/// <param name="magazineSize">Magazine size.</param>
		/// <param name="playerID">Player I.</param>
		/// <param name="displayTotal">If set to <c>true</c> display total.</param>
		public virtual void UpdateAmmoDisplays(bool magazineBased, int totalAmmo, int maxAmmo, int ammoInMagazine, int magazineSize, string playerID, int ammoDisplayID, bool displayTotal)
		{
			if (AmmoDisplays == null)
			{
				return;
			}

			foreach (AmmoDisplay ammoDisplay in AmmoDisplays)
			{
				if (ammoDisplay == null) { return; }
				if ((ammoDisplay.PlayerID == playerID) && (ammoDisplayID == ammoDisplay.AmmoDisplayID))
				{
					ammoDisplay.UpdateAmmoDisplays(magazineBased, totalAmmo, maxAmmo, ammoInMagazine, magazineSize, displayTotal);
				}
			}
		}
	}
}