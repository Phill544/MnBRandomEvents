using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class LightsInTheSkies : BaseEvent
    {
        private readonly bool eventDisabled;

        public LightsInTheSkies() : base(ModSettings.RandomEvents.LightsInTheSkiesData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("LightsInTheSkies", "EventDisabled");
        }

        public override void CancelEvent()
        {
        }
        
        private bool EventCanRun()
        {
            return eventDisabled == false;
        }

        public override bool CanExecuteEvent()
        {
            return EventCanRun() && GeneralSettings.SupernaturalEvents.IsDisabled() == false && CurrentTimeOfDay.IsNight;
        }

        public override void StartEvent()
        {
            if (GeneralSettings.DebugMode.IsActive())
            {
                var debugMsg = new TextObject(
                        "Starting “{randomEvent}”. This event has no configurable settings.\n\n" +
                        "However the string of characters you'll see in this event can be decrypted by following these steps:\n\n1: Decompress the string\n2:USCII Decrypt\n3: ASCII to text\n\n" +
                        "To disable these messages make sure you set the DebugMode = false in the ini settings\n\nThe ini file is located here : \n{path}"
                    )
                    .SetTextVariable("randomEvent", randomEventData.eventType)
                    .SetTextVariable("path", ParseIniFile.GetTheConfigFile())
                    .ToString();
                
                InformationManager.ShowInquiry(new InquiryData("Debug Info", debugMsg, true, false, "Start Event", null, null, null), true);
            }
            
            var eventTitle = new TextObject("{=LightsInTheSkies_Title}Lights In The Skies").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            const string unknownText = "eNqtVFEOQyEIuxIgTt630fsfab5lSd1GCEvki1CppRiJTsR8x0hnzFeVaabU9CHDeu07Xy+d6a5JIVNh3lHUVm+r/" +
                                       "T4Hvo3FzOTF0lRlRRkb+qlgkl3mKkAWK3BZQgVgjlFvNtSKCH/7l53NdS30Pkbje9GBzHM361qsHpvxFEzTFcLk+odzQ" +
                                       "D2+rFfeHHj3QHf/fjvOqIp3dOZdYYNZN2Lm2Cvc9v8/FGX5eALTyCCh";

            var eventText1 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_1}Late one night, you are taking a walk in the woods near " +
                    "{closestSettlement} to clear your head. You manage to find a nice clearing not to far into the " +
                    "woods where you sit down and lean against a big boulder to relax. As you sit there you notice " +
                    "there is a light approaching in the sky from the south. You stand up and look at the light that " +
                    "now has come even closer. It's glowing and keeps changing colors, but it hovers in the sky without " +
                    "a sound. After you've stared at the light for a few minutes it disappears over the treeline. " +
                    "Having no clue to what you witnessed you make your way back but you are somewhat shocked when you " +
                    "see the sun is rising in the east as by your calculations it's just after midnight. You make haste " +
                    "back to the main camp.\nWhen you arrive some of your high ranking men run towards you and asks you " +
                    "where you have been. Perplexed, you answer them and tell them about the strange light. You are " +
                    "shocked when the men tell you that you have been gone for over 5 hours.\nA few hours later your " +
                    "party is on the move again but the men can clearly notice that you are preoccupied with last nights events.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventText2 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_2}When you finally decide to make camp for the night you go to wash " +
                    "yourself in your tent. While washing your face you notice that you are bleeding from the nose. You " +
                    "attempt to blow your noise, only to find a small metallic looking object come out of your nose. " +
                    "You pick it up and study it for a few seconds before it crumbles to ash before your eyes.\n\nYou " +
                    "also notice that you have several small, fresh scars on your extremities. They are small, and all " +
                    "are identical, perfectly rounded with no signs of bleeding from the wounds. You are even more " +
                    "shocked when you realize that you are, in fact, missing two of your molars and the nails on your " +
                    "right foot are gone. This event now has you scared senseless.\n\nAs night falls, and you drifting " +
                    "to sleep along with the last remnants of daylight you suddenly awaken, drenched in sweat, heart " +
                    "beating fast and you also notice that you have wet the bed. You try to recall the dream as you " +
                    "make your way to the nightstand where your diary is. You attempt to write down what you remember.")
                .ToString();
            
            
            var eventText3 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_3}“I am in the clearing at night looking at the strange light when " +
                    "I'm suddenly struck with an intense beam of some kind that lifts me from the ground and towards " +
                    "the light. As I got closer I could see that the light is in fact an object made from a silvery " +
                    "metal and that there is an opening in it that the light is pulling me  towards. I faint from " +
                    "horror.”\n\n“When I regain consciousness, I find myself strapped to a cold metallic table and I " +
                    "notice that I am surrounded by three strange creatures. These creatures look somewhat like humans, " +
                    "except from the fact they are smaller and their faces are covered in thick hair. Their " +
                    "ears and noses are for some reason hairless.”")
                .ToString();
            
            var eventText4 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_4}“I lie there unable to move as the creatures poke " +
                    "me with several instruments and take notes, all while I can hear their unrecognizable language. " +
                    "After a few minutes they put a helmet over my face and burn a distinct sequence into my mind. They remove " +
                    "it after a few minutes and then they put a mask over my mouth and nose and I pass out again.”\n\n" +
                    "“When I come to I am standing in the clearing looking at the light disappearing across the treeline.”")
                .ToString();
            
            var eventTextUnknown =new TextObject(
                    "{=LightsInTheSkies_Event_Text_Unknown}You turn the page and write down the sequence now burned into your mind.\n\n{unknownText}\n\n" +
                    "There are some symbols here you have never seen before. You wonder what this means.")
                .SetTextVariable("unknownText", unknownText)
                .ToString();
            
            var eventText5 =new TextObject(
                    "{=LightsInTheSkies_Event_Text_5}As you finish writing and stare at the gibberish you have just written, " +
                    "you realize you cannot stop shaking so you leave your tent to get some fresh air. Some of your men are " +
                    "still up and they can see that something is wrong but you assure them that everything is fine. You go to " +
                    "a lookout point in the camp and watch over the land as you try to make sense of this whole situation. You " +
                    "end up standing there for a few minutes.\n\nWhen you decide to leave you swear you can see the same strange " +
                    "light streaking across the sky in the distance one last time.")
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
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventTextUnknown, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText5, true, false, eventButtonText2, null, null, null), true);

            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_MED_Outcome));

            MobileParty.MainParty.SetDisorganized(true);

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