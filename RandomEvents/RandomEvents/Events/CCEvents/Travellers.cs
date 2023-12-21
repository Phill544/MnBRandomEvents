using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class Travellers : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minGoldStolen;
        private readonly int maxGoldStolen;
        private readonly int minEngineeringLevel;
        private readonly int minRogueryLevel;
        private readonly int minStewardLevel;

        public Travellers() : base(ModSettings.RandomEvents.TravellersData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

            eventDisabled = ConfigFile.ReadBoolean("Travellers", "EventDisabled");
            minGoldStolen = ConfigFile.ReadInteger("Travellers", "MinGoldStolen");
            maxGoldStolen = ConfigFile.ReadInteger("Travellers", "MaxGoldStolen");
            minEngineeringLevel = ConfigFile.ReadInteger("Travellers", "MinEngineeringLevel");
            minRogueryLevel = ConfigFile.ReadInteger("Travellers", "MinRogueryLevel");
            minStewardLevel = ConfigFile.ReadInteger("Travellers", "MinStewardLevel");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minGoldStolen != 0 || maxGoldStolen != 0 || minEngineeringLevel != 0 || minRogueryLevel != 0 || minStewardLevel != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && (CurrentTimeOfDay.IsEvening || CurrentTimeOfDay.IsMidday || CurrentTimeOfDay.IsMorning)  && MobileParty.MainParty.MemberRoster.TotalRegulars >= 100;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=Travellers_Title}Travellers").ToString();

            var heroName = Hero.MainHero.Name.ToString();

            var engineeringLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Engineering);
            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
            var stewardLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Steward);

            var randomSettlement = Settlement.All.GetRandomElement();
            var familyEthnicity = randomSettlement.Culture.Name.ToString();
            var familyDemonym = Demonym.GetTheDemonym(familyEthnicity, true);

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            var stolenGold = MBRandom.RandomInt(minGoldStolen, maxGoldStolen);

            var canRepairWagon = false;
            var canRaidWagon = false;
            var canOfferDinner = false;

            var engineeringAppendedText = "";
            var rogueryAppendedText = "";
            var stewardAppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                
                canRepairWagon = true;
                canRaidWagon = true;
                canOfferDinner = true;
                
                engineeringAppendedText = new TextObject("{=Travellers_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
                rogueryAppendedText = new TextObject("{=Travellers_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
                stewardAppendedText = new TextObject("{=Travellers_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
                
            }
            else
            {
                if (engineeringLevel >= minEngineeringLevel)
                {
                    canRepairWagon = true;
                    
                    engineeringAppendedText = new TextObject("{=Travellers_Engineering_Appended_Text}[Engineering - lvl {minEngineeringLevel}]")
                        .SetTextVariable("minEngineeringLevel", minEngineeringLevel)
                        .ToString();
                }

                if (rogueryLevel >= minRogueryLevel)
                {
                    canRaidWagon = true;
                    
                    rogueryAppendedText = new TextObject("{=Travellers_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }

                if (stewardLevel >= minStewardLevel)
                {
                    canOfferDinner = true;
                    
                    stewardAppendedText = new TextObject("{=Travellers_Steward_Appended_Text}[Steward - lvl {minStewardLevel}]")
                        .SetTextVariable("minStewardLevel", minStewardLevel)
                        .ToString();
                }
            }
            

            var eventDescription = new TextObject(
                    "{=Travellers_Event_Desc}Your party is on the move not to far from {closestSettlement} when you come across {familyDemonym} family with a broken waggon. The wheel seems to have come off and " +
                    "rolled down into a small river not to far from you. The family is asking if you can help them by any chance. They explain that they are running from bandits who kept blackmailing " +
                    "them for money and tried to kidnap their two daughters on more than one occasion. They have brought everything they own with them in the hope of starting a new life somewhere. " +
                    "How do you proceed?")
                .SetTextVariable("familyDemonym", familyDemonym)
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();

            var eventOption1 = new TextObject("{=Travellers_Event_Option_1}[Engineering] Offer to repair the waggon").ToString();
            var eventOption1Hover = new TextObject("{=Travellers_Event_Option_1_Hover}You should be able to fix it.\n{engineeringAppendedText}")
                .SetTextVariable("engineeringAppendedText", engineeringAppendedText)
                .ToString();

            var eventOption2 = new TextObject("{=Travellers_Event_Option_2}[Roguery] Raid them!").ToString();
            var eventOption2Hover = new TextObject("{=Travellers_Event_Option_2_Hover}Take everything of value\n{rogueryAppendedText}")
                .SetTextVariable("rogueryAppendedText", rogueryAppendedText)
                .ToString();

            var eventOption3 = new TextObject("{=Travellers_Event_Option_3}[Steward] Offer them dinner").ToString();
            var eventOption3Hover = new TextObject("{=Travellers_Event_Option_3_Hover}While your men load all their belongings into a spare wagon you have {stewardAppendedText}")
                .SetTextVariable("stewardAppendedText", stewardAppendedText)
                .ToString();

            var eventOption4 = new TextObject("{=Travellers_Event_Option_4}Offer them directions").ToString();
            var eventOption4Hover = new TextObject("{=Travellers_Event_Option_4_Hover}Give them directions to the nearest settlement").ToString();
            
            var eventOption5 = new TextObject("{=Travellers_Event_Option_5}Ignore them").ToString();
            var eventOption5Hover = new TextObject("{=Travellers_Event_Option_5_Hover}This isn't your problem").ToString();
            
            var eventButtonText1 = new TextObject("{=Travellers_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=Travellers_Event_Button_Text_2}Done").ToString();
            

            var inquiryElements = new List<InquiryElement>();
            
            if (canRepairWagon)
            {
                inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            }
            if (canRaidWagon)
            {
                inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            }
            if (canOfferDinner)
            {
                inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            }
            inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            inquiryElements.Add(new InquiryElement("e", eventOption5, null, true, eventOption5Hover));

            
            var eventOptionAText = new TextObject(
                    "{=Travellers_Event_Choice_1}You take a look at the damage to the wagon and you quickly confirm you suspicion that you are able to fix this with relative ease. " +
                    "Your tell you men to fetch some tools from the back of the party and you order one man down into the river to fetch the wheel. You tell the family to sit in the shade beneath " +
                    "a giant oak tree while the men fetches something to drink and some snacks for the two children.\n\nYou waste no time getting to work and with the assistance of a few men you are able " +
                    "to quickly repair the wagon and put it back in working order. You head over to the family to inform them that they may now head towards their destination again. The man doesn't know how " +
                    "to thank you so you inform him that no thank you are necessary. You help the family load up their wagon again and you give them directions to {closestSettlement}, which is the closest " +
                    "place the wagon can get a proper repair. The man says he will tell stories of you when he arrives at his destination. You assure him that it won't be necessary but you don't think he'll listen.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();

            var eventOptionBText = new TextObject(
                    "{=Travellers_Event_Choice_2}You order your men to take them family away from the wagon but not to hurt them in any way as there are young children with them. The father attempts to resist " +
                    "but he is quickly overpowered by a few men who binds his hands. His wife begs him to just comply with what we are saying and he reluctantly agrees.\n\nYour men starts to empty the wagon and look " +
                    "for anything valuable, but you are disappointed when your can't find anything worthwhile. You take the mother aside and tells her to her dismay that if she doesn't give up all the valuables they " +
                    "have, you will kill her husband, sell the children into slavery and keep her for company. Of course you don't mean this, but the important thing is that she thinks you do.\nShe quickly tell you " +
                    "where to find their valuable possessions. After sometime you managed to steal {stolenGold} gold from them. Once you are done with everything you free the woman and tell her that once all your men " +
                    "are out of sight she may release her family. You're not entirely heartless so you give them directions to the nearest settlement where they may find help.")
                .SetTextVariable("stolenGold", stolenGold)
                .ToString();

            var eventOptionCText = new TextObject(
                    "{=Travellers_Event_Choice_3}You can clearly see that the family is exhausted and probably haven't eaten in a days. You ask them if you can prepare dinner for them. They reluctantly agree as " +
                    "they have only just met you. You order your men to set up a small tent for the family while you personally cook dinner for the family. Your men will take care of the wagon. You and some of your " +
                    "men bring out something to drink for the family and you get to work prepping dinner.\n\nSome time passes and just as the family have finished eating, your men inform you that the wagon is repaired. " +
                    "You help the family load up their wagon and give them directions to the nearest settlement. The mother tells you what an extraordinary man you are.")
                .ToString();

            var eventOptionDText = new TextObject(
                    "{=Travellers_Event_Choice_4}You stop and ask them what happened. You learn that they lost control of their wagon and it ended up in the ditch. You take a look at the wagon to see if you " +
                    "are able to fix it, but it seems to complex for you or any of your men to fix. They ask if they may buy a wagon from your party for 500 gold, but you tell them you will give them one for free " +
                    "as they are in distress and need a hand. You have your men bring fourth a wagon. You, the family and some of your men help to transfer their item to the new wagon\n\nOnce done you give them directions " +
                    "to the nearest settlement. They thank you yet again as you leave.")
                .ToString();
            
            var eventOptionEText = new TextObject(
                    "{=Travellers_Event_Choice_5}You cannot be bothered by this now so you tell them you cannot help. They try to ask you for directions but you ignore them, but after a few feet you can hear that some of " +
                    "your men have stopped to give them directions to the nearest settlement and some coin.")
                .ToString();
            
            
            var eventMsg1 =new TextObject(
                    "{=Travellers_Event_Msg_1}{heroName} was able to fix the stranded family's wagon.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=Travellers_Event_Msg_2}{heroName} stole {stolenGold} gold from the stranded family and left them.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("stolenGold", stolenGold)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=Travellers_Event_Msg_3}{heroName} made dinner while the men fixed the wagon.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=Travellers_Event_Msg_4}{heroName} gave the stranded family directions to {closestSettlement}.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventMsg5 =new TextObject(
                    "{=Travellers_Event_Msg_5}{heroName} ignored the stranded family. At least the men aren't that heartless!")
                .SetTextVariable("heroName", heroName)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null, elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            
                            break;
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));
                            
                            break;
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            
                            break;
                        
                        case "e":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionEText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg5, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            
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


    public class TravellersData : RandomEventData
    {
        public TravellersData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new Travellers();
        }
    }
}