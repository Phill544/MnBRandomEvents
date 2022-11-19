using System;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Helpers
{
    public static class ClosestSettlements
    {
        
        public static Settlement GetClosestAny(MobileParty heroParty)
        {
            Settlement closestSettlement = null;
            
            try
            {
                var settlements = Settlement.FindAll(s => s.IsTown || s.IsCastle || s.IsVillage).ToList();
                closestSettlement = settlements.MinBy(s => heroParty.GetPosition().DistanceSquared(s.GetPosition()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to find the closest settlement :\n\n {ex.Message} \n\n { ex.StackTrace}");
            }
            return closestSettlement;
        }
        
        
        public static Settlement GetClosestTown(MobileParty heroParty)
        {
            Settlement closestSettlement = null;
            
            try
            {
                var settlements = Settlement.FindAll(s => s.IsTown).ToList();
                closestSettlement = settlements.MinBy(s => heroParty.GetPosition().DistanceSquared(s.GetPosition()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to find the closest town :\n\n {ex.Message} \n\n { ex.StackTrace}");
            }
            return closestSettlement;
        }
        
        
        public static Settlement GetClosestCastle(MobileParty heroParty)
        {
            Settlement closestSettlement = null;
            
            try
            {
                var settlements = Settlement.FindAll(s => s.IsCastle).ToList();
                closestSettlement = settlements.MinBy(s => heroParty.GetPosition().DistanceSquared(s.GetPosition()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to find the closest castle :\n\n {ex.Message} \n\n { ex.StackTrace}");
            }
            return closestSettlement;
        }
        
        
        public static Settlement GetClosestVillage(MobileParty heroParty)
        {
            Settlement closestSettlement = null;
            
            try
            {
                var settlements = Settlement.FindAll(s => s.IsVillage).ToList();
                closestSettlement = settlements.MinBy(s => heroParty.GetPosition().DistanceSquared(s.GetPosition()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to find the closest village :\n\n {ex.Message} \n\n { ex.StackTrace}");
            }
            return closestSettlement;
        }
        
        
        public static Settlement GetClosestTownOrVillage(MobileParty heroParty)
        {
            Settlement closestSettlement = null;
            
            try
            {
                var settlements = Settlement.FindAll(s => s.IsTown || s.IsVillage).ToList();
                closestSettlement = settlements.MinBy(s => heroParty.GetPosition().DistanceSquared(s.GetPosition()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to find the closest town or village :\n\n {ex.Message} \n\n { ex.StackTrace}");
            }
            return closestSettlement;
        }

    }
}