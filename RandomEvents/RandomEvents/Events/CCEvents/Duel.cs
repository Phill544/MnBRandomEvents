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
    public sealed class Duel : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minTwoHandedLevel;
        private readonly int minRogueryLevel;

        public Duel() : base(ModSettings.RandomEvents.DuelData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("Duel", "EventDisabled");
            minTwoHandedLevel = ConfigFile.ReadInteger("Duel", "MinTwoHandedLevel");
            minRogueryLevel = ConfigFile.ReadInteger("Duel", "MinRogueryLevel");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minTwoHandedLevel != 0 || minRogueryLevel != 0 )
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null  && Clan.PlayerClan.Renown >= 750;
        }

        public override void StartEvent()
        {
            var mainHero = Hero.MainHero;

            var heroName = mainHero.FirstName;
            
            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).Name;

            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
            var twoHandedLevel = Hero.MainHero.GetSkillValue(DefaultSkills.TwoHanded);
            
            var canTrick = false;
            var canKillChallenger = false;
            
            var rogueryAppendedText = "";
            var twoHandedAppendedText = "";
            
            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                
                canTrick = true;
                canKillChallenger = true;

                rogueryAppendedText = new TextObject("{=Duel_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
                twoHandedAppendedText = new TextObject("{=Duel_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();

            }
            else
            {
                if (rogueryLevel >= minRogueryLevel)
                {
                    canTrick = true;
                    
                    rogueryAppendedText = new TextObject("{=Duel_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
                if (twoHandedLevel >= minTwoHandedLevel)
                {
                    canKillChallenger = true;
                    
                    twoHandedAppendedText = new TextObject("{=Duel_TwoHanded_Appended_Text}[Two-Handed - lvl {minTwoHandedLevel}]")
                        .SetTextVariable("minTwoHandedLevel", minTwoHandedLevel)
                        .ToString();
                }
            }

            var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription())
                    .SetTextVariable("closestSettlement", closestSettlement)
                    .ToString();
            
            var eventOption1 = new TextObject("{=Duel_Event_Option_1}Decline his challenge").ToString();
            var eventOption1Hover = new TextObject("{=Duel_Event_Option_1_Hover}No point in accepting.").ToString();
            
            var eventOption2 = new TextObject("{=Duel_Event_Option_2}Accept his challenge").ToString();
            var eventOption2Hover = new TextObject("{=Duel_Event_Option_2_Hover}Teach him a lesson.").ToString();

            var eventOption3 = new TextObject("{=Duel_Event_Option_3}[Two-Handed] Kill him").ToString();
            var eventOption3Hover = new TextObject("{=Duel_Event_Option_3_Hover}Kill him in combat.\n{twoHandedAppendedText}").SetTextVariable("twoHandedAppendedText", twoHandedAppendedText).ToString();
            
            var eventOption4 = new TextObject("{=Duel_Event_Option_4}[Roguery] Trick him").ToString();
            var eventOption4Hover = new TextObject("{=Duel_Event_Option_4_Hover}Kill him by tricking him.\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();


            var eventButtonText1 = new TextObject("{=Duel_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=Duel_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            
            if (canKillChallenger)
            {
                inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            }

            if (canTrick)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }

            var eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1())
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2())
                .SetTextVariable("closestSettlement",closestSettlement)
                .ToString();
            
            var eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3())
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4())
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            
            var eventMsg1 =new TextObject(EventTextHandler.GetRandomEventMessage1())
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg2 =new TextObject(EventTextHandler.GetRandomEventMessage2())
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg3 =new TextObject(EventTextHandler.GetRandomEventMessage3())
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg4 =new TextObject(EventTextHandler.GetRandomEventMessage4())
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            
            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1,
                    eventButtonText1, null,
                    elements =>
                    {
                        switch ((string)elements[0].Identifier)
                        {
                            case "a":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                                break;
                            case "b":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                                
                                Hero.MainHero.ChangeHeroGold(-5);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                                break;
                            case "c":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                                
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
                "{=Duel_Title_A}Duel of Fates",
                "{=Duel_Title_B}The Challenger's Call",
                "{=Duel_Title_C}Clash of Steel",
                "{=Duel_Title_D}The Duelist's Gauntlet",
                "{=Duel_Title_E}Honor's Contest",
                "{=Duel_Title_F}Showdown of Valor",
                "{=Duel_Title_G}The Face-Off",
                "{=Duel_Title_H}Confrontation"
            };
            
            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=Duel_Event_Desc_A}As your party rests near {closestSettlement}, a young man seeks you out for a challenge. " +
                "His intent is to duel, inspired by your renowned arena exploits across Calradia. Observing him, you realize " +
                "his inexperience would make for an effortless victory. This moment could serve as a lesson for him, or " +
                "you could simply refuse his request. The choice stands before you: accept the duel and teach him, or " +
                "decline and walk away. What will be your decision?",
                
                //Event Description B
                "{=Duel_Event_Desc_B}Near {closestSettlement}, while your party takes a break, a determined young man " +
                "approaches, seeking a duel. He's motivated by tales of your legendary battles in Calradia's arenas. " +
                "Assessing him, you note his lack of skill, suggesting an easy win. You ponder: should you use this duel " +
                "as a teaching moment, or dismiss his challenge altogether? The decision is yours to either engage in " +
                "the duel and impart a lesson, or to turn down his bold challenge.",
                
                //Event Description C
                "{=Duel_Event_Desc_C}Your party, pausing near {closestSettlement}, is approached by an ambitious young " +
                "man demanding a duel. He's eager to test his mettle against you, emboldened by your famed arena victories " +
                "throughout Calradia. You quickly gauge that he's no match for you. Now, you must decide: seize this " +
                "chance to educate him in combat, or reject his challenge outright. Will you accept and teach him a valuable " +
                "lesson, or deny and spare him the defeat?",
                
                //Event Description D
                "{=Duel_Event_Desc_D}While encamped by {closestSettlement}, a youthful challenger emerges, intent on " +
                "dueling you. Inspired by your celebrated arena triumphs in Calradia, he boldly steps forward. A quick " +
                "assessment reveals his inferior skills, promising an easy duel. You're now faced with a choice: accept " +
                "his duel and provide a practical lesson, or decline and leave him untested. Do you take up his challenge to " +
                "instruct, or refuse to engage and move on?",
                
                //Event Description E
                "{=Duel_Event_Desc_E}In the vicinity of {closestSettlement}, during a brief rest, a young man confronts " +
                "you, eager for a duel. His challenge stems from your well-known successes in the arenas of Calradia. " +
                "You can tell at a glance that he's inexperienced, suggesting a straightforward victory for you. The decision " +
                "lies with you: to accept his duel as an opportunity to teach, or to decline and avoid the confrontation. " +
                "Will you opt to duel and educate, or dismiss his request and abstain?"
            };
            
            private static readonly List<string> eventChoice1 = new List<string>
            {
                //Event Choice 1A
                "{=Duel_Event_Choice_1A}You inform the young man that you decline his challenge, certain of your easy victory. " +
                "Seizing the chance, you offer him some wisdom. You advise against recklessly challenging far more skilled " +
                "opponents and suggest that real combat skills are honed in a city garrison's training. You recommend he " +
                "join one to learn properly. Your parting is amicable, with a sense of mutual respect.",
                
                //Event Choice 1B
                "{=Duel_Event_Choice_1B}You gently refuse the challenge, explaining your certain triumph. Taking the " +
                "opportunity to mentor, you advise him on the dangers of facing vastly superior fighters. You suggest a " +
                "more prudent path: joining a city garrison for formal combat training. Your counsel is well-received, " +
                "and you part on friendly terms, leaving him with thoughtful guidance for his future endeavors.",
                
                //Event Choice 1C
                "{=Duel_Event_Choice_1B}You gently refuse the challenge, explaining your certain triumph. Taking the " +
                "opportunity to mentor, you advise him on the dangers of facing vastly superior fighters. You suggest " +
                "a more prudent path: joining a city garrison for formal combat training. Your counsel is well-received, a" +
                "nd you part on friendly terms, leaving him with thoughtful guidance for his future endeavors.",
                
                //Event Choice 1D
                "{=Duel_Event_Choice_1D}You turn down his challenge, assured of your upper hand in such a duel. You take " +
                "this moment to offer guidance, counseling him on the perils of challenging those with greater prowess. " +
                "Advising him to join a city garrison for disciplined training, you part ways amicably, with him nodding " +
                "in understanding and gratitude for your wise words.",
                
                //Event Choice 1E
                "{=Duel_Event_Choice_1E}Politely, you reject the duel, confident in your superior skills. You seize the " +
                "chance to mentor him, warning of the folly in challenging highly skilled fighters. Suggesting that joining a " +
                "city garrison would be a valuable learning experience, you part on friendly terms. He leaves, considering " +
                "your advice, grateful for the insight you've shared."
            };
            
            private static readonly List<string> eventChoice2 = new List<string>
            {
                //Event Choice 2A
                "{=Duel_Event_Choice_2A}You accept his challenge. Emerging from your tent in full plate armor, you see " +
                "fear in your opponent's eyes. Your men form a circle for the duel. He's in light leather armor. You pause " +
                "and get him plate armor. Struggling in the heavy gear, he's clearly inexperienced. As the duel begins, " +
                "he kneels, begging forgiveness. You anticipated this, so you use the chance to teach him a rough lesson. " +
                "After a few minutes, he kneels again, having learned his lesson. You assist him out of the armor, " +
                "offering advice about recklessly challenging skilled opponents. You suggest he trains with a city " +
                "garrison. The parting is amicable.",
                
                //Event Choice 2B
                "{=Duel_Event_Choice_2B}Agreeing to the duel, you don full plate armor, noticing your opponent's intimidated " +
                "expression. A circle is formed by your men. His attire is merely light leather. You halt the duel, " +
                "arranging for him to don plate armor. Inexperienced with the heavy armor, he struggles. When the duel " +
                "starts, he immediately pleads for mercy. Expecting this, you decide to give him a non-harmful scare. " +
                "He soon kneels again, lesson learned. Helping him out of the armor, you counsel him on avoiding duels " +
                "with more experienced fighters and advise training at a city garrison. You part on friendly terms.",
                
                //Event Choice 2C
                "{=Duel_Event_Choice_2C}You accept the challenge, stepping out in full plate armor, which visibly unnerves " +
                "your adversary. Your men create a circle for the duel, and you notice his simple leather armor. You p" +
                "ause the duel to equip him with plate armor. He's clearly not used to such heavy armor. As the duel " +
                "commences, he falls, begging for leniency. You use this moment to mildly intimidate him, as expected. " +
                "He kneels again, having absorbed the lesson. You assist him out of his armor and give him advice on not " +
                "challenging those more skilled. Recommending training at a nearby city garrison, you part on good terms.",
                
                //Event Choice 2D
                "{=Duel_Event_Choice_2D}Accepting the duel, you appear in full plate armor, causing discernible fear in " +
                "your challenger. A ring is formed by your men. He wears only light leather armor. You call for a pause " +
                "to provide him with plate armor. His unfamiliarity with the heavy armor is evident. When the duel starts, " +
                "he quickly kneels and asks for mercy. Anticipating this, you scare him harmlessly. After a short while, " +
                "he kneels again, lesson learned. You help him out of the armor, advising him on the risks of challenging " +
                "skilled fighters, and suggest joining a city garrison for training. The encounter ends amicably.",
                
                //Event Choice 2E
                "{=Duel_Event_Choice_2E}You decide to accept the duel, donning your plate armor, which instills fear in " +
                "your young challenger. Your men set up a dueling ring. He's in lightweight leather. You interrupt to " +
                "equip him with plate armor. It's clear he's never worn such heavy armor. As you engage, he kneels down, " +
                "pleading for forgiveness. You had foreseen this outcome and gently rough him up to teach a lesson. He " +
                "kneels once more, clearly having learned. You aid him out of the armor and advise him against challenging " +
                "more experienced individuals, suggesting he train with a garrison. You part on relatively friendly terms."
            };
            
            private static readonly List<string> eventChoice3 = new List<string>
            {
                //Event Choice 3A
                "{=Duel_Event_Choice_3A}You accept his challenge. Emerging in full plate armor, your opponent's fear is " +
                "evident. A circle is formed by your men. He wears light armor. The duel begins; you quickly advance, and " +
                "he attempts to dodge. However, he's too slow, and with a swift strike, you decapitate him. His head lands " +
                "nearby, and your men cheer. You arrange a warrior's funeral pyre for him, respecting his bravery despite " +
                "his foolishness. A courier is sent to {closestSettlement} to report the death resulting from the duel.",
                
                //Event Choice 3B
                "{=Duel_Event_Choice_3B}Accepting the challenge, you don full plate armor, seeing fear in your adversary's " +
                "eyes. Your men encircle you both. He's lightly armored. As the duel commences, you charge. He tries evading, " +
                "but you're too quick. One clean strike, and his head is severed, flying off. Cheers erupt from your " +
                "men. You command a pyre to honor him as a warrior, acknowledging his courage. You send a messenger to " +
                "{closestSettlement} to inform them of his duel-induced death.",
                
                //Event Choice 3C
                "{=Duel_Event_Choice_3C}You agree to the duel, clad in heavy plate armor, intimidating your opponent. A " +
                "large ring is made by your men. He dons light armor. The fight starts; you aggressively move in. His " +
                "dodge fails, and you swiftly decapitate him, his head rolling away. Your men's cheers fill the air. You " +
                "order a pyre to be built, honoring his bravery. Despite his mistake in challenging you, you respect his " +
                "commitment. A courier is dispatched to {closestSettlement} to report the fatal outcome of the duel.",
                
                //Event Choice 3D
                "{=Duel_Event_Choice_3D}Agreeing to the duel, you appear in imposing plate armor, instilling fear in him. " +
                "Your men form a combat arena. He's under-armored in leather. As the duel initiates, you swiftly close in." +
                " He attempts a dodge, but is too slow. In a single motion, you behead him. His head lands at a distance, " +
                "and your men acclaim your victory. A pyre is prepared for him, a gesture of respect for a warrior. " +
                "Despite his foolhardiness, you honor his bravery. News of his death is sent to {closestSettlement} via " +
                "a courier.",
                
                //Event Choice 3E
                "{=Duel_Event_Choice_3E}You take up his duel challenge, clad in your full plate armor. Your opponent " +
                "visibly trembles. A circular arena is set by your soldiers. He's in mere leather armor. The battle begins, " +
                "and you advance rapidly. His dodging attempt fails against your speed, and you execute a clean decapitation. " +
                "His head lands afar as your men cheer triumphantly. Arranging a warrior's funeral pyre, you respect his " +
                "daring spirit. A courier heads to {closestSettlement} to relay the news of his death due to the duel."
            };
            
            private static readonly List<string> eventChoice4= new List<string>
            {
                //Event Choice 4A
                "{=Duel_Event_Choice_4A}You accept his challenge. As he turns to leave, you quickly grab a sword and strike " +
                "him down in one swift move. You strike repeatedly, losing yourself in the moment. Eventually, your men pull " +
                "you away. The challenger is reduced to a gruesome mix of blood and remains, and you are drenched in blood. " +
                "You order your men to dispose of the remains in the woods, letting nature erase the evidence. As you pass " +
                "your men to clean yourself, their eyes reflect fear. This brutal display ensures they'll stay in line, " +
                "not wanting a similar fate.",
                
                //Event Choice 4B
                "{=Duel_Event_Choice_4B}Accepting the challenge, you swiftly grab a nearby sword as he turns his back. With " +
                "a rapid, lethal motion, you bring him down. You continue striking, each blow fueled by intensity. Two of " +
                "your men eventually intervene, pulling you back from the carnage. What remains of the challenger is a " +
                "horrific sight of blood and torn flesh. You command your men to hide the body in the forest, allowing " +
                "wildlife to obscure any traces. Walking past your men, their fearful gazes remind them of the c" +
                "onsequences of crossing you.",
                
                //Event Choice 4C
                "{=Duel_Event_Choice_4C}You agree to his challenge, but as he turns to depart, you seize a sword and " +
                "cut him down swiftly. Caught in a frenzy, you repeatedly strike, each blow more forceful than the last. " +
                "Only when two of your men restrain you do you cease. The scene is a brutal array of blood, organs, and " +
                "dismembered parts. You instruct your men to dispose of the gruesome remains in the woods, effectively " +
                "concealing the evidence. As you go to wash off the blood, the evident fear in the eyes of your men serves " +
                "as a stark warning to them all.",
                
                //Event Choice 4D
                "{=Duel_Event_Choice_4D}Upon his acceptance, you quickly seize a sword as he turns away. In a single, " +
                "decisive strike, you fell him. Overwhelmed, you continue striking mercilessly. It's only when two of your " +
                "men restrain you that the onslaught ends. What's left is a grotesque sight of blood and scattered " +
                "remains. You order the body's removal to the nearby woods for wildlife to take care of. Walking by your " +
                "men, their fearful looks reinforce the harsh lesson of what defiance leads to.",
                
                //Event Choice 4E
                "{=Duel_Event_Choice_4E}In response to his challenge, as he pivots to leave, you abruptly grab a sword and " +
                "violently strike him down. Your blows are relentless, turning him into an unrecognizable mass. Finally " +
                "restrained by your men, the brutal scene left behind is chilling. You command the cleanup and disposal of " +
                "the body in the forest, letting nature remove any trace. The fear in your men's eyes as you pass by serves " +
                "as a grim reminder of the severity of challenging your authority."
            };
            
            private static readonly List<string> eventMsg1 = new List<string>
            {
                "{=Duel_Event_Msg_1A}{heroName} declined the duel and instead taught the challenger a thing or two.",
                "{=Duel_Event_Msg_1B}{heroName} refused to duel, opting to impart wisdom to the challenger instead.",
                "{=Duel_Event_Msg_1C}{heroName} chose not to fight, but gave the challenger valuable lessons.",
                "{=Duel_Event_Msg_1D}{heroName} turned down the duel and provided the challenger with advice.",
                "{=Duel_Event_Msg_1E}{heroName} skipped the duel, taking the opportunity to educate the challenger."
            };
            
            private static readonly List<string> eventMsg2 = new List<string>
            {
                "{=Duel_Event_Msg_2A}{heroName} accepted the challenge. He humiliated the challenger.",
                "{=Duel_Event_Msg_2B}{heroName} took up the challenge and thoroughly embarrassed the challenger.",
                "{=Duel_Event_Msg_2C}{heroName} agreed to the duel and decisively outclassed the challenger.",
                "{=Duel_Event_Msg_2D}{heroName} accepted the duel, showcasing his superiority over the challenger.",
                "{=Duel_Event_Msg_2E}{heroName} faced the challenger in a duel, leaving him utterly humiliated."
            };
            
            private static readonly List<string> eventMsg3 = new List<string>
            {
                "{=Duel_Event_Msg_3A}{heroName} easily killed the challenger.",
                "{=Duel_Event_Msg_3B}{heroName} swiftly defeated and killed the challenger.",
                "{=Duel_Event_Msg_3C}{heroName} quickly dispatched the challenger with ease.",
                "{=Duel_Event_Msg_3D}{heroName} effortlessly eliminated the challenger.",
                "{=Duel_Event_Msg_3E}{heroName} made quick work of the challenger, ending his life."
            };
            
            private static readonly List<string> eventMsg4 = new List<string>
            { 
                "{=Duel_Event_Msg_4A}{heroName} completely lost it when he was challenged to a duel.",
                "{=Duel_Event_Msg_4B}{heroName} utterly lost composure upon being challenged to a duel.",
                "{=Duel_Event_Msg_4C}{heroName} became unhinged at the challenge for a duel.",
                "{=Duel_Event_Msg_4D}{heroName} reacted wildly to the duel challenge, losing all control.",
                "{=Duel_Event_Msg_4E}{heroName} spiraled out of control when faced with the duel challenge."
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


    public class DuelData : RandomEventData
    {
        public DuelData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new Duel();
        }
    }
}