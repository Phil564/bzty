using System;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200035A RID: 858
public class ChangeNameUI : UISingleton<ChangeNameUI>
{
	// Token: 0x060019A8 RID: 6568 RVA: 0x0007F698 File Offset: 0x0007D898
	private void Start()
	{
		InputField input = base.Window["InputName"].GetComponent<InputField>();
		input.text = CharacterService.RandomChineseName(GameCache.Character.Sex);
		base.Window["Change"].BindClick(delegate
		{
			input.text = CharacterService.RandomChineseName(GameCache.Character.Sex);
		}, true);
		base.Window["OK"].BindClick(delegate
		{
			int byteCount = Encoding.Default.GetByteCount(input.text.Trim());
			int num = 15;
			if (Application.platform == RuntimePlatform.Android)
			{
				num = 23;
			}
			if (byteCount > num)
			{
				Debuger.LogWarning(byteCount);
				UISingleton<LogConsoleUI>.Instance.Debug(byteCount.ToString());
				Msg.Show("名字太长！！");
				return;
			}
			ISFSObject isfsobject = new SFSObject();
			isfsobject.PutUtfString("nm", input.text.Trim());
			SFSUtil.Send("SetCharacter", "SetCharacter", isfsobject, delegate(bool succ, ISFSObject dt)
			{
				if (succ)
				{
					if (UISingleton<MainScene>.Instance != null)
					{
						UISingleton<MainScene>.Instance.RefreshCharacter();
					}
					UCSdkService.NotifyZone();
					MapAction.SendLogin();
					
					// VERY bad hack.... but i have to finish this before 4pm so ¯\_(ツ)_/¯
					UISingleton<ZhuchengUI>.Instance.FightLeftJoy.transform.parent.gameObject.SetActive(true);
					//if (!PrefsService.GetFuncOpen(OpenFuncType.邮箱))
					//{
						//OpenFunc.Show(OpenFuncType.邮箱);
					//}
					UISingleton<ChangeNameUI>.Close();
				}
			});
		}, true);
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x0007F724 File Offset: 0x0007D924
	public static void Show()
	{
		GameObject gameObject = GameCache.UIMainAssetBundle.LoadAsset<GameObject>("ChangeName");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.SetParent(GameObject.Find("Canvas").transform);
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.AddComponent<ChangeNameUI>();
	}
}
