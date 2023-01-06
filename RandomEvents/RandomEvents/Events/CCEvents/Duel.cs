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
    public class Duel : BaseEvent
    {

        private readonly int minTwoHandedLevel;
        private readonly int minRogueryLevel;

        public Duel() : base(ModSettings.RandomEvents.DuelData)
        {
            minTwoHandedLevel = 125;
            minRogueryLevel = 100;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {

            return MCM_MenuConfig_Toggle.Instance.DU_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
            var mainHero = Hero.MainHero;

            var heroName = mainHero.FirstName;
            
            var eventTitle = new TextObject("{=Duel_Title}Duel").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty);

            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
            var twoHandedLevel = Hero.MainHero.GetSkillValue(DefaultSkills.TwoHanded);
            
            var canTrick = false;
            var canKillChallenger = false;
            
            var rogueryAppendedText = "";
            var twoHandedAppendedText = "";
            
            if (MCM_ConfigMenu_General.Instance.GS_DisableSkillChecks)
            {
                
                canTrick = true;
                canKillChallenger = true;

                rogueryAppendedText = new TextObject("{=Duel_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
                twoHandedAppendedText = new TextObject("{=Duel_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();

            }
            else
            {
                if (rogueryLevel >= minRogueryLevel)
                {
                    canTrick = true;
                    
                    rogueryAppendedText = new TextObject("{=Duel_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
                if (twoHandedLevel >= minTwoHandedLevel)
                {
                    canKillChallenger = true;
                    
                    twoHandedAppendedText = new TextObject("{=Duel_TwoHanded_Appended_Text}[Two-Handed - lvl {minTwoHandedLevel}]")
                        .SetTextVariable("minTwoHandedLevel", minTwoHandedLevel)
                        .ToString();
                }
            }

            var eventDescription = new TextObject(
                    "{=Duel_Event_Desc}While you party is resting in the vicinity of {closestSettlement}, you are approached by a young man who demands an audience with you. He has heard of your deeds in the arenas around Calradia " +
                    "and he is here to challenge you to a duel. Taking a look at this you man it quickly becomes apparent that you will easily beat him. You could take this opportunity to teach him a lesson or you decline " +
                    "his challenge. What do you do?")
                    .SetTextVariable("closestSettlement", closestSettlement.Name)
                    .ToString();
            
            var eventOption1 = new TextObject("{=Duel_Event_Option_1}Decline his challenge").ToString();
            var eventOption1Hover = new TextObject("{=Duel_Event_Option_1_Hover}No point in accepting.").ToString();
            
            var eventOption2 = new TextObject("{=Duel_Event_Option_2}Accept his challenge").ToString();
            var eventOption2Hover = new TextObject("{=Duel_Event_Option_2_Hover}Teach him a lesson.").ToString();

            var eventOption3 = new TextObject("{=Duel_Event_Option_3}[Two-Handed] Kill him").ToString();
            var eventOption3Hover = new TextObject("{=Duel_Event_Option_3_Hover}Kill him in combat.\n{twoHandedAppendedText}").SetTextVariable("twoHandedAppendedText", twoHandedAppendedText).ToString();
            
            var eventOption4 = new TextObject("{=Duel_Event_Option_4}[Roguery] Trick him").ToString();
            var eventOption4Hover = new TextObject("{=Duel_Event_Option_4_Hover}Kill him by tricking him.\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();


            var eventButtonText1 = new TextObject("{=Duel_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=Duel_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            
            if (canKillChallenger)
            {
                inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            }

            if (canTrick)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }

            var eventOptionAText = new TextObject(
                    "{=Duel_Event_Choice_1}You tell him that you decline his challenge as you undoubtedly defeat him. You also take this opportunity to give him some advice going forward. Such as not challenging " +
                    "individuals with far more skill than himself. You tell him that if he really wants to learn how to fight he should join the garrison in a nearby city as they are properly trained. You part ways " +
                    "on relatively good terms.")
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=Duel_Event_Choice_2}You accept his challenge. When you emerge from your tent wearing a full set of plate armor, you can see the fear in your opponent's face. You have your men make a large circle " +
                    "around the two of you as you both prepare to fight. Your opponent is wearing light leather armor. You tell him to stop and you order your men to fetch a set of plate armor for him to use. After a few " +
                    "minutes he is all suited up and it becomes apparent that he has never worn such armor before. The duel starts. You approach him, but he falls to his knees begging for forgiveness. You knew this was " +
                    "gonna happen so take this opportunity to scare som sense into him. You spend a few minutes roughing him around, nothing dangerous naturally. The man falls to his knees again. You ask him if he has " +
                    "learned his lesson to which he frantically agrees.\n\nYou help him out of his armor and you also take this opportunity to give him some advice going forward. Such as not challenging " +
                    "individuals with far more skill than himself. You tell him that if he really wants to learn how to fight he should join the garrison in a nearby city as they are properly trained. You part ways " +
                    "on relatively good terms.")
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=Duel_Event_Choice_3}You accept his challenge. When you emerge from your tent wearing a full set of plate armor, you can see the fear in your opponent's face. You have your men make a large circle " +
                    "around the two of you as you both prepare to fight. Your opponent is wearing light leather armor. The duel starts and you rush forward towards him. He tries to dodge but he's too slow and in one swift " +
                    "move you decapitate him. His head flies through the air and lands a few feet away. Your men cheer for your victory.\n\nYou have your men prepare a pyre for his body as you are willing to give him " +
                    "a funeral worthy of a warrior. You know he was a fool to ask your for a duel but you also have respect for him for going through with it. You dispatch a courier to {closestSettlement} to inform them " +
                    "that a man has died as a result of a duel.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=Duel_Event_Choice_4}You accept his challenge. When he turns to leave you quickly grab a sword from a stand and with one swift motion you strike him down. You strike him again... and again... and again. " +
                    "Eventually you are pulled away by two of your men. All that is left of the challenger is a pulp of blood, organs and body parts and you yourself is covered in blood. You tell your men to clean this up " +
                    "and dump the body in the nearby woods. This way animals will soon get rid of the evidence. As you walk by some of your men to get washed you can see the fear in their eyes. At least this will keep " +
                    "them on their toes as nobody wants to end up like that.")
                .ToString();
            
            
            var eventMsg1 =new TextObject(
                    "{=Duel_Event_Msg_1}{heroName} declined the duel and instead taught the challenger a thing or two.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=Duel_Event_Msg_2}{heroName} accepted the challenge. He humiliated the challenger.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=Duel_Event_Msg_3}{heroName} easily killed the challenger.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=Duel_Event_Msg_4}{heroName} completely lost it when he was challenged to a duel.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            
            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1,
                    eventButtonText1, null,
                    elements =>
                    {
                        switch ((string)elements[0].Identifier)
                        {
                            case "a":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                                break;
                            case "b":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                                
                                Hero.MainHero.ChangeHeroGold(-5);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                                break;
                            case "c":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                                
                                break;
                            case "d":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                                
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


    public class DuelData : RandomEventData
    {
        public DuelData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new Duel();
        }
    }
}