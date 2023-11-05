using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameClient.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001BD RID: 445
public class LiveForEver : MonoBehaviour
{
	// Token: 0x06000A40 RID: 2624 RVA: 0x00039240 File Offset: 0x00037440
	private void Awake()
	{
		GameCache.localDebug = true;
		// if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		// {
			Debuger.EnableLog = true;
		// }
		// else
		// {
			Debug.logger.logEnabled = true;
			// Debuger.EnableLog = false;
		// }
		LiveForEver.Instance = this;
		this.progress = 0.12f;
		AudioManager.InitSet();
		Application.targetFrameRate = 30;
		Application.runInBackground = true;
		Screen.sleepTimeout = -1;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		GameCache.Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		GameCache.UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		UnityEngine.Object.DontDestroyOnLoad(GameCache.Canvas.gameObject);
		UnityEngine.Object.DontDestroyOnLoad(GameCache.UICamera.gameObject);
		UnityEngine.Object.DontDestroyOnLoad(GameObject.Find("EventSystem"));
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x00039308 File Offset: 0x00037508
	private void Start()
	{
		UCSdkService.Init();
		GameData.Init();
		this.startLoad();
		this.NameAndTitles = new GameObject("NameAndTitles");
		this.NameAndTitles.transform.SetParent(GameObject.Find("Canvas").transform);
		this.ScreenChats = new GameObject("ScreenChats");
		this.ScreenChats.transform.SetParent(GameObject.Find("Canvas").transform);
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x00039384 File Offset: 0x00037584
	private void Update()
	{
		if (this.MusicSource.clip == null && this.first)
		{
			AudioClip audioClip = Resources.Load("main") as AudioClip;
			Debuger.Log(audioClip);
			this.MusicSource.clip = audioClip;
			this.MusicSource.loop = true;
			this.MusicSource.enabled = false;
			this.MusicSource.enabled = true;
			this.first = false;
		}
		if (Application.platform != RuntimePlatform.IPhonePlayer && Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameCache.localDebug)
			{
				TishiSelectUI.Show("是否确定退出游戏？", delegate
				{
					Application.Quit();
				}, delegate
				{
				}, "确定", "取消");
			}
			else
			{
				UCSdkService.exitSDK();
			}
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00039478 File Offset: 0x00037678
	public void ConnectToSmartFoxServer(string user, Action callBack)
	{
		SmartFoxClient component = base.GetComponent<SmartFoxClient>();
		if (component == null)
		{
			base.gameObject.AddComponent<SmartFoxClient>();
		}
		if (GameCache.localDebug)
		{
			int configInt = GameData.GetConfigInt("DefaultServer");
			ServerData serverData = GameData.ServerDict[configInt];
			Debuger.Log(string.Format("[服务器信息] 服务器ID={0}, IP={1}, 端口={2}", configInt, serverData.IP, serverData.Port));
			this.ConnectToServer(user, serverData.IP, serverData.Port, callBack);
		}
		else
		{
			Area area = GameCache.Area[PrefsService.GetAreaId(GameCache.areaId)];
			Debuger.Log(string.Format("[服务器信息] 服务器ID={0}, IP={1}, 端口={2}", area.ID, area.IP, area.Port));
			this.ConnectToServer(user, area.IP, area.Port, callBack);
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00039558 File Offset: 0x00037758
	public void OnLoginSmartFoxFail()
	{
		Debug.LogError("SmartFox 登录失败");
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00039564 File Offset: 0x00037764
	public void OnConnectSmartFoxSuccess(string user, Action callBack)
	{
		int configInt = GameData.GetConfigInt("DefaultServer");
		ServerData serverData = GameData.ServerDict[configInt];
		Debuger.Log("开始登录SmartFox");
		// SmartFoxClient.Instance.Login(serverData.Zone, user, true, delegate(bool suc)
		// {
			// if (suc)
			// {
				Debuger.Log("登录SmartFox成功");
				if (callBack != null)
				{
					callBack();
				}
			// }
			// else
			// {
				// Debuger.Log("登录SmartFox失败");
				// this.OnLoginSmartFoxFail();
				// this.connecting = false;
			// }
		// });
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x000395C4 File Offset: 0x000377C4
	public void OnConnectSmartFoxFail()
	{
		Debug.LogError("连接失败");
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x000395D0 File Offset: 0x000377D0
	public void ConnectToServer(string user, string serverIP, int serverPort, Action callBack)
	{
		//if (this.connecting)
		//{
		//	return;
		//}
		//this.connecting = true;
		// SmartFoxClient.Instance.ProcessConnect(serverIP, serverPort, delegate(bool suc)
		// {
			// if (suc)
			// {
		this.OnConnectSmartFoxSuccess(user, callBack);
			// }
			// else
			// {
				// Msg.Show("连接服务器失败");
				// this.OnConnectSmartFoxFail();
				this.connecting = false;
			// }
		// });
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00039624 File Offset: 0x00037824
	private void OnDestroy()
	{
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00039628 File Offset: 0x00037828
	private void OnApplicationQuit()
	{
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0003962C File Offset: 0x0003782C
	private void startLoad()
	{
		this.OnFinishDownload();
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00039634 File Offset: 0x00037834
	public void OnDownloading(string assetPath)
	{
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00039638 File Offset: 0x00037838
	public void OnFinishDownload()
	{
		this.DoLoadResources();
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x00039640 File Offset: 0x00037840
	private void DoLoadResources()
	{
		Queue<KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>> queue = new Queue<KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>>();
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Data/Data.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadDataAllInOne))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/UI/Start.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadUIStart))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/UI/AlwaysIn.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadUIAlwaysIn))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Effects/Effect.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadEffects))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Audio/Sound.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadSound))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/UI/Icon.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadIcon))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Controllers/OverrideController.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadController))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/UI/PaintTextures.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadPaintTextures))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Model/Equips/Equips.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadEquips))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Model/Cloths/Clothes.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadClothes))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Model/Shoes/Shoes.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadShoes))));
		queue.Enqueue(new KeyValuePair<string, KeyValuePair<bool, AssetBundleLoader.CallbackDel>>("/Model/Tires/Tires.rs", new KeyValuePair<bool, AssetBundleLoader.CallbackDel>(false, new AssetBundleLoader.CallbackDel(this.OnLoadTires))));
		this.LoadMsg = "正在加载界面资源...1/6";
		AssetBundleLoader.LoadQueue(queue, new Action(this.OnLoadTableAndLuaFinished), delegate(WWW www)
		{
			this.progress = www.progress + 0.12f;
		});
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x00039814 File Offset: 0x00037A14
	private void OnLoadUIStart(WWW www, object userdata)
	{
		GameCache.UIStartAssetBundle = www.assetBundle;
		this.LoadMsg = "正在加载特效资源...2/6";
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x0003982C File Offset: 0x00037A2C
	private void OnLoadUIAlwaysIn(WWW www, object userdata)
	{
		GameCache.UIAlwaysInAssetBundle = www.assetBundle;
		this.LoadMsg = "正在加载特效资源...3/6";
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00039844 File Offset: 0x00037A44
	private void OnLoadMonster(WWW www, object userdata)
	{
		GameCache.MonsterAssetBundle = www.assetBundle;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00039854 File Offset: 0x00037A54
	private void OnLoadIcon(WWW www, object userdata)
	{
		GameCache.IconAssetBundle = www.assetBundle;
		this.LoadMsg = "正在进入游戏...6/6";
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0003986C File Offset: 0x00037A6C
	private void OnLoadEffects(WWW www, object userdata)
	{
		GameCache.EffectsAssetBundle = www.assetBundle;
		this.LoadMsg = "正在加载音效资源...4/6";
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00039884 File Offset: 0x00037A84
	private void OnLoadSound(WWW www, object userdata)
	{
		GameCache.SoundAssetBundle = www.assetBundle;
		this.LoadMsg = "正在初始化资源...5/6";
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x0003989C File Offset: 0x00037A9C
	private void OnLoadController(WWW www, object userdata)
	{
		GameCache.ControllerAssetBundle = www.assetBundle;
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x000398AC File Offset: 0x00037AAC
	private void OnLoadPaintTextures(WWW www, object userdata)
	{
		GameCache.PaintTexturesAssetBundle = www.assetBundle;
		GameCache.PaintTexturesAssetBundle.LoadAllAssets();
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x000398C4 File Offset: 0x00037AC4
	private void OnLoadEquips(WWW www, object userdata)
	{
		GameCache.EquipsAssetBundle = www.assetBundle;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x000398D4 File Offset: 0x00037AD4
	private void OnLoadClothes(WWW www, object userdata)
	{
		GameCache.ClothsAssetBundle = www.assetBundle;
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x000398E4 File Offset: 0x00037AE4
	private void OnLoadShoes(WWW www, object userdata)
	{
		GameCache.ShoesAssetBundle = www.assetBundle;
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x000398F4 File Offset: 0x00037AF4
	private void OnLoadTires(WWW www, object userdata)
	{
		GameCache.TiresAssetBundle = www.assetBundle;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00039904 File Offset: 0x00037B04
	private void OnLoadDataAllInOne(WWW www, object userdata)
	{
		this.LoadAsset(www, "resource", new Action<UnityEngine.Object, object>(this.OnLoadArtResourceData));
		this.LoadAsset(www, "fightbasedata", new Action<UnityEngine.Object, object>(this.OnLoadFightBaseData));
		this.LoadAsset(www, "area", new Action<UnityEngine.Object, object>(this.OnLoadAreaData));
		this.LoadAsset(www, "tips", new Action<UnityEngine.Object, object>(this.OnLoadTipsData));
		this.LoadAsset(www, "item", new Action<UnityEngine.Object, object>(this.OnLoadItem));
		this.LoadAsset(www, "skill", new Action<UnityEngine.Object, object>(this.OnLoadSkill));
		this.LoadAsset(www, "hero", new Action<UnityEngine.Object, object>(this.OnLoadHero));
		this.LoadAsset(www, "guider", new Action<UnityEngine.Object, object>(this.OnLoadGuiderData));
		this.LoadAsset(www, "newerguider", new Action<UnityEngine.Object, object>(this.OnLoadNewerGuiderData));
		this.LoadAsset(www, "map", new Action<UnityEngine.Object, object>(this.OnLoadMapData));
		this.LoadAsset(www, "shop", new Action<UnityEngine.Object, object>(this.OnLoadShop));
		this.LoadAsset(www, "livereward", new Action<UnityEngine.Object, object>(this.OnLoadLiveReward));
		this.LoadAsset(www, "RaidStarCondition", new Action<UnityEngine.Object, object>(this.OnLoadStar));
		this.LoadAsset(www, "Ruleread", new Action<UnityEngine.Object, object>(this.OnLoadRuleread));
		this.LoadAsset(www, "attribute", new Action<UnityEngine.Object, object>(this.OnLoadAttribute));
		this.LoadAsset(www, "gundata", new Action<UnityEngine.Object, object>(this.OnLoadGundata));
		this.LoadAsset(www, "bombdata", new Action<UnityEngine.Object, object>(this.OnLoadBombdata));
		this.LoadAsset(www, "attack", new Action<UnityEngine.Object, object>(this.OnLoadAttackdata));
		this.LoadAsset(www, "name", new Action<UnityEngine.Object, object>(this.OnLoadNamedata));
		this.LoadAsset(www, "monster", new Action<UnityEngine.Object, object>(this.OnLoadMonsterdata));
		this.LoadAsset(www, "npc", new Action<UnityEngine.Object, object>(this.OnLoadNPCData));
		this.LoadAsset(www, "zhuchengguider", new Action<UnityEngine.Object, object>(this.OnLoadZhuchengGuiderDatas));
		this.LoadAsset(www, "mappaint", new Action<UnityEngine.Object, object>(this.OnLoadMapPaintDatas));
		this.LoadAsset(www, "PVEColor", new Action<UnityEngine.Object, object>(this.OnLoadPVEColorData));
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00039B54 File Offset: 0x00037D54
	private void OnLoadNPCData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<NPCData> list = TableUtil.ReadTableOfRS<NPCData>(textAsset.text);
		GameCache.NPCDatas = new NPCDataList();
		foreach (NPCData npcdata in list)
		{
			GameCache.NPCDatas.Add(npcdata);
		}
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x00039BCC File Offset: 0x00037DCC
	private void OnLoadPVEColorData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<PVEColor> list = TableUtil.ReadTableOfRS<PVEColor>(textAsset.text);
		GameCache.PVEColorDatas = new PVEColorList();
		foreach (PVEColor pvecolor in list)
		{
			GameCache.PVEColorDatas.Add(pvecolor);
		}
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00039C44 File Offset: 0x00037E44
	private void OnLoadZhuchengGuiderDatas(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<ZhuchengGuiderData> list = TableUtil.ReadTableOfRS<ZhuchengGuiderData>(textAsset.text);
		GameCache.ZhuchengGuiderDatas = new ZhuchengGuiderDataList();
		foreach (ZhuchengGuiderData zhuchengGuiderData in list)
		{
			GameCache.ZhuchengGuiderDatas.Add(zhuchengGuiderData);
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00039CBC File Offset: 0x00037EBC
	private void OnLoadMapPaintDatas(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<MapPaintData> list = TableUtil.ReadTableOfRS<MapPaintData>(textAsset.text);
		GameCache.MapPaintDatas = new MapPaintDataList();
		foreach (MapPaintData mapPaintData in list)
		{
			GameCache.MapPaintDatas.Add(mapPaintData);
		}
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00039D34 File Offset: 0x00037F34
	private void OnLoadArtResourceData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<ArtResource> list = TableUtil.ReadTableOfRS<ArtResource>(textAsset.text);
		GameCache.ArtResource = new Dictionary<int, ArtResource>();
		foreach (ArtResource artResource in list)
		{
			GameCache.ArtResource.Add(artResource.ID, artResource);
		}
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00039DB0 File Offset: 0x00037FB0
	private void OnLoadFightBaseData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<FightBaseData> list = TableUtil.ReadTableOfRS<FightBaseData>(textAsset.text);
		GameCache.FightBaseData = list[0];
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x00039DDC File Offset: 0x00037FDC
	private void OnLoadAreaData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Area> list = TableUtil.ReadTableOfRS<Area>(textAsset.text);
		GameCache.Area = new Dictionary<int, Area>();
		foreach (Area area in list)
		{
			GameCache.Area.Add(area.ID, area);
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x00039E58 File Offset: 0x00038058
	private void OnLoadMapData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Map> list = TableUtil.ReadTableOfRS<Map>(textAsset.text);
		GameCache.Map = new Dictionary<int, Map>();
		foreach (Map map in list)
		{
			GameCache.Map.Add(map.ID, map);
		}
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00039ED4 File Offset: 0x000380D4
	private void OnLoadGuiderData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<GuiderData> list = TableUtil.ReadTableOfRS<GuiderData>(textAsset.text);
		GameCache.GuiderDatas = new GuiderList();
		foreach (GuiderData guiderData in list)
		{
			GameCache.GuiderDatas.Add(guiderData);
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00039F4C File Offset: 0x0003814C
	private void OnLoadNewerGuiderData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<NewerGuiderData> list = TableUtil.ReadTableOfRS<NewerGuiderData>(textAsset.text);
		GameCache.NewerGuiderDatas = new NewerGuiderList();
		foreach (NewerGuiderData newerGuiderData in list)
		{
			GameCache.NewerGuiderDatas.Add(newerGuiderData);
		}
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00039FC4 File Offset: 0x000381C4
	private void OnLoadTipsData(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Tips> list = TableUtil.ReadTableOfRS<Tips>(textAsset.text);
		GameCache.Tips = new Dictionary<int, Tips>();
		foreach (Tips tips in list)
		{
			GameCache.Tips.Add(tips.ID, tips);
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0003A040 File Offset: 0x00038240
	private void OnLoadItem(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Item> list = TableUtil.ReadTableOfRS<Item>(textAsset.text);
		GameCache.Items = new ItemList();
		foreach (Item item in list)
		{
			GameCache.Items.Add(item);
		}
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0003A0B8 File Offset: 0x000382B8
	private void OnLoadSkill(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<SkillData> list = TableUtil.ReadTableOfRS<SkillData>(textAsset.text);
		GameCache.BaseSkills = new SkillList();
		foreach (SkillData skillData in list)
		{
			GameCache.BaseSkills.Add(skillData);
		}
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0003A130 File Offset: 0x00038330
	private void OnLoadHero(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Hero> list = TableUtil.ReadTableOfRS<Hero>(textAsset.text);
		GameCache.HeroDatas = new HeroList();
		foreach (Hero hero in list)
		{
			GameCache.HeroDatas.Add(hero);
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0003A1A8 File Offset: 0x000383A8
	private void OnLoadGundata(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<GunData> list = TableUtil.ReadTableOfRS<GunData>(textAsset.text);
		GameCache.GunDatas = new GunDataList();
		foreach (GunData gunData in list)
		{
			GameCache.GunDatas.Add(gunData);
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0003A220 File Offset: 0x00038420
	private void OnLoadAttackdata(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Attack> list = TableUtil.ReadTableOfRS<Attack>(textAsset.text);
		GameCache.AttackDatas = new AttackDataList();
		foreach (Attack attack in list)
		{
			GameCache.AttackDatas.Add(attack);
		}
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0003A298 File Offset: 0x00038498
	private void OnLoadNamedata(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Name> list = TableUtil.ReadTableOfRS<Name>(textAsset.text);
		foreach (Name name in list)
		{
			GameCache.firstName.Add(name.Content);
		}
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0003A30C File Offset: 0x0003850C
	private void OnLoadMonsterdata(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<MonsterData> list = TableUtil.ReadTableOfRS<MonsterData>(textAsset.text);
		GameCache.MonsterDataLists = new MonsterDataList();
		foreach (MonsterData monsterData in list)
		{
			GameCache.MonsterDataLists.Add(monsterData);
		}
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0003A384 File Offset: 0x00038584
	private void OnLoadBombdata(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<BombData> list = TableUtil.ReadTableOfRS<BombData>(textAsset.text);
		GameCache.BombDatas = new BombDataList();
		foreach (BombData bombData in list)
		{
			GameCache.BombDatas.Add(bombData);
		}
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0003A3FC File Offset: 0x000385FC
	private void OnLoadShop(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Shop> list = TableUtil.ReadTableOfRS<Shop>(textAsset.text);
		GameCache.Shops = new ShopList();
		foreach (Shop shop in list)
		{
			GameCache.Shops.Add(shop);
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0003A474 File Offset: 0x00038674
	private void OnLoadLiveReward(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<LiveReward> list = TableUtil.ReadTableOfRS<LiveReward>(textAsset.text);
		GameCache.LiveRewards = new LiveRewardList();
		foreach (LiveReward liveReward in list)
		{
			GameCache.LiveRewards.Add(liveReward);
		}
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0003A4EC File Offset: 0x000386EC
	private void OnLoadStar(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<RaidStarCondition> list = TableUtil.ReadTableOfRS<RaidStarCondition>(textAsset.text);
		GameCache.RaidStar = new RaidStarList();
		foreach (RaidStarCondition raidStarCondition in list)
		{
			GameCache.RaidStar.Add(raidStarCondition);
		}
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0003A564 File Offset: 0x00038764
	private void OnLoadRuleread(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<Ruleread> list = TableUtil.ReadTableOfRS<Ruleread>(textAsset.text);
		GameCache.Ruleread = new RulereadList();
		foreach (Ruleread ruleread in list)
		{
			GameCache.Ruleread.Add(ruleread);
			Debuger.Log(ruleread.Desc);
		}
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0003A5E8 File Offset: 0x000387E8
	private void OnLoadAttribute(UnityEngine.Object asset, object userdata)
	{
		TextAsset textAsset = asset as TextAsset;
		List<GameClient.Entities.Attribute> list = TableUtil.ReadTableOfRS<GameClient.Entities.Attribute>(textAsset.text);
		GameCache.Attributes = new AttributeList();
		foreach (GameClient.Entities.Attribute attribute in list)
		{
			GameCache.Attributes.Add(attribute);
		}
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0003A660 File Offset: 0x00038860
	private void LoadAsset(WWW www, string name, Action<UnityEngine.Object, object> callback)
	{
		UnityEngine.Object @object = www.assetBundle.LoadAsset(name);
		callback(@object, null);
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0003A684 File Offset: 0x00038884
	private void OnLoadTableAndLuaFinished()
	{
		this.DoLoadLuaScripts();
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0003A68C File Offset: 0x0003888C
	private void DoLoadLuaScripts()
	{
		List<string> list = new List<string>();
		AssetBundleLoader.LoadPlatformBatchRS(list, new AssetBundleLoader.CallbackDel(this.OnLoadLua), null, new AssetBundleLoader.FinishDel(this.OnLoadLuaScriptsFinished), null);
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0003A6C0 File Offset: 0x000388C0
	private void OnLoadLua(WWW www, object userdata)
	{
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0003A6C4 File Offset: 0x000388C4
	private void OnLoadLuaScriptsFinished(List<string> assetPaths, object userdata)
	{
		this.OnLoadResourcesFinished();
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0003A6CC File Offset: 0x000388CC
	private void OnLoadResourcesFinished()
	{
		this.progress = 1f;
		base.StartCoroutine(this.LoadAllSound());
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0003A6E8 File Offset: 0x000388E8
	private IEnumerator LoadAllSound()
	{
		AudioManager.AudioName[] list = new AudioManager.AudioName[]
		{
			AudioManager.AudioName.背包,
			AudioManager.AudioName.商店
		};
		foreach (AudioManager.AudioName Audioname in list)
		{
			string name = GameCache.ArtResource[(int)Audioname].Url.Replace(".mp3", string.Empty);
			AssetBundleRequest astBund = GameCache.SoundAssetBundle.LoadAssetAsync<AudioClip>(name);
			while (!astBund.isDone)
			{
				this.progress = astBund.progress;
				yield return new WaitForFixedUpdate();
			}
			AudioClip clip = astBund.asset as AudioClip;
			AudioManager.Sounds.Add(clip.name, clip);
		}
		UISingleton<Loading>.Close();
		SceneManager.LoadScene("login");
		yield break;
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0003A704 File Offset: 0x00038904
	private IEnumerator OnLoadSQLiteData()
	{
		string filepath = string.Empty;
		string appDBPath = Application.persistentDataPath + "/SQLDatas/VertSaveDatas.sqlite";
		if (!Directory.Exists(Application.persistentDataPath + "/SQLDatas"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/SQLDatas");
		}
		if (!File.Exists(appDBPath))
		{
			Debug.LogError("文件不存在");
			filepath = AssetBundleLoader.StreamingAssetsPath2Url("SQLDatas/VertSaveDatas.sqlite");
			string url = AssetBundleLoader.StreamingAssetsPath2Url("SQLDatas/VertSaveDatas.sqlite");
			WWW www = new WWW(url);
			yield return www;
			Debug.LogError("yield return www");
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogError(string.Format("{0}: {1}", www.error, url));
			}
			else
			{
				File.WriteAllBytes(appDBPath, www.bytes);
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0003A718 File Offset: 0x00038918
	private void OnApplicationFocus(bool focusStatus)
	{
		if (Application.platform == RuntimePlatform.Android && focusStatus)
		{
			base.StartCoroutine(this.ResetRawCamera());
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0003A73C File Offset: 0x0003893C
	private IEnumerator ResetRawCamera()
	{
		yield return new WaitForSeconds(0.1f);
		RTTManager.Instance.ResetCamera();
		foreach (RenderTexture rt in GameCache.RawImageList.Keys)
		{
			if (GameCache.RawImageList[rt])
			{
				GameCache.RawImageList[rt].targetTexture = rt;
				GameCache.RawImageList[rt].ResetAspect();
			}
		}
		yield break;
	}

	// Token: 0x04000BDD RID: 3037
	public static LiveForEver Instance;

	// Token: 0x04000BDE RID: 3038
	public AudioSource MusicSource;

	// Token: 0x04000BDF RID: 3039
	public AudioSource AudioSource;

	// Token: 0x04000BE0 RID: 3040
	public float progress;

	// Token: 0x04000BE1 RID: 3041
	public string LoadMsg;

	// Token: 0x04000BE2 RID: 3042
	public GameObject NameAndTitles;

	// Token: 0x04000BE3 RID: 3043
	public GameObject ScreenChats;

	// Token: 0x04000BE4 RID: 3044
	private bool first = true;

	// Token: 0x04000BE5 RID: 3045
	public bool connecting;
}
