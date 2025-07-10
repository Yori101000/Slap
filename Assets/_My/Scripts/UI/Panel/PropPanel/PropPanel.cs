﻿///=====================================================
/// - FileName:      PropPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/27 13:26:19
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
using XFABManager;
namespace Slap.UI
{
	public partial class PropPanel : BasePanel
	{
		public GameObject SpawnProp(PlayerData playerData, GiftPropData propData)
		{
			var prop = GameObjectLoader.Load(propData.PropPre, transform);

			Animator animator;

			if (playerData.userCamp == 1)
			{
				animator = prop.GetComponent<Animator>();
				animator.Play("Left");
			}
			else
			{
				animator = prop.GetComponent<Animator>();
				animator.Play("Right");
			}
			return prop;
		}

	}
}
