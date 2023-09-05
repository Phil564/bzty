using System;
using GameClient.Entities;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class SFSUtil
{
	
	// Token: 0x06000997 RID: 2455 RVA: 0x00036704 File Offset: 0x00034904
	public static void CacheData(ISFSObject data)
	{
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00036708 File Offset: 0x00034908
	public static void CacheSend(string cmd, string source, ISFSObject param, Action callback)
	{
		SFSUtil.Send(cmd, source, param, delegate(bool suc, ISFSObject data)
		{
			if (suc)
			{
				SFSUtil.CacheData(data);
				if (callback != null)
				{
					callback();
				}
			}
		});
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00036738 File Offset: 0x00034938
	public static void Send(string cmd, string source, ISFSObject param, Action<bool, ISFSObject> callback)
	{
		switch (cmd) 
		{
			case "CreateCharacter": // called by CreateCharacter, to well, create a character.
				Hero hero;
				if (param.GetInt("sex") == 1)
				{
					hero = GameCache.HeroDatas[101];
				}
				else
				{
					hero = GameCache.HeroDatas[102];
				}
				ISFSObject isfsobject = new SFSObject();
				PlayerPrefs.SetInt("CCharacterID",1);
				isfsobject.PutInt("ID", PlayerPrefs.GetInt("CCharacterID")); // Character Id, temporary
				PlayerPrefs.SetInt("CGameUserID",1);
				isfsobject.PutInt("GameUserID", PlayerPrefs.GetInt("CGameUserID")); // User Id, temporary
				PlayerPrefs.SetInt("CSex", param.GetInt("sex"));
				isfsobject.PutInt("Sex", PlayerPrefs.GetInt("CSex")); // Character Sex/Gender
				PlayerPrefs.SetInt("CCharacterColor", param.GetInt("characterColor"));
				isfsobject.PutInt("CharacterColor", PlayerPrefs.GetInt("CCharacterColor")); // Character Skin Color
				PlayerPrefs.SetInt("CEyesColor", param.GetInt("eyseColor"));
				isfsobject.PutInt("EyesColor", PlayerPrefs.GetInt("CEyesColor")); // Character Eyes Color
				PlayerPrefs.SetInt("CHiddenPoints", 0);
				isfsobject.PutInt("HiddenPoints", PlayerPrefs.GetInt("CHiddenPoints")); // Unknown
				PlayerPrefs.SetInt("CGrade", 1);
				isfsobject.PutInt("Grade", PlayerPrefs.GetInt("CGrade")); // Level
				PlayerPrefs.SetInt("CStep", 1);
				isfsobject.PutInt("Step", PlayerPrefs.GetInt("CStep")); // Rank
				PlayerPrefs.SetInt("CExp", 0);
				isfsobject.PutInt("Exp", PlayerPrefs.GetInt("CExp")); // Level Exp
				PlayerPrefs.SetInt("CUpExp", 20);
				isfsobject.PutInt("UpExp", PlayerPrefs.GetInt("CUpExp")); // Level Exp needed for the next level
				PlayerPrefs.SetInt("CStepExp", 0);
				isfsobject.PutInt("StepExp", PlayerPrefs.GetInt("CStepExp")); // Rank Exp
				PlayerPrefs.SetInt("CUpStep", 500);
				isfsobject.PutInt("UpStep", PlayerPrefs.GetInt("CUpStep")); // Rank Exp needed for the next level
				PlayerPrefs.SetInt("CMoney", 10000);
				isfsobject.PutInt("Money", PlayerPrefs.GetInt("CMoney")); // Gems
				PlayerPrefs.SetInt("CGameGold", 10000);
				isfsobject.PutInt("GameGold", PlayerPrefs.GetInt("CGameGold")); // Coins/Gold
				ISFSArray isfsarray = new SFSArray(); // TEMPORARY!!!! character fit, doesn't save, loaded on the fly
				ISFSObject isfsobject2 = new SFSObject();
				isfsobject2.PutInt("ID", hero.WeaponId);
				isfsobject2.PutInt("CharacterID", 1);
				isfsobject2.PutInt("EquipID", hero.WeaponId);
				isfsobject2.PutInt("Type", 1);
				isfsarray.AddSFSObject(isfsobject2);
				ISFSObject isfsobject3 = new SFSObject();
				isfsobject3.PutInt("ID", hero.ClothId);
				isfsobject3.PutInt("CharacterID", 1);
				isfsobject3.PutInt("EquipID", hero.ClothId);
				isfsobject3.PutInt("Type", 3);
				isfsarray.AddSFSObject(isfsobject3);
				ISFSObject isfsobject4 = new SFSObject();
				isfsobject4.PutInt("ID", hero.ShoesId);
				isfsobject4.PutInt("CharacterID", 1);
				isfsobject4.PutInt("EquipID", hero.ShoesId);
				isfsobject4.PutInt("Type", 4);
				isfsarray.AddSFSObject(isfsobject4);
				ISFSObject isfsobject5 = new SFSObject();
				isfsobject5.PutInt("ID", hero.TireId);
				isfsobject5.PutInt("CharacterID", 1);
				isfsobject5.PutInt("EquipID", hero.TireId);
				isfsobject5.PutInt("Type", 2);
				isfsarray.AddSFSObject(isfsobject5);
				isfsobject.PutSFSArray("CharacterEquip", isfsarray);
				ISFSArray isfsarray2 = new SFSArray();
				isfsobject.PutSFSArray("CharacterFuben", isfsarray2);
				ISFSObject isfsobject6 = new SFSObject();
				isfsobject6.PutInt("ID", PlayerPrefs.GetInt("CGameUserID"));
				SFSObject sfsobject = new SFSObject();
				((ISFSObject)sfsobject).PutSFSObject("cha", isfsobject);
				((ISFSObject)sfsobject).PutSFSObject("gu", isfsobject6);
				PlayerPrefs.SetInt("CCharacterCreated", 1);
				PlayerPrefs.Save();
				callback(true, sfsobject);
				break;
			case "SetCharacterFunc": // this is kind of how the game normally saves shit
				if (param.ContainsKey("func"))
				{
					GameCache.Character.Func = param.GetInt("func"); // setting the func
					PlayerPrefs.SetInt("CFunc", param.GetInt("func")); // saving the func in case that the player leaves
				} 
				PlayerPrefs.Save();
				callback(true, null);
				break;
			case "SetCharacter": // called by ChangeNameUI, probably to change a part of the character without literally fucking it up.
			{
				if (param.ContainsKey("nm"))
				{
					GameCache.Character.Name = param.GetUtfString("nm"); // idk where it sets the name so uhhh i set it here
					PlayerPrefs.SetString("CName", param.GetUtfString("nm")); // i also save the name
				}
				PlayerPrefs.Save();
				callback(true, null);
				break;
			}
			case "SDKLoginGame": // called by Server1UI (i think), returns character data. supposed to say if the login is good but i don't want to simulate that.
				//param.GetUtfString("nm");
				//param.GetUtfString("sid");
				//param.GetUtfString("znm");
				//param.GetUtfString("zid");
				//param.GetUtfString("sk");
				if (PlayerPrefs.HasKey("CCharacterCreated"))
				{
					Hero hero2;
					if (PlayerPrefs.GetInt("CSex") == 1)
					{
						hero2 = GameCache.HeroDatas[101];
					}
					else
					{
						hero2 = GameCache.HeroDatas[102];
					}
					ISFSObject isfsobject2_1 = new SFSObject();
					isfsobject2_1.PutInt("ID", PlayerPrefs.GetInt("CCharacterID")); // Character Id, temporary
					isfsobject2_1.PutUtfString("Name", PlayerPrefs.GetString("CName",string.Empty)); // Character Name
					isfsobject2_1.PutInt("GameUserID", PlayerPrefs.GetInt("CGameUserID")); // User Id, temporary
					isfsobject2_1.PutInt("Sex", PlayerPrefs.GetInt("CSex")); // Character Sex/Gender
					isfsobject2_1.PutInt("CharacterColor", PlayerPrefs.GetInt("CCharacterColor")); // Character Skin Color
					isfsobject2_1.PutInt("EyesColor", PlayerPrefs.GetInt("CEyesColor")); // Character Eyes Color
					isfsobject2_1.PutInt("HiddenPoints", PlayerPrefs.GetInt("CHiddenPoints")); // Unknown
					isfsobject2_1.PutInt("Grade", PlayerPrefs.GetInt("CGrade")); // Level
					isfsobject2_1.PutInt("Step", PlayerPrefs.GetInt("CStep")); // Rank
					isfsobject2_1.PutInt("Exp", PlayerPrefs.GetInt("CExp")); // Level Exp
					isfsobject2_1.PutInt("UpExp", PlayerPrefs.GetInt("CUpExp")); // Level Exp needed for the next level
					isfsobject2_1.PutInt("StepExp", PlayerPrefs.GetInt("CStepExp")); // Rank Exp
					isfsobject2_1.PutInt("UpStep", PlayerPrefs.GetInt("CUpStep")); // Rank Exp needed for the next level
					isfsobject2_1.PutInt("Money", PlayerPrefs.GetInt("CMoney")); // Gems
					isfsobject2_1.PutInt("GameGold", PlayerPrefs.GetInt("CGameGold")); // Coins/Gold
					if (PlayerPrefs.HasKey("CFunc"))
					{
						isfsobject2_1.PutInt("Func", PlayerPrefs.GetInt("CFunc"));
					}
					ISFSArray isfsarray2_1 = new SFSArray(); // TEMPORARY!!!! character fit, doesn't save, loaded on the fly
					ISFSObject isfsobject2_2 = new SFSObject();
					isfsobject2_2.PutInt("ID", hero2.WeaponId);
					isfsobject2_2.PutInt("CharacterID", 1);
					isfsobject2_2.PutInt("EquipID", hero2.WeaponId);
					isfsobject2_2.PutInt("Type", 1);
					isfsarray2_1.AddSFSObject(isfsobject2_2);
					ISFSObject isfsobject2_3 = new SFSObject();
					isfsobject2_3.PutInt("ID", hero2.ClothId);
					isfsobject2_3.PutInt("CharacterID", 1);
					isfsobject2_3.PutInt("EquipID", hero2.ClothId);
					isfsobject2_3.PutInt("Type", 3);
					isfsarray2_1.AddSFSObject(isfsobject2_3);
					ISFSObject isfsobject2_4 = new SFSObject();
					isfsobject2_4.PutInt("ID", hero2.ShoesId);
					isfsobject2_4.PutInt("CharacterID", 1);
					isfsobject2_4.PutInt("EquipID", hero2.ShoesId);
					isfsobject2_4.PutInt("Type", 4);
					isfsarray2_1.AddSFSObject(isfsobject2_4);
					ISFSObject isfsobject2_5 = new SFSObject();
					isfsobject2_5.PutInt("ID", hero2.TireId);
					isfsobject2_5.PutInt("CharacterID", 1);
					isfsobject2_5.PutInt("EquipID", hero2.TireId);
					isfsobject2_5.PutInt("Type", 2);
					isfsarray2_1.AddSFSObject(isfsobject2_5);
					isfsobject2_1.PutSFSArray("CharacterEquip", isfsarray2_1);
					ISFSArray isfsarray2_2 = new SFSArray();
					isfsobject2_1.PutSFSArray("CharacterFuben", isfsarray2_2);
					ISFSObject isfsobject2_6 = new SFSObject();
					isfsobject2_6.PutInt("ID", PlayerPrefs.GetInt("CGameUserID"));
					SFSObject sfsobject2_1 = new SFSObject();
					((ISFSObject)sfsobject2_1).PutSFSObject("cha", isfsobject2_1);
					((ISFSObject)sfsobject2_1).PutSFSObject("gu", isfsobject2_6);
					callback(true, sfsobject2_1);
				}
				else
				{
					ISFSObject isfsobject3_1 = new SFSObject();
					callback(true, isfsobject3_1);
				}
				break;
			case "ReadyBeginGame": // not well documented
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				//param.GetInt("tp");
				callback(false, null);
				break;
			case "DelMail": // deletes the mail off the server
				//param.GetInt("cmid"); // character mail id
				callback(true, null);
				break;
			case "Ping": // simulate server ping, referenced in ServerAPI
				callback(true, null);
				break;
			case "JoinGameRoom": // gives a signal to the server when you join a Turf War gameroom
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				//param.GetInt("grid"); // returns grid or Game Room ID
				callback(false, null); // callback is false since we do not want players getting softlocked in a room that doesn't exist
				//returns:
				//sfsobject.PutInt("id"); // (????????)
				break;
			case "SearchGameRoom": // supposed to return Turf War rooms, but because there aren't any Turf War rooms, the callback is false.
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(false, null); // callback is false since we do not want players getting softlocked in a room that doesn't exist
				break;
			case "CreateGameRoom": // gives a signal to the server when you create a Turf War gameroom
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(false, null); // callback is false since we do not want players getting softlocked in a room that doesn't exist
				break;
			case "LeaveGameRoom": // gives a signal to the server when you leave a Turf War gameroom
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(true, null); // callback is true since we do not want players getting softlocked in a room that doesn't exist
				break;
			case "JoinStepGameRoom": // gives a signal to the server when you join a Ranked Battle gameroom
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(false, null); // callback is false since we do not want players getting softlocked in a room that doesn't exist
				break;
			case "LeaveStepGameRoom": // gives a signal to the server when you leave a Ranked Battle gameroom
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(true, null); // callback is true since we do not want players getting softlocked in a room that doesn't exist
				break;
			case "RegeditGame": // UNUSED, never called, not documented
				//param.GetUtfString("nm"); // username
				//param.GetUtfString("pw"); // password
				//param.GetUtfString("mp"); // unknown
				//param.GetUtfString("sk"); // unknown
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(false, null); // callback is false since we do not know what this does
				break;
			case "FastRegeditGame": // UNUSED, never called, not documented, probably a faster version of RegeditGame without connectivity to the auth servers
				//param.GetUtfString("sk"); // unknown
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(false, null); // callback is false since we do not know what this does
				break;
			case "LoginGame": // UNUSED, never called, not documented, probably connects to the auth servers, supposed to say if the login is good
				//param.GetUtfString("nm"); // username
				//param.GetUtfString("pw"); // password
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				callback(false, null); // callback is false since we do not know what this does
				break;
			case "FubenSettlement": // tells the game if it should reward the player with the stars or not, this shouldn't be server sided
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				Map map = GameCache.Map[param.GetInt("fubenId")];
				CharacterFuben characterFubenByMapId = CharacterService.GetCharacterFubenByMapId(param.GetInt("fubenId"));
				int starnum = 0;
				SFSObject sfsobject3_1 = new SFSObject();
				sfsobject3_1.PutUtfString("sc", string.Empty);
				sfsobject3_1.PutUtfString("ck", string.Empty);
				sfsobject3_1.PutInt("goldNum", param.GetInt("goldNum")); // number of Coins/Gold collected in the Campaign Level
				sfsobject3_1.PutInt("fubenId", param.GetInt("fubenId")); // id of the Campaign Level
				sfsobject3_1.PutInt("dieNum", param.GetInt("dieNum")); // number of times the player died in the Campaign Level
				sfsobject3_1.PutInt("passTime", param.GetInt("passTime")); // time ever since the start of the Campaign Level
				string[] stars = {"1", "2", "3"};
				foreach (string s in stars)
				{
/*NEEDS A FIX*/		RaidStarCondition raidStarCondition = GameCache.RaidStar[map["GetStarCondition0" + s]];
					if ((raidStarCondition.ConditionType == 1 && param.GetInt("dieNum") < 1)
						|| raidStarCondition.ConditionType == 2
						|| (raidStarCondition.ConditionType == 3 && (param.GetInt("goldNum") > raidStarCondition.Parameter01 || param.GetInt("goldNum") == raidStarCondition.Parameter01))
						|| (raidStarCondition.ConditionType == 4 && param.GetInt("passTime") < raidStarCondition.Parameter01))
					{
						sfsobject3_1.PutInt(("star" + s), 1);
						starnum++;
					}
					else
					{
						sfsobject3_1.PutInt(("star" + s), 0);
					}
					if (characterFubenByMapId != null)
					{
/*NEEDS A FIX*/			if (characterFubenByMapId["Star" + s] = 1)
						{
							sfsobject3_1.PutInt(("star" + s + "get"), 1);
						}
						else
						{
							sfsobject3_1.PutInt(("star" + s + "get"), 0);
						}	
					}
					else
					{
						sfsobject3_1.PutInt(("star" + s + "get"), 0);
					}	
				}
				CharacterFuben characterFuben = new CharacterFuben();
				characterFuben.ID = GameCache.RoomInfo.mapId;
				characterFuben.CharacterID = 1;
				characterFuben.FubenID = GameCache.RoomInfo.mapId;
				characterFuben.Star1 = sfsobject3_1.GetInt("star1");
				characterFuben.Star2 = sfsobject3_1.GetInt("star2");
				characterFuben.Star3 = sfsobject3_1.GetInt("star3");
				characterFuben.Star = starnum;
				characterFuben.IsReceive = 1;
				if (characterFubenByMapId != null)
				{
					characterFubenByMapId = characterFuben;
				}
				else
				{
					GameCache.Character.CharacterFubenList.Add(characterFuben);
				}
				callback(true, sfsobject3_1);
				break;
			case "FightSettlement": // called at the end of a match (?), has no real purpose other than notifying the server (SendResultMessage)
				//param.GetInt("win"); // result; either 0 or 1
				//param.GetFloat("cnt1"); // random-ass float
				//param.GetFloat("cnt2"); // random-ass float
				//param.GetInt("tp"); // unknown
				callback(true, null); // callback is true because we do not want players to be softlocked
				break;
			case "GameSettlement": // called at the end of a match for the result reveal cutscene, has no real purpose other than notifying the server (SendResultMessage)
				//param.GetInt("total1"); // "leftscore"
				//param.GetInt("total2"); // "rightscore"
				callback(true, null); // callback is true because we do not want players to be softlocked
				break;
			case "AcceptInvite": // called at the end of a match for the result reveal cutscene, has no real purpose other than notifying the server (SendResultMessage)
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				//param.GetInt("cfid"); // ExtractRoomUserInfoList.GetInt("id")?????
				//param.GetInt("accept"); // example: 1
				callback(true, null); // callback is true because we do not want players to be softlocked
				break;
			case "GetStepTop": // GET leaderboard in plaza
				//EXAMPLE OF A LEADERBOARD!!!!
				/*ISFSArray sfsarray = new SFSArray()
				ISFSObject plrexample = new SFSObject();
				plrexample.PutInt("sex", 1), // character sex/gender
				plrexample.PutInt("tire", GameCache.HeroDatas[101].TireId), // character hat id
				plrexample.PutInt("cloth", GameCache.HeroDatas[101].ClothId), // character cloth id
				plrexample.PutInt("characterColor", 1), // character skin color
				plrexample.PutInt("eyseColor", 1) // character eye color
				plrexample.PutInt("ownrank", 1) // rank of the player
				plrexample.PutInt("guid", 1); // game user id???
				plrexample.PutInt("chaid", 1); // character id???
				sfsarray.AddSFSObject(plrexample);
				ISFSArray sfsobject = new SFSObject()
				sfsobject.PutSFSArray("arr", isfsarray);
				callback(true, sfsobject); // callback is false since we do not want players to load an unfinished leaderboard
				*/
				callback(false, null);
				break;
			case "OprChaFriend": // Send a friend request to a player in the leaderboard (incomplete code??? the server doesn't know who sends the request??)
				//param.GetInt("tp"); // example: 1
				//param.GetInt("id"); // example: character id of the player chosen
				callback(false, null);
				break;
			// undocumented
			/*
			case "SearchShopItem":
			case "CheckShopItemInfo":
			case "BuyShopItem":
			case "ExtractAttachment":
			case "SearchMailList":
			case "SearchMailInfo":
			case "ChangeGroup":
			case "ModifyGameRoom":
			case "DataForward":
			case "AddRobot":
			case "SetRobotState":
			case "GetChaFriend":
			case "OprFriendRequest":
			case "GetFriendRequest":
			case "SearchFriend":
			case "ReplaceHeroEquip":
			case "CheckItemInfo":
			case "SearchBlackList":
			case "InviteJoinGameRoom":
			case "InviteFriend":
			case "VerifySession":
			case "GetChaProperty":
			case "KickoutGameRoom":
			case "GetStarReward": // TODO, probably for fuben/campaign levels
			case "PVPAddMoney":
			case "GetWeekRewardItem":
			case "SignIn":
			case "GetReward":
			case "PostException": // BUG REPORTS??? LMFAO
			*/
			default: // when i haven't documented a case, we are just going to try to connect to the servers.
				EventListenerDelegate eventListener = null;
				eventListener = delegate(BaseEvent e)
				{
					ISFSObject isfsobject;
					bool flag;
					if (SmartFoxHelper.PreProcessExtensionCallBack(e, cmd, source, out isfsobject, out flag))
					{
						SmartFoxClient.Instance.SmartFox.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, eventListener);
						if (isfsobject != null)
						{
							if (callback != null)
							{
								callback(true, isfsobject);
							}
						}
						else if (callback != null)
						{
							callback(false, null);
						}
					}
				};
				SmartFoxClient.Instance.SmartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, eventListener);
				SmartFoxHelper.SendExtensionRequest(cmd, param, source, null, false);
				break;
		}
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x000367A8 File Offset: 0x000349A8
	public static void SendFightMessage(string cmd, string source, ISFSObject param, Action<bool, ISFSObject> callback)
	{
		SmartFoxHelper.SendExtensionRequestFightMessage(cmd, param, null, false);
	}
}
