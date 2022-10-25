using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class RunawaySon : BaseEvent
    {
        private const string EventTitle = "Runaway Son";
        
        private readonly int minGold;
        private readonly int maxGold;

        public RunawaySon() : base(Settings.Settings.RandomEvents.RunawaySonData)
        {
            minGold = Settings.Settings.RandomEvents.RunawaySonData.minGold;
            maxGold = Settings.Settings.RandomEvents.RunawaySonData.maxGold;
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
            if (Settings.Settings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", "Take him in and train him", null, true, "You could use the distraction of having someone to train"),
                new InquiryElement("b", "Tell him he can tag along", null, true, "You really don't have time to babysit him"),
                new InquiryElement("c", "Go away", null, true, "He needs to leave"),
                new InquiryElement("d", "Kill him", null, true, "It's a cruel world")
            };

            var goldLooted = MBRandom.RandomInt(minGold, maxGold);

            var msid = new MultiSelectionInquiryData(
                EventTitle,
                "As your party moves through the land you are approached by a young man. He explains that he ran away from the family farm after suffering abuse from his parents for years. He wants to your party but he lacks any real combat skill.",
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
                                    "You tell him he is welcome in your ranks and you will personally train him and make a fine solider of him.",
                                    true, false, "Done", null, null, null), true);

                            GainOneRecruit();
                            break;
                        case "b":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You tell him he can tag along, but under no circumstance should he interfere in your affairs.",
                                    true, false, "Done", null, null, null), true);
                            GainOneRecruit();
                            break;
                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You tell him to get lost. The man turns around and promptly leaves.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    $"You laugh as you hear his plea and your men soon joins in. You approach the man and thrust a dagger into his stomach. You watch him fall to the ground in a pool of blood and screaming in pain. You kneel down beside him and watch as the light soon leaves his eyes and he dies from his injury. You and some men decide to cut him open and hang his body from a tree as a warning but not before looting his body for {goldLooted} gold.",
                                    true, false, "Done", null, null, null), true);

                            Hero.MainHero.ChangeHeroGold(goldLooted);
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

        private static void GainOneRecruit()
        {
            var settlements = Settlement.FindAll(s => !s.IsHideout).ToList();
            var closestSettlement =
                settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

            var bandits = PartySetup.CreateBanditParty();
            bandits.MemberRoster.Clear();
            PartySetup.AddRandomCultureUnits(bandits, 1, closestSettlement.Culture);
            MobileParty.MainParty.MemberRoster.Add(bandits.MemberRoster);
            bandits.RemoveParty();
        }
    }


    public class RunawaySonData : RandomEventData
    {
        public readonly int minGold;
        public readonly int maxGold;

        public RunawaySonData(string eventType, float chanceWeight, int minGold, int maxGold) : base(eventType,
            chanceWeight)
        {
            this.minGold = minGold;
            this.maxGold = maxGold;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new RunawaySon();
        }
    }
}