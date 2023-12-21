using System;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.BicEvents
{
	public sealed class ArmyInvite : BaseEvent
	{
		private readonly bool eventDisabled;

		public ArmyInvite() : base(ModSettings.RandomEvents.ArmyInviteData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("ArmyInvite", "EventDisabled");
			
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			return eventDisabled == false;
		}

		public override bool CanExecuteEvent()
		{
			if (Clan.PlayerClan.Kingdom != null)
			{
				var Lords = Clan.PlayerClan.Kingdom.Lords.ToList();
				var Generals = Lords.Where(lord => lord == lord.PartyBelongedTo?.Army?.ArmyOwner).ToList();
				if (Generals.Count == 0)
					return false;
			}
			
			return HasValidEventData() && Clan.PlayerClan.Kingdom != null && MobileParty.MainParty.Army == null && Clan.PlayerClan.Kingdom.Armies != null;
		}

		public override void StartEvent()
		{
			var Lords = Clan.PlayerClan.Kingdom.Lords.ToList();
			var Generals = Lords.Where(lord => lord == lord.PartyBelongedTo?.Army?.ArmyOwner).ToList();
			var random = new Random();
			var index = random.Next(Generals.Count);
			var ArmyLeader = Generals[index];
			var ArmyLeaderName = ArmyLeader.Name.ToString();

			var LeaderIsFemale = ArmyLeader.IsFemale;
			var LeaderGender = LeaderIsFemale ? "female" : "male";
			var gender = GenderAssignment.GetTheGenderAssignment(LeaderGender, false, "adjective");
			var playerName = Hero.MainHero.Name;

			var settlements = Settlement.FindAll(s => s.IsTown || s.IsCastle || s.IsVillage).ToList();
			var closestSettlement = settlements.MinBy(s => ArmyLeader.GetPosition().DistanceSquared(s.GetPosition()));

			var eventTitle = new TextObject("{=ArmyInvite_Title}Army Invitation").ToString();
			
			var eventOption1 = new TextObject("{=ArmyInvite_Event_Text}A messenger from {ArmyLeader} has arrived inviting {player} to join {gender} army located near {settlement}.")
				.SetTextVariable("ArmyLeader", ArmyLeaderName)
				.SetTextVariable("settlement", closestSettlement.Name)
				.SetTextVariable("gender", gender)
				.SetTextVariable("player", playerName)
				.ToString();
				
			var eventButtonText = new TextObject("{=ArmyInvite_Event_Button_Text}Done").ToString();

			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

			StopEvent();
		}


        private void StopEvent()
		{
			try
			{
				onEventCompleted.Invoke();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class ArmyInviteData : RandomEventData
	{

		public ArmyInviteData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new ArmyInvite();
		}
	}
}
