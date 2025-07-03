///=====================================================
/// - FileName:      GiftSystem.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/2 11:08:01
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Slap.UI;
using YukiFrameWork.UI;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class GiftSystem : AbstractSystem
    {
        //弹窗队列
        private bool isPlayingPopWindow;
        private Queue<PopData> que_popWindow;
        private PopData curPopWindow;
        private List<GiftPopData> list_PopWindow;
        private Dictionary<string, Coroutine> dic_CoProp;
        private Queue que_Animation;

        //面板
        private PopPanel popPanel;
        private PropPanel propPanel;
        private AnimationPanel animationPanel;

        //系统
        private PlayerDataSystem playerDataSystem;

        //礼物处理字典
        public Dictionary<string, Coroutine> dic_CorGiftHandler { get; set; } = new Dictionary<string, Coroutine>();

        public override void Init()
        {
            que_popWindow = new Queue<PopData>();
            dic_CoProp = new Dictionary<string, Coroutine>();
        }

        public void Start()
        {
            MonoHelper.Update_AddListener(Update);

            popPanel = UIKit.GetPanel<PopPanel>();
            propPanel = UIKit.GetPanel<PropPanel>();
            animationPanel = UIKit.GetPanel<AnimationPanel>();
            playerDataSystem = this.GetSystem<PlayerDataSystem>();

        }
        public void End()
        {
            MonoHelper.Update_RemoveListener(Update);
        }
        public void Update(MonoHelper monoHelper)
        {
            CheckPopWindow();
        }

        #region 弹窗设置
        public void PopWindow(PlayerData playerData, GiftPopData giftPopData)
        {
            var popData = new PopData();
            popData.playerData = playerData;
            popData.giftPopData = giftPopData;
            que_popWindow.Enqueue(popData);
        }
        private void CheckPopWindow()
        {

            if (que_popWindow != null && que_popWindow.Count > 0)
            {
                if (!isPlayingPopWindow && curPopWindow != que_popWindow.Peek())
                {
                    curPopWindow = que_popWindow.Peek();
                    popPanel.PopWindow(curPopWindow.playerData, curPopWindow.giftPopData);
                    PlayPopWindow();
                }


            }
        }
        public void PlayPopWindow() => isPlayingPopWindow = true;
        public void StopPopWindow()
        {
            isPlayingPopWindow = false;
            que_popWindow.Dequeue();
        }
        #endregion

        public void HandleGift(PlayerData playerData, EffectData giftEffectData)
        {
            string timeStamp = DateTime.Now.Ticks.ToString();
            dic_CorGiftHandler.Add(
            $"{playerData.userName}_{timeStamp}"
            , MonoHelper.Instance.StartCoroutine(playerDataSystem.AddPlayerScoreCor(playerData, giftEffectData))
            );
        }
    }
    public class PopData
    {
        public PlayerData playerData;
        public GiftPopData giftPopData;
    }
}
