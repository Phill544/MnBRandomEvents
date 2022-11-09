using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class ViolatedGirl : BaseEvent
    {
        private readonly int minGoldCompensation;
        private readonly int maxGoldCompensation;

        public ViolatedGirl() : base(Settings.ModSettings.RandomEvents.ViolatedGirlData)
        {
            minGoldCompensation = Settings.ModSettings.RandomEvents.ViolatedGirlData.minGoldCompensation;
            maxGoldCompensation = Settings.ModSettings.RandomEvents.ViolatedGirlData.maxGoldCompensation;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return true;
        }

        public override void StartEvent()
        {
            if (Settings.ModSettings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }

            var heroName = Hero.MainHero.FirstName;
            
            var eventTitle = new TextObject("{=ViolatedGirl_Title}A violated girl").ToString();
            
            var closestCity = ClosestSettlements.GetClosestTown(MobileParty.MainParty).ToString();
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            var goldToCompensate = MBRandom.RandomInt(minGoldCompensation, maxGoldCompensation);
            var totalCompensation = goldToCompensate + 300;

            var compensation = MBRandom.RandomInt(minGoldCompensation, maxGoldCompensation);
            
            var eventDescription = new TextObject(
                    "{=ViolatedGirl_Event_Desc}As your party is resting near {closestSettlement}, you are approached by a young girl. She asks to speak to you privately. You invite her into your tent to listen to what she has to say. " +
                    "She claims that while you were in the previous town she was violated by one of your men. What do you do?")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=ViolatedGirl_Event_Option_1}Find the man").ToString();
            var eventOption1Hover = new TextObject("{=ViolatedGirl_Event_Option_1_Hover}This is unacceptable behaviour!").ToString();
            
            var eventOption2 = new TextObject("{=ViolatedGirl_Event_Option_2}Ask how much to keep this quiet?").ToString();
            var eventOption2Hover = new TextObject("{=ViolatedGirl_Event_Option_2_Hover}Everyone has a price").ToString();
            
            var eventOption3 = new TextObject("{=ViolatedGirl_Event_Option_3}Tell her to leave").ToString();
            var eventOption3Hover = new TextObject("{=ViolatedGirl_Event_Option_3_Hover}Leave NOW").ToString();
            
            var eventOption4 = new TextObject("{=ViolatedGirl_Event_Option_4}Kill her").ToString();
            var eventOption4Hover = new TextObject("{=ViolatedGirl_Event_Option_4_Hover}She is too dangerous to be left alive").ToString();
            
            var eventButtonText1 = new TextObject("{=ViolatedGirl_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=ViolatedGirl_Event_Button_Text_2}Done").ToString();
            

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };
            
            var eventOptionAText = new TextObject(
                    "{=ViolatedGirl_Event_Choice_1}You tell her that this sort of behaviour is unacceptable. You order all your men to attention as you and the girl search for the man. " +
                    "She finally stops and points to one of your men. You order him to you.\n You ask him if her story is true and he confirms that it is. You immediately punch him in the " +
                    "face so hard that he falls on his back. You strip him of his rank on the spot and have some men take him in chains.\nYou ask the girl what she wants to do with him. " +
                    "She wants him to pay for his misdeed so you have 5 of your men escort him to {closestCity} where he will face justice. \nYou also give the girl {compensation} gold as " +
                    "an apology from you personally. The girl thank you for believing her and appreciate your swift action.")
                .SetTextVariable("closestCity", closestCity)
                .SetTextVariable("compensation", compensation)
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=ViolatedGirl_Event_Choice_2}You listen to the girl's story. You do believe her but you cannot risk this news damaging the morale of the men. You ask her how much gold " +
                    "it would take to keep this quiet.\n The girl says she doesn't have a lot of gold as she is tending to her sick father at home. She says a sum of {goldToCompensate} gold " +
                    "would be enough to help her father and forget about this incident.\n You agree to pay her the requested amount plus 300 additional gold as a sign of goodwill. " +
                    "The girl then promptly leaves your camp. \n \nAll in all this event has cost you {totalCompensation} gold.")
                .SetTextVariable("goldToCompensate", goldToCompensate)
                .SetTextVariable("totalCompensation", totalCompensation)
                .ToString();
            
            
            var eventOptionCText = new TextObject(
                    "{=ViolatedGirl_Event_Choice_3}You tell her you don't buy her story and even if you did you can't take her word for it. You have three guards escort her from the camp. " +
                    "The girl leaves but not before she screams a few profanities your way.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=ViolatedGirl_Event_Choice_4}You know that if this gets out you are in trouble. You offer the girl a glass of wine which she accepts. While she has her back to you, " +
                    "you pull out your dagger and in one swift move you have cut her throat.\n It doesn't take long for her to bleed out. Once she does you call on 5 of your most trusted " +
                    "men to help you with the cleanup. You personally put her body in a sheet of linen and put her on your horse.\n Your ride away from camp for a few minutes until you stop " +
                    "and dig a shallow grave. You bury the girl and return to camp. The men inform you that the cleanup is done. No one will ever know what transpired here today.")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=ViolatedGirl_Event_Msg_1}You gave the girl {compensation} gold and had the perpetrator sent to the dungeons of {closestCity}.")
                .SetTextVariable("compensation", compensation)
                .SetTextVariable("closestCity", closestCity)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=ViolatedGirl_Event_Msg_2}You bought the girls silence for {totalCompensation} gold.")
                .SetTextVariable("totalCompensation", totalCompensation)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=ViolatedGirl_Event_Msg_3}There are rumors that {heroName} killed someone to keep a secret.")
                .SetTextVariable("heroName", heroName)
                .ToString();


            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-compensation);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
                            break;
                        case "b":
                        {
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-totalCompensation);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
                            break;
                        }
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
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


    public class ViolatedGirlData : RandomEventData
    {
        public readonly int minGoldCompensation;
        public readonly int maxGoldCompensation;

        public ViolatedGirlData(string eventType, float chanceWeight, int minGoldCompensation, int maxGoldCompensation) : base(eventType,
            chanceWeight)
        {
            this.minGoldCompensation = minGoldCompensation;
            this.maxGoldCompensation = maxGoldCompensation;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new ViolatedGirl();
        }
    }
}