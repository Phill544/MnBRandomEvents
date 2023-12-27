using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class FleeingFate : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minGoldReward;
        private readonly int maxGoldReward;
        private readonly int minAge;
        private readonly int maxAge;
        private readonly int minStewardLevel;
        private readonly int minRogueryLevel;
        private readonly int successChance;

        public FleeingFate() : base(ModSettings.RandomEvents.FleeingFateData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("FleeingFate", "EventDisabled");
            minGoldReward = ConfigFile.ReadInteger("FleeingFate", "MinGoldReward");
            maxGoldReward = ConfigFile.ReadInteger("FleeingFate", "MaxGoldReward");
            minAge = ConfigFile.ReadInteger("FleeingFate", "MinAge");
            maxAge = ConfigFile.ReadInteger("FleeingFate", "MaxAge");
            minStewardLevel = ConfigFile.ReadInteger("FleeingFate", "MinStewardLevel");
            minRogueryLevel = ConfigFile.ReadInteger("FleeingFate", "MinRogueryLevel");
            successChance = ConfigFile.ReadInteger("FleeingFate", "SuccessChance");

            //Overrides the min age.
            minAge = Math.Max(minAge, 16);
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled) return false;

            return minGoldReward != 0 || maxGoldReward != 0 || minAge != 0 || maxAge != 0 || minStewardLevel != 0 || minRogueryLevel != 0;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && Settlement.CurrentSettlement != null && (CurrentTimeOfDay.IsEvening || CurrentTimeOfDay.IsNight);
        }

        public override void StartEvent()
        {
            var heroName = Hero.MainHero.FirstName;
            
            var stewardLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Steward);
            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);

            var currentSettlement = Settlement.CurrentSettlement.Name;

            var noblewomanName = EventTextHandler.GetRandomNoblewomanNames();

            var goldReward = MBRandom.RandomInt(minGoldReward, maxGoldReward);

            var success = MBRandom.RandomInt(0, 100);
            
            var canNegotiate = false;
            var canUseViolence = false;
            var canKill = false;

            var stewardAppendedText = "";
            var rogueryAppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                
                canNegotiate = true;
                stewardAppendedText = new TextObject("{=FleeingFate_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
                
                canUseViolence = true;
                canKill = true;
                rogueryAppendedText = stewardAppendedText;

            }
            else
            {
                if (stewardLevel >= minStewardLevel)
                {
                    canNegotiate = true;
                    
                    stewardAppendedText = new TextObject("{=FleeingFate_Roguery_Appended_Text}[Steward - lvl {minStewardLevel}]")
                        .SetTextVariable("minStewardLevel", minStewardLevel)
                        .ToString();
                }
                if (rogueryLevel >= minRogueryLevel)
                {
                    canUseViolence = true;
                    canKill = true;
                    
                    rogueryAppendedText = new TextObject("{=FleeingFate_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
            }


            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

            var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription())
                .SetTextVariable("noblewomanName", noblewomanName)
                .SetTextVariable("currentSettlement", currentSettlement).ToString();

            var eventOption1 = new TextObject("{=FleeingFate_Event_Option_1}Help her").ToString();
            var eventOption1Hover = new TextObject("{=FleeingFate_Event_Option_1_Hover}Try to help her get away from the city.").ToString();
            
            var eventOption2 = new TextObject("{=FleeingFate_Event_Option_2}Convince her").ToString();
            var eventOption2Hover = new TextObject("{=FleeingFate_Event_Option_2_Hover}Remind her of her duty as a Noble.").ToString();

            var eventOption3 = new TextObject("{=FleeingFate_Event_Option_3}Words of wisdom").ToString();
            var eventOption3Hover = new TextObject("{=FleeingFate_Event_Option_3_Hover}Try to her to make her own choice.").ToString();
            
            var eventOption4 = new TextObject("{=FleeingFate_Event_Option_4}[Steward] Negotiate").ToString();
            var eventOption4Hover = new TextObject("{=FleeingFate_Event_Option_4_Hover}Try and talk to her family.\n{stewardAppendedText}").SetTextVariable("stewardAppendedText", stewardAppendedText).ToString();
            
            var eventOption5 = new TextObject("{=FleeingFate_Event_Option_5}[Roguery] Beat her").ToString();
            var eventOption5Hover = new TextObject("{=FleeingFate_Event_Option_5_Hover}Make sure she gets the message.\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();
            
            var eventOption6 = new TextObject("{=FleeingFate_Event_Option_5}[Roguery] Kill her").ToString();
            var eventOption6Hover = new TextObject("{=FleeingFate_Event_Option_5_Hover}One less problem.\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();

            var eventButtonText1 = new TextObject("{=FleeingFate_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=FleeingFate_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            
            if (canNegotiate)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }
            
            if (canUseViolence)
            {
                inquiryElements.Add(new InquiryElement("e", eventOption5, null, true, eventOption5Hover));
            }
            
            if (canKill)
            {
                inquiryElements.Add(new InquiryElement("f", eventOption6, null, true, eventOption6Hover));
            }
            
            string eventOptionAText;
            
            string eventOptionBText;
            
            string eventOptionCText;
            
            string eventOptionDText;
            
            var eventOptionEText = new TextObject(EventTextHandler.GetRandomEventChoice5A())
                .SetTextVariable("noblewomanName", noblewomanName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("goldReward", goldReward)
                .ToString();
            
            var eventOptionFText = new TextObject(EventTextHandler.GetRandomEventChoice6A())
                .SetTextVariable("noblewomanName", noblewomanName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("goldReward", goldReward)
                .ToString();
            
            string eventMsg1;
            
            string eventMsg2;
            
            string eventMsg3;
            
            string eventMsg4;

            var eventMsg5 = new TextObject(EventTextHandler.GetRandomEventMessage5A())
                .SetTextVariable("noblewomanName", noblewomanName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("goldReward", goldReward)
                .ToString();
            
            var eventMsg6 = new TextObject(EventTextHandler.GetRandomEventMessage6A())
                .SetTextVariable("noblewomanName", noblewomanName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("goldReward", goldReward)
                .ToString();
            

            if (success >= successChance)
            {
                
                eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .ToString();
            
                eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .ToString();
            
                eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .ToString();
            
                eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .ToString();
            
                eventMsg1 = new TextObject(EventTextHandler.GetRandomEventMessage1A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventMsg2 = new TextObject(EventTextHandler.GetRandomEventMessage2A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventMsg3 = new TextObject(EventTextHandler.GetRandomEventMessage3A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventMsg4 = new TextObject(EventTextHandler.GetRandomEventMessage4A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
                
                eventMsg5 = new TextObject(EventTextHandler.GetRandomEventMessage5A())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
                
            }
            else
            {
                
                eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventMsg1 = new TextObject(EventTextHandler.GetRandomEventMessage1B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventMsg2 = new TextObject(EventTextHandler.GetRandomEventMessage2B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventMsg3 = new TextObject(EventTextHandler.GetRandomEventMessage3B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
            
                eventMsg4 = new TextObject(EventTextHandler.GetRandomEventMessage4B())
                    .SetTextVariable("noblewomanName", noblewomanName)
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("goldReward", goldReward)
                    .SetTextVariable("heroName", heroName)
                    .ToString();
                
            }

            
            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        
                        case "e":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionEText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg5, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));
                            break;
                        
                        case "f":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionFText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg6, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));
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
                
                "{=FleeingFate_Title_A}Fleeing Fate" ,
                "{=FleeingFate_Title_B}Escape from Bondage" ,
                "{=FleeingFate_Title_C}The Unwilling Bride" ,
                "{=FleeingFate_Title_D}Noble Dilemma" ,
                "{=FleeingFate_Title_E}Bound by Duty" ,
                "{=FleeingFate_Title_F}A Cry for Freedom" ,
                "{=FleeingFate_Title_G}Breaking Chains" ,
                "{=FleeingFate_Title_H}Against the Betrothal"

            };
            
            private static readonly List<string> noblewomanNames = new List<string>
            {
                "{=FleeingFate_Noblewoman_Name_A}Lady Cyrenia" ,
                "{=FleeingFate_Noblewoman_Name_B}Lady Elithina" ,
                "{=FleeingFate_Noblewoman_Name_C}Lady Galereth" ,
                "{=FleeingFate_Noblewoman_Name_D}Lady Ysmera" ,
                "{=FleeingFate_Noblewoman_Name_E}Lady Thalica" ,
                "{=FleeingFate_Noblewoman_Name_F}Lady Venalia" ,
                "{=FleeingFate_Noblewoman_Name_G}Lady Rhydena" ,
                "{=FleeingFate_Noblewoman_Name_H}Lady Esmereth" ,
                "{=FleeingFate_Noblewoman_Name_I}Lady Lythania" ,
                "{=FleeingFate_Noblewoman_Name_J}Lady Saralia"
            };
            
            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=FleeingFate_Event_Desc_A}In the city's dim evening light, {noblewomanName}, a distressed noblewoman, " +
                "quietly approaches you. She's been forced into marrying a cruel baron and desperately seeks a way " +
                "out to pursue a life of her own choosing, free from tyranny and despair. Her eyes, filled with " +
                "a mix of fear and hope, turn to you as her last hope. Faced with her plea, what will you do? " +
                "Will you help her escape this unwanted fate or leave her to face the consequences of her noble birth?" ,
                
                //Event Description B
                "{=FleeingFate_Event_Desc_B}As night descends upon {currentSettlement}, you're approached by {noblewomanName}, " +
                "a young woman of noble birth, who reveals her plight. She faces a loveless marriage to a notorious " +
                "baron known for his cruelty. Seeking a way out, she turns to you, her eyes filled with a plea for " +
                "freedom. Will you assist her in evading this grim future, or will her noble duty force her to endure " +
                "a life of unhappiness?" ,

                //Event Description C
                "{=FleeingFate_Event_Desc_C}Under the city's twilight, {noblewomanName}, a noblewoman, seeks your help. " +
                "She's been unwillingly betrothed to a ruthless baron and longs for a different life. With her fate " +
                "hanging in the balance, she looks to you for salvation. Do you choose to aid her quest for freedom, " +
                "or will you step back, leaving her to fulfill her aristocratic obligations?" ,

                //Event Description D
                "{=FleeingFate_Event_Desc_D}In the city's evening quiet, {noblewomanName}, burdened by her noble lineage, " +
                "confides in you. She's bound to marry a barbarous baron against her will and seeks an escape to live " +
                "life on her own terms. Her gaze, fraught with desperation, rests upon you. Will you take action to " +
                "help her break free, or will you ignore her plea and let her face her ill-fated destiny?" ,

                //Event Description E
                "{=FleeingFate_Event_Desc_E}As the city's evening shadows grow, a desperate plea from {noblewomanName}, " +
                "a noblewoman, reaches you. She's to be wed to a tyrannical baron and implores you for an escape from " +
                "her impending, joyless marriage. Her hopeful eyes are fixed on you, seeking your intervention. Do you " +
                "decide to aid her pursuit of a happier future, or do you let her succumb to the harsh demands of her " +
                "noble birth?"

            };
            
            private static readonly List<string> eventChoice1A = new List<string>
            {
                //Event Choice 1AA
                "{=FleeingFate_Event_Choice_1AA}You choose the rooftops for their vantage point and relative secrecy. " +
                "As the city's lights flicker below, you and {noblewomanName} navigate the skyline with careful precision. " +
                "Each leap and dash across the tiled roofs is calculated, avoiding areas lit by torches or moonlight. " +
                "You occasionally pause, ensuring no curious eyes spot your silhouettes against the night sky. After " +
                "a heart-racing journey across the city, you finally reach a secluded part of the wall, where a hidden " +
                "ladder allows for a safe descent. Once on the ground, {noblewomanName} reveals a small, hidden pouch " +
                "and hands it to you, the coins inside amounting to {goldReward}. Her eyes glisten with unshed tears a" +
                "s she thanks you, then turns to disappear into the night, her spirit unburdened by the " +
                "shackles of her past.",
                
                //Event Choice 1AB
                "{=FleeingFate_Event_Choice_1AB}In this meticulously planned escape, you arrange for a merchant's cart, " +
                "laden with goods, to leave the city at dusk. {noblewomanName} is concealed among the cargo, disguised " +
                "as a simple crate. You adopt the role of a merchant, sharing light banter with the city guards as they " +
                "inspect the cart. They laugh at your jokes, unaware of the precious 'goods' you're smuggling out. Once " +
                "through the gates, the tension that had built up in your chest eases. Several miles from the city, you " +
                "help {noblewomanName} out of her cramped hiding place. She stretches, relief flooding her face, and " +
                "gratefully hands you a hefty pouch containing {goldReward}. With a final, hopeful glance back at the " +
                "city walls that once confined her, she sets off into the night, towards a future of her own making.",
                
                //Event Choice 1AC
                "{=FleeingFate_Event_Choice_1AC}Timing the escape with the city's annual festival, you and {noblewomanName} " +
                "merge into the crowd of revelers. The city is alive with music and laughter, providing the perfect cover. " +
                "You guide her through the maze of celebration, using the dense crowd to shield her from the gaze of " +
                "patrolling guards. At the height of the festivities, when the sky bursts into colors with fireworks, " +
                "you quicken your pace. The gates are less guarded, the guards' attention diverted. Once safely beyond " +
                "the city, {noblewomanName} reveals a purse filled with {goldReward} as a token of her immense gratitude. " +
                "She watches the distant fireworks one last time, a symbol of her own liberation, before disappearing " +
                "into the embrace of the night.",
                
                //Event Choice 1AD
                "{=FleeingFate_Event_Choice_1AD}The forgotten underground passage, a remnant of ancient times, serves as " +
                "your escape route. You and {noblewomanName} navigate its damp corridors, guided by the flickering " +
                "torchlight. The passage is suffocatingly narrow in places, and you often have to proceed single file. " +
                "Echoes of your footsteps are your constant companions, creating an eerie rhythm in the otherwise silent " +
                "tunnel. When you finally emerge into the open air, a sense of liberation fills the atmosphere. " +
                "{noblewomanName} reaches into her cloak and retrieves a well-hidden bag of coins, amounting to {goldReward}, " +
                "and hands it to you with a mixture of relief and joy. She takes a deep breath of the free night air, " +
                "then steps away from the city and the life she's leaving behind, her steps light with newfound " +
                "hope and freedom.",
                
                //Event Choice 1AE
                "{=FleeingFate_Event_Choice_1AE}The river, a silent ally in your plan, promises a discreet escape route. " +
                "The small boat you've arranged rocks gently in the water as you and {noblewomanName}, dressed as humble " +
                "fishermen, row away from the city. The rhythm of the oars slicing through the water is soothing, and the " +
                "further you get from the city, the more {noblewomanName}'s tense shoulders relax. The journey is long, " +
                "winding through the quiet countryside under the cover of darkness. At a remote landing, far from the " +
                "prying eyes of the city, you part ways. {noblewomanName} hands you a substantial purse, heavy with " +
                "{goldReward}, her eyes shining with tears of gratitude. She steps onto the bank, a free woman at last, " +
                "her heart full of dreams and possibilities."
                
            };
            
            private static readonly List<string> eventChoice1B = new List<string>
            {
                //Event Choice 1BA
                "{=FleeingFate_Event_Choice_1BA}Your plan to traverse the city via its rooftops starts with promise, " +
                "but as you and {noblewomanName} deftly navigate the maze of moonlit roofs, a guard spots your movements. " +
                "The alarm is raised quickly, echoing through the night. Despite your efforts to evade the pursuing guards, " +
                "a misstep by {noblewomanName} leads to her capture. You manage to escape by scaling down a wall and " +
                "disappearing into a narrow alley, but the knowledge that she has been taken by the guards, her dreams of " +
                "freedom crushed, leaves you with a sense of deep failure.",
                
                //Event Choice 1BB
                "{=FleeingFate_Event_Choice_1BB}Disguised as a simple merchant, you drive the cart with {noblewomanName} " +
                "hidden beneath the goods. The tension rises as you approach the city gates. Suddenly, the guards, spurred " +
                "by a suspicion or perhaps routine, decide to search the cart more thoroughly. Despite your attempts to " +
                "talk them out of it, they uncover {noblewomanName}'s hiding place. She is immediately seized and taken away. " +
                "You blend into the nearby crowd, narrowly avoiding capture, but the plan's failure and {noblewomanName}'s " +
                "capture weigh heavily on your conscience.",
                
                //Event Choice 1BC
                "{=FleeingFate_Event_Choice_1BC}The vibrant festival, filled with music and dance, seems like the perfect " +
                "cover. However, as you navigate through the crowd with {noblewomanName}, a former acquaintance from her " +
                "noble life recognizes her. The alarm is sounded before you can make your escape. Guards swarm the area, " +
                "and in the chaos, {noblewomanName} is caught. You manage to evade capture by quickly blending into a group " +
                "of performers, but the regret of leaving her behind taints your narrow escape.",
                
                //Event Choice 1BD
                "{=FleeingFate_Event_Choice_1BD}The ancient underground passage, once a beacon of hope, turns into a trap. " +
                "A routine patrol, unexpected and untimely, blocks your path. Cornered and outnumbered, a brief struggle " +
                "ensues. In the confusion, you find a momentary gap and slip away through a narrow offshoot tunnel. However, " +
                "{noblewomanName} is not so lucky. She is apprehended by the guards. Your escape through the damp and " +
                "echoing tunnels is haunted by the thought of her now being in the hands of those she sought to flee.",
                
                //Event Choice 1BE
                "{=FleeingFate_Event_Choice_1BE}The plan to escape by river starts smoothly as you row the small boat " +
                "away from the city. But as you approach the city limits, a patrol boat, an unforeseen hindrance, intercepts " +
                "you. In a desperate bid for freedom, you dive into the river, using the cover of night and the water's flow " +
                "to escape. However, {noblewomanName} is unable to follow suit and is captured by the patrol. You reach the " +
                "shore, drenched and safe, but the escape's failure and her capture leave you grappling with guilt " +
                "and frustration."
                
            };
            
            private static readonly List<string> eventChoice2A = new List<string>
            {
                //Event Choice 2AA
                "{=FleeingFate_Event_Choice_2AA}In a quiet corner of the city, you speak with {noblewomanName}, offering " +
                "words of wisdom. You explain the power of patience and adaptation, illustrating how she could use her " +
                "position to enact change from within. You suggest that she could turn this marriage into an opportunity to " +
                "gain influence and perhaps even reform the baron's harsh ways. As the conversation unfolds, {noblewomanName} " +
                "begins to see the potential in your words. She decides to stay, embracing the challenge with newfound " +
                "determination. As she leaves, she expresses her gratitude for your counsel, promising to remember your " +
                "advice in the days to come." ,
                
                //Event Choice 2AB
                "{=FleeingFate_Event_Choice_2AB}You meet {noblewomanName} in a secluded spot and discuss the importance of " +
                "strategic alliances. You point out that her marriage could open doors to powerful connections and significant " +
                "influence. You advise her to forge alliances within the baron's circle, turning her situation into a stepping " +
                "stone for greater things. As she listens, her demeanor changes from despair to contemplation. By the end of " +
                "the conversation, she resolves to stay and make the best of her circumstances, using your advice as a guide " +
                "to navigate her new life." ,
                
                //Event Choice 2AC
                "{=FleeingFate_Event_Choice_2AC}Under the dim city lights, you encourage {noblewomanName} to find strength " +
                "in her predicament. You speak of resilience and the hidden power in adversity. Your words focus on the inner " +
                "strength she possesses to withstand and eventually thrive in her new life. You paint a picture of a future " +
                "where she emerges stronger and more influential. Moved by your encouragement, {noblewomanName} decides to " +
                "face her marriage with courage, inspired to uncover her true potential amidst the challenges." ,
                
                //Event Choice 2AD
                "{=FleeingFate_Event_Choice_2AD}In a deep conversation with {noblewomanName}, you emphasize the power she " +
                "holds as a noblewoman. You suggest that she could use her position to influence the baron for the better, " +
                "potentially improving the lives of those under his rule. You paint a vision of her as a beacon of hope, " +
                "subtly guiding decisions and policies from the sidelines. Intrigued by this perspective, {noblewomanName} " +
                "chooses to accept her fate, seeing it as a platform for change and positive influence." ,
                
                //Event Choice 2AE
                "{=FleeingFate_Event_Choice_2AE}During your conversation, you highlight the opportunities for personal growth " +
                "that her situation presents. You talk about the skills she could develop and the wisdom she could gain in " +
                "navigating court politics and managing estate affairs. You point out that this experience could be invaluable " +
                "in shaping her into a more powerful and capable individual. Your perspective resonates with {noblewomanName}, " +
                "and she decides to stay, viewing her marriage as a challenging yet rewarding journey of personal development."
                
            };
            
            private static readonly List<string> eventChoice2B = new List<string>
            {
                //Event Choice 2BA
                "{=FleeingFate_Event_Choice_2BA}In the dim light of the city, you meet with {noblewomanName} and try to " +
                "impart the importance of resilience and adaptation. Despite your earnest attempts to illustrate how she " +
                "could find power within her new life, she remains unconvinced. Your words fail to pierce the veil of her d" +
                "espair, and she quietly rejects your counsel. She leaves the conversation with a sense of hopelessness, " +
                "unable to envision a future where this marriage could lead to anything but misery." ,
                
                //Event Choice 2BB
                "{=FleeingFate_Event_Choice_2BB}As you sit with {noblewomanName} in a quiet, secluded area, you discuss " +
                "the potential of forming strategic alliances within her new family. However, she seems disinterested in " +
                "the idea of navigating court politics or leveraging her position for influence. She can't see past the " +
                "immediate unhappiness of her situation, and your suggestions of long-term strategy fall on deaf ears. She " +
                "departs from the conversation with a heavy heart, her mind set on the immediate dread of her pending nuptials." ,
                
                //Event Choice 2BC
                "{=FleeingFate_Event_Choice_2BC}You attempt to inspire {noblewomanName} with words about finding inner " +
                "strength and the hidden power in adversity. However, your pep talk fails to resonate with her. She is " +
                "too overwhelmed by the reality of her situation to consider the possibility of personal growth through " +
                "hardship. She leaves the conversation feeling more isolated and misunderstood, her belief in a brighter " +
                "future further diminished." ,
                
                //Event Choice 2BD
                "{=FleeingFate_Event_Choice_2BD}In your conversation with {noblewomanName}, you highlight the influential " +
                "power she could wield as a noblewoman. But she views your ideas with skepticism. She struggles to see " +
                "how she could possibly sway the baron or effect any meaningful change in her constrained position. " +
                "Disheartened, she dismisses your suggestions as unrealistic, leaving the conversation with her sense of " +
                "entrapment and helplessness intact." ,
                
                //Event Choice 2BE
                "{=FleeingFate_Event_Choice_2BE}You speak to {noblewomanName} about the opportunities for personal growth " +
                "that her situation presents. However, she meets your ideas with disdain. The thought of personal development " +
                "pales in comparison to her immediate dread of the marriage. She cannot fathom enduring the union for the sake " +
                "of growth. She departs from your meeting feeling even more trapped, viewing your conversation as a series " +
                "of well-meaning but ultimately hollow platitudes."
                
            };
            
            private static readonly List<string> eventChoice3A = new List<string>
            {
                //Event Choice 3AA
                "{=FleeingFate_Event_Choice_3AA}Your decision not to intervene is coupled with a heartening talk about " +
                "finding inner courage and facing life's challenges. Initially, {noblewomanName} feels a wave of " +
                "disappointment, but as your conversation progresses, she starts to see things differently. You share " +
                "stories of resilience and the power of self-belief, which gradually shift her perspective. By the end " +
                "of your discussion, she finds an inner strength she didn't realize she had. She leaves with a sense of " +
                "purpose and a plan to approach her situation not as a victim but as a woman in control of her destiny.", 
                
                //Event Choice 3AB
                "{=FleeingFate_Event_Choice_3AB}Although you refuse direct assistance, you spend time instilling a sense " +
                "of self-belief in {noblewomanName}. You emphasize the importance of confidence and self-reliance in " +
                "overcoming life’s hurdles. She listens intently, her initial dismay giving way to a contemplative state. " +
                "Your words strike a chord, and by the end of the exchange, she's visibly uplifted. She departs with a " +
                "new mindset, determined to use her situation as a platform to grow stronger and more independent.", 
                
                //Event Choice 3AC
                "{=FleeingFate_Event_Choice_3AC}While you decline to help {noblewomanName} escape, you encourage her to " +
                "take a proactive approach to her life. You discuss how adversity can be a powerful motivator and catalyst " +
                "for change. Though disheartened at first, she gradually opens up to the idea of facing her challenges " +
                "head-on. Your conversation plants seeds of determination and resolve, leading her to leave with a renewed " +
                "spirit, ready to take charge of her life and turn obstacles into opportunities.", 
                
                //Event Choice 3AD
                "{=FleeingFate_Event_Choice_3AD}You choose not to help {noblewomanName} flee but instead guide her towards " +
                "self-discovery. You talk about the power of understanding one’s own strengths and using them to navigate " +
                "difficult situations. As she absorbs your words, a visible change occurs. She starts to see her impending " +
                "marriage not as an end but as a new beginning, a chance to assert herself and influence her surroundings. " +
                "Empowered by this realization, she sets forth, ready to explore and utilize her newfound understanding " +
                "of herself.", 
                
                //Event Choice 3AE
                "{=FleeingFate_Event_Choice_3AE}In denying her request for help, you focus on fostering her sense of " +
                "independence and resilience. You share insights into how challenging situations can be transformed into " +
                "platforms for personal growth and self-assertion. Initially, {noblewomanName} appears crestfallen, but " +
                "as the conversation unfolds, she begins to resonate with your perspective. Inspired by your belief in " +
                "her capabilities, she leaves feeling energized and ready to face her future with a resilient and " +
                "independent spirit."
                
            };
            
            private static readonly List<string> eventChoice3B = new List<string>
            {
                //Event Choice 3BA
                "{=FleeingFate_Event_Choice_3BA}In your conversation with {noblewomanName}, you express your decision not " +
                "to assist her directly, instead offering guidance to find inner fortitude. However, your words " +
                "seem to exacerbate her feelings of despair. She listens, but the hope in her eyes dims with each " +
                "passing moment. She had come seeking a lifeline, but instead, she feels as if she’s been handed an anchor. " +
                "The conversation, intended to empower her, ironically leaves her feeling more powerless and resigned to her " +
                "fate than before. As she departs, the weight of her situation seems to press down on her even more heavily, " +
                "leaving a shadow of the vibrant person she once was.", 
                
                //Event Choice 3BB
                "{=FleeingFate_Event_Choice_3BB}Your refusal to aid her escape, coupled with a speech about self-reliance, " +
                "is taken by {noblewomanName} not as encouragement, but as dismissive and insensitive. She hears your words, " +
                "but they sound hollow to her, echoing in the void of her despair. She responds with a bitter edge to her " +
                "voice, a sharp contrast to the gentle plea she had initially brought to you. Feeling misunderstood and let " +
                "down, she leaves with a sense of betrayal and a jaded view of seeking help, her trust in others deeply shaken.", 
                
                //Event Choice 3BC
                "{=FleeingFate_Event_Choice_3BC}When you decline to help {noblewomanName}, aiming instead to inspire her to " +
                "draw upon her own resilience, she takes it as a clear sign of your unwillingness to understand her plight. " +
                "Her expression falls, a visible representation of her crumbling hope. The conversation, rather than lifting " +
                "her spirits, pushes her further into a feeling of isolation. As she turns to leave, there’s a palpable sense " +
                "of loss – not just of her hopes of escaping her situation but also of her belief in compassion and empathy " +
                "from others.", 
                
                //Event Choice 3BD
                "{=FleeingFate_Event_Choice_3BD}As you suggest to {noblewomanName} that she find strength in herself, she " +
                "struggles to find comfort in your words. Instead of feeling empowered, she feels more anxious and scared. " +
                "The conversation heightens her fears, turning them into near-tangible phantoms that loom larger in her mind. " +
                "She leaves feeling overwhelmed by the magnitude of her predicament, her steps faltering and her resolve c" +
                "rumbling under the weight of her impending marriage.", 
                
                //Event Choice 3BE
                "{=FleeingFate_Event_Choice_3BE}Your well-meaning advice on self-reliance and inner strength falls on deaf " +
                "ears as {noblewomanName} finds herself unable to connect with the optimism and hope you try to instill. " +
                "She had hoped for a savior, but instead, she finds herself facing a mirror reflecting her own helplessness. " +
                "The conversation leaves her spiraling further into despair, convinced now more than ever that her situation " +
                "is a dark, inescapable well. She leaves not with a renewed sense of purpose, but with a heavy heart, her " +
                "spirit sinking under the burden of her unavoidable future."
                
            };
            
            private static readonly List<string> eventChoice4A= new List<string>
            {
                //Event Choice 4AA
                "{=FleeingFate_Event_Choice_4AA}You set up a meeting with {noblewomanName}'s family under the guise of " +
                "discussing her future. Utilizing your high steward skill, you meticulously lay out the benefits of aligning " +
                "their house with a more compatible, yet equally prestigious, noble family. You argue not just for {noblewomanName}'s " +
                "happiness, but also for the strategic advantages a different alliance would bring. Your persuasive eloquence " +
                "and detailed knowledge of noble lineages make a strong impression. After intense deliberation, her family " +
                "concedes to your insightful proposal. To show their gratitude for the favorable outcome you engineered, " +
                "they present you with a handsome sum of {goldReward}.", 
                
                //Event Choice 4AB
                "{=FleeingFate_Event_Choice_4AB}Your extensive knowledge of noble houses and their alliances leads you to " +
                "identify a suitable match for {noblewomanName}. This potential suitor, though lesser-known, possesses " +
                "qualities that complement {noblewomanName}’s aspirations and character. With great tact, you present this " +
                "match to her family, highlighting not only the match’s suitability but also its potential benefits, such " +
                "as reinforcing political ties or enhancing their social standing. Your strategic foresight in this matchmaking " +
                "process impresses her family. They agree to this more fitting alliance and, in recognition of your invaluable " +
                "service, reward you with a substantial amount of {goldReward}.", 
                
                //Event Choice 4AC
                "{=FleeingFate_Event_Choice_4AC}Your negotiation approach involves a deep dive into the social network of nobility. " +
                "Through your connections and astute observations, you identify a hidden gem: a nobleman whose qualities are " +
                "perfectly aligned with {noblewomanName}'s desires and values. He is of good standing and is known for his " +
                "progressive views and kindness. You arrange a meeting with her family and present your findings, emphasizing the " +
                "long-term happiness and stability this match could provide. The family, convinced by your thorough research " +
                "and thoughtful matchmaking, consents to this new engagement. For your exceptional matchmaking skills, they " +
                "gratefully bestow upon you a reward of {goldReward}.", 
                
                //Event Choice 4AD
                "{=FleeingFate_Event_Choice_4AD}In your meeting with {noblewomanName}'s family, you bring an old but " +
                "significant alliance to their attention, one that had been beneficial in the past but had since lapsed. " +
                "You suggest this alliance could be rekindled and strengthened through a new marital bond with {noblewomanName}. " +
                "You outline the mutual benefits, the shared history, and the potential for renewed cooperation and prosperity " +
                "between the two houses. Your knowledge of historical alliances and your ability to project these benefits into " +
                "future gains resonate well with her family. Seeing the wisdom in your proposal, they agree to pursue this " +
                "renewed alliance. Impressed by your historical insight and strategic thinking, they reward your efforts " +
                "with a generous sum of {goldReward}.", 
                
                //Event Choice 4AE
                "{=FleeingFate_Event_Choice_4AE}In your negotiation with {noblewomanName}'s family, you emphasize the importance " +
                "of looking to the future. You propose a marriage alliance with a family known for its progressive stance, " +
                "aligning well with {noblewomanName}'s own modern views and her aspiration for a more meaningful life. This " +
                "union, you argue, would not only ensure her happiness but also position both families as forward-thinking " +
                "leaders in a changing world. You discuss how this alliance could open up new opportunities and set a precedent " +
                "for future generations. Captivated by your vision of a forward-looking union, her family agrees to your innovative " +
                "proposal. In appreciation of your foresight and the promising future you’ve helped to secure, they present you " +
                "with a significant reward of {goldReward}."
                
            };
            
            private static readonly List<string> eventChoice4B= new List<string>
            {
                //Event Choice 4BA
                "{=FleeingFate_Event_Choice_4BA}During a formal negotiation session with {noblewomanName}'s family, you meticulously " +
                "present a well-crafted argument for a more suitable marital match, emphasizing mutual benefits and long-term " +
                "happiness. Despite your eloquent speech and strategic insights, her family remains staunchly unyielding. They v" +
                "iew your suggestions as unnecessary interference in their affairs. The conversation grows heated as they staunchly " +
                "defend their decision, and you realize that they are not open to outside influence. The meeting concludes abruptly " +
                "with you being escorted out by guards, leaving you reflecting on the failed negotiation and the potential damage " +
                "to your rapport with a key noble house.", 
                
                //Event Choice 4BB
                "{=FleeingFate_Event_Choice_4BB}You approach {noblewomanName}'s family with confidence, proposing an alternative " +
                "suitor who would be a more compatible match. However, instead of interest, your proposal is met with immediate " +
                "and intense hostility. The family, deeply offended by what they perceive as a challenge to their authority, " +
                "accuses you of overstepping your boundaries. Despite your attempts to diffuse the situation and explain your " +
                "rationale, the atmosphere becomes increasingly hostile. The meeting ends with you being forcefully removed, a " +
                "clear sign that not only has the negotiation failed, but it has also potentially incited animosity " +
                "from a powerful family.", 
                
                //Event Choice 4BC
                "{=FleeingFate_Event_Choice_4BC}With the intent to offer a mutually beneficial alternative, you present your " +
                "proposal to {noblewomanName}'s family. However, your ideas are grossly misinterpreted. The family, taking " +
                "umbrage at what they perceive as an insinuation of their poor judgment, reacts defensively. Your attempts " +
                "to steer the conversation back on track are in vain as they grow increasingly indignant. The dialogue escalates " +
                "quickly, culminating in accusations of disrespect and meddling. The meeting concludes with you being briskly " +
                "ushered out, leaving you to contemplate the abrupt and harsh dismissal and its implications for your " +
                "future dealings.", 
                
                //Event Choice 4BD
                "{=FleeingFate_Event_Choice_4BD}You enter the negotiation with a clear strategy, advocating for a marriage " +
                "alliance that you believe would be more beneficial for all involved. Unfortunately, your approach is met" +
                " with unexpected resistance. The family, offended by your attempt to influence their decision, questions " +
                "your understanding of their values and traditions. The discussion takes a turn for the worse, with the " +
                "family expressing their displeasure at your audacity to suggest an alternative. The tense meeting ends with " +
                "a stern warning for you to refrain from such overtures in the future, and you're escorted out, realizing " +
                "that your well-intentioned strategy has led to a diplomatic strain.", 
                
                //Event Choice 4BE
                "{=FleeingFate_Event_Choice_4BE}With a vision of a progressive and beneficial union, you meet with " +
                "{noblewomanName}'s family to present your innovative proposal. However, the family is entrenched in " +
                "traditional views and is not receptive to your modern ideas. They dismiss your proposal as too avant-garde " +
                "and risky, refusing to deviate from their established plans. Displeased with your unorthodox approach, " +
                "they bring the meeting to an abrupt close. You're shown the door with a clear message that your forward-thinking " +
                "proposals are unwelcome, leaving you to ponder the conservative nature of the nobility and the possible " +
                "repercussions for your reputation among them."

            };
            
            private static readonly List<string> eventChoice5A= new List<string>
            {
                //Event Choice 5A
                "{=FleeingFate_Event_Choice_5A}In a secluded and dimly lit part of the city, you confront {noblewomanName} " +
                "with a sadistic glee. You begin with a display of raw physical power, inflicting pain through calculated " +
                "strikes, each blow a message of your control and her helplessness. As she cowers, you lean in, whispering " +
                "detailed threats about the horrors and tortures that await her and her loved ones if she dares to defy. " +
                "The combination of your physical brutality and the horrific future you paint breaks her. She nods in " +
                "submission, traumatized and defeated.", 
                
                //Event Choice 5B
                "{=FleeingFate_Event_Choice_5B}You ambush {noblewomanName} in an isolated area, where your violent outburst " +
                "leaves no room for doubt or hope. Your fists and words are weapons, each strike accompanied by verbal assaults, " +
                "crushing her dignity and resolve. As you see her spirit crumble under the assault, you reinforce the inevitability " +
                "of her situation. Her acquiescence comes not out of agreement but from a place of utter despair and brokenness, " +
                "a direct result of your merciless attack.", 
                
                //Event Choice 5C
                "{=FleeingFate_Event_Choice_5C}In an abandoned building, you subject {noblewomanName} to a night of relentless " +
                "mental and physical torture. You methodically alternate between inflicting pain and instilling fear, breaking " +
                "down her will to resist. Each act of violence is designed to show her the futility of defiance and the depth " +
                "of your cruelty. By morning, she is a shell of herself, agreeing to anything in a desperate bid to end her suffering.", 
                
                //Event Choice 5D
                "{=FleeingFate_Event_Choice_5D}You lure {noblewomanName} to a place where you've staged a brutal demonstration " +
                "of your power. You coldly execute a series of actions designed to instill a deep-seated fear of your capabilities. " +
                "She watches, horror-struck, as you demonstrate what happens to those who cross you. The display leaves her " +
                "in no doubt about the severity of her situation. Shaken to the core, she agrees to the marriage, her will to " +
                "resist completely extinguished.", 
                
                //Event Choice 5E
                "{=FleeingFate_Event_Choice_5E}You orchestrate {noblewomanName}'s abduction to a hidden location where she " +
                "is kept captive. Over several days, you visit her, each time using physical force and menacing threats to erode " +
                "her resolve. Isolated and subjected to regular bouts of violence, her hope diminishes with each passing hour. " +
                "When you finally offer her the chance to end her ordeal by agreeing to the marriage, she does so, seeing it as " +
                "her only escape from the nightmare you've created."

            };
            
            private static readonly List<string> eventChoice6A= new List<string>
            {
                //Event Choice 6A
                "{=FleeingFate_Event_Choice_6A}Deep in the secluded wilderness, you lead {noblewomanName} under the false " +
                "hope of escape. As the desolation of the surroundings becomes apparent, you reveal your true, malevolent " +
                "self. Her initial shock turns to abject terror as you relish in describing the fate you have planned for " +
                "her. You proceed to inflict a series of calculated, cruel tortures, each methodically designed to maximize " +
                "her suffering and fear. Her cries of agony and betrayal fill the air, a twisted symphony to your ears. You " +
                "prolong her torment, savoring each moment of her despair, before finally, in a barbaric act, you end her life. " +
                "You leave her in an unmarked grave, her last moments a prolonged ordeal of merciless cruelty.", 
                
                //Event Choice 6B
                "{=FleeingFate_Event_Choice_6B}In the eerie silence of ancient ruins, {noblewomanName} faces the horrifying " +
                "reality of your betrayal. What starts as a promise of liberation quickly turns into a nightmare. You take " +
                "perverse pleasure in slowly dismantling her hopes, replacing them with the terror of impending doom. Each of " +
                "your actions is more brutal and heartless than the last, as you push the boundaries of cruelty. Her suffering under " +
                "your hands is prolonged and excruciating, her screams echoing off the forgotten walls. Her life ends not with a swift " +
                "act but a drawn-out, savage display of your darkest desires, leaving her broken body as a testament to your heinous deed.", 
                
                //Event Choice 6C
                "{=FleeingFate_Event_Choice_6C}At the river's edge, the moonlight casts a sinister glow as you enact your " +
                "deceitful plan. With a smile, you push {noblewomanName} into the frigid waters. As she struggles, the realization " +
                "of your treachery overwhelms her. You watch with cold detachment as her struggle turns to desperation. Her attempts " +
                "to cling to life become weaker, each gasp for air a fading hope. You ensure her demise is not quick; her drawn-out " +
                "battle with the river becomes a spectacle of your cruelty. Finally, as the river claims her, you leave, her demise " +
                "a dark secret swallowed by the waters.", 
                
                //Event Choice 6D
                "{=FleeingFate_Event_Choice_6D}The abandoned warehouse becomes a chamber of horrors as you reveal your monstrous " +
                "intentions to {noblewomanName}. Each word you utter is laced with venom, each action a calculated assault on her body " +
                "and spirit. You methodically break her, physically and mentally, enjoying the crescendo of her screams. Your methods " +
                "are barbaric, each more vicious than the last, as you relish the complete domination over her being. The warehouse " +
                "reverberates with the sounds of your savagery until finally, you silence her permanently, her life ending in a " +
                "crescendo of cruelty.", 
                
                //Event Choice 6E
                "{=FleeingFate_Event_Choice_6E}In a dank, forgotten cellar, you bring {noblewomanName} to face a fate worse than " +
                "death. The chilling realization of your betrayal is just the beginning of her nightmare. You unleash upon her a " +
                "series of inhuman tortures, each more depraved and gruesome. Her pain and fear fuel your malevolence, her screams " +
                "a melody to your dark soul. You drag out her suffering, inflicting wound upon wound, breaking her in body and mind. " +
                "When you finally grant her release from the torment, it's in a manner so brutal and merciless that it extinguishes " +
                "all that she once was. Her end is a slow, agonizing descent into oblivion, marked by your unrelenting malice."

            };

            
            private static readonly List<string> eventMsg1A = new List<string>
            {
                "{=FleeingFate_Event_Msg_1AA}{heroName} helped {noblewomanName} escape and received {goldReward} gold as a reward." ,
                "{=FleeingFate_Event_Msg_1AB}{heroName} successfully aided {noblewomanName} in fleeing the city, earning {goldReward} gold for the deed." ,
                "{=FleeingFate_Event_Msg_1AC}{heroName} orchestrated {noblewomanName}'s escape, being compensated with {goldReward} gold for the effort." ,
                "{=FleeingFate_Event_Msg_1AD}{heroName} ensured {noblewomanName}'s safe departure from the city and was rewarded with {goldReward} gold." ,
                "{=FleeingFate_Event_Msg_1AE}{heroName} facilitated {noblewomanName}'s successful escape and was given {goldReward} gold in gratitude."
            };
            
            private static readonly List<string> eventMsg1B = new List<string>
            {
                "{=FleeingFate_Event_Msg_1BA}{heroName}'s attempt to help {noblewomanName} escape was thwarted, narrowly avoided capture." ,
                "{=FleeingFate_Event_Msg_1BB}{heroName} failed in aiding {noblewomanName}'s escape, barely escaping the guards' clutches." ,
                "{=FleeingFate_Event_Msg_1BC}{heroName} could not secure {noblewomanName}'s freedom, having to flee alone as the plan fell apart." ,
                "{=FleeingFate_Event_Msg_1BD}{heroName}'s plan to help {noblewomanName} flee was foiled, forcing a retreat empty-handed." ,
                "{=FleeingFate_Event_Msg_1BE}{heroName} attempted to assist {noblewomanName}'s escape, but the plan failed, resulting in a narrow escape."
                
            };
            
            private static readonly List<string> eventMsg2A = new List<string>
            {
                "{=FleeingFate_Event_Msg_2AA}{heroName} persuaded {noblewomanName} to stay, enhancing her resolve and perspective." ,
                "{=FleeingFate_Event_Msg_2AB}{heroName} counseled {noblewomanName}, leading to her renewed determination and hope." ,
                "{=FleeingFate_Event_Msg_2AC}{heroName}'s advice led {noblewomanName} to a path of resilience and acceptance." ,
                "{=FleeingFate_Event_Msg_2AD}{heroName} influenced {noblewomanName} to adapt, fostering her growth and strength." ,
                "{=FleeingFate_Event_Msg_2AE}{heroName} guided {noblewomanName}, turning her predicament into a newfound opportunity."
                
            };
            
            private static readonly List<string> eventMsg2B = new List<string>
            {
                "{=FleeingFate_Event_Msg_2BA}{heroName}'s attempt to persuade {noblewomanName} failed, leaving her despondent." ,
                "{=FleeingFate_Event_Msg_2BB}{heroName} could not sway {noblewomanName}, who remained set against her situation." ,
                "{=FleeingFate_Event_Msg_2BC}{heroName}'s counsel was rejected by {noblewomanName}, deepening her despair." ,
                "{=FleeingFate_Event_Msg_2BD}{heroName}'s efforts to convince {noblewomanName} were in vain, leaving her unresolved." ,
                "{=FleeingFate_Event_Msg_2BE}{heroName}'s advice to {noblewomanName} fell short, failing to alter her grim outlook."

            };

            
            private static readonly List<string> eventMsg3A = new List<string>
            {
                "{=FleeingFate_Event_Msg_3AA}{heroName}'s encouragement led {noblewomanName} to find her own strength, becoming a valuable ally." ,
                "{=FleeingFate_Event_Msg_3AB}{heroName} inspired {noblewomanName} to self-reliance, resulting in her influential friendship." ,
                "{=FleeingFate_Event_Msg_3AC}{heroName}'s advice empowered {noblewomanName}, who forged a path of success and alliance." ,
                "{=FleeingFate_Event_Msg_3AD}{heroName} motivated {noblewomanName} to independence, leading to her becoming a supportive ally." ,
                "{=FleeingFate_Event_Msg_3AE}{heroName}'s guidance helped {noblewomanName} to self-empowerment, securing a future ally."

            };
            
            private static readonly List<string> eventMsg3B = new List<string>
            {
                "{=FleeingFate_Event_Msg_3BA}{heroName}'s refusal to help left {noblewomanName} bitter, resulting in damaging rumors." ,
                "{=FleeingFate_Event_Msg_3BB}{heroName}'s denial of aid led {noblewomanName} to resentment, sparking adverse gossip." ,
                "{=FleeingFate_Event_Msg_3BC}{heroName} declined to assist {noblewomanName}, who later spread disparaging words." ,
                "{=FleeingFate_Event_Msg_3BD}{heroName}'s lack of support caused {noblewomanName}'s enmity, leading to negative hearsay." ,
                "{=FleeingFate_Event_Msg_3BE}{heroName}'s rejection made {noblewomanName} hostile, resulting in harmful backlash."

            };
            
            private static readonly List<string> eventMsg4A = new List<string>
            { 
                "{=FleeingFate_Event_Msg_4AA}{heroName}'s negotiation secured a better match for {noblewomanName}, earning gratitude and {goldReward}." ,
                "{=FleeingFate_Event_Msg_4AB}{heroName} successfully brokered a more suitable alliance for {noblewomanName}, rewarded with {goldReward}." ,
                "{=FleeingFate_Event_Msg_4AC}{heroName}'s diplomatic skills found {noblewomanName} a happier future, gaining {goldReward}." ,
                "{=FleeingFate_Event_Msg_4AD}{heroName} arranged a favorable marriage for {noblewomanName}, receiving {goldReward} for the effort." ,
                "{=FleeingFate_Event_Msg_4AE}{heroName} negotiated a better suitor for {noblewomanName}, rewarded with {goldReward}." 

            };
            
            private static readonly List<string> eventMsg4B = new List<string>
            { 
                "{=FleeingFate_Event_Msg_4BA}{heroName}'s negotiation for {noblewomanName} failed, leading to their dismissal." ,
                "{=FleeingFate_Event_Msg_4BB}{heroName} could not sway {noblewomanName}'s family, resulting in being expelled." ,
                "{=FleeingFate_Event_Msg_4BC}{heroName}'s efforts to find a new match for {noblewomanName} were rebuffed." ,
                "{=FleeingFate_Event_Msg_4BD}{heroName}'s proposal for {noblewomanName} was rejected, ending in ejection." ,
                "{=FleeingFate_Event_Msg_4BE}{heroName} faced refusal in re-matching {noblewomanName}, leading to their removal."

            };
            
            private static readonly List<string> eventMsg5A = new List<string>
            { 
                "{=FleeingFate_Event_Msg_5A}{heroName} coerced {noblewomanName}'s submission through unsparing means, ensuring her forced agreement." ,
                "{=FleeingFate_Event_Msg_5B}{heroName} used extreme measures to break {noblewomanName}'s defiance, leaving her no choice but compliance." ,
                "{=FleeingFate_Event_Msg_5C}{heroName} forced {noblewomanName} into submission with a display of overwhelming power and intimidation." ,
                "{=FleeingFate_Event_Msg_5D}{heroName} ensured {noblewomanName}'s reluctant acquiescence through a harrowing show of force." ,
                "{=FleeingFate_Event_Msg_5E}{heroName} subjugated {noblewomanName} with a ruthless combination of mental and physical domination."
                
            };
            
            private static readonly List<string> eventMsg6A = new List<string>
            { 
                "{=FleeingFate_Event_Msg_6A}{heroName} orchestrated a grim fate for {noblewomanName}, ensuring her tragic and undetected demise." ,
                "{=FleeingFate_Event_Msg_6B}{heroName} enacted a merciless plan, leading to the permanent and untraceable disappearance of {noblewomanName}." ,
                "{=FleeingFate_Event_Msg_6C}{heroName} cruelly ensured {noblewomanName}'s fate was sealed in secrecy, leaving no trace behind." ,
                "{=FleeingFate_Event_Msg_6D}{heroName}'s actions resulted in the silent vanishing of {noblewomanName}, her end shrouded in darkness." ,
                "{=FleeingFate_Event_Msg_6E}{heroName} masterminded a harrowing end for {noblewomanName}, with her final moments lost to oblivion."
                
            };

            
            public static string GetRandomEventTitle()
            {
                var index = random.Next(eventTitles.Count);
                return eventTitles[index];
            }
            
            public static string GetRandomNoblewomanNames()
            {
                var index = random.Next(noblewomanNames.Count);
                return noblewomanNames[index];
            }
            
            public static string GetRandomEventDescription()
            {
                var index = random.Next(eventDescriptions.Count);
                return eventDescriptions[index];
            }
            
            public static string GetRandomEventChoice1A()
            {
                var index = random.Next(eventChoice1A.Count);
                return eventChoice1A[index];
            }
            
            public static string GetRandomEventChoice1B()
            {
                var index = random.Next(eventChoice1B.Count);
                return eventChoice1B[index];
            }
            
            public static string GetRandomEventChoice2A()
            {
                var index = random.Next(eventChoice2A.Count);
                return eventChoice2A[index];
            }
            
            public static string GetRandomEventChoice2B()
            {
                var index = random.Next(eventChoice2B.Count);
                return eventChoice2B[index];
            }
            
            public static string GetRandomEventChoice3A()
            {
                var index = random.Next(eventChoice3A.Count);
                return eventChoice3A[index];
            }
            
            public static string GetRandomEventChoice3B()
            {
                var index = random.Next(eventChoice3B.Count);
                return eventChoice3B[index];
            }
            
            public static string GetRandomEventChoice4A()
            {
                var index = random.Next(eventChoice4A.Count);
                return eventChoice4A[index];
            }
            
            public static string GetRandomEventChoice4B()
            {
                var index = random.Next(eventChoice4B.Count);
                return eventChoice4B[index];
            }
            
            public static string GetRandomEventChoice5A()
            {
                var index = random.Next(eventChoice5A.Count);
                return eventChoice5A[index];
            }
            
            public static string GetRandomEventChoice6A()
            {
                var index = random.Next(eventChoice6A.Count);
                return eventChoice6A[index];
            }
            
            public static string GetRandomEventMessage1A()
            {
                var index = random.Next(eventMsg1A.Count);
                return eventMsg1A[index];
            }
            
            public static string GetRandomEventMessage1B()
            {
                var index = random.Next(eventMsg1B.Count);
                return eventMsg1B[index];
            }
            
            public static string GetRandomEventMessage2A()
            {
                var index = random.Next(eventMsg2A.Count);
                return eventMsg2A[index];
            }
            
            public static string GetRandomEventMessage2B()
            {
                var index = random.Next(eventMsg2B.Count);
                return eventMsg2B[index];
            }
            
            public static string GetRandomEventMessage3A()
            {
                var index = random.Next(eventMsg3A.Count);
                return eventMsg3A[index];
            }
            
            public static string GetRandomEventMessage3B()
            {
                var index = random.Next(eventMsg3B.Count);
                return eventMsg3B[index];
            }
            
            public static string GetRandomEventMessage4A()
            {
                var index = random.Next(eventMsg4A.Count);
                return eventMsg4A[index];
            }
            
            public static string GetRandomEventMessage4B()
            {
                var index = random.Next(eventMsg4B.Count);
                return eventMsg4B[index];
            }
            
            public static string GetRandomEventMessage5A()
            {
                var index = random.Next(eventMsg5A.Count);
                return eventMsg5A[index];
            }
            
            public static string GetRandomEventMessage6A()
            {
                var index = random.Next(eventMsg6A.Count);
                return eventMsg6A[index];
            }
        }
    }


    public class FleeingFateData : RandomEventData
    {
        public FleeingFateData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new FleeingFate();
        }
    }
}