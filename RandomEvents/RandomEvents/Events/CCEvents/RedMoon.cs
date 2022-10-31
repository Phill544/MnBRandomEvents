using System;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
	public sealed class RedMoon : BaseEvent
	{
		private const string EventTitle = "A Coming Apocalypse";

		private readonly int minGoldLost;
		private readonly int maxGoldLost;

		public RedMoon() : base(Settings.ModSettings.RandomEvents.RedMoonData)
		{
			minGoldLost = Settings.ModSettings.RandomEvents.RedMoonData.minGoldLost;
			maxGoldLost = Settings.ModSettings.RandomEvents.RedMoonData.maxGoldLost;
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

			var goldLostToReligion = MBRandom.RandomInt(minGoldLost, maxGoldLost);
			
			var settlements = Settlement.FindAll(s => s.IsTown || s.IsVillage ).ToList();
			var closestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

			InformationManager.ShowInquiry(
				new InquiryData(EventTitle,
					"You are in your tent late one night when you hear your men starting a commotion. Annoyed, you go out and tell them to quiet down but as soon as you step foot outside your tent you see what the commotion is about. " +
					"The moon has become blood red. As you gaze up on the moon you fall to your knees and start praying to the gods. Several of your men join you in prayer. \n \n" +
					$"After praying for almost 10 minutes you realize that this won't help. You order your men to give you {goldLostToReligion} gold that you will rush to the nearest chapel. Hopefully the priests can help you. You mount your steed and ride of. " +
					$"You ride like a madman towards {closestSettlement} as you know the settlement has a chapel. As your steed jumps over a fence you fall off and black out.\n" +
					"When you wake up it's morning. The chest of gold you had with you is gone, but you are alive. You make your way back to camp.",
					true,
					false,
					"Done",
					null,
					null,
					null
					),
				true);

			Hero.MainHero.ChangeHeroGold(-goldLostToReligion);
			
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
	

	public class RedMoonData : RandomEventData
	{
		public readonly int minGoldLost;
		public readonly int maxGoldLost;

		public RedMoonData(string eventType, float chanceWeight, int minGoldLost, int maxGoldLost) : base(eventType, chanceWeight)
		{
			this.minGoldLost = minGoldLost;
			this.maxGoldLost = maxGoldLost;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new RedMoon();
		}
	}
}
