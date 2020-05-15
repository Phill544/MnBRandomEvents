using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace CryingBuffalo.RandomEvents.Helpers
{
	public static class PlayerStatus
	{
		/// <summary>
		/// Checks whether the player currently owns any settlements.
		/// </summary>
		public static bool HasSettlement()
		{
			if (Hero.MainHero.Clan.Settlements.Count() > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
