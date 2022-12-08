﻿using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
	public sealed class PassingComet : BaseEvent
	{
		public PassingComet() : base(ModSettings.RandomEvents.PassingCometData)
		{
			
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_P_Z.Instance.PC_Disable == false && MobileParty.MainParty.CurrentSettlement == null && CurrentTimeOfDay.IsNight;
		}

		public override void StartEvent()
		{
			
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			var eventTitle = new TextObject("{=PassingComet_Title}The Celestial Visitor").ToString();
			
			var eventText =new TextObject(
					"{=PassingComet_Event_Text}You and some of your men are standing in a field at night gazing up at a" +
					" comet. It is one of the most beautiful sights you have ever seen. You cannot help wondering what " +
					"it really is. You have always been fascinated by the stars and night sky. Most people believe it's " +
					"the gods looking down on us, but you think otherwise.\n\nYou have often wondered if the stars " +
					"are the same thing as the Sun just much further away... Or perhaps they're angels of our fallen " +
					"ancestors watching over us. You have never shared these thoughts with anyone as most would think " +
					"you to be crazy. At least for now you can be fascinated by the amazing comet passing by. You and a " +
					"couple of your men end up standing there all night looking up and talking.")
				.ToString();
			
			var eventButtonText = new TextObject("{=PassingComet_Event_Button_Text}Done")
				.ToString();
			
			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText, true, false, eventButtonText, null, null, null), true);

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
	

	public class PassingCometData : RandomEventData
	{
		public PassingCometData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new PassingComet();
		}
	}
}
