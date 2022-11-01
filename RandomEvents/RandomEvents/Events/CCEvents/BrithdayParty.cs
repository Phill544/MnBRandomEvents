using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class BirthdayParty : BaseEvent
    {
        private const string EventTitle = "The Birthday Party";
        
        private readonly int minAttending;
        private readonly int maxAttending;
        private readonly int minYourMenAttending;
        private readonly int maxYourMenAttending;
        private readonly int minAge;
        private readonly int maxAge;
        private readonly int minBandits;
        private readonly int maxBandits;
        private readonly int minGoldGiven;
        private readonly int maxGoldGiven;
        private readonly int minRenownGain;
        private readonly int maxRenownGain;

        public BirthdayParty() : base(Settings.ModSettings.RandomEvents.BirthdayPartyData)
        {
            minAttending = Settings.ModSettings.RandomEvents.BirthdayPartyData.minAttending;
            maxAttending = Settings.ModSettings.RandomEvents.BirthdayPartyData.maxAttending;
            minYourMenAttending = Settings.ModSettings.RandomEvents.BirthdayPartyData.minYourMenAttending;
            maxYourMenAttending = Settings.ModSettings.RandomEvents.BirthdayPartyData.maxYourMenAttending;
            minAge = Settings.ModSettings.RandomEvents.BirthdayPartyData.minAge;
            maxAge = Settings.ModSettings.RandomEvents.BirthdayPartyData.maxAge;
            minBandits = Settings.ModSettings.RandomEvents.BirthdayPartyData.minBandits;
            maxBandits = Settings.ModSettings.RandomEvents.BirthdayPartyData.maxBandits;
            minGoldGiven = Settings.ModSettings.RandomEvents.BirthdayPartyData.minGoldGiven;
            maxGoldGiven = Settings.ModSettings.RandomEvents.BirthdayPartyData.maxGoldGiven;
            minRenownGain = Settings.ModSettings.RandomEvents.BirthdayPartyData.minRenownGain;
            maxRenownGain = Settings.ModSettings.RandomEvents.BirthdayPartyData.maxRenownGain;
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

            var birthdayAge = MBRandom.RandomInt(minAge, maxAge);
            var yourMenAttending = MBRandom.RandomInt(minYourMenAttending, maxYourMenAttending);
            var peopleAttending = MBRandom.RandomInt(minAttending, maxAttending);
            var bandits = MBRandom.RandomInt(minBandits, maxBandits);
            var goldGiven = MBRandom.RandomInt(minGoldGiven, maxGoldGiven);
            var renownGain = MBRandom.RandomInt(minRenownGain, maxRenownGain);
            
            var settlements = Settlement.FindAll(s => s.IsTown || s.IsCastle || s.IsVillage ).ToList();
            var closestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", "Join them in celebration", null, true, $"Not everyday you turn {birthdayAge} years old!"),
                new InquiryElement("b", "Give the girls some gold", null, true, "You don't have time to stay but you can still be nice right ? "),
                new InquiryElement("c", "Leave", null, true, "Don't have time")
            };
            

            var msid = new MultiSelectionInquiryData(
                EventTitle,
                $"As you and your party are traveling in the vicinity of {closestSettlement}, you come across {peopleAttending} people in what seems to be a birthday party for a young girl. A couple of the guests invites " +
                $"you to join them in celebrating the girls {birthdayAge} birthday! What should you do?",
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
                                    $"You and {yourMenAttending} of your men decide to stay for the party while the rest makes their way to {closestSettlement}. You approach the girl and give her {goldGiven} gold as a gift. " +
                                    "She give you a hug and a thank you. You get yourself some beer and sit down to enjoy the moment.\n \n" +
                                    $"Some time later, {bandits} bandits decide to crash the party. They go around from person to person and takes everything of value. You order your men to stand down as you don't want to " +
                                    "start a fight with innocents caught in the middle. After they have taken everything of value they also try to take the young girl with them. This you will not stand for so you signal " +
                                    $"your men to strike. You and your men make quick work in incapacitating the bandits. One of your men rides to {closestSettlement} to fetch someone to throw these scum in the dungeon. \n" +
                                    "The rest of the night you are celebrated as a hero! You even got to dance with the birthday girl!",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            Clan.PlayerClan.AddRenown(renownGain);
                            break;
                        case "b":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    $"Your really dont' have time to stay but you don't want to be rude either. You manage to scrape together {goldGiven} gold and give it to the girl as a gift. She seems grateful. \n" +
                                    $"You say your goodbyes to the partygoers and you leave in the direction of {closestSettlement}.",
                                    true, false, "Done", null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            break;
                        case "c":
                        {
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    $"You don't have time for this so you leave for {closestSettlement} but not before casting one last look at the party.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        }
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


    public class BirthdayPartyData : RandomEventData
    {
        public readonly int minAttending;
        public readonly int maxAttending;
        public readonly int minYourMenAttending;
        public readonly int maxYourMenAttending;
        public readonly int minAge;
        public readonly int maxAge;
        public readonly int minBandits;
        public readonly int maxBandits;
        public readonly int minGoldGiven;
        public readonly int maxGoldGiven;
        public readonly int minRenownGain;
        public readonly int maxRenownGain;

        public BirthdayPartyData(
            string eventType,
            float chanceWeight,
            int minAttending,
            int maxAttending,
            int minYourMenAttending,
            int maxYourMenAttending,
            int minAge, int maxAge,
            int minBandits,
            int maxBandits,
            int minGoldGiven,
            int maxGoldGiven,
            int minRenownGain,
            int maxRenownGain
            ) : base(eventType, chanceWeight)
        {
            this.minAttending = minAttending;
            this.maxAttending = maxAttending;
            this.minYourMenAttending = minYourMenAttending;
            this.maxYourMenAttending = maxYourMenAttending;
            this.minAge = minAge;
            this.maxAge = maxAge;
            this.minBandits = minBandits;
            this.maxBandits = maxBandits;
            this.minGoldGiven = minGoldGiven;
            this.maxGoldGiven = maxGoldGiven;
            this.minRenownGain = minRenownGain;
            this.maxRenownGain = maxRenownGain;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new BirthdayParty();
        }
    }
}