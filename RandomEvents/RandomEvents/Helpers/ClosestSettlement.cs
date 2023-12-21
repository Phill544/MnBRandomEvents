using System;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace Bannerlord.RandomEvents.Helpers
{
    public static class ClosestSettlements
    {
        private static Settlement GetClosestSettlement(MobileParty heroParty, Func<Settlement, bool> condition)
        {
            try
            {
                var settlements = Settlement.FindAll(condition).ToList();
                return settlements.MinBy(settlement => heroParty.GetPosition().DistanceSquared(settlement.GetPosition()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to find the closest settlement :\n\n {ex.Message} \n\n {ex.StackTrace}");
                return null;
            }
        }

        public static Settlement GetClosestAny(MobileParty heroParty)
        {
            return GetClosestSettlement(heroParty, settlement => settlement.IsTown || settlement.IsCastle || settlement.IsVillage);
        }

        public static Settlement GetClosestTown(MobileParty heroParty)
        {
            return GetClosestSettlement(heroParty, settlement => settlement.IsTown);
        }

        public static Settlement GetClosestCastle(MobileParty heroParty)
        {
            return GetClosestSettlement(heroParty, settlement => settlement.IsCastle);
        }

        public static Settlement GetClosestVillage(MobileParty heroParty)
        {
            return GetClosestSettlement(heroParty, settlement => settlement.IsVillage);
        }

        public static Settlement GetClosestTownOrVillage(MobileParty heroParty)
        {
            return GetClosestSettlement(heroParty, settlement => settlement.IsTown || settlement.IsVillage);
        }
    }
}