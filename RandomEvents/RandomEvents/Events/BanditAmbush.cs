using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.TwoDimension;

namespace CryingBuffalo.RandomEvents.Events
{
	public class BanditAmbush : BaseEvent
	{

		private float moneyMinPercent;
		private float moneyMaxPercent;
		private int lowMoneyThreshold;
		private int troopScareCount;

		private string eventTitle = "Ambushed by bandits";

		public BanditAmbush() : base(Settings.RandomEvents.BanditAmbushData)
		{
			this.moneyMinPercent = Settings.RandomEvents.BanditAmbushData.moneyMinPercent;
			this.moneyMaxPercent = Settings.RandomEvents.BanditAmbushData.moneyMaxPercent;
			this.lowMoneyThreshold = Settings.RandomEvents.BanditAmbushData.lowMoneyThreshold;
			this.troopScareCount = Settings.RandomEvents.BanditAmbushData.troopScareCount;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Pay gold to have them leave", null, true, "What is gold good for, if not to dissuade people from killing you?"));
			inquiryElements.Add(new InquiryElement("b", "Attack", null));

			if (Hero.MainHero.PartyBelongedTo.MemberRoster.Count > troopScareCount)
			{
				inquiryElements.Add(new InquiryElement("c", "Intimidate them", null)); 
			}

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				CalculateDescription(), // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						float percentMoneyLost = MBRandom.RandomFloatRanged(moneyMinPercent, moneyMaxPercent);
						int goldLost = (int)Mathf.Floor(Hero.MainHero.Gold * percentMoneyLost);
						Hero.MainHero.ChangeHeroGold(-goldLost);
						InformationManager.ShowInquiry(new InquiryData(eventTitle, $"You give the bandits {goldLost} coins and they quickly flee. At least you and your soldiers live to fight another day.", true, false, "Done", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						SpawnBandits(false);
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "Seeing you won't back down, the bandits get ready for a fight.", true, false, "Done", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "c")
					{
						SpawnBandits(true);
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "You laugh as you watch the rest of your party emerge over the crest of the hill. The bandits get ready to flee.", true, false, "Done", null, null, null), true);
					}
					else
					{
						MessageBox.Show($"Error while selecting option for \"{this.RandomEventData.EventType}\"");
					}
					
				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

			InformationManager.ShowMultiSelectionInquiry(msid, true);

			StopEvent();
		}

		public override void StopEvent()
		{
			try
			{
				OnEventCompleted.Invoke();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while stopping \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private string CalculateDescription()
		{
			if (Hero.MainHero.PartyBelongedTo.MemberRoster.Count > troopScareCount)
			{
				return "You are traveling with your forward party when you get surrounded by a group of bandits!";
			}
			else
			{
				return "While traveling your party gets surrounded by a group of bandits!";
			}
		}

		private void SpawnBandits(bool shouldFlee)
		{
			try
			{
				MobileParty banditParty = PartySetup.CreateBanditParty();

				banditParty.MemberRoster.Clear();

				if (shouldFlee)
				{
					banditParty.Aggressiveness = 0.2f;
				}
				else
				{
					banditParty.Aggressiveness = 10f;
					banditParty.SetMoveEngageParty(MobileParty.MainParty);
				}

				PartySetup.AddRandomCultureUnits(banditParty, 100);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class BanditAmbushData : RandomEventData
	{
		/// <summary>
		/// The min percent the bandits will ask
		/// </summary>
		public float moneyMinPercent;
		/// <summary>
		/// The max percent the bandits will ask
		/// </summary>
		public float moneyMaxPercent;

		/// <summary>
		/// The max amount of goal that the bandits will take pity
		/// </summary>
		public int lowMoneyThreshold;

		/// <summary>
		/// The amount of troops the player needs in order to scare the bandits
		/// </summary>
		public int troopScareCount;

		public BanditAmbushData(string eventType, float chanceWeight, float moneyMinPercent, float moneyMaxPercent, int lowMoneyThreshold, int troopScareCount) : base(eventType, chanceWeight)
		{
			this.moneyMinPercent = moneyMinPercent;
			this.moneyMaxPercent = moneyMaxPercent;
			this.lowMoneyThreshold = lowMoneyThreshold;
			this.troopScareCount = troopScareCount;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BanditAmbush();
		}


	}
}
