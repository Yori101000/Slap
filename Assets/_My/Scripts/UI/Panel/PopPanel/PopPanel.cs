///=====================================================
/// - FileName:      PopPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/27 13:27:41
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
using TMPro;
namespace Slap.UI
{
	public partial class PopPanel : BasePanel
	{

		//弹出弹幕
		public void PopWindow(PlayerData playerData, GiftPopData giftPopData)
		{
			PopedWindow.SetActive(true);
			Image playerIcon = PopedWindow.gameObject.Find("玩家头像").GetComponent<Image>();
			Image giftIcon = PopedWindow.gameObject.Find("礼物").GetComponent<Image>();
			TextMeshProUGUI counter = PopedWindow.gameObject.Find("数量").GetComponent<TextMeshProUGUI>();

			playerIcon.sprite = playerData.icon;
			giftIcon.sprite = giftPopData.giftIcon;
			counter.text = $"×{giftPopData.number}";
			

		}


	}
	public enum PopType
	{
		Default
	}
}
