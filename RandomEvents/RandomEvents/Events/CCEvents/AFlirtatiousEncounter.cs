using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Library;

//New type of event I will attempt to make once the MCM is in place.
namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class AFlirtatiousEncounter : BaseEvent
    {

        public AFlirtatiousEncounter() : base(ModSettings.RandomEvents.AFlirtatiousEncounterData)
        {
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            //Will need a few conditions to align in order to execute.
            return true;
            
        }

        public override void StartEvent()
        {
            if (MCM_MenuConfig.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
            //PHILL
            //What is needed her is the following
            //Get if the main character is male or female.
            //Get all heroes and notables in the settlement.
            //Make a new list with characters that are the opposite gender.
            //Then pick a random character that fits.
            //Random variable for pregnancy chance to occur.
            //Event ends with either a NPC gets pregnant or not.

            var heroGender = Hero.MainHero.IsFemale;

            var notables = Settlement.CurrentSettlement.Notables;
            var heroes = Settlement.CurrentSettlement.HeroesWithoutParty;

            foreach (var character in notables)
            {
                var name = character.Name;
                var gender = character.IsFemale;

                InformationManager.DisplayMessage(new InformationMessage($"{name}, {gender},", RandomEventsSubmodule.Dbg_Color));
            }

        }

        private void StopEvent()
        {
            try
            {
                onEventCompleted.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }


    public class AFlirtatiousEncounterData : RandomEventData
    {
        public AFlirtatiousEncounterData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new AFlirtatiousEncounter();
        }
    }
}