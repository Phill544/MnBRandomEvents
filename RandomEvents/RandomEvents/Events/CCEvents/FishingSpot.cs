using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class FishingSpot : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minSoldiersToGo;
        private readonly int maxSoldiersToGo;
        private readonly int maxFishCatch;
        private readonly int minMoraleGain;
        private readonly int maxMoraleGain;
        

        public FishingSpot() : base(ModSettings.RandomEvents.FishingSpotData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("FishingSpot", "EventDisabled");
            minSoldiersToGo = ConfigFile.ReadInteger("FishingSpot", "MinSoldiersToGo");
            maxSoldiersToGo = ConfigFile.ReadInteger("FishingSpot", "MaxSoldiersToGo");
            maxFishCatch = ConfigFile.ReadInteger("FishingSpot", "MaxFishCatch");
            minMoraleGain = ConfigFile.ReadInteger("FishingSpot", "MinMoraleGain");
            maxMoraleGain = ConfigFile.ReadInteger("FishingSpot", "MaxMoraleGain");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minSoldiersToGo != 0 || maxSoldiersToGo != 0 || maxFishCatch != 0 || minMoraleGain != 0 || maxMoraleGain != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxSoldiersToGo && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=FishingSpot_Title}A Great Fishing Spot").ToString();

            var soldiersGoneFishing = MBRandom.RandomInt(minSoldiersToGo, maxSoldiersToGo);
            var fishCaught = MBRandom.RandomInt(0, maxFishCatch);
            
            var moraleGained = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
            
            var fish = MBObjectManager.Instance.GetObject<ItemObject>("fish");
            
            var eventDescription = new TextObject(
                    "{=FishingSpot_Event_Desc}While camping, {soldiersGoneFishing} of your men decide they want to go to the lake just outside the camp to try and catch some fish.\n" +
                    "You could always use the additional resources and it would be a great morale booster for the party if they catch some. You tell them to be back before nightfall.")
                .SetTextVariable("soldiersGoneFishing", soldiersGoneFishing)
                .ToString();
            
            var eventOutcome1 = new TextObject("{=FishingSpot_Outcome_1}Your men return empty handed just before nightfall. At least they had a good time together.")
                .ToString();
            
            var eventOutcome2 = new TextObject("{=FishingSpot_Outcome_2}Your men return with {fishCaught} fish just before nightfall. At least its better than nothing. You let the fishermen enjoy their catch.")
                .SetTextVariable("fishCaught",fishCaught)
                .ToString();
            
            var eventOutcome3 = new TextObject("{=FishingSpot_Outcome_3}Your men return with {fishCaught} fish just before nightfall. This is a sizeable catch so you congratulate them. You join the fishermen in their feast.")
                .SetTextVariable("fishCaught",fishCaught)
                .ToString();
            
            var eventOutcome4 = new TextObject("{=FishingSpot_Outcome_4}Your men return triumphant with {fishCaught} fish just after nightfall. This is a massive catch so you congratulate them. You order your men to start preparing food for everyone.")
                .SetTextVariable("fishCaught",fishCaught)
                .ToString();
            
            var eventButtonText = new TextObject("{=FishingSpot_Event_Button_Text}Continue")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=FishingSpot_Event_Msg_1}Your men returned empty handed but it raised morale by {moraleGained}.")
                .SetTextVariable("moraleGained", MathF.Floor(moraleGained * 0.02f))
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=FishingSpot_Event_Msg_2}Your men returned with {fishCaught} and it raised morale by {moraleGained}.")
                .SetTextVariable("fishCaught", fishCaught)
                .SetTextVariable("moraleGained", MathF.Floor(moraleGained * 0.04f))
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=FishingSpot_Event_Msg_3}Your men returned with {fishCaught} and it raised morale by {moraleGained}.")
                .SetTextVariable("fishCaught", fishCaught)
                .SetTextVariable("moraleGained", MathF.Floor(moraleGained * 0.08f))
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=FishingSpot_Event_Msg_4}Your men returned with {fishCaught} and it raised morale by {moraleGained}.")
                .SetTextVariable("fishCaught", fishCaught)
                .SetTextVariable("moraleGained", moraleGained)
                .ToString();
            

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription, true, false, eventButtonText, null, null, null), true);
            
            MobileParty.MainParty.ItemRoster.AddToCounts(fish, fishCaught);

            if (fishCaught == 0)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false,eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += MathF.Floor(moraleGained * 0.02f);
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_MED_Outcome));
            }
            else if (fishCaught > 0 && fishCaught <= 5)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2, true, false, eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += MathF.Floor(moraleGained * 0.04f);
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
            }
            else if (fishCaught > 5 && fishCaught <= 15)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome3, true, false, eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += MathF.Floor(moraleGained * 0.08f);
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));
            }
            else if (fishCaught > 15 && fishCaught <= maxFishCatch)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome4, true, false, eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += moraleGained;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_POS_Outcome));
            }
            

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


    public class FishingSpotData : RandomEventData
    {

        public FishingSpotData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new FishingSpot();
        }
    }
}