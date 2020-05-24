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

		private string eventTitle = "Pris en embuscade par des bandits";

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
			inquiryElements.Add(new InquiryElement("a", "Leur donnée de l'or pour les faire dispersée", null, true, "À quoi sert l'or, sinon à dissuader les gens de vous tuer?"));
			inquiryElements.Add(new InquiryElement("b", "Les attaquer", null));

			if (Hero.MainHero.PartyBelongedTo.MemberRoster.Count > troopScareCount)
			{
				inquiryElements.Add(new InquiryElement("c", "Les intimider", null)); 
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
						InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Vous donnez aux bandits {goldLost} pièces et ils s'enfuient rapidement. Au moins vous et vos soldats vivrez pour combattre un autre jour.", true, false, "Terminé", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						SpawnBandits(false);
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "Voyant que vous ne reculerez pas, les bandits se préparent pour un combat.", true, false, "Terminé", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "c")
					{
						SpawnBandits(true);
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "Vous riez en regardant le reste de votre groupe émerger de la crête de la colline. Les bandits se préparent à fuir.", true, false, "Terminé", null, null, null), true);
					}
					else
					{
						MessageBox.Show($"Erreur lors de la sélection de l'option pour \"{this.RandomEventData.EventType}\"");
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
				return "Vous voyagez avec votre avant-garde quand vous vous retrouvez encerclé par un groupe de bandits!";
			}
			else
			{
				return "Durant votre voyage, votre groupe est encerclé par un groupe de bandits!";
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

				PartySetup.AddRandomCultureUnits(banditParty, 10 + (int)(MobileParty.MainParty.MemberRoster.TotalManCount * 0.75f));
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
