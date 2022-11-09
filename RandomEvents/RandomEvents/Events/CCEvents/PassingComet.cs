using System;
using System.Windows;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
	public sealed class PassingComet : BaseEvent
	{
		public PassingComet() : base(Settings.ModSettings.RandomEvents.PassingCometData)
		{
			
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return true;
		}

		public override void StartEvent()
		{
			
			if (Settings.ModSettings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			var eventTitle = new TextObject("{=PassingComet_Title}A Celestial Visitor").ToString();
			
			var eventText =new TextObject(
					"{=PassingComet_Event_Text}You and some of your men are standing in a field at night gazing up at a comet. It is one of the most beautiful sights you have ever seen. " +
					"You cannot help wondering what it really is. You have always been fascinated by the stars and night sky. Most people believe it's the gods looking down on us, but you think otherwise.\n \n" +
					"You have always thought that the stars are nothing more than the same thing as the Sun just much further away. You have never shared this thought with anyone " +
					"as most would think you to be crazy. At least for now you can be fascinated by the amazing comet passing by. You and a couple of your men end up standing there " +
					"all night looking up and talking.")
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
