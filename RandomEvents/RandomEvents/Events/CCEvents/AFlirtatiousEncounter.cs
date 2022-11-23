using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class AFlirtatiousEncounter : BaseEvent
    {
        private readonly int minWomanAge;
        private readonly int maxWomanAge;
        private readonly float minRelationshipIncrease;
        private readonly float maxRelationshipIncrease;

        public AFlirtatiousEncounter() : base(ModSettings.RandomEvents.AFlirtatiousEncounterData)
        {
            minWomanAge = MCM_MenuConfig_A_M.Instance.AFE_minWomanAge;
            maxWomanAge = MCM_MenuConfig_A_M.Instance.AFE_maxWomanAge;
            minRelationshipIncrease = MCM_MenuConfig_A_M.Instance.AFE_minRelationshipIncrease;
            maxRelationshipIncrease = MCM_MenuConfig_A_M.Instance.AFE_maxRelationshipIncrease;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_A_M.Instance.AFE_Disable == false && 
                   Hero.MainHero.IsFemale == false && 
                   Settlement.CurrentSettlement != null && 
                   (Settlement.CurrentSettlement.IsTown || Settlement.CurrentSettlement.IsVillage);
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.Dbg_Color));
            }


            var eventTitle = new TextObject("{=AFlirtatiousEncounter_Title}A Flirtatious Encounter").ToString();

            var notables = Settlement.CurrentSettlement.Notables;
            var heroes = Settlement.CurrentSettlement.HeroesWithoutParty;

            var characters = notables.Concat(heroes).Distinct().ToList();

            var femaleList = characters.Where(character => character.IsFemale).Where(character => character.Age >= minWomanAge && character.Age <= maxWomanAge).ToList();

            if (femaleList.Count != 0)
            {
                var random = new Random();
                var index = random.Next(femaleList.Count);

                var target = femaleList[index];

                var currentRelationship = target.GetRelation(Hero.MainHero);

                var relationshipGainPercent = MBRandom.RandomFloatRanged(minRelationshipIncrease, maxRelationshipIncrease);

                var newRelationship = (int)Math.Round(Math.Floor(currentRelationship * relationshipGainPercent));

                var targetCulture = target.Culture.Name.ToString();

                var currentSettlement = Settlement.CurrentSettlement.Name.ToString();

                var theDemonym = Demonym.GetTheDemonym(targetCulture, true);

                var eventDescription = new TextObject(
                        "{=AFlirtatiousEncounter_Event_Desc}This evening you are enjoying a drink in the tavern at " +
                        "{currentSettlement}, just relaxing. You take a look at the various guest in the tavern. There " +
                        "is quite a diverse mix of people here tonight. Your eyes suddenly lock eyes with {Demonym} " +
                        "woman who is standing across the room. She smiles back when you notice her and starts to make " +
                        "her way over to you. How shall you proceed?")
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("Demonym", theDemonym)
                    .ToString();

                var eventOption1 = new TextObject("{=AFlirtatiousEncounter_Event_Option_1}Strike up a conversation").ToString();
                var eventOption1Hover = new TextObject("{=AFlirtatiousEncounter_Event_Option_1_Hover}Have a nice chat with her").ToString();

                var eventOption2 = new TextObject("{=AFlirtatiousEncounter_Event_Option_2}Buy her a drink").ToString();
                var eventOption2Hover = new TextObject("{=AFlirtatiousEncounter_Event_Option_2_Hover}Always a gentleman.").ToString();

                var eventOption3 = new TextObject("{=AFlirtatiousEncounter_Event_Option_3}Hit on her").ToString();
                var eventOption3Hover = new TextObject("{=AFlirtatiousEncounter_Event_Option_3_Hover}She's cute!").ToString();

                var eventOption4 = new TextObject("{=AFlirtatiousEncounter_Event_Option_4}Be an ass").ToString();
                var eventOption4Hover = new TextObject("{=AFlirtatiousEncounter_Event_Option_4_Hover}Seriously?").ToString();

                var eventButtonText1 = new TextObject("{=AFlirtatiousEncounter_Event_Button_Text_1}Choose").ToString();
                var eventButtonText2 = new TextObject("{=AFlirtatiousEncounter_Event_Button_Text_2}Done").ToString();

                var inquiryElements = new List<InquiryElement>
                {
                    new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                    new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                    new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                    new InquiryElement("d", eventOption4, null, true, eventOption4Hover),
                };


                var eventOptionAText = new TextObject(
                        "{=AFlirtatiousEncounter_Event_Choice_1}When she gets over to you the two of you start talking. " +
                        "You learn that her name is {name} and she's {age} years old. Both of you end up sitting and " +
                        "chatting for a few hours but eventually you both go your separate ways, but not before sharing " +
                        "a warming hug. As you watch her walk away into the night you are left wondering if you'll ever see her again.")
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .ToString();

                var eventOptionBText = new TextObject(
                        "{=AFlirtatiousEncounter_Event_Choice_2}When she gets over to you the two of you starts talking. " +
                        "You offer to buy her a drink which she accepts. As the evening progresses the two of you get " +
                        "more and more drunk. You manage to learn that her name is {name} and she's {age} years old. " +
                        "Eventually you are both so drunk that you are thrown out of the bar.\n\nBoth of you giggle as " +
                        "you wobble down the streets of {currentSettlement} while singing songs. Eventually the alcohol " +
                        "starts wearing off and you both decide it was a lovely night. You give each other a tender hug " +
                        "before you go your separate ways. After a few seconds you hear your name being called. " +
                        "“{heroName} wait!” You turn around and see {name} running back to you. She stops just a few " +
                        "inches from you face and gives you a deep and tender kiss on the lips. “That was for a lovely " +
                        "evening” she said. You kiss her back before saying you final goodbye for the night.")
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var eventOptionCText = new TextObject(
                        "{=AFlirtatiousEncounter_Event_Choice_3}When she gets over to you the two of you starts talking. " +
                        "You tell her that you don't often get noticed by such beautiful women. She says it's not often " +
                        "she find such handsome men in town. With the tone being set for the event the two of you " +
                        "continue to talk (and naturally to hit on each other as well...) She tells you that her name " +
                        "is {name} and she's {age} years old. After a few minutes the two of you move away from the bar " +
                        "and go somewhere a bit more comfortable. There you both continue your flirtatious behaviour. It " +
                        "does not take long before the two of you start kissing each other.\n\n You kiss her on the neck, " +
                        "on the cheeks, on the lips and so on. She whispers to you “I know a more private place...” with " +
                        "fire in her eyes. Wanna head there she asks. You tell her to lead the way. She takes your hand " +
                        "and leads you out into the street. After a few minutes of walking you arrive at what is clearly " +
                        "{name}'s home in {currentSettlement}. As you enter her house she turns to you and start kissing " +
                        "you passionately again with more lust that you can ever remember. After just a few seconds all " +
                        "your clothes have been removed and you make your way towards {name}'s bed. \n\nThe rest of the " +
                        "night fades out in ecstasy.")
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .ToString();

                var eventOptionDText = new TextObject(
                        "{=AFlirtatiousEncounter_Event_Choice_4}You cannot help but be a little annoyed as you'd rather " +
                        "drink in peace. When she gets over to you she introduces herself as {name}. It does not take " +
                        "long for you to have insulted her enough that she leaves you alone. You may have hurt her " +
                        "feelings but at least  you are able to enjoy the night alone. A few hours later as you are " +
                        "about to leave you come across {name} again. You tell her that you're sorry for what you said " +
                        "and you didn't mean it as you just wanted to be left alone. She somehow manages to politely " +
                        "and kindly tell you to F**k off.")
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();
                
                var eventMsg1 =new TextObject(
                        "{=AFlirtatiousEncounter_Event_Msg_1}Relationship between {name} and {heroName}is now {newRelationship}.")
                    .SetTextVariable("newRelationship", newRelationship - 5)
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();
                
                var eventMsg2 =new TextObject(
                        "{=AFlirtatiousEncounter_Event_Msg_1}Relationship between {name} and {heroName}is now {newRelationship}.")
                    .SetTextVariable("newRelationship", newRelationship - 3)
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();
                
                var eventMsg3 =new TextObject(
                        "{=AFlirtatiousEncounter_Event_Msg_1}Relationship between {name} and {heroName}is now {newRelationship}.")
                    .SetTextVariable("newRelationship", newRelationship)
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();
                
                var eventMsg4 =new TextObject(
                        "{=AFlirtatiousEncounter_Event_Msg_1}Relationship between {name} and {heroName}is now {newRelationship}.")
                    .SetTextVariable("newRelationship", newRelationship - 30)
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1,
                    eventButtonText1, null,
                    elements =>
                    {
                        switch ((string)elements[0].Identifier)
                        {
                            case "a":
                                InformationManager.ShowInquiry(
                                    new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null,
                                        null, null), true);
                                CharacterRelationManager.SetHeroRelation(target,Hero.MainHero, newRelationship - 5);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
                                break;
                            case "b":
                                InformationManager.ShowInquiry(
                                    new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null,
                                        null, null), true);
                                
                                CharacterRelationManager.SetHeroRelation(target,Hero.MainHero, newRelationship - 3);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
                                break;
                            case "c":
                                InformationManager.ShowInquiry(
                                    new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null,
                                        null, null), true);
                                
                                CharacterRelationManager.SetHeroRelation(target,Hero.MainHero, newRelationship);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
                                
                                break;
                            case "d":
                                InformationManager.ShowInquiry(
                                    new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null,
                                        null, null), true);
                                
                                CharacterRelationManager.SetHeroRelation(target,Hero.MainHero, newRelationship - 30);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
                                
                                break;
                            default:
                                MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                                break;
                        }
                    },
                    null);

                MBInformationManager.ShowMultiSelectionInquiry(msid, true);
                
                StopEvent();
            }
            StopEvent();
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