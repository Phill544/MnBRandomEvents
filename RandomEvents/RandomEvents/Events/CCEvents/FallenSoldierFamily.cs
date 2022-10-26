using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class FallenSoldierFamily : BaseEvent
    {
        private const string EventTitle = "Family of a fallen soldier";
        
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
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }
            

            var familyCompensation = MBRandom.RandomInt(minFamilyCompensation, maxFamilyCompensation);

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", "Offer them compensation", null, true, "They should be compensated"),
                new InquiryElement("b", "Explain that you owe them nothing", null, true, "Not my problem"),
                new InquiryElement("c", "Leave", null, true, "You have a headache so you leave"),
                new InquiryElement("d", "Plot something malicious", null, true, "The audacity!")
            };
            

            var msid = new MultiSelectionInquiryData(
                EventTitle,
                "As you are having a drink at the local tavern, you are approached by 3 individuals. It's a woman and two young boys. They ask if they can talk to you. They explain that they are the family of a soldier who died under your command." +
                "They are here requesting compensation for his death as they are in desperate need of gold to be able to keep their farm. What do you do?",
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
                                    "You ask for the name and rank of the man who died. When she tells you his name you do remember him and how he died. The soldier in question was executed by your hands as it was discovered he was a traitor. \n" +
                                    "The question you ask yourself now is if his entire family should suffer from his mistake. They have spoken so warmly about him that you don't want to tell them the truth about how he died so you make up a heroic story.\n" +
                                    $"Even though the family have no right for compensation, you agree to pay them {familyCompensation} gold in compensation so they can keep their family farm.\n \n" +
                                    "After you have handed over they gold to them and they have left, you cannot help but wonder if you did the right thing keeping the mother in the dark about her son's true nature. You end up drinking the night away.",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-familyCompensation);
                            break;
                        case "b":
                        {
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You ask for the name and rank of the man who died. When she tells you his name you do remember him and how he died. The soldier in question was executed by your hands as it was discovered he was a traitor.\n" +
                                    "You tell the family that you cannot grant them compensation as it was specified in his contract that the family left behind had no right to claim compensation. The women starts to cry and begs you to help them. " +
                                    "You decline to help them and leave the tavern.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        }
                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You don't have the energy to deal with this so you tell them that you don't owe them anything. You then leave the tavern.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You ask for the name and rank of the man who died. When she tells you his name you do remember him and how he died. The soldier in question was executed by your hands as it was discovered he was a traitor.\n" +
                                    "You ask them where their farm is and you tell them you will be there tomorrow. You then excuse yourself and leave. \n \n " +
                                    "The following day you and your men arrive at the farm but you have no intention to pay them. Instead you order your men to burn the farm to the ground and kill the owners.\n " +
                                    "You watch as your men execute your orders. You see them dragging the family outside with their hands bound behind their backs. You watch as the farmhouse burns and you witness your men " +
                                    "executing all of the family.\n" +
                                    "Once they are done you order your men back and you ride back to your main party. ",
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