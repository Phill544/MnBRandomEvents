using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

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

		public static bool HasRangedWeaponEquipped()
		{
			Equipment playerEquipment = Hero.MainHero.BattleEquipment;

			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				ItemObject item = playerEquipment[equipmentIndex].Item;
				if (item != null && (item.Type == ItemObject.ItemTypeEnum.Thrown || 
					item.Type == ItemObject.ItemTypeEnum.Bow || 
					item.Type == ItemObject.ItemTypeEnum.Crossbow))
				{
					return true;
				}
			}

			return false;
		}
	}
}
