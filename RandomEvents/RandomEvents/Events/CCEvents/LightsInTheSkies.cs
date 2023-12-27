using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
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

        private bool HasValidEventData()
        {
            return eventDisabled == false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && GeneralSettings.SupernaturalEvents.IsDisabled() == false && CurrentTimeOfDay.IsNight;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=LightsInTheSkies_Title}Lights In The Skies").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

        /*
         * Follow these steps to decode the text:
         * 1. Visit https://cryptii.com/pipes/caesar-cipher. Choose 'Decode' and select the 'Nihilist cipher'.
         * 2. Input "elana" as the key.
         * 3. Copy the decoded output and go to https://crypt-online.ru/en/crypts/text2hex/.
         * 4. Choose the 'XX UTF-8' option and decode.
         * 5. Next, visit https://www.devglan.com/online-tools/text-encryption-decryption.
         * 6. Select 'decrypt' and use "bannerlord" as the Secret Key.
         * 7. The resulting string is the correctly decoded text.
         * 8. Search the string on Google to find its reference.
         */

            const string unknownText = "46 62 54 75 42 46 66 45 64 42 57 66 42 64 46 50 62 42 68 66 46 62 46 84 42 46 " +
                                       "66 52 64 42 50 82 42 64 44 76 62 42 68 44 46 62 54 77 42 46 65 52 64 42 50 74 " +
                                       "42 64 46 67 62 42 74 42 46 62 46 85 42 46 74 54 64 42 56 74 42 64 53 70 62 42 " +
                                       "76 52 46 62 53 86 42 46 65 52 64 42 58 73 42 64 45 46 62 42 68 56 46 62 52 74 " +
                                       "42 46 65 44 64 42 49 72 42 64 46 68 62 42 75 45 46 62 53 76 42 46 65 44 64 42 " +
                                       "57 84 42 64 52 56 62 42 68 45 46 62 46 74 42 46 74 44 64 42 58 74 42 64 46 67 " +
                                       "62 42 67 53 46 62 46 78 42 46 65 43 64 42 49 74 42 64 53 48 62 42 68 62 46 62 " +
                                       "46 85 42 46 65 54 64 42 58 75 42 64 46 59 62 42 76 53 46 62 46 84 42 46 65 54 " +
                                       "64 42 56 64 42 64 46 66 62 42 75 46 46 62 46 76 42 46 72 53 64 42 50 84 42 64 " +
                                       "53 69 62 42 74 55 46 62 52 76 42 46 73 53 64 42 58 66 42 64 53 67";

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
                    "party is on the move again but the men can clearly notice that you are preoccupied with last " +
                    "nights events.")
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
                    "{=LightsInTheSkies_Event_Text_Unknown}You turn the page and write down the sequence now burned into " +
                    "your mind.\n\n{unknownText}\n\n There are some symbols here you have never seen before. You wonder " +
                    "what this means.")
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
            var eventButtonText3 = new TextObject("{=LightsInTheSkies_Event_Button_Text_3}What?").ToString();
            
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText1, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText2, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText3, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText4, true, false, eventButtonText2, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventTextUnknown, true, false, eventButtonText3, null, null, null), true);
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