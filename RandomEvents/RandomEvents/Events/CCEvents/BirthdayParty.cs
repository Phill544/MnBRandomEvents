using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class BirthdayParty : BaseEvent
    {
        private readonly int minAttending;
        private readonly int maxAttending;
        private readonly int minYourMenAttending;
        private readonly int maxYourMenAttending;
        private readonly int minAge;
        private readonly int maxAge;
        private readonly int minBandits;
        private readonly int maxBandits;
        private readonly int minGoldGiven;
        private readonly int maxGoldGiven;
        private readonly int minRenownGain;
        private readonly int maxRenownGain;
        private readonly int minGoldLooted;
        private readonly int maxGoldLooted;

        public BirthdayParty() : base(ModSettings.RandomEvents.BirthdayPartyData)
        {
            minAttending = MCM_MenuConfig_A_M.Instance.BP_MinAttending;
            maxAttending = MCM_MenuConfig_A_M.Instance.BP_MaxAttending;
            minYourMenAttending = MCM_MenuConfig_A_M.Instance.BP_MinYourMenAttending;
            maxYourMenAttending = MCM_MenuConfig_A_M.Instance.BP_MaxYourMenAttending;
            minAge = MCM_MenuConfig_A_M.Instance.BP_MinAge;
            maxAge = MCM_MenuConfig_A_M.Instance.BP_MaxAge;
            minBandits = MCM_MenuConfig_A_M.Instance.BP_MinBandits;
            maxBandits = MCM_MenuConfig_A_M.Instance.BP_MaxBandits;
            minGoldGiven = MCM_MenuConfig_A_M.Instance.BP_MinGoldGiven;
            maxGoldGiven = MCM_MenuConfig_A_M.Instance.BP_MaxGoldGiven;
            minRenownGain = MCM_MenuConfig_A_M.Instance.BP_MinRenownGain;
            maxRenownGain = MCM_MenuConfig_A_M.Instance.BP_MaxRenownGain;
            minGoldLooted = MCM_MenuConfig_A_M.Instance.BP_MinGoldLooted;
            maxGoldLooted = MCM_MenuConfig_A_M.Instance.BP_MaxGoldLooted;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_A_M.Instance.BP_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }

            var heroName = Hero.MainHero.FirstName;
            

            var birthdayAge = MBRandom.RandomInt(minAge, maxAge);
            var yourMenAttending = MBRandom.RandomInt(minYourMenAttending, maxYourMenAttending);
            var peopleAttending = MBRandom.RandomInt(minAttending, maxAttending);
            var bandits = MBRandom.RandomInt(minBandits, maxBandits);
            var goldGiven = MBRandom.RandomInt(minGoldGiven, maxGoldGiven);
            var renownGain = MBRandom.RandomInt(minRenownGain, maxRenownGain);
            var goldLooted = MBRandom.RandomInt(minGoldLooted, maxGoldLooted);

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var eventTitle = new TextObject("{=BirthdayParty_Title}The Birthday Party!").ToString();

            var eventDescription = new TextObject(
                "{=BirthdayParty_Event_Desc}As you and your party are traveling in the vicinity of {closestSettlement}, you come across {peopleAttending} " +
                "people in what seems to be a birthday party for a young girl. A couple of the guests invite you to join them in celebrating the girls {birthdayAge} " +
                "birthday! What should you do?")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("peopleAttending", peopleAttending)
                .SetTextVariable("birthdayAge", birthdayAge).ToString();

            var eventOption1 = new TextObject("{=BirthdayParty_Event_Option_1}Join them in celebration!").ToString();
            var eventOption1Hover = new TextObject("{=BirthdayParty_Event_Option_1_Hover}Not everyday you turn {birthdayAge} years old!").SetTextVariable("birthdayAge", birthdayAge).ToString();
            
            var eventOption2 = new TextObject("{=BirthdayParty_Event_Option_2}Give the girls some gold").ToString();
            var eventOption2Hover = new TextObject("{=BirthdayParty_Event_Option_2_Hover}You don't have time to stay but you can still be nice, right?").ToString();
            
            var eventOption3 = new TextObject("{=BirthdayParty_Event_Option_3}Leave").ToString();
            var eventOption3Hover = new TextObject("{=BirthdayParty_Event_Option_3_Hover}Don't have time").ToString();
            
            var eventOption4 = new TextObject("{=BirthdayParty_Event_Option_4}Raid the party").ToString();
            var eventOption4Hover = new TextObject("{=BirthdayParty_Event_Option_4_Hover}Have some fun").ToString();

            var eventButtonText = new TextObject("{=BirthdayParty_Event_Button_Text}Okay").ToString();
            
            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };

            var eventOptionAText = new TextObject(
                "{=BirthdayParty_Event_Choice_1}You and {yourMenAttending} of your men decide to stay for the party while the rest makes their way to {closestSettlement}. " +
                "You approach the girl and give her {goldGiven} gold as a gift. She give you a hug and a thank you. You get yourself some beer and sit down to enjoy the moment.\n \n" +
                "Some time later, {bandits} bandits decide to crash the party. They go around from person to person and takes everything of value. " +
                "You order your men to stand down as you don't want to start a fight with innocents caught in the middle. After they have taken everything of " +
                "value they also try to take the young girl with them. This you will not stand for so you signal your men to strike. " +
                "You and your men make quick work in incapacitating the bandits. One of your men rides to {closestSettlement} to fetch someone to throw these scum in the dungeon. \n" +
                "The rest of the night you are celebrated as a hero! You even got to dance with the birthday girl!")
                .SetTextVariable("yourMenAttending", yourMenAttending)
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("goldGiven", goldGiven)
                .SetTextVariable("bandits", bandits)
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=BirthdayParty_Event_Choice_2}Your really don't have time to stay but you don't want to be rude either. You manage to scrape together {goldGiven} gold and give it to the girl as a gift. " +
                    "She seems grateful. \nYou say your goodbyes to the partygoers and you leave in the direction of {closestSettlement}.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("goldGiven", goldGiven)
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=BirthdayParty_Event_Choice_3}You don't have time for this so you leave for {closestSettlement} but not before casting one last look at the party.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=BirthdayParty_Event_Choice_4}You decide that this is the perfect moment to let loose your somewhat evil side. You order your men you surround the party and you tell every guest to hand over " +
                    "everything of value. They refuse at the beginning but you have one of your men kill a random person. They all fall in line after that and hand over everything. Once you have everything " +
                    "you and your men leave but not before tossing over 1 gold coin to the birthday girl who is clearly very upset. You are left with {goldLooted} gold.")
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=BirthdayParty_Event_Msg_1}{heroName} gave away {goldGiven} to the girl and gained {renownGain} renown for slaying the bandits.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldGiven", goldGiven)
                .SetTextVariable("renownGain", renownGain)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=BirthdayParty_Event_Msg_2}{heroName} gave away {goldGiven} to the girl.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldGiven", goldGiven)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=BirthdayParty_Event_Msg_3}{heroName} looted {goldLooted} from the birthday party.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldGiven", goldGiven)
                .ToString();

            

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            Clan.PlayerClan.AddRenown(renownGain);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
                            break;
                        
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
                            break;
                        
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionCText, true, false, eventButtonText, null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionDText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(+goldGiven);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
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


    public class BirthdayPartyData : RandomEventData
    {
        public BirthdayPartyData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new BirthdayParty();
        }
    }
}