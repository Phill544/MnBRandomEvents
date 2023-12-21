using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class UnexpectedWedding : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minGoldToDonate;
        private readonly int maxGoldToDonate;
        private readonly int minPeopleInWedding;
        private readonly int maxPeopleInWedding;
        private readonly int embarrassedSoliderMaxGold;
        private readonly int minGoldRaided;
        private readonly int maxGoldRaided;
        private readonly int minRogueryLevel;

        public UnexpectedWedding() : base(ModSettings.RandomEvents.UnexpectedWeddingData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

            eventDisabled = ConfigFile.ReadBoolean("UnexpectedWedding", "EventDisabled");
            minGoldToDonate = ConfigFile.ReadInteger("UnexpectedWedding", "MinGoldToDonate");
            maxGoldToDonate = ConfigFile.ReadInteger("UnexpectedWedding", "MaxGoldToDonate");
            minPeopleInWedding = ConfigFile.ReadInteger("UnexpectedWedding", "MinPeopleInWedding");
            maxPeopleInWedding = ConfigFile.ReadInteger("UnexpectedWedding", "MaxPeopleInWedding");
            embarrassedSoliderMaxGold = ConfigFile.ReadInteger("UnexpectedWedding", "EmbarrassedSoliderMaxGold");
            minGoldRaided = ConfigFile.ReadInteger("UnexpectedWedding", "MinGoldRaided");
            maxGoldRaided = ConfigFile.ReadInteger("UnexpectedWedding", "MaxGoldRaided");
            minRogueryLevel = ConfigFile.ReadInteger("UnexpectedWedding", "MinRogueryLevel");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minGoldToDonate != 0 || maxGoldToDonate != 0 || minPeopleInWedding != 0 || maxPeopleInWedding != 0 || embarrassedSoliderMaxGold != 0 || minGoldRaided != 0 || maxGoldRaided != 0 || minRogueryLevel != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null  && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxPeopleInWedding;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=UnexpectedWedding_Title}An Unexpected Wedding").ToString();
            
            var goldToDonate = MBRandom.RandomInt(minGoldToDonate, maxGoldToDonate);
            
            var heroName = Hero.MainHero.FirstName;
            
            var peopleInWedding = MBRandom.RandomInt(minPeopleInWedding, maxPeopleInWedding);
            
            var partyFood = MobileParty.MainParty.TotalFoodAtInventory;

            var goldBase = MBRandom.RandomInt(minGoldRaided, maxGoldRaided);

            var raidedGold = goldBase * peopleInWedding;
            
            var embarrassedSoliderGold = MBRandom.RandomInt(10, embarrassedSoliderMaxGold);
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);

            var canRaidWedding = false;
            var rogueryAppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                canRaidWedding = true;
                rogueryAppendedText = new TextObject("{=UnexpectedWedding_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
                
            }
            else
            {
                if (rogueryLevel >= minRogueryLevel)
                {
                    canRaidWedding = true;
                    rogueryAppendedText = new TextObject("{=UnexpectedWedding_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
                
            }

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
            
            var eventOption5 = new TextObject("{=UnexpectedWedding_Event_Option_5}[Roguery] Raid the wedding").ToString();
            var eventOption5Hover = new TextObject("{=UnexpectedWedding_Event_Option_5_Hover}You could do with some gold\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();
            
            var eventButtonText1 = new TextObject("{=UnexpectedWedding_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=UnexpectedWedding_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>();
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            if (canRaidWedding)
            {
                inquiryElements.Add(new InquiryElement("e", eventOption5, null, true, eventOption5Hover));
            }

            var eventOptionAText = new TextObject(
                    "{=UnexpectedWedding_Event_Choice_1}You congratulate the couple and you and your men scrape together {goldToDonate} gold and give it as a gift. Your party then spend the evening having fun!")
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
                    "hate in the groom's eyes. He will undoubtedly remember you.\n\nAfter you have personally made sure that you have thoroughly ruined this once joyful moment, you order your men to leave.")
                .SetTextVariable("raidedGold", raidedGold)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_1}{heroName} gave the newlyweds {goldToDonate} gold.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldToDonate", goldToDonate)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_2}{heroName} made the soldier who embarrassed you give the newlyweds {embarrassedSoliderGold} gold.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("embarrassedSoliderGold", embarrassedSoliderGold)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_3}{heroName} gave the newlyweds {goldToDonate} gold.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldToDonate", goldToDonate)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=UnexpectedWedding_Event_Msg_4}{heroName} stole {raidedGold} gold from the wedding.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("raidedGold", raidedGold)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
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
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
                            
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "e":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionEText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(+raidedGold);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
                            
                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                }, null, null);

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
        public UnexpectedWeddingData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new UnexpectedWedding();
        }
    }
}