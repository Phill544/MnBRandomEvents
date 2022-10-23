using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class BanditAmbush : BaseEvent
	{

		private readonly float moneyMinPercent;
		private readonly float moneyMaxPercent;
		private readonly int troopScareCount;
		private readonly int banditCap;

		private const string EventTitle = "Ambushed by bandits";

		public BanditAmbush() : base(Settings.Settings.RandomEvents.BanditAmbushData)
		{
			moneyMinPercent = Settings.Settings.RandomEvents.BanditAmbushData.moneyMinPercent;
			moneyMaxPercent = Settings.Settings.RandomEvents.BanditAmbushData.moneyMaxPercent;
			troopScareCount = Settings.Settings.RandomEvents.BanditAmbushData.troopScareCount;
			banditCap = Settings.Settings.RandomEvents.BanditAmbushData.banditCap;
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
			if (Settings.Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", "Pay gold to have them leave", null, true, "What is gold good for, if not to dissuade people from killing you?"),
				new InquiryElement("b", "Attack", null)
			};

			if (Hero.MainHero.PartyBelongedTo.MemberRoster.TotalHealthyCount > troopScareCount)
			{
				inquiryElements.Add(new InquiryElement("c", "Intimidate them", null)); 
			}

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				CalculateDescription(), // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				elements => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
						{
							float percentMoneyLost = MBRandom.RandomFloatRanged(moneyMinPercent, moneyMaxPercent);
							int goldLost = MathF.Floor(Hero.MainHero.Gold * percentMoneyLost);
							Hero.MainHero.ChangeHeroGold(-goldLost);
							InformationManager.ShowInquiry(new InquiryData(EventTitle, $"You give the bandits {goldLost} coins and they quickly flee. At least you and your soldiers live to fight another day.", true, false, "Done", null, null, null), true);
							break;
						}
						case "b":
							SpawnBandits(false);
							InformationManager.ShowInquiry(new InquiryData(EventTitle, "Seeing you won't back down, the bandits get ready for a fight.", true, false, "Done", null, null, null), true);
							break;
						case "c":
							SpawnBandits(true);
							InformationManager.ShowInquiry(new InquiryData(EventTitle, "You laugh as you watch the rest of your party emerge over the crest of the hill. The bandits get ready to flee.", true, false, "Done", null, null, null), true);
							break;
						default:
							MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
							break;
					}
				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

			MBInformationManager.ShowMultiSelectionInquiry(msid, true);

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

		private string CalculateDescription()
		{
			return Hero.MainHero.PartyBelongedTo.MemberRoster.Count > troopScareCount ? "You are traveling with your forward party when you get surrounded by a group of bandits!" : "While traveling your party gets surrounded by a group of bandits!";
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

				int numberToSpawn = Math.Min((int)(MobileParty.MainParty.MemberRoster.TotalManCount * 0.50f), banditCap);

				PartySetup.AddRandomCultureUnits(banditParty, 10 + numberToSpawn);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class BanditAmbushData : RandomEventData
	{
		/// <summary>
		/// The min percent the bandits will ask
		/// </summary>
		public readonly float moneyMinPercent;
		/// <summary>
		/// The max percent the bandits will ask
		/// </summary>
		public readonly float moneyMaxPercent;

		/// <summary>
		/// The amount of troops the player needs in order to scare the bandits
		/// </summary>
		public readonly int troopScareCount;

		/// <summary>
		///  The maximum amount of bandits that can spawn
		/// </summary>
		public readonly int banditCap;

		public BanditAmbushData(string eventType, float chanceWeight, float moneyMinPercent, float moneyMaxPercent, int troopScareCount, int banditCap) : base(eventType, chanceWeight)
		{
			this.moneyMinPercent = moneyMinPercent;
			this.moneyMaxPercent = moneyMaxPercent;
			this.troopScareCount = troopScareCount;
			this.banditCap = banditCap;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BanditAmbush();
		}


	}
}
