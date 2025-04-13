using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGameplay.GameData;
using GridMovement;
using System.Runtime.InteropServices.ComTypes;

namespace MyGameplay.Logic
{
	//public class LevelLogicCommon : GameLogicCommon
	// ������ÿ���ؿ�һ��Logic�� �������йؿ�����һ��Logic�� ...
	public class LevelLogic : GameLogicCommon
	{
		//[SerializeField]
		//protected int levelId = 0;


		/*protected int nextLevelId = 0;

		public void SetNextLevelId(int levelId)
		{
			nextLevelId = levelId;
		}*/


		protected int curLevelId = 0; // Global variable
		protected Dictionary<int, string> currentLevelName; // Hash map

		// Set up hash map for currentLevelName
		public void setup()
		{
            for (int lvl = 1; lvl <= 6; lvl++)
            {
                currentLevelName[lvl] = "Level" + lvl;

            }
        }

		//public void LoadNextGameLevel(int nextLevelId)
		public void SetNextGameLevel(int nextLevelId)
		{
			setup(); // initiate hash map
			if (curLevelId == nextLevelId)
				return;

			UnloadCurrentLevel();
			curLevelId = nextLevelId;
			LoadCurrentLevel();
		}
		public void SetNextGameLevelForce(int nextLevelId)
		{
            setup(); // initiate hash map
            UnloadCurrentLevel();
			curLevelId = nextLevelId;
			LoadCurrentLevel();
		}

		public void RestartGameLevel()
		{
            setup(); // initiate hash map
            UnloadCurrentLevel();
			LoadCurrentLevel();
		}


#if false
		protected string MakeRangeName(int levelId)
		{
			return "Level" + levelId;
		}
		protected string MakeContainerName(int levelId)
		{
			return "Level" + levelId;
		}

		//protected string MakeLevelShowName(int levelId)
		//{
		//	return "Level " + levelId;
		//}

		//[SerializeField]
		[SerializeField, Multiline(2)]
		protected List<string> levelTitleNameList = new List<string>();

		protected string GetLevelTitleName(int levelId)
		{
			if (1 <= levelId && levelId <= levelTitleNameList.Count)
				return levelTitleNameList[levelId - 1];
			else
				return "";
		}

		protected string MakeLevelCoverText(int levelId)
		{
			//var levelTitleName = GetLevelTitleName(levelId);
			//if (levelTitleName != "")
			//	return MakeLevelShowName(levelId) + "\n" + levelTitleName;
			//else
			//	return MakeLevelShowName(levelId);
			return GetLevelTitleName(levelId);
		}
#else
		//protected string MakeContainerName(int levelId)
		//{
		//	return "Level" + levelId;
		//}
#endif



		protected void UnloadCurrentLevel()
		{
			if (curLevelId <= 0)
				return;

			var container = GridGameMap.Inst.currentCharacterContainer as LevelContainer;
			if (container)
			{
				//GridGameMap.Inst.DeactivatePlayer();
				container.InvokeLevelExitEvent();
			}

			GridGameMap.Inst.DeactivatePlayer();
			GridGameMap.Inst.UnloadPowerSupplyPoolItems();
		}

		protected void LoadCurrentLevel()
		{
			if (curLevelId <= 0)
				return;



			var containerName = currentLevelName[curLevelId]; // Using hash map

			//LevelRangeManager.Inst.SetRangeName(MakeRangeName(curLevelId), true);
			LevelRangeManager.Inst.SetRange(GridGameMap.Inst.GetLevelRange(containerName), true);

			GridGameMap.Inst.SetCurrentContainer(containerName);

			//GameMainController.Inst.uiCover.SetCoverText(MakeLevelShowName(curLevelId));
			//GameMainController.Inst.uiCover.SetCoverText(MakeLevelShowName(curLevelId), GetLevelTitleName(curLevelId));
			//GameMainController.Inst.uiCover.SetCoverText(MakeLevelCoverText(curLevelId));
			GameMainController.Inst.uiCover.SetCoverText(GridGameMap.Inst.GetLevelTitleText(containerName));

			GameMainController.Inst.uiCover.StartCover();

			var container = GridGameMap.Inst.currentCharacterContainer as LevelContainer;
			if (container)
			{
				GridGameMap.Inst.ActivatePlayer(container.levelStartPos);
				container.InvokeLevelEnterEvent();
			}

			GameMainController.Inst.uiWheelDisc.RefreshAllNumber();

			// ...
		}
	}
}
