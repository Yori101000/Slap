using Slap.UI;
using UnityEngine;
using UnityEngine.UI;
using YukiFrameWork;

namespace Slap.Test
{
    public class GiftUI : MonoBehaviour, IController
    {
        //面板显示
        public Button gift => GetComponent<Button>();
        public GameObject giftItemPrefab;

        //基础信息
        public GiftsConfig config;

        //面板
        //弹窗面板
        private PopPanel popPanel;
        //道具面板
        private PropPanel propPanel;
        //动画面板
        private AnimationPanel animationPanel;

        //系统
        private GiftSystem giftSystem;
        private PlayerDataSystem playerDataSystem;


        void Start()
        {
            giftSystem = this.GetSystem<GiftSystem>();
            playerDataSystem = this.GetSystem<PlayerDataSystem>();

            gift.onClick.AddListener(() =>
            {
                giftItemPrefab.SetActive(true);
                Button[] buttons = giftItemPrefab.GetComponentsInChildren<Button>();
                buttons[0].AddListenerPure(OnClickBtn0);
                buttons[1].AddListenerPure(OnClickBtn1);
                buttons[2].AddListenerPure(OnClickBtn2);
                buttons[3].AddListenerPure(OnClickBtn3);
                buttons[4].AddListenerPure(OnClickBtn4);
                buttons[5].AddListenerPure(OnClickBtn5);
                buttons[6].AddListenerPure(OnClickBtn6);
            });

        }

        #region 按钮事件

        private void OnClickBtn0()
        {
            //检查玩家
            if (TestManager.Instance.curPlayer.userName == string.Empty)
            {
                Debug.LogWarning("当前玩家为空");
                return;
            }
            if (TestManager.Instance.curPlayer.userCamp == 0)
                playerDataSystem.AllotPlayerToCamp(TestManager.Instance.curPlayer, Random.Range(1, 3));

            Debug.Log($"{gift.name} + 按钮1被点击了");

            //弹窗设置
            if (config.popDatas[0].giftIcon != null)
            {
                giftItemPrefab.SetActive(false);
                giftSystem.PopWindow(TestManager.Instance.curPlayer, config.popDatas[0]);
            }


            //TODO 加分
            giftSystem.HandleGift(TestManager.Instance.curPlayer, config.effectDatas[0]);


        }

        private void OnClickBtn1()
        {
            Debug.Log($"{gift.name} + 按钮2被点击了");
            giftItemPrefab.SetActive(false);
            giftSystem.PopWindow(TestManager.Instance.curPlayer, config.popDatas[1]);

        }
        private void OnClickBtn2()
        {
            Debug.Log($"{gift.name} + 按钮3被点击了");
            giftItemPrefab.SetActive(false);
            giftSystem.PopWindow(TestManager.Instance.curPlayer, config.popDatas[2]);

        }
        private void OnClickBtn3()
        {
            Debug.Log($"{gift.name} + 按钮4被点击了");
            giftItemPrefab.SetActive(false);
            giftSystem.PopWindow(TestManager.Instance.curPlayer, config.popDatas[3]);

        }
        private void OnClickBtn4()
        {
            Debug.Log($"{gift.name} + 按钮5被点击了");
            giftItemPrefab.SetActive(false);
            giftSystem.PopWindow(TestManager.Instance.curPlayer, config.popDatas[4]);

        }
        private void OnClickBtn5()
        {
            Debug.Log($"{gift.name} + 按钮6被点击了");
            giftItemPrefab.SetActive(false);
            giftSystem.PopWindow(TestManager.Instance.curPlayer, config.popDatas[5]);

        }
        private void OnClickBtn6()
        {
            Debug.Log($"{gift.name} + 按钮7被点击了");
            giftItemPrefab.SetActive(false);
            giftSystem.PopWindow(TestManager.Instance.curPlayer, config.popDatas[6]);

        }

        #endregion




        public IArchitecture GetArchitecture()
        {
            return Push.Global;
        }

    }

}
