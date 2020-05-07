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
	public class FantasticFighters : BaseEvent
	{
		private int renownGain;

		public FantasticFighters() : base(Settings.RandomEvents.FantasticFightersData)
		{
			renownGain = Settings.RandomEvents.FantasticFightersData.renownGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan != null;
		}

		public override void StartEvent()
		{
			try
			{
				Hero.MainHero.Clan.Renown += renownGain;

				InformationManager.ShowInquiry(
					new InquiryData("Fantastic Fighters?",
						$"A rumor spreads that your clan managed to decisively win a battle when outnumbered 10-1.",
						true,
						false,
						"Done",
						null,
						null,
						null
						),
					true);

				StopEvent();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while playing \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
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
	}

	public class FantasticFightersData : RandomEventData
	{
		public int renownGain;

		public FantasticFightersData(RandomEventType eventType, float chanceWeight, int renownGain) : base(eventType, chanceWeight)
		{
			this.renownGain = renownGain;
		}
	}
}
