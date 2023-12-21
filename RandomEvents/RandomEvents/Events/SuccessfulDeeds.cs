using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class SuccessfulDeeds : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minInfluenceGain;
		private readonly int maxInfluenceGain;

		public SuccessfulDeeds() : base(ModSettings.RandomEvents.SuccessfulDeedsData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("SuccessfulDeeds", "EventDisabled");
			minInfluenceGain = ConfigFile.ReadInteger("SuccessfulDeeds", "MinInfluenceGain");
			maxInfluenceGain = ConfigFile.ReadInteger("SuccessfulDeeds", "MaxInfluenceGain");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minInfluenceGain != 0 || maxInfluenceGain != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && Hero.MainHero.Clan.Kingdom != null;
		}

		public override void StartEvent()
		{
			try
			{

				var influenceGain = MBRandom.RandomInt(minInfluenceGain,maxInfluenceGain);
				
				Hero.MainHero.AddInfluenceWithKingdom(influenceGain);
				
				var eventTitle = new TextObject("{=SuccessfulDeeds_Title}Successful Deeds!").ToString();
			
				var eventOption1 = new TextObject("{=SuccessfulDeeds_Event_Text}Some of your deeds have reached other members of the kingdom.")
					.ToString();
				
				var eventButtonText = new TextObject("{=SuccessfulDeeds_Event_Button_Text}Done").ToString();

				InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

				StopEvent();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while playing \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
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

	public class SuccessfulDeedsData : RandomEventData
	{

		public SuccessfulDeedsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SuccessfulDeeds();
		}
	}
}
