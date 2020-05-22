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
	public class SecretSinger : BaseEvent
	{
		private int moraleGain;

		public SecretSinger() : base(Settings.RandomEvents.SecretSingerData)
		{
			moraleGain = Settings.RandomEvents.SecretSingerData.moraleGain;
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
			try
			{
				MobileParty.MainParty.RecentEventsMorale += moraleGain;
				MobileParty.MainParty.MoraleExplainer.AddLine("Random Event", moraleGain, StatExplainer.OperationType.Custom);

				string currentTestString = "nope :(";

				if (SaveSystem.TryGetData("SecretSinger_ValueTest", out object rawStringObject))
				{
					currentTestString = (string)rawStringObject;
					SaveSystem.UpdateData("SecretSinger_ValueTest", currentTestString += "Updated!");
				}
				else
				{
					SaveSystem.AddData("SecretSinger_ValueTest", "Test");
				}

				ExtraSecretData esd = new ExtraSecretData();

				esd.boolValue = false;
				esd.chr = '-';
				esd.counter = 0;
				esd.name = "Default";

				if (SaveSystem.TryGetData("SecretSinger_ESD", out object rawEsdObject))
				{
					esd.boolValue = true;
					esd.chr = '+';
					esd.counter = 1000;
					esd.name = "Winner!";
					SaveSystem.UpdateData("SecretSinger_ESD", esd);
				}
				else
				{
					SaveSystem.AddData("SecretSinger_ESD", esd);
				}


				InformationManager.ShowInquiry(
					new InquiryData("Secret Singer!",
						$"You discover one of your party members is an extremely good singer! \nLoaded string: {currentTestString}\nExtra Data:\n{esd.ToString()}",
						true,
						false,
						"Done",
						null,
						null,
						null
						),
					true);
			}
			catch (Exception)
			{
			}

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
	}

	[Serializable]
	class ExtraSecretData
	{
		public bool boolValue;
		public uint counter;
		public string name;
		public char chr;

		public override string ToString()
		{
			return $"bool: {boolValue}\ncounter{counter}\nname{name}\nchar{chr}";
		}
	}

	public class SecretSingerData : RandomEventData
	{
		public int moraleGain;

		public SecretSingerData(string eventType, float chanceWeight, int moraleGain) : base(eventType, chanceWeight)
		{
			this.moraleGain = moraleGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SecretSinger();
		}
	}
}
