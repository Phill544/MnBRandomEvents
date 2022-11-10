using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.Library;

//New type of event I will attempt to make once the MCM is in place.
namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class AFlirtatiousEncounter : BaseEvent
    {

        public AFlirtatiousEncounter() : base(ModSettings.RandomEvents.AFlirtatiousEncounterData)
        {
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            //Will need a few conditions to align in order to execute.
            return true;
        }

        public override void StartEvent()
        {
            if (MCM_MenuConfig.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
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


    public class AFlirtatiousEncounterData : RandomEventData
    {
        public AFlirtatiousEncounterData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new AFlirtatiousEncounter();
        }
    }
}