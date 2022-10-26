using System;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class NotOfThisWorld : BaseEvent
    {
        private const string EventTitle = "Not of this world";
        
        private readonly int minSoldiersToDisappear;
        private readonly int maxSoldiersToDisappear;

        public NotOfThisWorld() : base(Settings.ModSettings.RandomEvents.NotOfThisWorldData)
        {
            minSoldiersToDisappear = Settings.ModSettings.RandomEvents.NotOfThisWorldData.minSoldiersToDisappear;
            maxSoldiersToDisappear = Settings.ModSettings.RandomEvents.NotOfThisWorldData.maxSoldiersToDisappear;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            /*
             * Ideally this can only run when the the troop count of the main party
             * is greater than maxSoldiersToDisappear.
             */
            
            return true;
        }

        public override void StartEvent()
        {
            if (Settings.ModSettings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }

            
            var settlements = Settlement.FindAll(s => s.IsTown || s.IsCastle || s.IsVillage ).ToList();
            var closestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

            var missingSoldiers = MBRandom.RandomInt(minSoldiersToDisappear, maxSoldiersToDisappear);

            MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(MobileParty.MainParty.MemberRoster.TotalManCount - missingSoldiers, false);
            
            InformationManager.ShowInquiry(
                new InquiryData(EventTitle,
                $"Your party is currently resting in the vicinity of {closestSettlement} when you observe a strange object descending from the sky and into the nearby forest. " +
                $"{missingSoldiers} of your men decide to go investigate, but you remain at the camp." +
                "After a few minutes you see an object leaving the forest and heads towards the sky in an unbelievable speed. Eventually only one of your men returns from the forest. He tells you a bizarre story. \n \n" +
                "When you men entered the forest they could see an object had landed in the forest. The object was emitting intense heat. After a few seconds an opening appeared and three small figures in a strange armor got out. " +
                "One of your men tried to approach the entities, but he was hit in the abdomen by a strange glowing arrow that came out of a small weapon the entities had. The man subsequently dropped dead.\n" +
                $"At this point the survivor had hid in some bushes , but he witnessed the events that followed. He told you that the entities proceeded to kill the other {missingSoldiers-1} who went to investigate.\n",
                    true,
                    false,
                    "Read on!",
                    null,
                    null,
                    null
                ),
                true);
            
            InformationManager.ShowInquiry(
                new InquiryData(EventTitle+" part II", 
                    "The entities then proceeded to load their bodies into the craft while speaking a language unlike anything ever heard before. The entities also seemed to load up pieces of the nearby flora as well almost like they " +
                    "are taking samples for study. After a few minutes the object leaves without a sound towards the sky.\n" +
                    "The man who returned from this ordeal is a man whom you trust so you are inclined to believe his story. You tell him to go get some food and go to bed. As he is heading towards his tent he suddenly bends over and starts " +
                    "vomiting violently. He then falls to the ground in agony while blood is flowing from all his facial orifices. You grab a sword and put him out of his misery and you order your men to burn his body at once.\n \n" +
                    $"When you return to your tent you are shaking. You try to make a note of this event in your diary but you find yourself too distraught to write anything. You ponder the question of who and what could have killed {missingSoldiers} " +
                    "of your men like this. The answer may never be known.",
                    true,
                    false,
                    "Try to sleep",
                    null,
                    null,
                    null
                ),
                true);

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


    public class NotOfThisWorldData : RandomEventData
    {
        public readonly int minSoldiersToDisappear;
        public readonly int maxSoldiersToDisappear;

        public NotOfThisWorldData(string eventType, float chanceWeight, int minSoldiersToDisappear, int maxSoldiersToDisappear) : base(eventType,
            chanceWeight)
        {
            this.minSoldiersToDisappear = minSoldiersToDisappear;
            this.maxSoldiersToDisappear = maxSoldiersToDisappear;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new NotOfThisWorld();
        }
    }
}