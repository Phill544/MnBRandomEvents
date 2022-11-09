using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class FallenSoldierFamily : BaseEvent
    {
        private readonly int minFamilyCompensation;
        private readonly int maxFamilyCompensation;

        public FallenSoldierFamily() : base(Settings.ModSettings.RandomEvents.FallenSoldierFamilyData)
        {
            minFamilyCompensation = Settings.ModSettings.RandomEvents.FallenSoldierFamilyData.minFamilyCompensation;
            maxFamilyCompensation = Settings.ModSettings.RandomEvents.FallenSoldierFamilyData.maxFamilyCompensation;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MobileParty.MainParty.CurrentSettlement != null && (MobileParty.MainParty.CurrentSettlement.IsTown || MobileParty.MainParty.CurrentSettlement.IsVillage);
        }

        public override void StartEvent()
        {
            if (Settings.ModSettings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }

            var heroName = Hero.MainHero.FirstName;
            
            var currentSettlement = MobileParty.MainParty.CurrentSettlement.Name;
            
            var familyCompensation = MBRandom.RandomInt(minFamilyCompensation, maxFamilyCompensation);
            
            var eventTitle = new TextObject("{=FallenSoldier_Title}Family of a fallen soldier").ToString();
            
            var eventDescription = new TextObject(
                    "{=FallenSoldier_Event_Desc}As you are having a drink at the local tavern in {currentSettlement}, you are approached by 3 individuals. " +
                    "It's a woman and two young boys. They ask if they can talk to you. They explain that they are the family of a soldier who died under your command. " +
                    "They are here requesting compensation for his death as they are in desperate need of gold to be able to keep their farm. What do you do?")
                .SetTextVariable("currentSettlement", currentSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=FallenSoldier_Event_Option_1}Offer them compensation").ToString();
            var eventOption1Hover = new TextObject("{=FallenSoldier_Event_Option_1_Hover}They should be compensated").ToString();
            
            var eventOption2 = new TextObject("{=FallenSoldier_Event_Option_2}Explain that you owe them nothing").ToString();
            var eventOption2Hover = new TextObject("{=FallenSoldier_Event_Option_2_Hover}Not my problem!").ToString();
            
            var eventOption3 = new TextObject("{=FallenSoldier_Event_Option_3}Leave").ToString();
            var eventOption3Hover = new TextObject("{=FallenSoldier_Event_Option_3_Hover}You have a headache so you leave").ToString();
            
            var eventOption4 = new TextObject("{=FallenSoldier_Event_Option_4}Plot something malicious").ToString();
            var eventOption4Hover = new TextObject("{=FallenSoldier_Event_Option_4_Hover}The audacity!").ToString();
            
            var eventButtonText1 = new TextObject("{=FallenSoldier_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=FallenSoldier_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };
            
            var eventOptionAText = new TextObject(
                    "{=FallenSoldier_Event_Choice_1}You ask for the name and rank of the man who died. When she tells you his name you do remember him and how he died. " +
                    "The soldier in question was executed by your hands as it was discovered he was a traitor. \n The question you ask yourself now is if his entire family should suffer from his mistake. " +
                    "They have spoken so warmly about him that you don't want to tell them the truth about how he died so you make up a heroic story.\n" +
                    "Even though the family have no right for compensation, you agree to pay them {familyCompensation} gold in compensation so they can keep their family farm.\n \n" +
                    "After you have handed over they gold to them and they have left, you cannot help but wonder if you did the right thing keeping the mother in the dark about her son's true nature.\n \n" +
                    "You end up drinking the night away.")
                .SetTextVariable("familyCompensation", familyCompensation)
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=FallenSoldier_Event_Choice_2}You ask for the name and rank of the man who died. When she tells you his name you do remember him and how he died. " +
                    "The soldier in question was executed by your hands as it was discovered he was a traitor.\n" +
                    "You tell the family that you cannot grant them compensation as it was specified in his contract that the family left behind had no right to claim compensation." +
                    "The women starts to cry and begs you to help them. You decline to help them and leave the tavern.")
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=FallenSoldier_Event_Choice_3}You don't have the energy to deal with this so you tell them that you don't owe them anything. You then leave the tavern.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=FallenSoldier_Event_Choice_4}You ask for the name and rank of the man who died. When she tells you his name you do remember him and how he died. " +
                    "The soldier in question was executed by your hands as it was discovered he was a traitor.\n You ask them where their farm is and you tell them you will be there tomorrow. " +
                    "You then excuse yourself and leave. \n \n The following day you and your men arrive at the farm but you have no intention to pay them. " +
                    "Instead you order your men to burn the farm to the ground and kill the owners.\n  You watch as your men execute your orders. " +
                    "You see them dragging the family outside with their hands bound behind their backs. You watch as the farmhouse burns and you witness your men executing all of the family.\n" +
                    "Once they are done you order your men back and you ride back to your main party.")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=FallenSoldier_Event_Msg_1}{heroName} gives the family {familyCompensation} gold in compensation.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("familyCompensation", familyCompensation)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=FallenSoldier_Event_Msg_2}No one messes with {heroName}!")
                .SetTextVariable("heroName", heroName)
                .ToString();
            

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle,eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-familyCompensation);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
                            break;
                        case "b":
                        {
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle,eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        }
                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle,eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle,eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
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


    public class FallenSoldierFamilyData : RandomEventData
    {
        public readonly int minFamilyCompensation;
        public readonly int maxFamilyCompensation;

        public FallenSoldierFamilyData(string eventType, float chanceWeight, int minFamilyCompensation, int maxFamilyCompensation) : base(eventType,
            chanceWeight)
        {
            this.minFamilyCompensation = minFamilyCompensation;
            this.maxFamilyCompensation = maxFamilyCompensation;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new FallenSoldierFamily();
        }
    }
}