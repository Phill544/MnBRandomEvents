using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class NotOfThisWorld : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minSoldiersToDisappear;
        private readonly int maxSoldiersToDisappear;

        public NotOfThisWorld() : base(ModSettings.RandomEvents.NotOfThisWorldData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("NotOfThisWorld", "EventDisabled");
            minSoldiersToDisappear = ConfigFile.ReadInteger("NotOfThisWorld", "MinSoldiersToDisappear");
            maxSoldiersToDisappear = ConfigFile.ReadInteger("NotOfThisWorld", "MaxSoldiersToDisappear");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minSoldiersToDisappear != 0 || maxSoldiersToDisappear != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && GeneralSettings.SupernaturalEvents.IsDisabled() == false && MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxSoldiersToDisappear && CurrentTimeOfDay.IsNight;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var soldiersInvestigating = MBRandom.RandomInt(minSoldiersToDisappear, maxSoldiersToDisappear);
            
            MobileParty.MainParty.MemberRoster.KillNumberOfNonHeroTroopsRandomly(soldiersInvestigating);
            
            var eventPt1 =new TextObject(EventTextHandler.GetRandomEventPart1())
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("soldiersInvestigating", soldiersInvestigating)
                .SetTextVariable("killedSoldiers", soldiersInvestigating - 1)
                .ToString();
            
            var eventPt2 =new TextObject(EventTextHandler.GetRandomEventPart2())
                .ToString();

            var eventButtonText1 = new TextObject("{=NotOfThisWorld_Event_Button_Text_1}Read on!").ToString();
            var eventButtonText2 = new TextObject("{=NotOfThisWorld_Event_Button_Text_2}Try to sleep").ToString();
            
            var eventMsg1 =new TextObject(EventTextHandler.GetRandomEventConcludedMsg())
                .SetTextVariable("soldiersInvestigating", soldiersInvestigating)
                .ToString();

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventPt1, true, false, eventButtonText1, null, null, null), true);
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventPt2, true, false, eventButtonText2, null, null, null), true);
            
            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
            

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
        
        private static class EventTextHandler
        {
            private static readonly Random random = new Random();
            
            private static readonly List<string> eventTitles = new List<string>
            {
                "{=NotOfThisWorld_Title_A}Not of This World",
                "{=NotOfThisWorld_Title_B}Alien Encounters",
                "{=NotOfThisWorld_Title_C}Extraterrestrial Visit",
                "{=NotOfThisWorld_Title_D}Beyond Earthly Realms",
                "{=NotOfThisWorld_Title_E}Interstellar Mystery",
                "{=NotOfThisWorld_Title_F}Cosmic Strangers",
                "{=NotOfThisWorld_Title_G}Unearthly Beings",
                "{=NotOfThisWorld_Title_H}Visitors from the Stars"
            };
            
            private static readonly List<string> eventPart1 = new List<string>
            {
                //Event Part 1A
                "{=NotOfThisWorld_Part_1A}Your party is currently resting in the vicinity of {closestSettlement} when " +
                "you observe a strange object descending from the sky and into the nearby forest. {soldiersInvestigating} " +
                "of your men decide to go investigate, but you remain at the camp. After a few minutes you see an " +
                "object leaving the forest and head towards the sky at an unbelievable speed. Eventually only one of " +
                "your men returns from the forest. He tells you a bizarre story. When your men entered the " +
                "forest they could see some object amongst the trees. This object was emitting an intense heat. After " +
                "a few seconds an opening appeared and three small figures in a strange armor got out. One of your men " +
                "tried to approach the entities, but was hit in the abdomen by a strange glowing arrow that came out of " +
                "a small weapon the entities had. The man subsequently dropped dead. At this point the survivor hid in " +
                "some bushes, but he witnessed the events that followed. He told you that the entities proceeded to kill " +
                "the other {killedSoldiers} soldiers who went to investigate.",
                
                //Event Part 1B
                "{=NotOfThisWorld_Part_1B}Resting near {closestSettlement}, your group sees a strange object descending " +
                "into the nearby woods. Intrigued, {soldiersInvestigating} of your men decide to go investigate this " +
                "curious phenomenon, leaving you at the camp. After a short while, you observe the object re-emerging from " +
                "the forest and ascending into the sky at an astonishing speed. Eventually, only one of your men returns, " +
                "appearing shaken. He tells you a bizarre story. Upon entering the forest, they encountered a mysterious " +
                "object amidst the trees, which was emanating an intense heat and strange light. Suddenly, an opening appeared " +
                "and three small figures, clad in unfamiliar, shining armor, stepped out. As one of your men moved to approach " +
                "these entities, he was struck in the abdomen by a glowing projectile emitted from a small, futuristic weapon " +
                "the entities wielded. He died almost instantly. The survivor, having concealed himself in dense undergrowth, " +
                "witnessed the rest of the events unfold. He recounts that the entities then proceeded to methodically eliminate the " +
                "other {killedSoldiers} soldiers. After completing their grim task, they re-entered their craft, leaving the " +
                "forest as quickly as they had arrived.",
                
                //Event Part 1C
                "{=NotOfThisWorld_Part_1C}While your party relaxes in the vicinity of {closestSettlement}, an unusual object " +
                "catches your eye as it descends into the forest. Curious, {soldiersInvestigating} of your men decide to " +
                "investigate the mysterious occurrence, while you opt to stay at the camp. Not long after, the object swiftly " +
                "leaves the forest, shooting towards the sky at a breathtaking velocity. Finally, only one of your soldiers returns, " +
                "looking bewildered and alarmed. He shares a rather extraordinary tale with you. He explains that as they " +
                "ventured into the forest, they discovered an object that was emitting both heat and a bright light. A hatch " +
                "opened, revealing three small, oddly dressed figures in what seemed like advanced armor. One soldier, daring " +
                "to get closer, was suddenly struck by a luminous arrow, launched from a peculiar, compact weapon carried by the " +
                "figures. He collapsed, lifeless. The remaining soldier, who had prudently hidden behind some bushes, silently " +
                "observed the subsequent actions of these beings. They methodically and coldly killed the remaining {killedSoldiers} " +
                "soldiers. After their ruthless act, they quickly retreated to their vessel and disappeared into the sky.",
                
                //Event Part 1D
                "{=NotOfThisWorld_Part_1D}As you and your party take a break near {closestSettlement}, a mysterious object " +
                "descends into the nearby forest, catching everyone's attention. {soldiersInvestigating} of your brave " +
                "men decide to delve into the woods to uncover the source of this enigma, leaving you behind at the camp. " +
                "Moments later, you witness the object rapidly ascend from the forest canopy, vanishing into the sky at an " +
                "incredible pace. Only one of your soldiers manages to return from the investigation, his expression one of shock " +
                "and disbelief. He recounts a strange and unsettling encounter. He tells you that upon entering the forest, " +
                "they stumbled upon an unusual object among the trees, which was radiating an extraordinary amount of heat " +
                "and light. An aperture appeared on the object, and from it emerged three small figures, donning unfamiliar, " +
                "reflective armor. When one of your men attempted to communicate or approach them, he was hit in the abdomen " +
                "by a radiant arrow from a strange, small weapon they possessed, resulting in his immediate death. The surviving " +
                "soldier, who had wisely taken cover in the thick underbrush, watched in horror as the alien figures proceeded to " +
                "eliminate systematically the other {killedSoldiers} soldiers who had accompanied him. After their lethal mission, " +
                "the beings retreated back into their craft and swiftly departed, leaving behind a trail of mystery and death.",
                
                //Event Part 1E
                "{=NotOfThisWorld_Part_1E}In the vicinity of {closestSettlement}, your party pauses as an unidentifiable object makes " +
                "its descent into the neighboring forest. Driven by curiosity, {soldiersInvestigating} of your men set out to " +
                "explore the source of this unusual occurrence, while you choose to remain at the campsite. Shortly thereafter, " +
                "the object is seen hastily exiting the forest, soaring into the sky at an incredible rate. Only one of your men " +
                "makes it back, visibly disturbed and anxious. He recounts an eerie and unexpected experience. He describes " +
                "how they came across a peculiar object hidden amongst the trees, emitting an intense heat and a piercing " +
                "light. Abruptly, an entryway opened on the object, and out stepped three small figures in what appeared to " +
                "be advanced, gleaming armor. When one brave soldier approached them, he was immediately struck down by a " +
                "shining arrow shot from a compact, unknown weapon that the figures carried. He died instantly. The remaining " +
                "soldier, having hidden himself among dense foliage, remained unseen and watched the unsettling events unfold. " +
                "He observed the figures as they coldly and efficiently killed the rest of the {killedSoldiers} soldiers who had " +
                "ventured into the forest. Completing their grim task, they returned to their vessel and left as quickly as " +
                "they had arrived, leaving a sense of foreboding and fear in their wake."
            };
            
            private static readonly List<string> eventPart2 = new List<string>
            {
                //Event Part 2A
                "{=NotOfThisWorld_Part_2A}These creatures then proceeded to load the bodies of the soldiers into the vessel " +
                "while speaking a language unlike anything ever heard before. They also seemed to load up pieces of the " +
                "nearby flora as if taking samples for study. After a few minutes the vessel leaves without a sound " +
                "towards the sky. The soldier who returned from this ordeal was a trusted man whom you are inclined " +
                "to believe. You tell him to go get some food and go to bed. As he is heading towards his tent he " +
                "suddenly bends over and starts vomiting violently. He then falls to the ground in agony while blood is " +
                "flowing from all his facial orifices. You grab a sword and put him out of his misery and order your men " +
                "to burn his body at once. When you return to your tent you are shaking. You try to make a note of " +
                "this event in your diary but you find yourself too distraught to write anything. You ponder the " +
                "question of who or what could have killed your men like this. The answer may never be known.",
                
                //Event Part 2B
                "{=NotOfThisWorld_Part_2B}The mysterious beings began methodically loading the fallen soldiers' bodies onto " +
                "their craft, all the while conversing in an unintelligible, alien language. They appeared to be collecting " +
                "samples of the local plants too, perhaps for some unknown research. Minutes later, the vessel ascended " +
                "silently into the sky and vanished. The surviving soldier, a reliable and trusted member of your team, " +
                "seemed genuinely shaken. Believing his account, you instruct him to eat and rest. As he heads to his " +
                "tent, he suddenly doubles over, violently vomiting. Within moments, he collapses in excruciating pain, " +
                "blood streaming from his eyes, nose, and mouth. You swiftly end his suffering with your sword and order " +
                "an immediate cremation of his body. Returning to your tent, a sense of dread overwhelms you. " +
                "You attempt to document the harrowing incident in your diary, but find yourself too unsettled to " +
                "write coherently. The thought of what entity could have inflicted such horror upon your men " +
                "lingers hauntingly in your mind, a question that might remain forever unanswered.",
                
                //Event Part 2C
                "{=NotOfThisWorld_Part_2C}The alien figures efficiently transferred the soldiers' corpses into their vessel, " +
                "speaking in a bizarre, unheard language. They also gathered samples of the forest's vegetation, suggesting " +
                "scientific interest. Shortly, the craft lifted off noiselessly into the sky. The returned soldier, known for " +
                "his honesty, was visibly distressed. Trusting his testimony, you advise him to eat and sleep. However, as he " +
                "moves towards his tent, he suddenly starts to retch violently, then falls, blood oozing from his facial " +
                "orifices. Acting quickly, you mercifully kill him with your sword and command your men to burn the body " +
                "without delay. Back in your tent, you're visibly shaken. Trying to record the event in your diary proves " +
                "futile, as you're too disturbed. The identity of your men's killer and the nature of the attack plague your " +
                "thoughts, a mystery that might never be solved.",
                
                //Event Part 2D
                "{=NotOfThisWorld_Part_2D}The strange creatures proceeded to load the deceased soldiers into their ship, " +
                "speaking a language that defied comprehension. They also seemed to collect various plant specimens, likely " +
                "for analysis. After completing their eerie task, the vessel ascended silently into the heavens. The soldier " +
                "who returned, usually steadfast and believable, was clearly traumatized. Accepting his account, you tell him " +
                "to rest and eat. Tragically, as he walks to his tent, he is overtaken by violent vomiting and collapses, " +
                "blood pouring from every facial opening. You end his agony swiftly with your sword and order an immediate " +
                "cremation of his body. Shaken, you return to your tent, struggling to process the day's events. You open " +
                "your diary to document the occurrence but find yourself too distraught. The question of what could have " +
                "annihilated your men in such a manner lingers ominously, likely remaining an unsolved enigma.",
                
                //Event Part 2E
                "{=NotOfThisWorld_Part_2E}The entities began to transport the soldiers' bodies into their spacecraft, " +
                "communicating in an alien dialect that was completely foreign. They also appeared to be collecting " +
                "samples of the local flora, possibly for analysis. Within minutes, the vessel departed silently skyward. " +
                "The soldier who made it back, known for his reliability, was visibly shaken by the experience. Taking " +
                "his word, you suggest he get some food and rest. However, as he staggers to his tent, he is suddenly " +
                "struck by violent vomiting, collapsing in pain as blood flows from his face. With a heavy heart, you " +
                "end his suffering with your sword and order a prompt cremation. You return to your tent, trembling from " +
                "the ordeal. Attempting to note down the event in your diary, you find yourself too distraught to write. " +
                "The question of what force could have decimated your men in such a way haunts you, a mystery that might " +
                "forever remain unsolved."

            };
            
            private static readonly List<string> eventConcludedMsg = new List<string>
            {
                "{=NotOfThisWorld_Event_Msg_1A}{soldiersInvestigating} of your men were killed by the mysterious entities.",
                "{=NotOfThisWorld_Event_Msg_1B}{soldiersInvestigating} soldiers met their end at the hands of the enigmatic beings.",
                "{=NotOfThisWorld_Event_Msg_1C}The alien figures claimed the lives of {soldiersInvestigating} of your troops.",
                "{=NotOfThisWorld_Event_Msg_1D}Mysterious creatures have fatally struck down {soldiersInvestigating} of your soldiers.",
                "{=NotOfThisWorld_Event_Msg_1E}{soldiersInvestigating} of your men fell victim to the lethal force of the unknown entities."

            };
            
            public static string GetRandomEventTitle()
            {
                var index = random.Next(eventTitles.Count);
                return eventTitles[index];
            }
            
            public static string GetRandomEventPart1()
            {
                var index = random.Next(eventPart1.Count);
                return eventPart1[index];
            }
            
            public static string GetRandomEventPart2()
            {
                var index = random.Next(eventPart2.Count);
                return eventPart2[index];
            }
            
            public static string GetRandomEventConcludedMsg()
            {
                var index = random.Next(eventConcludedMsg.Count);
                return eventConcludedMsg[index];
            }
        }
    }


    public class NotOfThisWorldData : RandomEventData
    {

        public NotOfThisWorldData(string eventType, float chanceWeight) : base(eventType,
            chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new NotOfThisWorld();
        }
    }
}