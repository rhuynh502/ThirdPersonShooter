﻿using UnityEngine;

namespace ThirdPersonShooter.UI
{
	public class GameOver : MenuBase
	{
		public override string ID => "Game Over";
		
		public override void OnOpenMenu(UIManager _manager)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public override void OnCloseMenu(UIManager _manager)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void OnClickMainMenu()
		{
			GameManager.Instance.LevelManager.UnloadGame(() => UIManager.ShowMenu("Main", false));
		}
	}
}