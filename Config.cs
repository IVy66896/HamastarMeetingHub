using Hamastar.DefaultConfig;
using Hamastar.DefaultEnum;
using MeetingHubLibrary;
using MeetingHubLibrary.Utilities;
using System;
using System.Collections.Generic;

public class Config : MeetingHubLibrary.Config
{
	public override void Init()
	{
		base.Init();
		Hamastar.DefaultConfig.Config.ProjectName = "MeetingHub";
		Hamastar.DefaultConfig.Config.HostURL = "http://meetinghub.hamastar.com.tw/";
		Hamastar.DefaultConfig.Config.SyncServerDemain = "meetinghub.hamastar.com.tw";
		Hamastar.DefaultConfig.Config.SyncServerPort = 4503;
		Hamastar.DefaultConfig.Config.SyncVersion = 1.6;
		Hamastar.DefaultConfig.Config.AppVersion = "1.4.1";
		MeetingHubLibrary.Config.IsAutoDeleteMeetingFile = true;
		MeetingHubLibrary.Config.IconName = "Meetinghub.ico";
		MeetingHubLibrary.Config.LogoName = "MeetingHubLogo.png";
		Hamastar.DefaultConfig.Config.RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + Hamastar.DefaultConfig.Config.ProjectName + "\\";
		Hamastar.DefaultConfig.Config.EnableSyncInterrupt = true;
		Hamastar.DefaultConfig.Config.IsEnforceSync = false;
		MeetingHubLibrary.Config.loginButtonSet = "0;1;2;3;4;8;10;5";
		MeetingHubLibrary.Config.guestLoginButtonSet = "0;1;2;3;4;5";
		MeetingHubLibrary.Config.ShowLoginInfo = false;
		Hamastar.DefaultConfig.Config.Language = Hamastar.DefaultEnum.Enums.Language.zh_TW;
		List<string> list = new List<string>();
		list.Add("zh-Hans");
		list.Add("zh-Hant");
		list.Add("en-US");
		Hamastar.DefaultConfig.Config.SupportLanguage = list;
		Hamastar.DefaultConfig.Config.NoteUploadUseFileName = false;
		Hamastar.DefaultConfig.Config.BookType = Hamastar.DefaultEnum.Enums.BookType.m;
		Hamastar.DefaultConfig.Config.IsMaskSyncUserID = false;
		MeetingHubLibrary.Config.UseWebsiteMeetingRoomData = true;
		MeetingHubConfigLoader meetingHubConfigLoader = new MeetingHubConfigLoader();
		meetingHubConfigLoader.LoadConfig(Hamastar.DefaultConfig.Config.ProcessPath + "config.xml");
	}
}
