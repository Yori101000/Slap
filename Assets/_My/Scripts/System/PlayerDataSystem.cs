///=====================================================
/// - FileName:      PlayerDataProcessingSystem.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/6/30 13:38:31
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Slap.UI;
using YukiFrameWork.UI;
using System.Collections;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class PlayerDataSystem : AbstractSystem
    {
        private PlayerModel playerModel;
        private Action OnLeftScoreChanged;
        private Action OnRightScoreChanged;
        private List<PlayerData> list_leftPlayerData = new List<PlayerData>();
        private List<PlayerData> list_rightPlayerData = new List<PlayerData>();
        private GameUIPanel gameUIPanel;
     



        public override void Init()
        {
            playerModel = this.GetModel<PlayerModel>();
        }

        //开始系统的逻辑更新
        public void Start()
        {
            //注册更新方法
            MonoHelper.Update_AddListener(Update);
            //得到对应的UI面板
            gameUIPanel = UIKit.GetPanel<GameUIPanel>();

            OnLeftScoreChanged += () => UpdateData(1);
            OnLeftScoreChanged += () => gameUIPanel.UpdateLeftViewer(list_leftPlayerData);

            OnRightScoreChanged += () => UpdateData(2);
            OnRightScoreChanged += () => gameUIPanel.UpdateRightViewer(list_rightPlayerData);


#if UNITY_EDITOR

            //测试用，加载一些头像
            icons = Resources.LoadAll<Sprite>("Arts/UI/头像");
#endif

        }
        public void End()
        {
            //注销更新方法
            MonoHelper.Update_RemoveListener(Update);

            OnLeftScoreChanged -= () => UpdateData(1);
            OnLeftScoreChanged -= () => gameUIPanel.UpdateLeftViewer(list_leftPlayerData);

            OnRightScoreChanged -= () => UpdateData(2);
            OnRightScoreChanged -= () => gameUIPanel.UpdateRightViewer(list_rightPlayerData);

        }

        private void Update(MonoHelper helper)
        {
            gameUIPanel.UpdateCharge();
        }

        //更新数据
        private void UpdateData(int camp)
        {
            if (camp == 1)
            {
                list_leftPlayerData = playerModel.Dic_LeftPlayerData.OrderByDescending(pair => pair.Value.userScore)
                    .Select(pair => pair.Value).ToList();
                Debug.Log($"更新左侧玩家数据，当前数量: {list_leftPlayerData.Count}");
            }
            else if (camp == 2)
            {
                list_rightPlayerData = playerModel.Dic_RightPlayerData.OrderByDescending(pair => pair.Value.userScore)
                    .Select(pair => pair.Value).ToList();
                Debug.Log($"更新右侧玩家数据，当前数量: {list_rightPlayerData.Count}");
            }
        }


        //增加指定玩家的分数
        public void AddPlayerScore(string userName, int score)
        {
            if (playerModel.Dic_AllPlayerData.TryGetValue(userName, out PlayerData playerData))
            {
                playerData.userScore += score;
                UpdatePlayerScore(playerData);

                Debug.Log($"玩家 {userName} 的分数增加了 {score}，当前分数为 {playerData.userScore}");
            }
            else
            {
                Debug.LogWarning($"玩家 {userName} 不存在，无法增加分数");
            }


        }
        public IEnumerator AddPlayerScoreCor(PlayerData playerData, EffectData effectData)
        {
            int timer = 0;
            int additions = 0;
            while (timer < effectData.duration)
            {
                playerData.userScore += effectData.baseScore;
                additions += effectData.baseScore;

                UpdatePlayerScore(playerData);

                yield return new WaitForSeconds(1f);
                timer++;
            }

            playerData.userScore -= additions;
            UpdatePlayerScore(playerData);
        }

        private void UpdatePlayerScore(PlayerData playerData)
        {
            if (playerData.userCamp == 1)
            {
                OnLeftScoreChanged?.Invoke();
            }
            else if (playerData.userCamp == 2)
            {
                
                OnRightScoreChanged?.Invoke();
            }
        }

        //创建玩家数据
        public bool CreatePlayerData(PlayerData playerData)
        {
            if (playerModel.Dic_AllPlayerData.ContainsKey(playerData.userName))
            {
                Debug.LogWarning($"玩家 {playerData.userName} 已经存在，无法创建重复的玩家数据。");
                return false;
            }
#if UNITY_EDITOR
            SetRandomIcon(playerData);
#endif
            playerModel.Dic_AllPlayerData.Add(playerData.userName, playerData);
            return true;
        }

        //分配阵营
        public bool AllotPlayerToCamp(PlayerData playerData, int toCamp)
        {
            if (playerModel.Dic_AllPlayerData.TryGetValue(playerData.userName, out PlayerData existingPlayerData))
            {
                //如果玩家数据的阵营为0，则设置为当前分配的阵营
                if (playerData.userCamp == 0)
                {
                    playerData.userCamp = toCamp;

                    //根据阵营更新到对应的字典中
                    if (playerData.userCamp == 1)
                    {
                        playerModel.Dic_LeftPlayerData.Add(playerData.userName, playerData);
                        OnLeftScoreChanged?.Invoke();
                    }
                    else if (playerData.userCamp == 2)
                    {
                        playerModel.Dic_RightPlayerData.Add(playerData.userName, playerData);
                        OnRightScoreChanged?.Invoke();
                    }
                    Debug.Log($"玩家 {playerData.userName} 第一次被分配");
                    return true;
                }
                else
                {
                    Debug.Log($"玩家 {playerData.userName} 已在 {playerData.userCamp} 阵营");

                    return false;
                }
                
                // if (playerData.userCamp == toCamp)
                // {
                //     Debug.Log($"玩家 {playerData.userName} 的阵营没有变化，无需更新");
                //     return true;
                // }

                // //根据前面判断，以下的数据不可能不在左右阵营两者中
                // //根据阵营更新到对应的字典中
                // if (toCamp == 1)
                // {
                //     playerData.userCamp = 1; // 确保阵营被设置为1
                //     playerModel.Dic_LeftPlayerData.Add(playerData.userName, playerData);

                //     playerModel.Dic_RightPlayerData.Remove(playerData.userName);
                //     OnLeftScoreChanged?.Invoke();
                // }
                // else if (toCamp == 2)
                // {
                //     playerData.userCamp = 2; // 确保阵营被设置为2
                //     playerModel.Dic_RightPlayerData.Add(playerData.userName, playerData);
                //     playerModel.Dic_LeftPlayerData.Remove(playerData.userName);
                //     OnRightScoreChanged?.Invoke();
                // }

                // //如果玩家数据存在，则更新阵营
                // existingPlayerData.userCamp = playerData.userCamp;

               
            }
            return false;
        }

        public PlayerData GetPlayerData(string userName)
        {
            if (playerModel.Dic_AllPlayerData.TryGetValue(userName, out PlayerData playerData))
            {
                return playerData;
            }
            Debug.LogWarning($"玩家 {userName} 不存在");
            return null;
        }

        #region Test

        private Sprite[] icons;

        public void SetRandomIcon(PlayerData playerData)
        {
            if (icons != null && icons.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, icons.Length);
                playerData.icon = icons[index];
            }
            else
            {
                Debug.LogWarning("未找到任何图标资源！");
            }
        }
    
        #endregion

    }
    
    
}
