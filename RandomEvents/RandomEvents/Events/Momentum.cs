using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class Momentum : BaseEvent
	{
		public Momentum() : base(Settings.RandomEvents.MomentumData)
		{
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
			bool isOnFoot = Hero.MainHero.CharacterObject.Equipment.Horse.IsEmpty;

			string dialogue = "";

			if (isOnFoot)
			{
				float xpToGive = Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Athletics);
				Hero.MainHero.AddSkillXp(DefaultSkills.Athletics, xpToGive);
				dialogue = "à pied";
			}
			else
			{
				float xpToGive = Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Riding);
				Hero.MainHero.AddSkillXp(DefaultSkills.Riding, xpToGive);
				dialogue = "à cheval";
			}

			InformationManager.ShowInquiry(
				new InquiryData("Momentum",
					$"Après avoir passé tellement de temps {dialogue}, vous gagnez un second souffle!",
					true,
					false,
					"Terminé",
					null,
					null,
					null
					),
				true);

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
				MessageBox.Show($"Erreur lors de l'arrêt \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class MomentumData : RandomEventData
	{
		public MomentumData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new Momentum();
		}
	}
}
