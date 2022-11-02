using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class ViolatedGirl : BaseEvent
    {
        private const string EventTitle = "A violated woman";
        
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
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }

            var closestCity = ClosestSettlements.GetClosestTown(MobileParty.MainParty);

            var goldToCompensate = MBRandom.RandomInt(minGoldCompensation, maxGoldCompensation);
            var totalCompensation = goldToCompensate + 300;

            var compensation = MBRandom.RandomInt(minGoldCompensation, maxGoldCompensation);

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", "Find the man", null, true, "This is unacceptable behaviour!"),
                new InquiryElement("b", "Ask how much to keep this quiet?", null, true, "Everyone has a price"),
                new InquiryElement("c", "Tell her to leave", null, true, "Leave NOW"),
                new InquiryElement("d", "Kill her", null, true, "She is too dangerous to be left alive")
            };
            

            var msid = new MultiSelectionInquiryData(
                EventTitle,
                "As your party is resting you are approached by an unknown young woman. You invite her into your tent to listen to what she has to say. She claims that while you were in the previous town she was violated by one of your men. What do you do?",
                inquiryElements,
                false,
                1,
                "Okay",
                null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You tell her that this sort of behaviour is unacceptable. You order all your men to attention as you and the woman search for the man. She finally stops and points to one of your men. You order him to you.\n" +
                                    "You ask him if her story is true and he confirms that it is. You immediately punch him in the face so hard that he falls on his back. You strip him of his rank on the spot and have some men take him in chains.\n" +
                                    $"You ask the woman what she wants to do with him. She wants him to pay for his misdeed so you have 5 of your men escort him to {closestCity} where he will face justice. \n" +
                                    $"You also give the woman {compensation} gold as an apology from you personally. The woman thank you for believing her and appreciate your swift action.",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-compensation);
                            break;
                        case "b":
                        {
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You listen to the woman's story. You do believe her but you cannot risk this news damaging the morale of the men. You ask her how much gold it would take to keep this quiet.\n" +
                                    $"The woman says she doesn't have a lot of gold as she is tending to her sick father at home. She says a sum of {goldToCompensate} gold would be enough to help her father and forget about this incident.\n" +
                                    "You agree to pay her the requested amount plus 300 additional gold as a sign of goodwill. The woman then promptly leaves your camp. \n \n " +
                                    $"All in all this event has cost you {totalCompensation} gold.",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-totalCompensation);
                            break;
                        }
                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You tell her you don't buy her story and even if you did you can't take her word for it. You have three guards escort her from the camp. The woman leaves but not before she screams a few profanities your way.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You know that if this gets out you are in trouble. You offer the woman a glass of wine which she accepts. While she has her back to you, you pull out your dagger and in one swift move you have cut her throat.\n" +
                                    "It doesn't take long for her to bleed out. Once she does you call on 5 of your most trusted men to help you with the cleanup. You personally put her body in a sheet of linen and put her on your horse.\n" +
                                    "Your ride away from camp for a few minutes until you stop and dig a shallow grave. You bury the woman and return to camp. The men inform you that the cleanup is done. No one will ever know what transpired here today.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                },
                null); // What to do on the "cancel" button, shouldn't ever need it.

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