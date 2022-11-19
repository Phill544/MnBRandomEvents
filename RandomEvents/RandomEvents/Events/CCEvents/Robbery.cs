using System;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class Robbery : BaseEvent
    {
        private readonly int minGoldLost;
        private readonly int maxGoldLost;
        private readonly int minRenownLost;
        private readonly int maxRenownLost;

        public Robbery() : base(ModSettings.RandomEvents.RobberyData)
        {
            minGoldLost = MCM_MenuConfig_N_Z.Instance.RO_MinGoldLost;
            maxGoldLost = MCM_MenuConfig_N_Z.Instance.RO_MaxGoldLost;
            minRenownLost = MCM_MenuConfig_N_Z.Instance.RO_MinRenownLost;
            maxRenownLost = MCM_MenuConfig_N_Z.Instance.RO_MaxRenownLost;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            var notables = Settlement.CurrentSettlement.Notables.ToList();
            var gangLeaders = notables.Where(character => character.IsGangLeader).ToList();

            if (gangLeaders.Count != 0)
            {
                return Settlement.CurrentSettlement != null && CampaignTime.Now.IsNightTime;
            }

            return false;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.Dbg_Color));
            }
            
            var eventTitle = new TextObject("{=Robbery_Title}You've been robbed!").ToString();

            var currentSettlement = Settlement.CurrentSettlement.Name.ToString();

            var thugs = MBRandom.RandomInt(5, 30);
            
            var goldLost = MBRandom.RandomInt(minGoldLost, maxGoldLost);

            var renownLost = MBRandom.RandomInt(minRenownLost, maxRenownLost);

            var clanRenown = Clan.PlayerClan.Renown;

            var notables = Settlement.CurrentSettlement.Notables.ToList();
            
            var gangLeaders = notables.Where(character => character.IsGangLeader).ToList();

            var random = new Random();
            var index = random.Next(gangLeaders.Count);

            var gangLeader = gangLeaders[index].Name.ToString();

            var eventText =new TextObject(
                    "{=Robbery_Event_Text}While leaving the tavern late at night in {currentSettlement} you decide to take a shortcut back through an alley. You quickly realize that this was a bad idea as you are surrounded by {thugs} thugs " +
                    "who are threatening you. Their leader, {gangLeader}, tells you to hand over any valuables you have on you. Knowing you are outnumbered, you comply and hand over {goldLost} gold to {gangLeader} and the thugs.\n\n" +
                    "{gangLeader} tells the thugs to teach you a lesson that will keep you out of their alley next time. The next few minutes are a blur as you are punched, kicked, thrown and intimidated.\nYou black out\n\n" +
                    "You awake some time later only to realize the thugs have dumped you in a pigsty. You stumble to your feet and make haste back to where you are staying.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeader)
                .SetTextVariable("goldLost", goldLost)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=Robbery_Event_Msg_1}Clan {playerClan} lost {renownLost} renown after word spread that {heroName} was robbed by {gangLeader}'s thugs!")
                .SetTextVariable("playerClan", Clan.PlayerClan.Name.ToString())
                .SetTextVariable("renownLost", renownLost)
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeader)
                .ToString();


            var eventButtonText = new TextObject("{=Robbery_Event_Button_Text}Rough Night").ToString();

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText, true, false, eventButtonText, null, null, null), true);

            Clan.PlayerClan.Renown = clanRenown - renownLost;
            
            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));

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


    public class RobberyData : RandomEventData
    {
        public RobberyData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new Robbery();
        }
    }
}