using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGameplay.Power;

namespace GridMovement
{
	public class CharacterContainer : MonoBehaviour
	{
		[SerializeField]
		public string containerName;

		[SerializeField]
		public bool IsFixedContainer = true;

		protected List<CharacterCommon> children = new List<CharacterCommon>(8);

		protected bool m_bInited = false;
		public void TouchInit()
		{
			if (m_bInited)
				return;
			m_bInited = true;

			for (int i = 0, l = transform.childCount; i < l; i++)
			{
				var child = transform.GetChild(i).GetComponent<CharacterCommon>();
				if (child)
				{
					children.Add(child);
					//if (child is PowerSupply)
					//{
					//	(child as PowerSupply).IsPoolItem = false;
					//}
					// ������ ...
				}
			}
		}

		protected void Awake()
		{
			TouchInit();
		}

		public IEnumerable<CharacterCommon> EachCharacterActive()
		{
			TouchInit();
			foreach (var each in children)
				if (each.gameObject.activeSelf)
					yield return each;
		}
		public IEnumerable<CharacterCommon> EachCharacterRegistered()
		{
			TouchInit();
			foreach (var each in children)
				if (each.isRegistered)
					yield return each;
		}
		public IEnumerable<CharacterCommon> EachCharacterAll()
		{
			TouchInit();
			foreach (var each in children)
				yield return each;
		}

		public void OnRegisterCharacter(CharacterCommon character)
		{
			character.gameObject.SetActive(true);
			if (!IsFixedContainer)
			{
				character.transform.SetParent(transform);
			}
		}

		protected bool PowerSupplyIsPoolItem(CharacterCommon character)
		{
			return (character is PowerSupply) && ((character as PowerSupply).isPoolItem);
		}

		public void OnDeregisterCharacter(CharacterCommon character)
		{
			character.gameObject.SetActive(false);
			if (!IsFixedContainer)
			{
				if (PowerSupplyIsPoolItem(character))
				{
					PowerSupplyPool.Inst.ReleasePowerSupply(character as PowerSupply);
				}
				else
				{
					// ��ʱֱ���Ƴ�
					//character.transform.SetParent(null);
					// �����Ƴ� ...
				}
			}
		}

		// ���̶�Container�ĵ�Դ������� �����ٱ����� ...
		// ��֮����õĵ�Դ ����ͨ������ش��� ������������ʱContainer ...
	}
}
