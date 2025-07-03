///=====================================================
/// - FileName:      GameUIPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/26 17:01:43
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
namespace Slap.UI
{
	public partial class GameUIPanel : BasePanel
	{

		#region  UI组件

		//顶尖观众UI
		private Image[] leftTopViewerIcon;
		private Image[] rightTopViewerIcon;
		private TextMeshProUGUI[] txt_LeftTopViewer;
		private TextMeshProUGUI[] txt_RightTopViewer;

		#endregion

		#region 临时数据

		//最多每秒推进进度（因为使用slider左减右加，所以需要 / 2）
		private float maxPushPerSecond = 1f / 2;

		//气势
		private int MaxMomentumDiff = 1000;
		private int leftMomentum;
		private int rightMomentum;

		#endregion

		public override void OnEnter(params object[] param)
		{
			leftTopViewerIcon = Pnt_LeftTapRank.GetComponentsInChildren<Image>();
			txt_LeftTopViewer = Pnt_LeftTapRank.GetComponentsInChildren<TextMeshProUGUI>();

			rightTopViewerIcon = Pnt_RightTapRank.GetComponentsInChildren<Image>();
			txt_RightTopViewer = Pnt_RightTapRank.GetComponentsInChildren<TextMeshProUGUI>();
		}


		public void UpdateUI(List<PlayerData> list_PlayerData)
		{
			//TODO 上方更新


			//观众更新


			//进度条更新

		}

		public void UpdateLeftViewer(List<PlayerData> list_PlayerData)
		{
			//更新顶部观众
			int length = list_PlayerData.Count > leftTopViewerIcon.Length ? leftTopViewerIcon.Length : list_PlayerData.Count;
			for (int i = 0; i < length; i++)
			{
				leftTopViewerIcon[i].sprite = list_PlayerData[i].icon;
				txt_LeftTopViewer[i].text = list_PlayerData[i].userName;
			}

			//TODO 更新观众详情


			//更新气势
			UpdateLeftMomentum(list_PlayerData);

		}

		public void UpdateRightViewer(List<PlayerData> list_PlayerData)
		{
			//更新顶部观众
			int length = list_PlayerData.Count > rightTopViewerIcon.Length ? rightTopViewerIcon.Length : list_PlayerData.Count;
			for (int i = 0; i < length; i++)
			{
				rightTopViewerIcon[i].sprite = list_PlayerData[i].icon;
				txt_RightTopViewer[i].text = list_PlayerData[i].userName;
			}
			
			//更新气势
			UpdateRightMomentum(list_PlayerData);

		}

		public void UpdateLeftMomentum(List<PlayerData> list_PlayerData)
		{
			var i = 0;
			foreach (var playerData in list_PlayerData)
				i += playerData.userScore;
			Txt_LeftMomentum.text = i.ToString();
			leftMomentum = i;
		}
		public void UpdateRightMomentum(List<PlayerData> list_PlayerData)
		{
			var i = 0;
			foreach (var playerData in list_PlayerData)
				i += playerData.userScore;
			Txt_RightMomentum.text = i.ToString();
			rightMomentum = i;
		}

	
		

		public void UpdateCharge()
		{
			//更新蓄力条
			float momentumDiff = leftMomentum - rightMomentum;
			float diffRatio = Mathf.Clamp(momentumDiff / MaxMomentumDiff, -1f, 1f);
			float pushSpeed = diffRatio * maxPushPerSecond;

			Sdr_ChargeBar.value += pushSpeed * Time.deltaTime;

			//更新相关文本
			Txt_LeftChargeProgress.text = $"{Sdr_ChargeBar.value * 100:F1}%";
			Txt_RightChargeProgress.text = $"{(1 - Sdr_ChargeBar.value) * 100:F1}%";

		}
	}
}
