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
    public class UnexpectedWedding : BaseEvent
    {
        private const string EventTitle = "An Unexpected Wedding";
        
        private readonly int minGoldToDonate;
        private readonly int maxGoldToDonate;
        private readonly int minPeopleInWedding;
        private readonly int maxPeopleInWedding;
        private readonly int embarrassedSoliderMaxGold;
        private readonly int minMoraleGain;
        private readonly int maxMoraleGain;
        private readonly int minGoldRaided;
        private readonly int maxGoldRaided;

        public UnexpectedWedding() : base(Settings.ModSettings.RandomEvents.UnexpectedWeddingData)
        {
            minGoldToDonate = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.minGoldToDonate;
            maxGoldToDonate = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.maxGoldToDonate;
            minPeopleInWedding = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.minPeopleInWedding;
            maxPeopleInWedding = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.maxPeopleInWedding;
            embarrassedSoliderMaxGold = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.embarrassedSoliderMaxGold;
            minMoraleGain = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.minMoraleGain;
            maxMoraleGain = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.maxMoraleGain;
            minGoldRaided = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.minGoldRaided;
            maxGoldRaided = Settings.ModSettings.RandomEvents.UnexpectedWeddingData.maxGoldRaided;
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
            
            var goldToDonate = MBRandom.RandomInt(minGoldToDonate, maxGoldToDonate);

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", $"Give them {goldToDonate} gold as a gift", null, true, "This is a special day after all"),
                new InquiryElement("b", "Give them some wine to enjoy", null, true, "Who doesn't appreciate a good bottle of wine, right?"),
                new InquiryElement("c", "Watch the ceremony but leave once it's concluded", null, true, "It's beautiful but you really don't want to waste any time"),
                new InquiryElement("d", "Leave", null, true, "Not interested"),
                new InquiryElement("e", "Raid the wedding", null, true, "You could do with some gold.")
            };

            
            var peopleInWedding = MBRandom.RandomInt(minPeopleInWedding, maxPeopleInWedding);
            
            var partyFood = MobileParty.MainParty.TotalFoodAtInventory;

            var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);

            var raidedGold = MBRandom.RandomInt(minGoldRaided, maxGoldRaided);

            var msid = new MultiSelectionInquiryData(
                EventTitle,
                $"You and your party stumble across {peopleInWedding} people in a wedding taking place. The guests invite you over to celebrate this momentous event with them.",
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
                                    $"You congratulate the couple and you and your men scrape together {goldToDonate} gold and give it as a gift. You and your men spend the evening having fun. You really feel the morale of the men increase.",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldToDonate);

                            MobileParty.MainParty.RecentEventsMorale += moraleGain;
                            MobileParty.MainParty.MoraleExplained.Add(moraleGain, new TextObject("Random Event"));
                            break;
                        case "b" when partyFood >= 5:
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You have your men find 5 bottles of your best wine. You offer it to the bride and groom. They thank you for this exquisite gift.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "b" when partyFood < 5:
                        {
                            var embarrassedSoliderGold = MBRandom.RandomInt(10, embarrassedSoliderMaxGold);
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You have your men find 5 bottles of your best wine. After  a few minutes, one clearly embarrassed solider approaches you and tells you you are all out of wine. " +
                                    "You slap him across his face for putting you in such a humiliating situation. You tell the solider to hand over all his coin to you. He does as you command him to do. " +
                                    $"You apologises to the bride and hand her {embarrassedSoliderGold} gold instead of wine. She thanks you and your party moves on. ",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-embarrassedSoliderGold);
                            break;
                        }
                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    $"You and your men stay for the ceremony but you leave once it is concluded. You leave a small gift of {goldToDonate} gold to the newlyweds.",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldToDonate);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You don't have time for this so you order your men to leave.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "e":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You have your men surround the area while you go and talk to the guests. You have all guests empty their pockets and give you anything valuable. " +
                                    $"Some guests resists but after a few threatening gestures from your men they too fall in line. After you have stolen {raidedGold} gold and anything of value from the wedding, you order your men to trash the entire area. " +
                                    "Your men does so without blinking an eye. You see the bride crying while being comforted by some guests. You can see the hate in the groom's eyes. He will undoubtedly remember you.\n \n" +
                                    "After you have personally made sure that you have thoroughly ruined this once joyful moment, you order your men to leave.",
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


    public class UnexpectedWeddingData : RandomEventData
    {
        public readonly int minGoldToDonate;
        public readonly int maxGoldToDonate;
        public readonly int minPeopleInWedding;
        public readonly int maxPeopleInWedding;
        public readonly int embarrassedSoliderMaxGold;
        public readonly int minMoraleGain;
        public readonly int maxMoraleGain;
        public readonly int minGoldRaided;
        public readonly int maxGoldRaided;

        public UnexpectedWeddingData(string eventType, float chanceWeight, int minGoldToDonate, int maxGoldToDonate, int minPeopleInWedding, int maxPeopleInWedding, int embarrassedSoliderMaxGold, int minMoraleGain, int maxMoraleGain, int minGoldRaided, int maxGoldRaided) : base(eventType,
            chanceWeight)
        {
            this.minGoldToDonate = minGoldToDonate;
            this.maxGoldToDonate = maxGoldToDonate;
            this.minPeopleInWedding = minPeopleInWedding;
            this.maxPeopleInWedding = maxPeopleInWedding;
            this.embarrassedSoliderMaxGold = embarrassedSoliderMaxGold;
            this.minMoraleGain = minMoraleGain;
            this.maxMoraleGain = maxMoraleGain;
            this.minGoldRaided = minGoldRaided;
            this.maxGoldRaided = maxGoldRaided;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new UnexpectedWedding();
        }
    }
}