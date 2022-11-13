using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
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
            return Hero.MainHero.IsFemale == false;
            
        }

        public override void StartEvent()
        {
            if (MCM_MenuConfig.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
            //PHILL
            //What is needed her is the following
            //Get all heroes and notables in the settlement.
            //Make a new list with characters that are the opposite gender.
            //Then pick a random character that fits.
            //Random variable for pregnancy chance to occur.
            //Event ends with either a NPC gets pregnant or not.
            

            var target = GetTarget();

            var targetCulture = target.Culture.Name.ToString();

            var Demonym = GetDemonym(targetCulture);
            
                var text = $"Your woman is {target.Name}. She is {Math.Round(target.Age)} years old. She is a {Demonym}. Enjoy!";
                

            InformationManager.ShowInquiry(new InquiryData(target.Name.ToString(), text, true, false, "OK", null, null, null), true);


            StopEvent();
        }
        
        private string GetDemonym(string culture)
        {
            string citizenName = culture switch
            {
                "Empire" => "imperial",
                "Vlandia" => "vlandian",
                "Sturgia" => "sturgian",
                "Battania" => "battanian",
                "Aserai" => "aserai",
                "Khuzait" => "khuzait",
                _ => "ERROR_DEMONYM"
            };

            return citizenName;
        }

        private static Hero GetTarget()
        {

            var notables = Settlement.CurrentSettlement.Notables;
            var heroes = Settlement.CurrentSettlement.HeroesWithoutParty;

            var characters = notables.Concat(heroes).Distinct().ToList();

            var femaleList = new List<Hero>();

            foreach (var character in characters)
            {
                if (character.IsFemale)
                {
                    if (!character.IsPregnant)
                    {
                        if (character.Age >= 18 && character.Age <= 50)
                        {
                            femaleList.Add(character);
                        }
                    }
                }
            }

            if (femaleList.Count != 0)
            {
                var random = new Random();
                var index = random.Next(femaleList.Count);

                 var target = femaleList[index];
                
                return target;
            }

            return null;

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