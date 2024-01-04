using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Bannerlord.RandomEvents.Helpers
{
	public static class PlayerStatus
	{ 
        public static bool HasRangedWeaponEquipped()
        {
            var playerEquipment = Hero.MainHero.BattleEquipment;

            for (var equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
            {
                var item = playerEquipment[equipmentIndex].Item;
                
                if (item != null && (item.Type == ItemObject.ItemTypeEnum.Thrown || item.Type == ItemObject.ItemTypeEnum.Bow || item.Type == ItemObject.ItemTypeEnum.Crossbow))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
