using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
	public sealed class PassingComet : BaseEvent
	{
		private const string EventTitle = "A Celestial Visitor";
		
		private readonly int moraleGained;
		
		public PassingComet() : base(Settings.ModSettings.RandomEvents.PassingCometData)
		{
			moraleGained = Settings.ModSettings.RandomEvents.PassingCometData.moraleGained;
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


			InformationManager.ShowInquiry(
				new InquiryData(EventTitle,
					"You and some of your men are standing in a field at night gazing up at a comet. It is one of the most beautiful sights you have ever seen. You cannot help wondering what it really is. You have always been fascinated " +
					"by the stars and night sky. Most people believe it's the gods looking down on us. But you think otherwise.\n \n" +
					"You have always thought that the stars are nothing more than the same thing as the Sun just much further away. You have never shared this thought with anyone as most would think you to be crazy. At least for now you " +
					"can be fascinated by the amazing comet passing by. You and a couple of your men end up standing there all night looking up and talking. This evening brought you closer with your men who have.",
					true,
					false,
					"Done",
					null,
					null,
					null
					),
				true);

			MobileParty.MainParty.RecentEventsMorale += moraleGained;
			MobileParty.MainParty.MoraleExplained.Add(moraleGained, new TaleWorlds.Localization.TextObject("Random Event"));
			
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
		public readonly int moraleGained;

		public PassingCometData(string eventType, float chanceWeight, int moraleGained) : base(eventType, chanceWeight)
		{
			this.moraleGained = moraleGained;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new PassingComet();
		}
	}
}
