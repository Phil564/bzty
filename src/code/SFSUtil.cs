using System;
using GameClient.Entities;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using System.IO;

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
		Debug.Log("SFSUtil.Send, cmd: " + cmd);
		switch (cmd) 
		{
			case "CreateCharacter": // called by CreateCharacter, to well, create a character.
				PlayerPrefs.SetInt("CCharacterID",1);
				PlayerPrefs.SetInt("CGameUserID",1);
				PlayerPrefs.SetInt("CSex", param.GetInt("sex"));
				PlayerPrefs.SetInt("CCharacterColor", param.GetInt("characterColor"));
				PlayerPrefs.SetInt("CEyesColor", param.GetInt("eyseColor"));
				PlayerPrefs.SetInt("CHiddenPoints", 0);
				PlayerPrefs.SetInt("CGrade", 1);
				PlayerPrefs.SetInt("CStep", 1);
				PlayerPrefs.SetInt("CExp", 0);
				PlayerPrefs.SetInt("CUpExp", 20);
				PlayerPrefs.SetInt("CStepExp", 0);
				PlayerPrefs.SetInt("CUpStep", 500);
				PlayerPrefs.SetInt("CMoney", 10000);
				PlayerPrefs.SetInt("CGameGold", 10000);
				PlayerPrefs.SetInt("CCharacterCreated", 1);
				PlayerPrefs.Save();
				ISFSObject isfsobjCC_cha = new SFSObject();
				isfsobjCC_cha.PutInt("ID", PlayerPrefs.GetInt("CCharacterID")); // Character Id, temporary
				isfsobjCC_cha.PutInt("GameUserID", PlayerPrefs.GetInt("CGameUserID")); // User Id, temporary
				isfsobjCC_cha.PutInt("Sex", PlayerPrefs.GetInt("CSex")); // Character Sex/Gender
				isfsobjCC_cha.PutInt("CharacterColor", PlayerPrefs.GetInt("CCharacterColor")); // Character Skin Color
				isfsobjCC_cha.PutInt("EyesColor", PlayerPrefs.GetInt("CEyesColor")); // Character Eyes Color
				isfsobjCC_cha.PutInt("HiddenPoints", PlayerPrefs.GetInt("CHiddenPoints")); // Unknown
				isfsobjCC_cha.PutInt("Grade", PlayerPrefs.GetInt("CGrade")); // Level
				isfsobjCC_cha.PutInt("Step", PlayerPrefs.GetInt("CStep")); // Rank
				isfsobjCC_cha.PutInt("Exp", PlayerPrefs.GetInt("CExp")); // Level Exp
				isfsobjCC_cha.PutInt("UpExp", PlayerPrefs.GetInt("CUpExp")); // Level Exp needed for the next level
				isfsobjCC_cha.PutInt("StepExp", PlayerPrefs.GetInt("CStepExp")); // Rank Exp
				isfsobjCC_cha.PutInt("UpStep", PlayerPrefs.GetInt("CUpStep")); // Rank Exp needed for the next level
				isfsobjCC_cha.PutInt("Money", PlayerPrefs.GetInt("CMoney")); // Gems
				isfsobjCC_cha.PutInt("GameGold", PlayerPrefs.GetInt("CGameGold")); // Coins/Gold
				isfsobjCC_cha.PutInt("Func", 0); // i have to do this since the game is dumb
				Hero heroCC;
				if (PlayerPrefs.GetInt("CSex") == 1)
				{
					heroCC = GameCache.HeroDatas[101];
				}
				else
				{
					heroCC = GameCache.HeroDatas[102];
				}
				ISFSArray isfsarrCC_CE = new SFSArray(); // TEMPORARY!!!! character fit, doesn't save, loaded on the fly
				ISFSObject isfsobjCC_Weapon = new SFSObject();
				isfsobjCC_Weapon.PutInt("ID", heroCC.WeaponId);
				isfsobjCC_Weapon.PutInt("CharacterID", 1);
				isfsobjCC_Weapon.PutInt("EquipID", heroCC.WeaponId);
				isfsobjCC_Weapon.PutInt("Type", 1);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_Weapon);
				ISFSObject isfsobjCC_Cloth = new SFSObject();
				isfsobjCC_Cloth.PutInt("ID", heroCC.ClothId);
				isfsobjCC_Cloth.PutInt("CharacterID", 1);
				isfsobjCC_Cloth.PutInt("EquipID", heroCC.ClothId);
				isfsobjCC_Cloth.PutInt("Type", 3);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_Cloth);
				ISFSObject isfsobjCC_Shoes = new SFSObject();
				isfsobjCC_Shoes.PutInt("ID", heroCC.ShoesId);
				isfsobjCC_Shoes.PutInt("CharacterID", 1);
				isfsobjCC_Shoes.PutInt("EquipID", heroCC.ShoesId);
				isfsobjCC_Shoes.PutInt("Type", 4);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_Shoes);
				ISFSObject isfsobjCC_Tire = new SFSObject();
				isfsobjCC_Tire.PutInt("ID", heroCC.TireId);
				isfsobjCC_Tire.PutInt("CharacterID", 1);
				isfsobjCC_Tire.PutInt("EquipID", heroCC.TireId);
				isfsobjCC_Tire.PutInt("Type", 2);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_Tire);
				isfsobjCC_cha.PutSFSArray("CharacterEquip", isfsarrCC_CE);
				ISFSArray isfsarrCC_CF = new SFSArray(); // TEMPORARY!!!! story mode save data, doesn't save
				isfsobjCC_cha.PutSFSArray("CharacterFuben", isfsarrCC_CF);
				/*
				ISFSArray isfsarrCC_CI = new SFSArray(); // TEMPORARY!!!! inventory save data, doesn't save
				
				ISFSObject isfsobjCC_IWeapon = new SFSObject();
				isfsobjCC_IWeapon.PutInt("ID", heroCC.WeaponId);
				isfsobjCC_IWeapon.PutInt("CharacterID", 1);
				isfsobjCC_IWeapon.PutInt("EquipID", heroCC.WeaponId);
				isfsobjCC_IWeapon.PutInt("Type", 1);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_IWeapon);
				ISFSObject isfsobjCC_ICloth = new SFSObject();
				isfsobjCC_ICloth.PutInt("ID", heroCC.ClothId);
				isfsobjCC_ICloth.PutInt("CharacterID", 1);
				isfsobjCC_ICloth.PutInt("EquipID", heroCC.ClothId);
				isfsobjCC_ICloth.PutInt("Type", 3);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_ICloth);
				ISFSObject isfsobjCC_IShoes = new SFSObject();
				isfsobjCC_IShoes.PutInt("ID", heroCC.ShoesId);
				isfsobjCC_IShoes.PutInt("CharacterID", 1);
				isfsobjCC_IShoes.PutInt("EquipID", heroCC.ShoesId);
				isfsobjCC_IShoes.PutInt("Type", 4);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_IShoes);
				ISFSObject isfsobjCC_ITire = new SFSObject();
				isfsobjCC_ITire.PutInt("ID", heroCC.TireId);
				isfsobjCC_ITire.PutInt("CharacterID", 1);
				isfsobjCC_ITire.PutInt("EquipID", heroCC.TireId);
				isfsobjCC_ITire.PutInt("Type", 2);
				isfsarrCC_CE.AddSFSObject(isfsobjCC_ITire);
				
				isfsobjCC_cha.PutSFSArray("CharacterItem", isfsarrCC_CI);
				*/
				ISFSObject isfsobjCC_gu = new SFSObject();
				isfsobjCC_gu.PutInt("ID", PlayerPrefs.GetInt("CGameUserID"));
				ISFSObject isfsobjCC_send = new SFSObject();
				isfsobjCC_send.PutSFSObject("cha", isfsobjCC_cha);
				isfsobjCC_send.PutSFSObject("gu", isfsobjCC_gu);
				callback(true, isfsobjCC_send);
				// string json = JsonUtility.ToJson(isfsobjCC_send,true);
				// File.WriteAllText(Application.persistentDataPath + "playerdata.json", json);
				break;
			case "SetCharacterFunc": // this is kind of how the game normally saves shit
				if (param.ContainsKey("func"))
				{
					if (param.GetInt("func") == 68) // NASTY HACK!!! I DON'T WANT TO PROGRAM AN INVENTORY
					{
						PlayerPrefs.SetInt("CFunc", 3012);
						PlayerPrefs.Save();
						if (GameCache.Character != null)
						{
							GameCache.Character.Func = 3012;
						}
					}	
					else
					{
						PlayerPrefs.SetInt("CFunc", param.GetInt("func")); // saving the func in case that the player leaves
						PlayerPrefs.Save();
						if (GameCache.Character != null)
						{
							GameCache.Character.Func = param.GetInt("func");
						}
					}
					Debug.Log("SetCharacterFunc, Func: " + PlayerPrefs.GetInt("CFunc").ToString());
				} 
				if (callback != null)
				{
					callback(false, null);
				}
				break;
			case "SetCharacter": // called by ChangeNameUI, probably to change a part of the character without literally fucking it up.
			{
				if (param.ContainsKey("nm"))
				{
					PlayerPrefs.SetString("CName", param.GetUtfString("nm")); // i also save the name
					PlayerPrefs.Save();
					GameCache.Character.Name = param.GetUtfString("nm"); // idk where it sets the name so uhhh i set it here
				}
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
					Hero heroSDKLG;
					if (PlayerPrefs.GetInt("CSex") == 1)
					{
						heroSDKLG = GameCache.HeroDatas[101];
					}
					else
					{
						heroSDKLG = GameCache.HeroDatas[102];
					}
					ISFSObject isfsobjSDKLG_Ccha = new SFSObject();
					isfsobjSDKLG_Ccha.PutInt("ID", PlayerPrefs.GetInt("CCharacterID")); // Character Id, temporary
					isfsobjSDKLG_Ccha.PutUtfString("Name", PlayerPrefs.GetString("CName",string.Empty)); // Character Name
					isfsobjSDKLG_Ccha.PutInt("GameUserID", PlayerPrefs.GetInt("CGameUserID")); // User Id, temporary
					isfsobjSDKLG_Ccha.PutInt("Sex", PlayerPrefs.GetInt("CSex")); // Character Sex/Gender
					isfsobjSDKLG_Ccha.PutInt("CharacterColor", PlayerPrefs.GetInt("CCharacterColor")); // Character Skin Color
					isfsobjSDKLG_Ccha.PutInt("EyesColor", PlayerPrefs.GetInt("CEyesColor")); // Character Eyes Color
					isfsobjSDKLG_Ccha.PutInt("HiddenPoints", PlayerPrefs.GetInt("CHiddenPoints")); // Unknown
					isfsobjSDKLG_Ccha.PutInt("Grade", PlayerPrefs.GetInt("CGrade")); // Level
					isfsobjSDKLG_Ccha.PutInt("Step", PlayerPrefs.GetInt("CStep")); // Rank
					isfsobjSDKLG_Ccha.PutInt("Exp", PlayerPrefs.GetInt("CExp")); // Level Exp
					isfsobjSDKLG_Ccha.PutInt("UpExp", PlayerPrefs.GetInt("CUpExp")); // Level Exp needed for the next level
					isfsobjSDKLG_Ccha.PutInt("StepExp", PlayerPrefs.GetInt("CStepExp")); // Rank Exp
					isfsobjSDKLG_Ccha.PutInt("UpStep", PlayerPrefs.GetInt("CUpStep")); // Rank Exp needed for the next level
					isfsobjSDKLG_Ccha.PutInt("Money", PlayerPrefs.GetInt("CMoney")); // Gems
					isfsobjSDKLG_Ccha.PutInt("GameGold", PlayerPrefs.GetInt("CGameGold")); // Coins/Gold
					if (PlayerPrefs.HasKey("CFunc"))
					{
						isfsobjSDKLG_Ccha.PutInt("Func", PlayerPrefs.GetInt("CFunc"));
					}
					ISFSArray isfsarrSDKLG_CCE = new SFSArray(); // TEMPORARY!!!! character fit, doesn't save, loaded on the fly
					ISFSObject isfsarrSDKLG_CWeapon = new SFSObject();
					isfsarrSDKLG_CWeapon.PutInt("ID", heroSDKLG.WeaponId);
					isfsarrSDKLG_CWeapon.PutInt("CharacterID", 1);
					isfsarrSDKLG_CWeapon.PutInt("EquipID", heroSDKLG.WeaponId);
					isfsarrSDKLG_CWeapon.PutInt("Type", 1);
					isfsarrSDKLG_CCE.AddSFSObject(isfsarrSDKLG_CWeapon);
					ISFSObject isfsarrSDKLG_CCloth = new SFSObject();
					isfsarrSDKLG_CCloth.PutInt("ID", heroSDKLG.ClothId);
					isfsarrSDKLG_CCloth.PutInt("CharacterID", 1);
					isfsarrSDKLG_CCloth.PutInt("EquipID", heroSDKLG.ClothId);
					isfsarrSDKLG_CCloth.PutInt("Type", 3);
					isfsarrSDKLG_CCE.AddSFSObject(isfsarrSDKLG_CCloth);
					ISFSObject isfsarrSDKLG_CShoes = new SFSObject();
					isfsarrSDKLG_CShoes.PutInt("ID", heroSDKLG.ShoesId);
					isfsarrSDKLG_CShoes.PutInt("CharacterID", 1);
					isfsarrSDKLG_CShoes.PutInt("EquipID", heroSDKLG.ShoesId);
					isfsarrSDKLG_CShoes.PutInt("Type", 4);
					isfsarrSDKLG_CCE.AddSFSObject(isfsarrSDKLG_CShoes);
					ISFSObject isfsarrSDKLG_CTire = new SFSObject();
					isfsarrSDKLG_CTire.PutInt("ID", heroSDKLG.TireId);
					isfsarrSDKLG_CTire.PutInt("CharacterID", 1);
					isfsarrSDKLG_CTire.PutInt("EquipID", heroSDKLG.TireId);
					isfsarrSDKLG_CTire.PutInt("Type", 2);
					isfsarrSDKLG_CCE.AddSFSObject(isfsarrSDKLG_CTire);
					isfsobjSDKLG_Ccha.PutSFSArray("CharacterEquip", isfsarrSDKLG_CCE);
					ISFSArray isfsarrSDKLG_CCF = new SFSArray();
					isfsobjSDKLG_Ccha.PutSFSArray("CharacterFuben", isfsarrSDKLG_CCF);
					ISFSObject isfsobjSDKLG_Cgu = new SFSObject();
					isfsobjSDKLG_Cgu.PutInt("ID", PlayerPrefs.GetInt("CGameUserID"));
					ISFSObject isfsobjSDKLG_C = new SFSObject();
					isfsobjSDKLG_C.PutSFSObject("cha", isfsobjSDKLG_Ccha);
					isfsobjSDKLG_C.PutSFSObject("gu", isfsobjSDKLG_Cgu);
					callback(true, isfsobjSDKLG_C);
				}
				else
				{
					ISFSObject isfsobjSDKLG = new SFSObject();
					callback(true, isfsobjSDKLG);
				}
				break;
			case "Ping": // simulate server ping, referenced in ServerAPI
				callback(true, null);
				break;
			case "JoinGameRoom": // gives a signal to the server when you join a Turf War gameroom
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				//param.GetInt("grid"); // returns grid or Game Room ID (0 for fastjoin or -1 for create)
				/*switch(param.GetInt("grid"))
					case 0:
						break;
					case -1:
						break;
					default:
						break;*/
				if (callback != null)
				{
					callback(true, null); // callback is false since we do not want players getting softlocked in a room that doesn't exist
				}
				//returns:
				//sfsobject.PutInt("id"); // (????????)
				break;
			case "ReadyBeginGame": // not well documented
				//param.GetUtfString("sc"); // empty
				//param.GetUtfString("ck"); // empty
				//param.GetInt("tp");
				if (callback != null)
				{
					callback(true, null);
				}
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
				int[] istars = {0, 1, 2};
				string[] stars = {"star1", "star2", "star3"};
				string[] starsget = {"star1get", "star2get", "star3get"};
				int[] starconditions = {map.GetStarCondition01, map.GetStarCondition02, map.GetStarCondition03};
				foreach (int s in istars)
				{
					RaidStarCondition raidStarCondition = GameCache.RaidStar[starconditions[s]];
					if ((raidStarCondition.ConditionType == 1 && param.GetInt("dieNum") < 1)
						|| raidStarCondition.ConditionType == 2
						|| (raidStarCondition.ConditionType == 3 && (param.GetInt("goldNum") > raidStarCondition.Parameter01 || param.GetInt("goldNum") == raidStarCondition.Parameter01))
						|| (raidStarCondition.ConditionType == 4 && param.GetInt("passTime") < raidStarCondition.Parameter01))
					{
						sfsobject3_1.PutInt(stars[s], 1);
						starnum++;
					}
					else
					{
						sfsobject3_1.PutInt(stars[s], 0);
					}
					if (characterFubenByMapId != null)
					{
						int[] starssaveddata = {characterFubenByMapId.Star1, characterFubenByMapId.Star2, characterFubenByMapId.Star3};
						if (starssaveddata[s] == 1)
						{
							sfsobject3_1.PutInt(starsget[s], 1);
						}
						else
						{
							sfsobject3_1.PutInt(starsget[s], 0);
						}	
					}
					else
					{
						sfsobject3_1.PutInt(starsget[s], 0);
					}	
				}
				// ALRIGHT this shouldn't be necessary and the server wouldn't do that but the game seems to have some parts missing so????????
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
			case "VerifySession": // Verify the Session. We are going to return local data
				//param.GetUtfString("nm"); // dunno what this returns (this.user)
				//param.GetUtfString("sid"); // dunno what this returns (this.sid)
				//param.GetUtfString("sk"); // GameCache.ServerKey
				ISFSObject isfsobjVS = new SFSObject();
				isfsobjVS.PutUtfString("nm","LocalUser");
				callback(true, isfsobjVS);
				break;
			case "SearchMailList": // NOT PROPERLY IMPLEMENTED!, Called by MailListUI, returns the Mail List for the player
				ISFSArray isfsarrSML = new SFSArray(); // PLACEHOLDER!!!! i ain't programming a whole mail system
				ISFSObject isfsobjSML_Mail = new SFSObject();
				isfsobjSML_Mail.PutUtfString("SendReceiveName","都市市长"); // data from https://youtu.be/QyCTp9fgC_U?t=461, bottom text
				isfsobjSML_Mail.PutUtfString("SendTime","2016-07-01");
				isfsobjSML_Mail.PutInt("Image",71016);
				isfsobjSML_Mail.PutInt("ID",0); // cmid in SearchMailInfo
				isfsarrSML.AddSFSObject(isfsobjSML_Mail);
				ISFSObject isfsobjSML = new SFSObject();
				isfsobjSML.PutSFSArray("cms", isfsarrSML);
				callback(true, isfsobjSML);
				break;
			case "SearchMailInfo": // NOT PROPERLY IMPLEMENTED!, Called by MailInfoUI, returns the Info for the Mail
				//param.GetUtfString("cmid"); // gets ID from a mail from SearchMailList (example: 0)
				ISFSObject isfsobjSMI = new SFSObject(); // PLACEHOLDER!!!! i ain't programming a whole mail system
				isfsobjSMI.PutUtfString("name","都市市长"); // data from https://youtu.be/QyCTp9fgC_U?t=461
				isfsobjSMI.PutUtfString("title","新手奖励");
				isfsobjSMI.PutUtfString("time","2016-07-01");
				isfsobjSMI.PutUtfString("content","欢迎来到墨汁都市,我们给你准备了一件衣服,穿上 试试吧。");
				isfsobjSMI.PutInt("allread",1); // needs to be 1 since 
				isfsobjSMI.PutInt("moneynum",100); // needs to be 1 since 
				isfsobjSMI.PutInt("goldnum",1000); // needs to be 1 since 
				isfsobjSMI.PutInt("allread",1); // needs to be 1 since 
				isfsobjSMI.PutInt("itemnum0",71016);
				callback(true, isfsobjSMI);
				break;
			case "ExtractAttachment": // NOT PROPERLY IMPLEMENTED!, Called by MailInfoUI, returns nothing. Used to tell the server to extract the attachment to the player's data
				//param.GetUtfString("cmid"); // gets ID from a mail from SearchMailInfo (example: 0)
				if (callback != null)
				{
					callback(true, null);
				}
				break;
			case "DelMail": // NOT PROPERLY IMPLEMENTED!, Called by MailInfoUI, deletes the mail off the server
				//param.GetInt("cmid"); // character mail id
				callback(true, null);
				break;
			case "GetWeekRewardItem": // NOT PROPERLY IMPLEMENTED!, Called by MainScene, supposed to return the day. Used to tell the server to extract the attachment to the player's data
				//isfsobj.PutInt("day",1);
				callback(false, null); // FUCK YOU MY FELLA!
				break;
			// undocumented
			/*
			case "SearchShopItem":
			case "CheckShopItemInfo":
			case "BuyShopItem":
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
			case "GetChaProperty":
			case "KickoutGameRoom":
			case "GetStarReward": // TODO, probably for fuben/campaign levels
			case "PVPAddMoney":
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
