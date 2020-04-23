using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class BanditAmbush : BaseEvent
	{

		private float moneyMinPercent;
		private float moneyMaxPercent;
		private int lowMoneyThreshold;

		public BanditAmbush()
		{
			this.moneyMinPercent = Settings.RandomEvents.BanditAmbushData.moneyMinPercent;
			this.moneyMaxPercent = Settings.RandomEvents.BanditAmbushData.moneyMaxPercent;
			this.lowMoneyThreshold = Settings.RandomEvents.BanditAmbushData.lowMoneyThreshold;
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
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {Settings.RandomEvents.BanditAmbushData.EventType}", RandomEventsSubmodule.Instance.textColor));
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
				MessageBox.Show($"Error while stopping \"{Settings.RandomEvents.BanditAmbushData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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

		public BanditAmbushData(RandomEventType eventType, float chanceWeight, float moneyMinPercent, float moneyMaxPercent, int lowMoneyThreshold) : base(eventType, chanceWeight)
		{
			this.moneyMinPercent = moneyMinPercent;
			this.moneyMaxPercent = moneyMaxPercent;
			this.lowMoneyThreshold = lowMoneyThreshold;
		}

	}
}
