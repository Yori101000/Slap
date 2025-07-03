///=====================================================
/// - FileName:      GlobalState.cs
/// - NameSpace:     Slap
/// - Description:   YUKI 有限状态机构建状态类
/// - Creation Time: 2025/6/24 12:02:12
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.Machine;
using YukiFrameWork.UI;
using Slap.UI;
using YukiFrameWork;
namespace Slap
{
	//管理游戏中全局
	public class GlobalState : StateBehaviour
	{
		PlayerDataSystem playerDataSystem;
		GiftSystem giftSystem;
		public override async void OnEnter()
		{
			await SceneTool.LoadSceneAsync(ConstModel.GameSceneName);


			//打开有关UI面板
			UIKit.OpenPanel<BackGroundPanel>();
			UIKit.OpenPanel<PropPanel>();

			UIKit.OpenPanel<CharacterPanel>();
			UIKit.OpenPanel<GameUIPanel>();
			var explainPanel = UIKit.OpenPanel<ExplainPanel>();

			UIKit.OpenPanel<AnimationPanel>();
			UIKit.OpenPanel<PopPanel>();

			UIKit.OpenPanel<TestPanel>();

			explainPanel.OnClickClose(() => UIKit.ClosePanel<ExplainPanel>());

			//系统初始化
			playerDataSystem = this.GetSystem<PlayerDataSystem>();
			giftSystem = this.GetSystem<GiftSystem>();

			playerDataSystem.Start();
			giftSystem.Start();

			//在所有东西加载完毕后，关闭加载面板
			UIKit.ClosePanel<LoadingPanel>();
		}
		public override void OnUpdate()
		{

		}
		public override void OnExit()
		{
			//结束系统逻辑更新
			playerDataSystem.End();
			giftSystem.End();
			
		}

    }
}
