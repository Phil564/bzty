using System;
using System.Collections;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000477 RID: 1143
public class GuiderClick : UISingleton<GuiderClick>
{
	// Token: 0x060021D0 RID: 8656 RVA: 0x000BD518 File Offset: 0x000BB718
	public static void Show(OpenFuncType type)
	{
		GameObject gameObject = GameCache.UIGuiderAssetBundle.LoadAsset<GameObject>("GuiderClick");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.SetParent(GameCache.Canvas.transform);
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
		gameObject2.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;
		UISingleton<GuiderClick>.Instance = gameObject2.AddComponent<GuiderClick>();
		UISingleton<GuiderClick>.Instance._funcType = type;
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000BD5A4 File Offset: 0x000BB7A4
	private void Start()
	{
		this._finger = base.Window["Finger"];
		this._area = base.Window["Area"];
		this._guiderUI = base.Window["Guider"];
		this._desc = this._guiderUI["TalkBG"]["Desc"];
		AssetBundleLoader.ResetRender(this._finger.gameObject);
		this._sW = (float)(Screen.width / 2);
		this._sH = (float)(Screen.height / 2);
		Debuger.Log(Screen.width);
		Debuger.Log(Screen.height);
		switch (this._funcType)
		{
		case OpenFuncType.第一次PVE:
			this.SetHole(new Vector3(334f, -172f, 0f), new Vector2(164f, 80f), new Vector3(378f, -215f, 0f));
			this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			UISingleton<PVEFightResult>.Instance.Window["SureBtn"].transform.SetParent(base.Window["Buttom"].gameObject.transform);
			base.StartCoroutine(this.WaitExitPVE());
			break;
		case OpenFuncType.第二次PVE:
			this.SetHole(new Vector3(162f, -172f, 0f), new Vector2(164f, 80f), new Vector3(206f, -212f, 0f));
			this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			base.Window["RightBottom"]["Text"].text = "点击此处，返回到墨汁都市";
			base.Window["RightBottom"].transform.localPosition = new Vector3(43f, -120f, 0f);
			base.Window["RightBottom"].Show();
			UISingleton<PVEFightResult>.Instance.Window["BackZhucheng"].transform.SetParent(base.Window["Buttom"].gameObject.transform);
			base.StartCoroutine(this.WaitExitPVE());
			break;
		case OpenFuncType.命名:
			this._finger.Hide();
			base.Window["Buttom"].Hide();
			this._desc.text = "干的不错，请留下你的名字，我要告诉所有墨水都市的人，你成为一个合格的喷射战士。";
			AudioManager.PlayAudio(AudioManager.AudioName.猫一, null);
			this._guiderUI["Next"].BindClick(delegate
			{
				ChangeNameUI.Show();
				UISingleton<GuiderClick>.Close();
			}, true);
			this._guiderUI.Show();
			break;
		case OpenFuncType.邮箱:
			UISingleton<MailListUI>.Instance.Window["Scroll View"].GetComponent<ScrollRect>().enabled = false;
			UISingleton<MailListUI>.Instance.Window["Scroll View"]["Viewport"]["Content"].GetComponent<GridLayoutGroup>().enabled = false;
			UISingleton<MailListUI>.Instance.CreateItem.transform.SetParent(base.Window["Buttom"].gameObject.transform);
			this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			base.Window["RightBottom"]["Text"].text = "点击<color=#FFB90F>邮件</color>,查阅信息";
			base.Window["RightBottom"].transform.localPosition = UISingleton<MailListUI>.Instance.CreateItem.transform.localPosition + new Vector3(-100f, 70f, 0f);
			base.Window["RightBottom"].Show();
			this._finger.transform.localPosition = UISingleton<MailListUI>.Instance.CreateItem.transform.localPosition + new Vector3(0f, 50f, 0f);
			base.StartCoroutine(this.WaitMailInfo());
			break;
		case OpenFuncType.背包:
			base.StartCoroutine(this.WaitTopLeft());
			break;
		case OpenFuncType.竞技场:
			base.StartCoroutine(this.WaitJJC());
			break;
		case OpenFuncType.活动:
			this.SetHole(new Vector3(225f, -113f, 0f), new Vector2(135f, 57f), new Vector3(272f, -158f, 0f));
			this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			base.Window["RightBottom"]["Text"].text = "点击签到，领取<color=#FFB90F>奖励</color>";
			base.Window["RightBottom"].transform.localPosition = new Vector3(101f, -59f, -1f);
			base.Window["RightBottom"].Show();
			this.SignBtn = UISingleton<TaskListUI>.Instance.Window["Sign In"];
			this.SignBtn.transform.SetParent(base.Window["Buttom"].gameObject.transform);
			base.StartCoroutine(this.WaitSign());
			break;
		case OpenFuncType.商城:
			this._finger.Hide();
			base.Window["Buttom"].Hide();
			this._guiderUI["Mao"].Hide();
			this._guiderUI["Boss"].Show();
			base.StartCoroutine(this.SetWeaponScroll());
			this._guiderUI["Next"].BindClick(delegate
			{
				this._finger.Show();
				base.Window["Buttom"].Show();
				this.SetHole(new Vector3(177f, -163f, -1f), new Vector2(110f, 110f), new Vector3(222f, -213f, 0f));
				this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
				this.RightBottom = base.Window["RightBottom"];
				this.RightBottom["Text"].text = "点击选择武器";
				this.RightBottom.transform.localPosition = new Vector3(51f, -107.4f, -1f);
				this.RightBottom.Show();
				this.RightBottom.transform.SetParent(this._finger.transform);
				UISingleton<WeaponShopUI>.Instance.CreateItem.SetParent(base.Window["Buttom"].gameObject.transform);
				this._finger.transform.localPosition = UISingleton<WeaponShopUI>.Instance.CreateItem.localPosition + new Vector3(-46f, 50f, 0f);
				this.RightBottom.transform.SetParent(base.gameObject.transform);
				this._guiderUI.Hide();
				this._guiderUI["Mao"].Show();
				this._guiderUI["Boss"].Hide();
				this._guiderUI["TalkBG"]["Name"].text = "Judd 猫";
				base.StartCoroutine(this.WaitGuiderClick());
			}, true);
			break;
		}
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000BDC10 File Offset: 0x000BBE10
	private IEnumerator WaitJJC()
	{
		this._finger.Hide();
		yield return new WaitForSeconds(1f);
		this._finger.Show();
		this.SetHole(new Vector3(-45f, 85f, 0f), new Vector2(370f, 272f), new Vector3(0f, 44f, 0f));
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
		base.Window["RightBottom"]["Text"].text = "选择<color=#FFB90F>竞技模式</color>";
		base.Window["RightBottom"].transform.localPosition = new Vector3(-178f, 146f, -1f);
		base.Window["RightBottom"].Show();
		this.GameUI = UISingleton<SelectFightModelUI>.Instance.Window["AthleticsGame"];
		this.GameUI.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		base.StartCoroutine(this.WaitRoomList());
		yield break;
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000BDC2C File Offset: 0x000BBE2C
	private IEnumerator WaitTopLeft()
	{
		this._finger.Hide();
		while (UISingleton<BagUI>.Instance.ClothBtn == null)
		{
			yield return 1;
		}
		yield return new WaitForSeconds(0.5f);
		this._finger.Show();
		UIWrapper clothUI = UISingleton<BagUI>.Instance.Window["TopLeft"]["cloth"];
		clothUI.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		clothUI.BindClick(delegate
		{
			UISingleton<BagUI>.Instance.ClothClick();
		}, true);
		this._finger.transform.localPosition = clothUI.transform.localPosition + new Vector3(20f, -50f, 0f);
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 215f);
		base.Window["LeftTop"]["Text"].text = "点击这里，选择衣服分类";
		base.Window["LeftTop"].transform.localPosition = clothUI.transform.localPosition + new Vector3(150f, -110f, 0f);
		base.Window["LeftTop"].Show();
		base.StartCoroutine(this.WaitClothBag());
		yield break;
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000BDC48 File Offset: 0x000BBE48
	private IEnumerator WaitMailInfo()
	{
		while (UISingleton<MailInfoUI>.Instance == null)
		{
			yield return 1;
		}
		UIWrapper mailInfo = UISingleton<MailInfoUI>.Instance.Window[" Receive"];
		mailInfo.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		mailInfo.BindClick(delegate
		{
			UISingleton<MailInfoUI>.Instance.GuiderReceive(delegate
			{
				mailInfo.Hide();
				PrefsService.SetCharacterFunc(OpenFuncType.邮箱, true, delegate(bool isSuc, ISFSObject iSFSObject)
				{
					this._finger.Hide();
					base.Window["RightBottom"].Hide();
					this._guiderUI["Next"].BindClick(delegate
					{
						if (this._desc.text.Contains("一下多了这么多新宝贝"))
						{
							this.GuiderReturn();
						}
						base.Window["RightBottom"].Hide();
						this._desc.text = "一下多了这么多新宝贝，是不是很开心。现在就跟随我来一起试试你的新装备吧。";
						AudioManager.PlayAudio(AudioManager.AudioName.猫二, null);
					}, true);
					this._desc.text = "都市内重要资讯，将会以信件的形式第一时间推送给大伙的，请养成查阅的好习惯哦。";
					AudioManager.PlayAudio(AudioManager.AudioName.猫一, null);
					this._guiderUI.Show();
				});
			});
		}, true);
		this.SetHole(new Vector3(322f, -177f, 0f), new Vector2(133f, 45f), new Vector3(363f, -218f, 0f));
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
		base.Window["RightBottom"]["Text"].text = "点击<color=#FFB90F>领取</color>邮件奖励";
		base.Window["RightBottom"].transform.localPosition = new Vector3(190f, -122f, -1f);
		base.Window["RightBottom"].Show();
		yield break;
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000BDC64 File Offset: 0x000BBE64
	private void GuiderReturn()
	{
		this._finger.Show();
		this.MailInfoUIback = UISingleton<MailInfoUI>.Instance.Window["Back"];
		this.MailInfoUIback.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		this._finger.transform.localPosition = this.MailInfoUIback.transform.localPosition + new Vector3(80f, 80f, 0f);
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 315f);
		base.Window["LeftBottom"]["Text"].text = "点击<color=#FFB90F>返回主城</color>按钮，返回到墨汁都市";
		base.Window["LeftBottom"].transform.localPosition = this._finger.transform.localPosition + new Vector3(100f, 50f, -1f);
		base.Window["LeftBottom"].Show();
		this._guiderUI.Hide();
		base.StartCoroutine(this.WaitMailClose());
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000BDDB4 File Offset: 0x000BBFB4
	private IEnumerator WaitMailClose()
	{
		while (UISingleton<MailInfoUI>.Instance)
		{
			yield return 1;
		}
		this.MailInfoUIback.Hide();
		UIWrapper MailListUIback = UISingleton<MailListUI>.Instance.Window["Back"];
		MailListUIback.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		yield return new WaitForSeconds(0.1f);
		while (UISingleton<MailListUI>.Instance)
		{
			yield return 1;
		}
		base.Window["LeftBottom"].Hide();
		// VERY ugly hack...
		//OpenFunc.Show(OpenFuncType.背包);
		UISingleton<GuiderClick>.Close();
		yield break;
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000BDDD0 File Offset: 0x000BBFD0
	private IEnumerator WaitClothBag()
	{
		while (UISingleton<BagUI>.Instance.type != 3)
		{
			yield return 1;
		}
		while (UISingleton<BagUI>.Instance.CreateItem == null)
		{
			yield return 1;
		}
		yield return 1;
		base.Window["LeftTop"].Hide();
		UISingleton<BagUI>.Instance.Window["Bottom"]["Scroll View"].GetComponent<ScrollRect>().enabled = false;
		UISingleton<BagUI>.Instance.Window["Bottom"]["Scroll View"]["Viewport"]["Content"].GetComponent<GridLayoutGroup>().enabled = false;
		this.SetHole(new Vector3(-184f, -161f, -1f), new Vector2(120f, 120f), new Vector3(-138f, -207f, 0f));
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
		this.RightBottom = base.Window["RightBottom"];
		this.RightBottom["Text"].text = "选择你需要的衣服";
		this.RightBottom.transform.localPosition = new Vector3(-321f, -93f, -1f);
		this.RightBottom.Show();
		this.RightBottom.transform.SetParent(this._finger.transform);
		UISingleton<BagUI>.Instance.CreateItem.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		this._finger.transform.localPosition = UISingleton<BagUI>.Instance.CreateItem.transform.localPosition + new Vector3(-50f, 50f, 0f);
		this.RightBottom.transform.SetParent(base.transform);
		UISingleton<BagUI>.Instance.IsGuiderClick = false;
		base.StartCoroutine(this.WaitClickCloth());
		yield break;
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000BDDEC File Offset: 0x000BBFEC
	private IEnumerator WaitClickCloth()
	{
		while (!UISingleton<BagUI>.Instance.IsGuiderClick)
		{
			yield return 1;
		}
		this.RightBottom.Hide();
		UISingleton<BagUI>.Instance.CreateItem.transform.SetParent(UISingleton<BagUI>.Instance.Window["Bottom"]["Scroll View"]["Viewport"]["Content"].transform);
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
		this.SetHole(new Vector3(244f, -140f, 0f), new Vector2(140f, 60f), new Vector3(207f, -86f, 0f));
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 215f);
		this.LeftBottom = base.Window["LeftBottom"];
		this.LeftBottom["Text"].text = "点击<color=#FFB90F>装备</color>按钮，更换装备";
		this.LeftBottom.transform.localPosition = new Vector3(340f, -23f, -1f);
		this.LeftBottom.Show();
		this.LeftBottom.transform.SetParent(this._finger.transform);
		UIWrapper equipUI = UISingleton<BagUI>.Instance.Window["TopCenter"]["QiTaNeirong"]["equip"];
		equipUI.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		this._finger.transform.localPosition = equipUI.transform.localPosition + new Vector3(35f, -50f, 0f);
		this.LeftBottom.transform.SetParent(base.gameObject.transform);
		base.StartCoroutine(this.WaitEquip());
		yield break;
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000BDE08 File Offset: 0x000BC008
	private IEnumerator WaitEquip()
	{
		while (!UISingleton<BagUI>.Instance.IsGuiderEquip)
		{
			yield return 1;
		}
		PrefsService.SetCharacterFunc(OpenFuncType.背包, true, delegate(bool isSucc, ISFSObject iSFSObject)
		{
			base.Window["LeftBottom"].Hide();
			this._finger.Hide();
			this._desc.text = "换上新衣服是不是感觉萌萌哒：）现在就到战场上来看看你的新装备吧。";
			AudioManager.PlayAudio(AudioManager.AudioName.猫一, null);
			this._guiderUI.Show();
			this._guiderUI["Next"].BindClick(delegate
			{
				this._finger.Show();
				this.SetHole(new Vector3(-395f, -181f, -1f), new Vector2(100f, 100f), new Vector3(-430f, -219f, 0f));
				this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 315f);
				base.Window["LeftBottom"]["Text"].text = "点击返回<color=#FFB90F>主城</color>按钮，返回到墨汁都市";
				base.Window["LeftBottom"].transform.localPosition = new Vector3(-270f, -130f, -1f);
				base.Window["LeftBottom"].Show();
				this._guiderUI.Hide();
				base.Window["LeftBottom"].transform.SetParent(this._finger.transform);
				UIWrapper uiwrapper = UISingleton<BagUI>.Instance.Window["Back"];
				uiwrapper.transform.SetParent(base.Window["Buttom"].gameObject.transform);
				this._finger.transform.localPosition = uiwrapper.transform.localPosition + new Vector3(78f, 80f, 0f);
				base.StartCoroutine(this.WaitBagClose());
			}, true);
		});
		yield break;
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000BDE24 File Offset: 0x000BC024
	private IEnumerator WaitBagClose()
	{
		while (UISingleton<BagUI>.Instance)
		{
			yield return 1;
		}
		MainScene.Show();
		OpenFunc.Show(OpenFuncType.竞技场);
		UISingleton<GuiderClick>.Close();
		yield break;
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000BDE38 File Offset: 0x000BC038
	private IEnumerator SetWeaponScroll()
	{
		while (UISingleton<WeaponShopUI>.Instance.CreateItem == null)
		{
			yield return 1;
		}
		yield return new WaitForSeconds(0.1f);
		UISingleton<WeaponShopUI>.Instance.Window["Bottom"]["Scroll View"].GetComponent<ScrollRect>().enabled = false;
		UISingleton<WeaponShopUI>.Instance.Window["Bottom"]["Scroll View"]["Viewport"]["Content"].GetComponent<GridLayoutGroup>().enabled = false;
		this._desc.text = "嘿，伙计！我叫Sheldon。大家都叫我“弹药骑士”，我想你一定听过我的大名，对吗？如果你现在需要什么武器请尽管来找我，不用客气。";
		AudioManager.PlayAudio(AudioManager.AudioName.武器商人, null);
		this._guiderUI["TalkBG"]["Name"].text = "Sheldon";
		this._guiderUI.Show();
		yield break;
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000BDE54 File Offset: 0x000BC054
	private IEnumerator WaitGuiderClick()
	{
		while (!UISingleton<WeaponShopUI>.Instance.IsGuiderClick)
		{
			yield return 1;
		}
		UISingleton<WeaponShopUI>.Instance.CreateItem.SetParent(UISingleton<WeaponShopUI>.Instance.Window["Bottom"]["Scroll View"]["Viewport"]["Content"].transform);
		this.RightBottom.Hide();
		this.SetHole(new Vector3(147f, -58f, 0f), new Vector2(144f, 74f), new Vector3(102f, -108f, 0f));
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 315f);
		this.LeftBottom = base.Window["LeftBottom"];
		this.LeftBottom["Text"].text = "点击<color=#FFB90F>购买</color>";
		this.LeftBottom.transform.localPosition = new Vector3(282f, 6f, -1f);
		this.LeftBottom.Show();
		UISingleton<WeaponShopUI>.Instance.BuyBtn.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		base.StartCoroutine(this.WaitBuyUI());
		yield break;
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000BDE70 File Offset: 0x000BC070
	private IEnumerator WaitBuyUI()
	{
		while (UISingleton<ShopThshiUI>.Instance == null)
		{
			yield return 1;
		}
		UISingleton<WeaponShopUI>.Instance.BuyBtn.transform.SetParent(UISingleton<WeaponShopUI>.Instance.Window["TopCenter"]["Neirong"].transform);
		this.TishiOk = UISingleton<ShopThshiUI>.Instance.Window["Root"]["OK"];
		this.TishiOk.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		this.SetHole(new Vector3(152f, -29f, 0f), new Vector2(144f, 100f), new Vector3(106f, -75f, 0f));
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 315f);
		base.Window["LeftBottom"]["Text"].text = "点击<color=#FFB90F>购买</color>";
		base.Window["LeftBottom"].transform.localPosition = new Vector3(282f, 32f, -1f);
		base.Window["LeftBottom"].Show();
		base.StartCoroutine(this.WaitBuy());
		yield break;
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000BDE8C File Offset: 0x000BC08C
	private IEnumerator WaitBuy()
	{
		while (UISingleton<ShopThshiUI>.Instance != null)
		{
			yield return 1;
		}
		this.TishiOk.Hide();
		PrefsService.SetCharacterFunc(OpenFuncType.商城, true, delegate(bool isSucc, ISFSObject iSFSObject)
		{
			base.Window["LeftBottom"].Hide();
			this._finger.Hide();
			this._desc.text = "So？Sheldon人很不错的样子呢。你现在看起来也很拉风就对了，赶快去使用下你的新武器，去试试它的威力。";
			AudioManager.PlayAudio(AudioManager.AudioName.猫一, null);
			this._guiderUI.Show();
			this._guiderUI["Next"].BindClick(delegate
			{
				this._finger.Show();
				UIWrapper uiwrapper = UISingleton<WeaponShopUI>.Instance.Window["Back"];
				uiwrapper.transform.SetParent(base.Window["Buttom"].gameObject.transform);
				this._finger.transform.localPosition = uiwrapper.transform.localPosition + new Vector3(78f, 80f, 0f);
				this._guiderUI.Hide();
				base.StartCoroutine(this.WaitWeaponShopClose());
			}, true);
		});
		yield break;
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000BDEA8 File Offset: 0x000BC0A8
	private IEnumerator WaitWeaponShopClose()
	{
		while (UISingleton<WeaponShopUI>.Instance != null)
		{
			yield return 1;
		}
		MainScene.Show();
		UISingleton<GuiderClick>.Close();
		yield break;
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000BDEBC File Offset: 0x000BC0BC
	private IEnumerator WaitRoomList()
	{
		while (UISingleton<FightGameList>.Instance == null)
		{
			yield return 1;
		}
		this.GameUI.transform.SetParent(UISingleton<SelectFightModelUI>.Instance.transform);
		this.GameUI.GetComponent<DragMove>().enabled = true;
		this.SetHole(new Vector3(324f, -185f, 0f), new Vector2(133f, 93f), new Vector3(353f, -228f, 0f));
		this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
		base.Window["RightBottom"]["Text"].text = "点击<color=#FFB90F>快速战斗</color>，开始战斗吧！";
		base.Window["RightBottom"].transform.localPosition = new Vector3(202f, -129f, -1f);
		base.Window["RightBottom"].Show();
		this.FastJoin = UISingleton<FightGameList>.Instance.Window["FastJoin"];
		this.FastJoin.transform.SetParent(base.Window["Buttom"].gameObject.transform);
		base.StartCoroutine(this.WaitRoomInfoUI());
		yield break;
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000BDED8 File Offset: 0x000BC0D8
	private IEnumerator WaitRoomInfoUI()
	{
		while (UISingleton<RoomInfoUI>.Instance == null)
		{
			yield return 1;
		}
		this.FastJoin.transform.SetParent(UISingleton<FightGameList>.Instance.transform);
		PrefsService.SetCharacterFunc(OpenFuncType.竞技场, true, delegate(bool isSucc, ISFSObject iSFSObject)
		{
			base.Window["RightBottom"].Hide();
			UISingleton<GuiderClick>.Close();
		});
		yield break;
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x000BDEF4 File Offset: 0x000BC0F4
	private IEnumerator WaitExitPVE()
	{
		while (UISingleton<PVEFightResult>.Instance != null)
		{
			yield return 1;
		}
		UISingleton<GuiderClick>.Close();
		yield break;
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000BDF08 File Offset: 0x000BC108
	private IEnumerator WaitSign()
	{
		while (!UISingleton<TaskListUI>.Instance.IsGuiderSign)
		{
			yield return 1;
		}
		this.SignBtn.transform.SetParent(UISingleton<TaskListUI>.Instance.transform);
		PrefsService.SetCharacterFunc(OpenFuncType.活动, true, delegate(bool isSuc, ISFSObject iSFSObject)
		{
			base.Window["RightBottom"].Hide();
			this._finger.Hide();
			this._desc.text = "ok！你已经完成了今日的签到，领取到了丰厚的奖品。记得每天都要来签到喔。";
			AudioManager.PlayAudio(AudioManager.AudioName.猫一, null);
			this._guiderUI.Show();
			this._guiderUI["Next"].BindClick(delegate
			{
				UISingleton<GuiderClick>.Close();
			}, true);
		});
		yield break;
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000BDF24 File Offset: 0x000BC124
	private void SetHole(Vector3 fingerPos, Vector2 areaSize, Vector3 areaPos)
	{
		this._finger.transform.localPosition = fingerPos;
		this._area.GetComponent<RectTransform>().sizeDelta = areaSize;
		this._area.transform.localPosition = areaPos;
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x000BDF64 File Offset: 0x000BC164
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			UISingleton<BagUI>.Instance.GetComponent<UIWrapper>()["Bottom"]["Scroll View"].GetComponent<ScrollRect>().enabled = false;
			this._finger.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.SetHole(new Vector3(-142f, -146f, 0f), new Vector2(120f, 120f), new Vector3(-138f, -207f, 0f));
		}
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x000BE008 File Offset: 0x000BC208
	private void InitHole(UIWrapper uiwrapper)
	{
		float num = uiwrapper.transform.position.x - uiwrapper.GetComponent<RectTransform>().sizeDelta.x / 2f;
		float num2 = uiwrapper.transform.position.y + uiwrapper.GetComponent<RectTransform>().sizeDelta.y / 2f;
		float num3 = uiwrapper.transform.position.x + uiwrapper.GetComponent<RectTransform>().sizeDelta.x / 2f;
		float num4 = uiwrapper.transform.position.y - uiwrapper.GetComponent<RectTransform>().sizeDelta.y / 2f;
		float num5 = -this._sW;
		float sW = this._sW;
		float sH = this._sH;
		float num6 = -this._sH;
		float num7 = this._sW + num;
		base.Window["Collider"]["Left"].GetComponent<RectTransform>().sizeDelta = new Vector2(num7, this._sH * 2f);
		Debuger.Log("Left:" + base.Window["Collider"]["Left"].GetComponent<RectTransform>().sizeDelta);
		base.Window["Collider"]["Left"].GetComponent<RectTransform>().localPosition = new Vector3((num + num5) / 2f, 0f, 0f);
		float num8 = this._sW - num3;
		base.Window["Collider"]["Right"].GetComponent<RectTransform>().sizeDelta = new Vector2(num8, this._sH * 2f);
		Debuger.Log("Right:" + base.Window["Collider"]["Right"].GetComponent<RectTransform>().sizeDelta);
		base.Window["Collider"]["Right"].GetComponent<RectTransform>().localPosition = new Vector3((num3 + sW) / 2f, 0f, 0f);
		float num9 = this._sH - num2;
		base.Window["Collider"]["Top"].GetComponent<RectTransform>().sizeDelta = new Vector2(this._sW * 2f, num9);
		Debuger.Log("Top:" + base.Window["Collider"]["Top"].GetComponent<RectTransform>().sizeDelta);
		base.Window["Collider"]["Top"].GetComponent<RectTransform>().localPosition = new Vector3(0f, (num2 + sH) / 2f, 0f);
		float num10 = this._sH + num4;
		base.Window["Collider"]["Buttom"].GetComponent<RectTransform>().sizeDelta = new Vector2(this._sW * 2f, num10);
		Debuger.Log("Buttom:" + base.Window["Collider"]["Buttom"].GetComponent<RectTransform>().sizeDelta);
		base.Window["Collider"]["Buttom"].GetComponent<RectTransform>().localPosition = new Vector3(0f, (num4 + num6) / 2f, 0f);
	}

	// Token: 0x04001E04 RID: 7684
	private OpenFuncType _funcType;

	// Token: 0x04001E05 RID: 7685
	private float _sW;

	// Token: 0x04001E06 RID: 7686
	private float _sH;

	// Token: 0x04001E07 RID: 7687
	private UIWrapper _finger;

	// Token: 0x04001E08 RID: 7688
	private UIWrapper _area;

	// Token: 0x04001E09 RID: 7689
	private UIWrapper _desc;

	// Token: 0x04001E0A RID: 7690
	private UIWrapper _guiderUI;

	// Token: 0x04001E0B RID: 7691
	private UIWrapper SignBtn;

	// Token: 0x04001E0C RID: 7692
	private UIWrapper GameUI;

	// Token: 0x04001E0D RID: 7693
	private UIWrapper MailInfoUIback;

	// Token: 0x04001E0E RID: 7694
	private UIWrapper RightBottom;

	// Token: 0x04001E0F RID: 7695
	private UIWrapper LeftBottom;

	// Token: 0x04001E10 RID: 7696
	private UIWrapper TishiOk;

	// Token: 0x04001E11 RID: 7697
	private UIWrapper FastJoin;
}
