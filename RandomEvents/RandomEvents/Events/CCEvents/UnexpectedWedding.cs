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
    public class UnexpectedWedding : BaseEvent
    {
        private readonly int minGoldToDonate;
        private readonly int maxGoldToDonate;
        private readonly int minPeopleInWedding;
        private readonly int maxPeopleInWedding;
        private readonly int embarrassedSoliderMaxGold;
        private readonly int minGoldRaided;
        private readonly int maxGoldRaided;

        public UnexpectedWedding() : base(ModSettings.RandomEvents.UnexpectedWeddingData)
        {
            minGoldToDonate = MCM_MenuConfig_P_Z.Instance.UW_MinGoldToDonate;
            maxGoldToDonate = MCM_MenuConfig_P_Z.Instance.UW_MaxGoldToDonate;
            minPeopleInWedding = MCM_MenuConfig_P_Z.Instance.UW_MinPeopleInWedding;
            maxPeopleInWedding = MCM_MenuConfig_P_Z.Instance.UW_MaxPeopleInWedding;
            embarrassedSoliderMaxGold = MCM_MenuConfig_P_Z.Instance.UW_EmbarrassedSoliderMaxGold;
            minGoldRaided = MCM_MenuConfig_P_Z.Instance.UW_MinGoldRaided;
            maxGoldRaided = MCM_MenuConfig_P_Z.Instance.UW_MaxGoldRaided;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_P_Z.Instance.UW_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
            var eventTitle = new TextObject("{=UnexpectedWedding_Title}An Unexpected Wedding").ToString();
            
            var goldToDonate = MBRandom.RandomInt(minGoldToDonate, maxGoldToDonate);
            
            var peopleInWedding = MBRandom.RandomInt(minPeopleInWedding, maxPeopleInWedding);
            
            var partyFood = MobileParty.MainParty.TotalFoodAtInventory;

            var raidedGold = MBRandom.RandomInt(minGoldRaided, maxGoldRaided);
            
            var embarrassedSoliderGold = MBRandom.RandomInt(10, embarrassedSoliderMaxGold);
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var eventDescription = new TextObject(
                    "{=UnexpectedWedding_Event_Desc}You and your party are traveling in the vicinity of {closestSettlement} when you stumble across {peopleInWedding} people in a wedding taking place. " +
                    "The guests invite you over to celebrate this momentous event with them.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("peopleInWedding",peopleInWedding)
                .ToString();
            
            var eventOption1 = new TextObject("{=UnexpectedWedding_Event_Option_1}Give them {goldToDonate} gold as a gift").SetTextVariable("goldToDonate",goldToDonate).ToString();
            var eventOption1Hover = new TextObject("{=UnexpectedWedding_Event_Option_1_Hover}This is a special day after all").ToString();
            
            var eventOption2 = new TextObject("{=UnexpectedWedding_Event_Option_2}Give them some wine to enjoy").ToString();
            var eventOption2Hover = new TextObject("{=UnexpectedWedding_Event_Option_2_Hover}Who doesn't appreciate a good bottle of wine, right?").ToString();
            
            var eventOption3 = new TextObject("{=UnexpectedWedding_Event_Option_3}Stay for the ceremony").ToString();
            var eventOption3Hover = new TextObject("{=UnexpectedWedding_Event_Option_3_Hover}It's beautiful but you really don't want to waste any time").ToString();
            
            var eventOption4 = new TextObject("{=UnexpectedWedding_Event_Option_4}Leave").ToString();
            var eventOption4Hover = new TextObject("{=UnexpectedWedding_Event_Option_4_Hover}Not interested").ToString();
            
            var eventOption5 = new TextObject("{=UnexpectedWedding_Event_Option_5}Raid the wedding").ToString();
            var eventOption5Hover = new TextObject("{=UnexpectedWedding_Event_Option_5_Hover}You could do with some gold").ToString();
            
            var eventButtonText1 = new TextObject("{=UnexpectedWedding_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=UnexpectedWedding_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover),
                new InquiryElement("e", eventOption5, null, true, eventOption5Hover)
            };
            
            var eventOptionAText = new TextObject(
                    "{=UnexpectedWedding_Event_Choice_1}You congratulate the couple and you and your men scrape together {goldToDonate} gold and give it as a gift. Your party then spends the evening having fun!")
                .SetTextVariable("goldToDonate", goldToDonate)
                .ToString();
            
            var eventOptionB1Text = new TextObject(
                    "{=UnexpectedWedding_Event_Choice_2-1}You have your men find 5 bottles of your best wine. You offer it to the bride and groom. They thank you for this exquisite gift.")
                .ToString();
            
            var eventOptionB2Text = new TextObject(
                    "{=UnexpectedWedding_Event_Choice_2-2}You have your men find 5 bottles of your best wine. After a few minutes, one clearly embarrassed solider approaches you and tells you you are all out of wine. " +
                    "You slap him across his face for putting you in such a humiliating situation. You tell the solider to hand over all his coin to you. He does as you command. " +
                    "You apologises to the bride and hand her {embarrassedSoliderGold} gold instead of wine. She thanks you and your party moves on.")
                .SetTextVariable("embarrassedSoliderGold", embarrassedSoliderGold)
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=UnexpectedWedding_Event_Choice_3}You and your men stay for the ceremony but you leave once it is concluded. You leave a small gift of {goldToDonate} gold to the newlyweds.")
                .SetTextVariable("goldToDonate", goldToDonate)
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=UnexpectedWedding_Event_Choice_4}You politely decline and order your men to leave.")
                .ToString();
            
            var eventOptionEText = new TextObject(
                    "{=UnexpectedWedding_Event_Choice_5}You have your men surround the area while you go and talk to the guests. You have all guests empty their pockets and give you anything valuable. " + 
                    "Some guests resist but after a few threatening gestures from your men they too fall in line. After you have stolen {raidedGold} gold and anything of value from the wedding, " +
                    "you order your men to trash the entire area. Your men do so without blinking an eye. You see the bride crying while being comforted by some guests. You can see the " +
                    "hate in the groom's eyes. He will undoubtedly remember you.\n \n After you have personally made sure that you have thoroughly ruined this once joyful moment, you order your men to leave.")
                .SetTextVariable("raidedGold", raidedGold)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_1}You gave the newlyweds {goldToDonate} gold.")
                .SetTextVariable("goldToDonate", goldToDonate)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_2}You made the soldier who embarrassed you give the newlyweds {embarrassedSoliderGold} gold.")
                .SetTextVariable("embarrassedSoliderGold", embarrassedSoliderGold)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_3}You gave the newlyweds {goldToDonate} gold.")
                .SetTextVariable("goldToDonate", goldToDonate)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_4}You stole {raidedGold} gold from the wedding.")
                .SetTextVariable("raidedGold", raidedGold)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldToDonate);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));

                            break;
                        case "b" when partyFood >= 5:
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionB1Text, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "b" when partyFood < 5:
                        {
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionB2Text, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-embarrassedSoliderGold);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            
                            break;
                        }
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldToDonate);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "e":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionEText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(+raidedGold);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));
                            
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


    public class UnexpectedWeddingData : RandomEventData
    {
        public UnexpectedWeddingData(string eventType, float chanceWeight) : base(eventType,
            chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new UnexpectedWedding();
        }
    }
}