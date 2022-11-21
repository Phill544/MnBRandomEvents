using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class LightsInTheSkies : BaseEvent
    {
        public LightsInTheSkies() : base(ModSettings.RandomEvents.LightsInTheSkiesData)
        {
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_A_M.Instance.LitS_Disable == false && CampaignTime.Now.IsNightTime;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.Dbg_Color));
            }
            
            var eventTitle = new TextObject("{=LightsInTheSkies_Title}Lights In The Skies").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            var eventText1 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_1}Late one night, you are taking a walk in the woods near {closestSettlement} to clear your head. You manage to find a nice clearing not to far into the woods where " +
                    "you sit down and lean against a big boulder to relax. As you sit there you notice there is a light approaching in the sky from the south. You stand up and look at the light that now " +
                    "has come even closer. It's glowing and keeps changing colors, but it hovers in the sky without a sound. After you've stared at the light for a few minutes it disappears over the treeline. " +
                    "Having no clue to what you witnessed you make your way back but your are somewhat shocked when you see the sun is rising in the east as by your calculations it's just after midnight. You " +
                    "make haste back to the main camp.\nWhen you arrive some of your high ranking men runs towards you and asks you where you have been. Perplexed you answer them and tell them about the strange light. " +
                    "You are shocked when the men tells you that you have been gone for over 5 hours.\n\nA few hours later your party is on the move again but the men can clearly notice that you are preoccupied with " +
                    "last nights events.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventText2 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_2}When you finally decide to make camp for the night you go to wash yourself in your tent. While washing your face you notice that you are bleeding from your " +
                    "nose. You attempt to blow your noise, only to find a small metallic looking object came out of your nose. You pick it up and study it for a few seconds before it fades to ash before your eyes.\n\nYou " +
                    "also notice that you have several small, fresh scars on your extremities. They are small and all are identical, perfectly rounded with no signs of bleeding on the wounds. You are even more shocked when " +
                    "you realize that you are, in fact, missing two of your molars and the nails on your right foot are gone. This event now have you scared senseless.\n\nWhen the night finally falls, and you fall into sleep " +
                    "along with the fading daylight you suddenly awaken, drenched in sweat, heart beating fast and you also noticed that you have wet the bed. You try to recall the dream as you make your way to the nightstand " +
                    "where your diary is. You attempt to write down what you remember.")
                .ToString();
            
            var eventText3 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_3}“I am in the clearing at night looking at the strange light when I'm suddenly struck with an intense beam of some kind that lifts me from the ground and towards " +
                    "the light. As I got closer I could see that the light is in fact an object made from a silvery metal and that there is an opening in it where the light is pulling me into. I faint from horror.”\n\n" +
                    "“When I regain consciousness, I find myself strapped to a cold metallic table while I notice that I am surrounded by three strange creatures. These creatures look somewhat like humans, " +
                    "except from the fact they are somewhat smaller and their faces are covered in thick hair. Their ears and noses are for some reason hairless.”\n\n“I lie there unable to move as the creatures pokes me " +
                    "with several instruments and take notes, all while I can hear their unrecognizable language. Eventually they put a mask over my mouth and nose and I pass out again.”\n\n" +
                    "“When I come to I am standing in the clearing looking at the light disappearing across the treeline.”")
                .ToString();
            
            var eventText4 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_4}You finish writing and you cannot stop shaking so you go out of your tent to get some fresh air. Some of your men are still up and they can see that something is " +
                    "wrong but you assure them that everything is fine. You go to a highpoint in the camp and look out over the land as you try to make sense of this whole situation. You end up standing there for a " +
                    "few minutes.\n\nWhen you decide to leave you swear you can see the same strange light streaking across the sky in the distance one last time.")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=LightsInTheSkies_Event_Msg_1}{heroName} had a strange experience one night near {closestSettlement}.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("closestSettlement",closestSettlement)
                .ToString();
            
            var eventButtonText1 = new TextObject("{=LightsInTheSkies_Event_Button_Text_1}Read More").ToString();
            var eventButtonText2 = new TextObject("{=LightsInTheSkies_Event_Button_Text_2}Done").ToString();
            

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText1, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText2, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText3, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText4, true, false, eventButtonText2, null, null, null), true);

            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));

            MobileParty.MainParty.SetIsDisorganized(true);

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


    public class LightsInTheSkiesData : RandomEventData
    {
        public LightsInTheSkiesData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new LightsInTheSkies();
        }
    }
}