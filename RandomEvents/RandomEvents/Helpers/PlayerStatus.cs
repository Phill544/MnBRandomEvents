using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Helpers
{
	public static class PlayerStatus
	{
		public static bool HasSettlement()
		{
			return Hero.MainHero.Clan.Settlements.Any();
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
