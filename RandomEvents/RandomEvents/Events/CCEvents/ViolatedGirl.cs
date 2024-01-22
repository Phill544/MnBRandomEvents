using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class ViolatedGirl : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minGoldCompensation;
        private readonly int maxGoldCompensation;
        private readonly int minRogueryLevel;

        public ViolatedGirl() : base(ModSettings.RandomEvents.ViolatedGirlData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

            eventDisabled = ConfigFile.ReadBoolean("ViolatedGirl", "EventDisabled");
            minGoldCompensation = ConfigFile.ReadInteger("ViolatedGirl", "MinGoldCompensation");
            maxGoldCompensation = ConfigFile.ReadInteger("ViolatedGirl", "MaxGoldCompensation");
            minRogueryLevel = ConfigFile.ReadInteger("ViolatedGirl", "MinRogueryLevel");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minGoldCompensation != 0 || maxGoldCompensation != 0 || minRogueryLevel != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null &&
                   Hero.MainHero.Gold >= maxGoldCompensation && MobileParty.MainParty.MemberRoster.TotalRegulars >= 100;
        }

        public override void StartEvent()
        {
            var heroName = Hero.MainHero.FirstName;

            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

            var closestCity = ClosestSettlements.GetClosestTown(MobileParty.MainParty).ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            var goldToCompensate = MBRandom.RandomInt(minGoldCompensation, maxGoldCompensation);
            var totalCompensation = goldToCompensate + 300;

            var compensation = MBRandom.RandomInt(minGoldCompensation, maxGoldCompensation);

            var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription())
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();

            var heroRogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);

            var canKillWoman = false;

            var rogueryAppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {

                canKillWoman = true;

                rogueryAppendedText = new TextObject("{=ViolatedGirl_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**")
                        .ToString();

            }
            else
            {
                if (heroRogueryLevel >= minRogueryLevel)
                {
                    canKillWoman = true;

                    rogueryAppendedText = new TextObject("{=ViolatedGirl_Roguery_Appended_Text_1}[Roguery - lvl {minRogueryLevel}]")
                            .SetTextVariable("minRogueryLevel", minRogueryLevel)
                            .ToString();
                }
            }

            var eventOption1 = new TextObject("{=ViolatedGirl_Event_Option_1}Find the culprit").ToString();
            var eventOption1Hover = new TextObject("{=ViolatedGirl_Event_Option_1_Hover}This is unacceptable behaviour!").ToString();

            var eventOption2 = new TextObject("{=ViolatedGirl_Event_Option_2}Ask how much to keep this quiet?").ToString();
            var eventOption2Hover = new TextObject("{=ViolatedGirl_Event_Option_2_Hover}Everyone has a price.").ToString();

            var eventOption3 = new TextObject("{=ViolatedGirl_Event_Option_3}Tell her to leave").ToString();
            var eventOption3Hover = new TextObject("{=ViolatedGirl_Event_Option_3_Hover}Leave... NOW!").ToString();

            var eventOption4 = new TextObject("{=ViolatedGirl_Event_Option_4}[Roguery] Kill her").ToString();
            var eventOption4Hover = new TextObject("{=ViolatedGirl_Event_Option_4_Hover}She is too dangerous to be left alive.\n{rogueryAppendedText}")
                    .SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();

            var eventButtonText1 = new TextObject("{=ViolatedGirl_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=ViolatedGirl_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>();

            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));

            if (canKillWoman)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }

            var eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1())
                .SetTextVariable("closestCity", closestCity)
                .SetTextVariable("compensation", compensation)
                .ToString();

            var eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2())
                .SetTextVariable("goldToCompensate", goldToCompensate)
                .SetTextVariable("totalCompensation", totalCompensation)
                .ToString();


            var eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3())
                .ToString();

            var eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4())
                .ToString();

            var eventMsg1 = new TextObject(EventTextHandler.GetRandomEventMessage1())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("compensation", compensation)
                .SetTextVariable("closestCity", closestCity)
                .ToString();

            var eventMsg2 = new TextObject(EventTextHandler.GetRandomEventMessage2())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("totalCompensation", totalCompensation)
                .ToString();

            var eventMsg3 = new TextObject(EventTextHandler.GetRandomEventMessage3())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg4 = new TextObject(EventTextHandler.GetRandomEventMessage4())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("closestCity", closestCity)
                .ToString();


            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1,
                eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-compensation);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        case "b":
                        {
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-totalCompensation);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        }
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));
                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                }, null, null);

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

        private static class EventTextHandler
        {
            private static readonly Random random = new Random();

            private static readonly List<string> eventTitles = new List<string>
            {
                "{=ViolatedGirl_Title_A}Shattered Innocence" ,
                "{=ViolatedGirl_Title_B}Broken Blossom" ,
                "{=ViolatedGirl_Title_C}Lost Purity" ,
                "{=ViolatedGirl_Title_D}Tarnished Youth" ,
                "{=ViolatedGirl_Title_E}Faded Petals" ,
                "{=ViolatedGirl_Title_F}Silenced Flower" ,
                "{=ViolatedGirl_Title_G}Echoes of Harm" ,
                "{=ViolatedGirl_Title_H}Wilted Innocence"
            };

            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=ViolatedGirl_Event_Desc_A}As your party is resting near {closestSettlement}, you are approached by a " +
                "young girl. She asks to speak to you privately. You invite her into your tent to listen to what she has " +
                "to say. She claims that while you were in the previous town she was violated by one of your men. What " +
                "do you do?",

                //Event Description B
                "{=ViolatedGirl_Event_Desc_B}While encamped near {closestSettlement}, a distressed young girl seeks your " +
                "audience. In the privacy of your tent, she reveals a harrowing tale. She alleges that during your recent " +
                "stay in a nearby town, she was subjected to violation by a member of your party. Faced with this grave " +
                "accusation, you ponder your next course of action.",

                //Event Description C
                "{=ViolatedGirl_Event_Desc_C}During a rest near {closestSettlement}, a somber girl approaches, requesting " +
                "a private conversation. Once secluded in your tent, she hesitantly confides that she suffered a violation " +
                "at the hands of one of your soldiers in the last town you visited. The gravity of her claim weighs upon " +
                "you as you consider how to respond.",

                //Event Description D
                "{=ViolatedGirl_Event_Desc_D}Near {closestSettlement}, your camp is approached by a young girl, visibly " +
                "upset. She asks to speak with you alone. Inside your tent, she tearfully accuses one of your men of " +
                "violating her in the previous town you passed through. Her words leave you in a moral quandary, " +
                "contemplating your next move.",

                //Event Description E
                "{=ViolatedGirl_Event_Desc_E}As you make camp by {closestSettlement}, a young girl, fraught with emotion, " +
                "requests a private word. In the confines of your tent, she bravely discloses that she was violated by a " +
                "member of your party in the last town you visited. Her revelation presents you with a difficult decision " +
                "on how to proceed."
            };

            private static readonly List<string> eventChoice1 = new List<string>
            {
                //Event Choice 1A
                "{=ViolatedGirl_Event_Choice_1A}Upon hearing the girl's accusation, you immediately call your men to assemble. " +
                "With the girl's assistance, the perpetrator is identified among your ranks. After he shamefully admits his " +
                "guilt, you deliver a forceful punch as a mark of your fury and strip him of his rank. Following the girl's " +
                "desire for justice, you send him to {closestCity} for a proper trial, escorted by your men. To the girl, you " +
                "offer {compensation} gold as a personal apology for the wrongdoing. Grateful for your prompt and just response, " +
                "she thanks you before departing.",

                //Event Choice 1B
                "{=ViolatedGirl_Event_Choice_1B}Reacting with zero tolerance, you gather your troops and, with the girl's help, " +
                "quickly identify the guilty soldier. Confronting him, his confession to the crime enrages you. You reprimand him " +
                "publicly and demote him on the spot. He is then taken to {closestCity} to face legal judgment, as per the girl's " +
                "wish. You compensate the girl with {compensation} gold, apologizing for the incident. She expresses her gratitude " +
                "for your fairness and decisive action.",

                //Event Choice 1C
                "{=ViolatedGirl_Event_Choice_1C}You take immediate action, lining up your men for identification. The girl points " +
                "out the culprit, who, when questioned, admits his guilt. In a swift act of justice, you strike him for his misconduct, " +
                "remove his rank, and order his detention. Respecting the girl's demand for justice, he is sent to {closestCity} for " +
                "trial. You present the girl with {compensation} gold as a gesture of remorse. She thanks you, relieved by your swift " +
                "and just handling of the matter.",

                //Event Choice 1D
                "{=ViolatedGirl_Event_Choice_1D}With a sense of urgency, you summon all your men for an identification parade. The " +
                "girl nervously identifies the offender, who confesses under your questioning. Outraged, you deliver a punishing blow " +
                "and strip him of his duties, signaling your intolerance for such behavior. He is dispatched to {closestCity} for trial, " +
                "according to the girl's wishes. You give the girl {compensation} gold, apologizing for the harm caused. She departs, " +
                "thanking you for your prompt and empathetic response.",

                //Event Choice 1E
                "{=ViolatedGirl_Event_Choice_1E}Without hesitation, you call your men to gather for a lineup. With the girl's help, the " +
                "perpetrator is singled out. His admission of guilt leads to a swift reprimand from you and immediate demotion. Following " +
                "the girl's desire for justice, he is sent to {closestCity} under guard. You hand the girl {compensation} gold, expressing " +
                "your personal regret for the incident. She acknowledges your actions with gratitude, appreciating your swift and " +
                "respectful response."
            };

            private static readonly List<string> eventChoice2 = new List<string>
            {
                //Event Choice 2A
                "{=ViolatedGirl_Event_Choice_2A}After hearing the girl's account, you find yourself in a dilemma. While believing " +
                "her, you also consider the impact on your troops' morale. You discreetly inquire about a sum that would ensure her " +
                "silence. She mentions needing {goldToCompensate} gold for her ailing father and to move past the incident. To " +
                "demonstrate goodwill, you give her the requested amount plus an additional 300 gold. She leaves your camp quickly, " +
                "and the incident costs you a total of {totalCompensation} gold." ,
                       
                //Event Choice 2B
                "{=ViolatedGirl_Event_Choice_2B}You attentively listen to the girl and, understanding the sensitivity of the " +
                "situation, you propose a monetary settlement. She suggests {goldToCompensate} gold would suffice for her father's " +
                "care and her silence. You add 300 gold to the amount as a gesture of goodwill. Accepting the money, the girl " +
                "departs from your camp, leaving you with a total expense of {totalCompensation} gold." ,
                       
                //Event Choice 2C
                "{=ViolatedGirl_Event_Choice_2C}Recognizing the potential impact of the girl's story, you opt for a discreet " +
                "resolution. You offer compensation, and she requests {goldToCompensate} gold for her father's treatment and to " +
                "forget the ordeal. You hand her the sum plus an extra 300 gold, aiming to ensure her silence. She quickly exits " +
                "your camp, the entire matter costing you {totalCompensation} gold in total." ,
                       
                //Event Choice 2D
                "{=ViolatedGirl_Event_Choice_2D}After hearing the girl's distressing story, you weigh the risks and decide to " +
                "settle the matter quietly. She asks for {goldToCompensate} gold to aid her sick father and keep the incident " +
                "to herself. You agree and generously add 300 gold to the amount. With the gold in hand, she hastily leaves your " +
                "camp, the resolution setting you back {totalCompensation} gold." ,
                               
                //Event Choice 2E
                "{=ViolatedGirl_Event_Choice_2E}Upon understanding the girl's predicament, you suggest a financial settlement " +
                "to maintain discretion. She indicates that {goldToCompensate} gold would cover her father's medical expenses " +
                "and her silence. You provide the requested gold along with an additional 300 gold, aiming for a peaceful " +
                "resolution. She departs swiftly, and you tally the cost of this quiet settlement to be {totalCompensation} gold."

            };

            private static readonly List<string> eventChoice3 = new List<string>
            {
                //Event Choice 3A
                "{=ViolatedGirl_Event_Choice_3A}You express skepticism towards the girl's tale and state that you cannot " +
                "act solely on her accusation. You order three guards to escort her out of the camp. As she departs, she " +
                "hurls a string of profanities in your direction, clearly upset by your disbelief." ,
                
                //Event Choice 3B
                "{=ViolatedGirl_Event_Choice_3B}You doubt the girl's story and decide not to take any action without concrete " +
                "evidence. Three of your guards are instructed to lead her away from your camp. She leaves, shouting insults " +
                "and curses back at you, angered by your dismissal of her claim." ,
                
                //Event Choice 3C
                "{=ViolatedGirl_Event_Choice_3C}Skeptical of the girl's allegations, you conclude that her word alone isn't " +
                "enough to warrant action. You signal for three guards to remove her from your presence. She exits the camp, " +
                "lobbing angry accusations and expletives at you." ,
                
                //Event Choice 3D
                "{=ViolatedGirl_Event_Choice_3D}Unconvinced by the girl's account, you refuse to take her claim at face value. " +
                "Three guards are assigned to ensure her departure from the camp. She storms out, unleashing a torrent of " +
                "harsh words towards you." ,
                
                //Event Choice 3E
                "{=ViolatedGirl_Event_Choice_3E}You express doubts about the authenticity of the girl's story and refuse to " +
                "accept her allegations without proof. Arranging for three guards to escort her out, she reacts with fury, " +
                "casting profanities in your direction as she's led away."
            };

            private static readonly List<string> eventChoice4 = new List<string>
            {
                //Event Choice 4A
                "{=ViolatedGirl_Event_Choice_4A}Facing the gravity of the situation and the potential repercussions, you realize " +
                "the need for a discreet resolution. You invite the girl for a private talk outside the camp to discuss her " +
                "claims. Under the guise of addressing her concerns, you lead her to a secluded area. In a tragic turn of events, " +
                "the girl meets an untimely end, the details of which remain obscure. You call upon a few of your most trusted " +
                "men to ensure there is no trace of what happened. The grim task is handled with utmost secrecy, and you return " +
                "to camp with the assurance that the incident will remain a hidden chapter of your journey." ,
                
                //Event Choice 4B
                "{=ViolatedGirl_Event_Choice_4B} In the face of potential ruin, you choose a path of ruthless pragmatism. Feigning " +
                "sympathy, you lead the girl to a secluded woodland under the pretext of ensuring her safety. Once isolated, you " +
                "reveal your true, sinister intent. Your actions are swift and merciless, extinguishing her life to protect your " +
                "own reputation. You enlist the help of your most loyal and unscrupulous men to meticulously remove every trace " +
                "of the girl and the deed. The night air is heavy with the grim reality of your actions as you coldly oversee the " +
                "erasure of this dark episode from history.",

                //Event Choice 4C
                "{=ViolatedGirl_Event_Choice_4C} Determined to suppress the girl's story at all costs, you mask your malevolent " +
                "intentions with concern. You escort her to a desolate cavern, where her hopes for justice are cruelly shattered. " +
                "In the echoing silence of the cave, you enact a plan of cold-blooded finality. Your actions are calculated and " +
                "brutal, ensuring her voice is silenced forever. With the help of a few handpicked, ruthless soldiers, you erase " +
                "the evidence of your heinous act, burying the truth beneath layers of deceit and darkness." ,

                //Event Choice 4D
                "{=ViolatedGirl_Event_Choice_4D} To eliminate the threat to your status, you adopt a façade of diplomacy and lead " +
                "the girl to a forgotten ruin under the guise of discussing her claims. In this forsaken place, you enact a grim " +
                "and irreversible judgment. Your methods are chilling and methodical, leaving no chance of her tale ever surfacing. " +
                "The few men you involve in covering up this dark deed are sworn to secrecy, complicit in a conspiracy that entombs " +
                "the girl's story in the shadows of oblivion." ,

                //Event Choice 4E
                "{=ViolatedGirl_Event_Choice_4E} With your reputation hanging in the balance, you decide on a path of utter " +
                "ruthlessness. Luring the girl to an abandoned quarry with promises of aid, you instead deliver a fate most grim. " +
                "The deed is done with chilling precision and malice, a stark testament to the lengths you'll go to protect your " +
                "name. You summon a cadre of your most hardened and unscrupulous men to dispose of any evidence, their loyalty bought " +
                "with the promise of silence. As you ride back to camp, the weight of your dark choice lingers, the girl's fate " +
                "sealed and buried in the depths of the night."
            };


            private static readonly List<string> eventMsg1 = new List<string>
            {
                "{=ViolatedGirl_Event_Msg_1A}{heroName} compensated the girl with {compensation} gold and ensured the offender was imprisoned in {closestCity}'s dungeons." ,
                "{=ViolatedGirl_Event_Msg_1B}{heroName} provided {compensation} gold to the girl and had the guilty party detained in the cells of {closestCity}." ,
                "{=ViolatedGirl_Event_Msg_1C}{heroName} handed {compensation} gold to the girl and dispatched the perpetrator to {closestCity}'s jail." ,
                "{=ViolatedGirl_Event_Msg_1D}{heroName} offered {compensation} gold as reparation to the girl and sent the accused to {closestCity}'s dungeons." ,
                "{=ViolatedGirl_Event_Msg_1E}{heroName} gave {compensation} gold in compensation to the girl and relegated the culprit to the dungeons in {closestCity}."            
            };

            private static readonly List<string> eventMsg2 = new List<string>
            {
                "{=ViolatedGirl_Event_Msg_2A}{heroName} secured the girl's silence with a payment of {totalCompensation} gold." ,
                "{=ViolatedGirl_Event_Msg_2B}{heroName} ensured the girl's discretion for the sum of {totalCompensation} gold." ,
                "{=ViolatedGirl_Event_Msg_2C}{heroName} paid {totalCompensation} gold to the girl for her silence on the matter." ,
                "{=ViolatedGirl_Event_Msg_2D}{heroName} exchanged {totalCompensation} gold for the girl's commitment to secrecy." ,
                "{=ViolatedGirl_Event_Msg_2E}{heroName} obtained the girl's silence, compensating her with {totalCompensation} gold."
            };

            private static readonly List<string> eventMsg3 = new List<string>
            {
                "{=ViolatedGirl_Event_Msg_3A}{heroName} was skeptical of the girl's account." ,
                "{=ViolatedGirl_Event_Msg_3B}{heroName} doubted the girl's version of events." ,
                "{=ViolatedGirl_Event_Msg_3C}{heroName} found the girl's story unconvincing." ,
                "{=ViolatedGirl_Event_Msg_3D}{heroName} did not believe the girl's claims." ,
                "{=ViolatedGirl_Event_Msg_3E}{heroName} questioned the truth in the girl's tale."
            };

            private static readonly List<string> eventMsg4 = new List<string>
            {
                "{=ViolatedGirl_Event_Msg_4A}Whispers circulate about {heroName} silencing a young girl outside {closestCity} to hide a truth." ,
                "{=ViolatedGirl_Event_Msg_4B}Gossip suggests {heroName} ended a girl's life near {closestCity} to conceal a secret." ,
                "{=ViolatedGirl_Event_Msg_4C}It's rumored that {heroName} caused a young girl's demise by {closestCity} to bury a secret." ,
                "{=ViolatedGirl_Event_Msg_4D}Talk is spreading that {heroName} took a girl's life around {closestCity} for secrecy." ,
                "{=ViolatedGirl_Event_Msg_4E}Rumors abound of {heroName} eliminating a girl near {closestCity} to keep something hidden."
            };


            public static string GetRandomEventTitle()
            {
                var index = random.Next(eventTitles.Count);
                return eventTitles[index];
            }

            public static string GetRandomEventDescription()
            {
                var index = random.Next(eventDescriptions.Count);
                return eventDescriptions[index];
            }

            public static string GetRandomEventChoice1()
            {
                var index = random.Next(eventChoice1.Count);
                return eventChoice1[index];
            }

            public static string GetRandomEventChoice2()
            {
                var index = random.Next(eventChoice2.Count);
                return eventChoice2[index];
            }

            public static string GetRandomEventChoice3()
            {
                var index = random.Next(eventChoice3.Count);
                return eventChoice3[index];
            }

            public static string GetRandomEventChoice4()
            {
                var index = random.Next(eventChoice4.Count);
                return eventChoice4[index];
            }

            public static string GetRandomEventMessage1()
            {
                var index = random.Next(eventMsg1.Count);
                return eventMsg1[index];
            }

            public static string GetRandomEventMessage2()
            {
                var index = random.Next(eventMsg2.Count);
                return eventMsg2[index];
            }

            public static string GetRandomEventMessage3()
            {
                var index = random.Next(eventMsg3.Count);
                return eventMsg3[index];
            }

            public static string GetRandomEventMessage4()
            {
                var index = random.Next(eventMsg4.Count);
                return eventMsg4[index];
            }
        }
    }


    public class ViolatedGirlData : RandomEventData
    {

        public ViolatedGirlData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new ViolatedGirl();
        }
    }
}