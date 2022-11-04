using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class NotOfThisWorld : BaseEvent
    {
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

            return true;
        }

        public override void StartEvent()
        {
            if (Settings.ModSettings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }
            
            var eventTitle = new TextObject("{=NotOfThisWorld_Title}Not of this world").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var soldiersInvestigating = MBRandom.RandomInt(minSoldiersToDisappear, maxSoldiersToDisappear);

            MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(soldiersInvestigating, false);
            
            var eventPt1 =new TextObject(
                    "{=NotOfThisWorld_Part_1}Your party is currently resting in the vicinity of {closestSettlement} when you observe a strange object descending from the sky and into the nearby forest. " +
                    "{soldiersInvestigating} of your men decide to go investigate, but you remain at the camp.\n After a few minutes you see an object leaving the forest and head towards the sky at an " +
                    "unbelievable speed. Eventually only one of your men returns from the forest. He tells you a bizarre story. \n \n" +
                    "When your men entered the forest they could see some object amongst the trees. This object was emitting an intense heat. After a few seconds an opening appeared " +
                    "and three small figures in a strange armor got out. One of your men tried to approach the entities, but was hit in the abdomen by a strange glowing arrow that came out of a small " +
                    "weapon the entities had. The man subsequently dropped dead.\n At this point the survivor hid in some bushes, but he witnessed the events that followed. " +
                    "He told you that the entities proceeded to kill the other {killedSoldiers} soldiers who went to investigate.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("soldiersInvestigating", soldiersInvestigating)
                .SetTextVariable("killedSoldiers", soldiersInvestigating - 1)
                .ToString();
            
            var eventPt2 =new TextObject(
                    "{=NotOfThisWorld_Part_2}These creatures then proceeded to load the bodies of the soldiers into the vessel while speaking a language unlike anything ever heard before. " +
                    "They also seemed to load up pieces of the nearby flora as if taking samples for study. After a few minutes the vessel leaves without a sound towards the sky.\n" +
                    "The soldier who returned from this ordeal was a trusted man whom you are inclined to believe. You tell him to go get some food and go to bed. As he is heading towards his " +
                    "tent he suddenly bends over and starts vomiting violently. He then falls to the ground in agony while blood is flowing from all his facial orifices. " +
                    "You grab a sword and put him out of his misery and order your men to burn his body at once.\n \n" +
                    "When you return to your tent you are shaking. You try to make a note of this event in your diary but you find yourself too distraught to write anything. " +
                    "You ponder the question of who or what could have killed your men like this. The answer may never be known.")
                .ToString();

            var eventButtonText1 = new TextObject("{=NotOfThisWorld_Event_Button_Text_1}Read on!").ToString();
            var eventButtonText2 = new TextObject("{=NotOfThisWorld_Event_Button_Text_2}Try to sleep").ToString();
            
            var eventMsg1 =new TextObject(
                    "{=NotOfThisWorld_Event_Msg_1}{soldiersInvestigating} of your men were killed by the mysterious entities.")
                .SetTextVariable("soldiersInvestigating", soldiersInvestigating)
                .ToString();

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventPt1, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventPt2, true, false, eventButtonText2, null, null, null), true);
            
            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.MsgColor));
            

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