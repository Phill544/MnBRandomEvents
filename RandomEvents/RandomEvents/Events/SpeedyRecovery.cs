﻿using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class SpeedyRecovery : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minTroopsToHeal;
		private readonly int maxTroopsToHeal;

		public SpeedyRecovery() : base(ModSettings.RandomEvents.SpeedyRecoveryData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheFile());
            
			eventDisabled = ConfigFile.ReadBoolean("SpeedyRecovery", "EventDisabled");
			minTroopsToHeal = ConfigFile.ReadInteger("SpeedyRecovery", "MinTroopsToHeal");
			maxTroopsToHeal = ConfigFile.ReadInteger("SpeedyRecovery", "MaxTroopsToHeal");
		}

		public override void CancelEvent()
		{
		}
		
		private bool EventCanRun()
		{
			if (eventDisabled == false)
			{
				if (minTroopsToHeal != 0 || maxTroopsToHeal != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return  EventCanRun() && MobileParty.MainParty.MemberRoster.TotalWoundedRegulars >= minTroopsToHeal;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			try
			{
				var totalToHeal = MBRandom.RandomInt(minTroopsToHeal, Math.Min(maxTroopsToHeal, MobileParty.MainParty.MemberRoster.TotalWoundedRegulars));
				
				var totalHealed = 0;

				while (totalHealed < totalToHeal)
				{
					var randomElement = MBRandom.RandomInt(MobileParty.MainParty.MemberRoster.Count);
					
					while (MobileParty.MainParty.MemberRoster.GetElementWoundedNumber(randomElement) == 0 || !MobileParty.MainParty.MemberRoster.GetCharacterAtIndex(randomElement).IsRegular)
					{
						randomElement++;

						if (randomElement == MobileParty.MainParty.MemberRoster.Count)
						{
							randomElement = 0;
						}
					}

					MobileParty.MainParty.MemberRoster.AddToCountsAtIndex(randomElement, 0, -1);
					totalHealed++;
				}
				
				var eventTitle = new TextObject("{=SpeedyRecovery_Title}Speedy Recovery!").ToString();
			
				var eventOption1 = new TextObject("{=SpeedyRecovery_Event_Text}You receive word that a group of your troops are feeling better, and are ready for combat.")
					.ToString();
				
				var eventButtonText = new TextObject("{=SpeedyRecovery_Event_Button_Text}Done").ToString();

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
	

	public class SpeedyRecoveryData : RandomEventData
	{
		public SpeedyRecoveryData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SpeedyRecovery();
		}
	}
}
