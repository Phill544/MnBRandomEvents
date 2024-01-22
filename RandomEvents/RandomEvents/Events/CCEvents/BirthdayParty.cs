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
    public sealed class BirthdayParty : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minAttending;
        private readonly int maxAttending;
        private readonly int minYourMenAttending;
        private readonly int maxYourMenAttending;
        private readonly int minAge;
        private readonly int maxAge;
        private readonly int minBandits;
        private readonly int maxBandits;
        private readonly int minGoldGiven;
        private readonly int maxGoldGiven;
        private readonly int minInfluenceGain;
        private readonly int maxInfluenceGain;
        private readonly int minGoldLooted;
        private readonly int maxGoldLooted;
        private readonly int minRogueryLevel;

        public BirthdayParty() : base(ModSettings.RandomEvents.BirthdayPartyData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("BirthdayParty", "EventDisabled");
            minAttending = ConfigFile.ReadInteger("BirthdayParty", "MinAttending");
            maxAttending = ConfigFile.ReadInteger("BirthdayParty", "MaxAttending");
            minYourMenAttending = ConfigFile.ReadInteger("BirthdayParty", "MinYourMenAttending");
            maxYourMenAttending = ConfigFile.ReadInteger("BirthdayParty", "MaxYourMenAttending");
            minAge = ConfigFile.ReadInteger("BirthdayParty", "MinAge");
            maxAge = ConfigFile.ReadInteger("BirthdayParty", "MaxAge");
            minBandits = ConfigFile.ReadInteger("BirthdayParty", "MinBandits");
            maxBandits = ConfigFile.ReadInteger("BirthdayParty", "MaxBandits");
            minGoldGiven = ConfigFile.ReadInteger("BirthdayParty", "MinGoldGiven");
            maxGoldGiven = ConfigFile.ReadInteger("BirthdayParty", "MaxGoldGiven");
            minInfluenceGain = ConfigFile.ReadInteger("BirthdayParty", "MinInfluenceGain");
            maxInfluenceGain = ConfigFile.ReadInteger("BirthdayParty", "MaxInfluenceGain");
            minGoldLooted = ConfigFile.ReadInteger("BirthdayParty", "MinGoldLooted");
            maxGoldLooted = ConfigFile.ReadInteger("BirthdayParty", "MaxGoldLooted");
            minRogueryLevel = ConfigFile.ReadInteger("BirthdayParty", "MinRogueryLevel");

            //Overrides the min age.
            minAge = Math.Max(minAge, 16);
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled) return false;

            return minAttending != 0 || maxAttending != 0 || minYourMenAttending != 0 || maxYourMenAttending != 0 ||
                   minAge != 0 || maxAge != 0 || minBandits != 0 || maxBandits != 0 || minGoldGiven != 0 ||
                   maxGoldGiven != 0 || minInfluenceGain != 0 || maxInfluenceGain != 0 || minGoldLooted != 0 ||
                   maxGoldLooted != 0 || minRogueryLevel != 0;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxYourMenAttending &&
                   Hero.MainHero.Gold > maxGoldGiven;
        }

        public override void StartEvent()
        {
            var heroName = Hero.MainHero.FirstName;

            var birthdayAge = MBRandom.RandomInt(minAge, maxAge);
            var yourMenAttending = MBRandom.RandomInt(minYourMenAttending, maxYourMenAttending);
            var peopleAttending = MBRandom.RandomInt(minAttending, maxAttending);
            var bandits = MBRandom.RandomInt(minBandits, maxBandits);
            var goldGiven = MBRandom.RandomInt(minGoldGiven, maxGoldGiven);
            var influenceGain = MBRandom.RandomInt(minInfluenceGain, maxInfluenceGain);
            var goldBase = MBRandom.RandomInt(minGoldLooted, maxGoldLooted);
            
            var goldLooted = goldBase * peopleAttending;
            
            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var canRaidWedding = false;

            var rogueryAppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                
                canRaidWedding = true;
                rogueryAppendedText = new TextObject("{=BirthdayParty_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();

            }
            else
            {
                if (rogueryLevel >= minRogueryLevel)
                {
                    canRaidWedding = true;
                    
                    rogueryAppendedText = new TextObject("{=BirthdayParty_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
            }


            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

            var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription())
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("peopleAttending", peopleAttending)
                .SetTextVariable("birthdayAge", birthdayAge).ToString();

            var eventOption1 = new TextObject("{=BirthdayParty_Event_Option_1}Join them in celebration!").ToString();
            var eventOption1Hover = new TextObject("{=BirthdayParty_Event_Option_1_Hover}Not everyday you turn {birthdayAge} years old!").SetTextVariable("birthdayAge", birthdayAge).ToString();
            
            var eventOption2 = new TextObject("{=BirthdayParty_Event_Option_2}Give the girl some gold").ToString();
            var eventOption2Hover = new TextObject("{=BirthdayParty_Event_Option_2_Hover}You don't have time to stay but you can still be nice, right?").ToString();

            var eventOption3 = new TextObject("{=BirthdayParty_Event_Option_3}Move on").ToString();
            var eventOption3Hover = new TextObject("{=BirthdayParty_Event_Option_3_Hover}You don't have time").ToString();
            
            var eventOption4 = new TextObject("{=BirthdayParty_Event_Option_4}[Roguery] Raid the party").ToString();
            var eventOption4Hover = new TextObject("{=BirthdayParty_Event_Option_4_Hover}Have some fun.\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();

            var eventButtonText = new TextObject("{=BirthdayParty_Event_Button_Text}Okay").ToString();
            
            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            
            if (canRaidWedding)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }
            

            var eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1())
                .SetTextVariable("yourMenAttending", yourMenAttending)
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("goldGiven", goldGiven)
                .SetTextVariable("bandits", bandits)
                .ToString();
            
            var eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2())
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("goldGiven", goldGiven)
                .ToString();
            
            var eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3())
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4())
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();
            
            var eventMsg1 =new TextObject(EventTextHandler.GetRandomEventMessage1())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldGiven", goldGiven)
                .SetTextVariable("influenceGain", influenceGain)
                .ToString();
            
            var eventMsg2 =new TextObject(EventTextHandler.GetRandomEventMessage2())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldGiven", goldGiven)
                .ToString();
            
            var eventMsg3 =new TextObject(EventTextHandler.GetRandomEventMessage3())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();
            
            var eventMsg4 =new TextObject(EventTextHandler.GetRandomEventMessage4())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();

            
            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            Hero.MainHero.AddInfluenceWithKingdom(influenceGain);
                            MobileParty.MainParty.MoraleExplained.Add(5);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionCText, true, false, eventButtonText, null, null, null), true);
                            MobileParty.MainParty.MoraleExplained.Add(-5);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionDText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(+goldGiven);
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
                "{=BirthdayParty_Title_A}The Birthday Party",
                "{=BirthdayParty_Title_B}Joyous Birthday Revelry",
                "{=BirthdayParty_Title_C}The Birthday Bash",
                "{=BirthdayParty_Title_D}Birthday Jubilee",
                "{=BirthdayParty_Title_E}The Birthday Celebration",
                "{=BirthdayParty_Title_F}Birthday Festivities",
                "{=BirthdayParty_Title_G}Anniversary of Birth",
                "{=BirthdayParty_Title_H}Ceremonial Birthday Gathering"
            };
            
            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=BirthdayParty_Event_Desc_A}As you and your party are traveling in the vicinity of {closestSettlement}, " +
                "you come across {peopleAttending} people in what seems to be a birthday party for a young girl. A " +
                "couple of the guests invite you to join them in celebrating the girl's {birthdayAge}th " +
                "birthday! What should you do?",
                
                //Event Description B
                "{=BirthdayParty_Event_Desc_B}While journeying near {closestSettlement} with your group, you stumble upon " +
                "a birthday celebration. There are {peopleAttending} guests gathered, joyously marking a young girl's " +
                "special day. Some of the attendees warmly invite you to partake in the festivities for the girl's " +
                "{birthdayAge}th birthday. Faced with this unexpected invitation, what course of action will you take?",
                
                //Event Description C
                "{=BirthdayParty_Event_Desc_C}On your travels close to {closestSettlement}, you and your companions " +
                "encounter a jovial gathering. It appears to be a birthday party for a young girl, attended by " +
                "{peopleAttending} individuals. A few of the partygoers extend an invitation to join the celebration " +
                "of the girl's {birthdayAge}th birthday. How will you respond to this cheerful invitation?",
                
                //Event Description D
                "{=BirthdayParty_Event_Desc_D}In the vicinity of {closestSettlement}, you and your party chance upon " +
                "{peopleAttending} people engaged in what looks to be a birthday bash for a young girl. Several guests " +
                "cordially invite you to join the merriment of her {birthdayAge}th birthday. Presented with this " +
                "unexpected but joyous opportunity, how do you choose to proceed?",
                
                //Event Description E
                "{=BirthdayParty_Event_Desc_E}As you traverse near {closestSettlement} with your group, you find a " +
                "birthday party underway, with {peopleAttending} attendees celebrating a young girl's milestone. " +
                "A couple of the guests hospitably invite you to partake in the young girl's {birthdayAge}th " +
                "birthday festivities. In this surprising yet pleasant situation, what decision will you make?"
            };
            
            private static readonly List<string> eventChoice1 = new List<string>
            {
                //Event Choice 1A
                "{=BirthdayParty_Event_Choice_1A}You and {yourMenAttending} of your men decide to stay for the party " +
                "while the rest makes their way to {closestSettlement}. You approach the girl and give her {goldGiven} " +
                "gold as a gift. She gives you a hug and says thank you. You get yourself some beer and sit down to enjoy " +
                "the moment.\n\nSome time later, {bandits} bandits decide to crash the party. They go around from person " +
                "to person and takes everything of value. You order your men to stand down as you don't want to start " +
                "a fight with innocent people caught in the middle. After they have taken everything of value they " +
                "also try to take the young girl with them. This you will not stand for so you signal your men to " +
                "strike. You and your men make quick work in incapacitating the bandits. One of your men rides " +
                "to {closestSettlement} to fetch someone to throw these scum in the dungeon. The rest of the night " +
                "you are celebrated as a hero! You even get to dance with the birthday girl!",
                
                //Event Choice 1B
                "{=BirthdayParty_Event_Choice_1B}You and {yourMenAttending} of your men choose to join the birthday " +
                "celebration, sending the rest to {closestSettlement}. Greeting the birthday girl, you gift her " +
                "{goldGiven} gold, earning a warm hug and heartfelt thanks. As you relax with a beer, the party is " +
                "suddenly invaded by {bandits} bandits, who start looting the guests. You command your men to refrain " +
                "from action, prioritizing the safety of the civilians. Yet, when the bandits try to abduct the " +
                "birthday girl, you decisively intervene. With a signal, your men engage, efficiently subduing the " +
                "bandits. One of your men rides off to {closestSettlement} for reinforcements, while the rest of the " +
                "evening sees you celebrated as a hero, even sharing a dance with the birthday girl in a joyous end " +
                "to the night.",
                
                //Event Choice 1C
                "{=BirthdayParty_Event_Choice_1C}Deciding to partake in the festivities, you and {yourMenAttending} of " +
                "your men stay for the party while the others head to {closestSettlement}. Upon meeting the birthday " +
                "girl, you present her with {goldGiven} gold, receiving a grateful hug and thanks in return. " +
                "Settling down with a beer, you savor the moment. However, the peace is shattered when {bandits} " +
                "bandits disrupt the party, ruthlessly taking valuables from each guest. Initially, you instruct your " +
                "men to hold back to avoid endangering bystanders. But when the bandits attempt to kidnap the young " +
                "girl, you refuse to stand idly by. Signaling your men, you swiftly overpower the bandits, averting the " +
                "kidnapping. One man dashes to {closestSettlement} for authorities, while you're hailed as a hero for " +
                "the remainder of the evening, even sharing a dance with the birthday girl.",
                
                //Event Choice 1D
                "{=BirthdayParty_Event_Choice_1D}You, along with {yourMenAttending} of your men, opt to stay at the party, " +
                "while the remainder proceed to {closestSettlement}. Approaching the girl, you kindly gift her {goldGiven} " +
                "gold, and she reciprocates with a hug and thanks. Settling down with a beer, you begin to relax, but the " +
                "tranquility is soon disrupted by {bandits} bandits. They start plundering the guests, and you order your " +
                "men to stand down, wary of risking civilian lives. However, when they target the birthday girl, your " +
                "tolerance ends. You signal your men, and together you quickly disarm the bandits. One of your men hastens " +
                "to {closestSettlement} to summon the authorities. The night then transforms, with the guests lauding " +
                "you as a hero, and you even share a celebratory dance with the birthday girl.",
                
                //Event Choice 1E
                "{=BirthdayParty_Event_Choice_1E}Choosing to stay, you and {yourMenAttending} of your men join the party, " +
                "while the others head towards {closestSettlement}. Upon meeting the birthday girl, you give her " +
                "{goldGiven} gold as a gift, and she responds with a hug and gratitude. Settling with a beer, you begin " +
                "to enjoy the festivities. This calm is soon disrupted by {bandits} bandits raiding the party and " +
                "looting the guests. You initially order your men not to engage, to avoid civilian casualties. But when " +
                "the bandits attempt to abduct the birthday girl, you take decisive action. Giving the signal, your men " +
                "swiftly incapacitate the bandits. One of your men rushes to {closestSettlement} for law enforcement, " +
                "while you spend the rest of the night as the celebrated hero, culminating in a dance with the " +
                "birthday girl."
            };
            
            private static readonly List<string> eventChoice2 = new List<string>
            {
                //Event Choice 2A
                "{=BirthdayParty_Event_Choice_2A}You really don't have time to linger, but the thought of coming " +
                "across as impolite weighs on you. So, with a sense of obligation mixed with a touch of goodwill, you " +
                "quickly gather {goldGiven} gold from your purse. Handing it over to the birthday girl, you notice her " +
                "eyes light up with gratitude, a gesture that warms your heart a little. You exchange a few pleasantries " +
                "with the partygoers, their warm smiles and sincere thanks making the moment more meaningful than you " +
                "anticipated. Despite the urgency of your schedule, this brief interaction leaves a pleasant imprint. " +
                "Finally, with a polite nod and a few well-wishes, you extricate yourself from the gathering and set off " +
                "towards {closestSettlement}, the sound of the ongoing celebration fading behind you, leaving you with " +
                "mixed feelings of haste and a hint of regret for not staying longer.",
                
                //Event Choice 2B
                "{=BirthdayParty_Event_Choice_2B}Despite the pressing nature of your journey, you don't wish to " +
                "come off as discourteous to the party. Hastily, you assemble {goldGiven} gold and present it to the birthday " +
                "girl, who responds with a genuine smile of thanks. You take a moment to exchange friendly words with the " +
                "other guests, their welcoming demeanor making your brief stay more enjoyable than expected. After these " +
                "short but sweet exchanges, you bid farewell to the group and proceed towards {closestSettlement}, the echoes " +
                "of the jovial party lingering in your ears as you leave, a subtle sense of warmth accompanying your departure.",
                
                //Event Choice 2C
                "{=BirthdayParty_Event_Choice_2C}Finding yourself short on time yet reluctant to appear uncaring, " +
                "you hurriedly pool together {goldGiven} gold as a gift for the girl. Her appreciative expression upon " +
                "receiving it brings a momentary sense of satisfaction. You make a round of quick goodbyes, receiving " +
                "heartfelt gratitude from the attendees. The brief interaction with the cheerful group leaves a trace of " +
                "joy in your heart. With a final wave and a promise to remember the warm encounter, you head off in the " +
                "direction of {closestSettlement}, the festive sounds gradually diminishing in the distance, leaving a " +
                "bittersweet feeling in their wake.",
                
                //Event Choice 2D
                "{=BirthdayParty_Event_Choice_2D}Time is of the essence, but your desire to avoid seeming unkind " +
                "prompts you to pause. You gather a modest sum of {goldGiven} gold and offer it to the birthday girl, " +
                "her thankful look making the gesture worthwhile. Engaging in brief but pleasant conversations with the " +
                "guests, you find their friendliness mildly infectious. As you say your farewells, their expressions of " +
                "gratitude add a layer of warmth to the encounter. Reluctantly, you leave the convivial atmosphere behind " +
                "and start your journey towards {closestSettlement}, the festive ambiance of the party lingering in your " +
                "thoughts, offering a gentle contrast to the urgency of your travel.",
                
                //Event Choice 2E
                "{=BirthdayParty_Event_Choice_2E}Although constrained by time, your reluctance to seem " +
                "dismissive leads you to stop briefly at the celebration. You quickly gather {goldGiven} gold and " +
                "hand it over to the girl, her thankful demeanor subtly uplifting your spirits. Engaging in brief " +
                "conversations with the guests, their geniality leaves a positive impression. After expressing your " +
                "goodbyes and receiving their sincere thanks, you depart, heading towards {closestSettlement}. " +
                "The cheerful sounds of the party gradually fade, but the memory of the brief, heartwarming interaction " +
                "stays with you, softening the haste of your journey."
                
            };
            
            private static readonly List<string> eventChoice3 = new List<string>
            {
                //Event Choice 3A
                "{=BirthdayParty_Event_Choice_3A}With a tight schedule pressing on your mind, you realize you " +
                "can't afford to linger at the party. There's a sense of urgency that overshadows the festive atmosphere, " +
                "pulling you towards your next destination. You begin to walk away towards {closestSettlement}, but not " +
                "without taking one final glance over your shoulder. In that fleeting moment, you see the joyous faces, " +
                "hear the laughter, and feel a pang of wistfulness. The sight of the birthday girl, surrounded by her " +
                "friends and family, celebrating a momentous day, etches a poignant image in your memory. Despite the " +
                "pressing need to move on, this snapshot of happiness leaves a lingering sense of what could have been a " +
                "delightful pause in your journey. With a mixture of regret and necessity, you turn your back on the scene " +
                "and continue on your path, the echoes of celebration softly fading into the distance.",
                
                //Event Choice 3B
                "{=BirthdayParty_Event_Choice_3B}Time constraints weigh heavily on you, making it clear that staying " +
                "at the party isn't an option. You start heading towards {closestSettlement}, but you can't help but cast a " +
                "lingering look back at the party. The sight of the gathering – the laughter, the cheerful chatter, and the " +
                "young girl in the midst of it all – creates a bittersweet moment. This brief, stolen glimpse of celebration " +
                "stays with you as you walk away, a gentle reminder of life's simple joys amidst your busy agenda.",
                
                //Event Choice 3C
                "{=BirthdayParty_Event_Choice_3C}The demands of your schedule leave no room for delay, compelling " +
                "you to move on. As you set off for {closestSettlement}, you give the party one final, fleeting glance. The " +
                "merriment, the sound of laughter, and the sight of the young girl enjoying her special day momentarily " +
                "capture your attention, instilling a brief sense of longing. It's a poignant reminder of life's fleeting " +
                "pleasures, etched in your mind as you walk away, the sounds of the party slowly diminishing behind you.",
                
                //Event Choice 3D
                "{=BirthdayParty_Event_Choice_3D}Recognizing the impossibility of a detour, you decide to continue " +
                "your journey to {closestSettlement}. However, you find yourself pausing momentarily to look back at the " +
                "birthday party. The scene of joy and celebration, with the birthday girl at the center, offers a stark " +
                "contrast to your pressing obligations. This final glimpse, filled with warmth and festivity, resonates " +
                "with you as you turn away, carrying the echoes of happiness as you proceed with your journey.",
                
                //Event Choice 3E
                "{=BirthdayParty_Event_Choice_3E}Your tight itinerary doesn't allow for any diversions, so you make " +
                "the decision to leave. As you walk towards {closestSettlement}, you take one last look at the birthday " +
                "party. The image of people celebrating, the air filled with laughter and cheer, and the birthday girl's " +
                "delighted face create a lasting impression. This brief moment of reflection amidst your haste offers a " +
                "poignant reminder of life's celebratory moments, gently fading away as you continue on your path."
            };
            
            private static readonly List<string> eventChoice4= new List<string>
            {
                //Event Choice 4A
                "{=BirthdayParty_Event_Choice_4A}You decide this is the moment to assert your dominance. Ordering your " +
                "men to surround the party, you demand the guests hand over their valuables. Met with initial refusal, " +
                "you turn to a young woman in the group. Seizing her, you display a ruthless streak, intimidating her " +
                "to emphasize your seriousness. Your method is harsh, yet non-lethal, designed to instill fear rather " +
                "than cause grave harm. The guests, witnessing this display, quickly comply, handing over their " +
                "possessions. Satisfied, you leave, but not before mockingly tossing a gold coin to the distraught " +
                "birthday girl. Counting your loot, you find yourself {goldLooted} gold richer, having successfully " +
                "instilled fear and gained material wealth.",
                
                //Event Choice 4B
                "{=BirthdayParty_Event_Choice_4B}You swiftly decide to take control. Commanding your men to encircle the " +
                "party, you sternly demand that the guests surrender their valuables. Facing initial resistance, you grab " +
                "a young woman from the crowd, using intimidation to prove your point. Your approach is intimidating yet " +
                "non-fatal, aiming to terrify rather than injure. Seeing your resolve, the partygoers relent, reluctantly " +
                "handing over their belongings. Content with your haul, you arrogantly flip a gold coin to the upset birthday " +
                "girl before leaving. As you count your gains, you find yourself {goldLooted} gold wealthier, having " +
                "effectively spread fear and secured a substantial profit.",
                
                //Event Choice 4C
                "{=BirthdayParty_Event_Choice_4C}In a moment of calculated ruthlessness, you instruct your men to encircle " +
                "the partygoers, demanding their valuables. Encountering initial defiance, you forcefully take a young " +
                "woman from the group, showcasing your willingness to go to extremes. Although your tactics are fierce, " +
                "they are deliberately non-lethal, meant to scare rather than harm. This show of force quickly persuades " +
                "the guests to hand over their possessions. With a sense of grim satisfaction, you flippantly throw a coin " +
                "to the traumatized birthday girl, then depart. Counting your loot, you realize you've acquired {goldLooted} " +
                "gold, effectively imposing terror and augmenting your wealth.",
                
                //Event Choice 4D
                "{=BirthdayParty_Event_Choice_4D}Deciding to dominate the situation, you order your men to besiege the " +
                "party. You then forcefully demand the guests' valuables. When they initially resist, you escalate the " +
                "situation by threatening a young woman, demonstrating your severity without causing serious harm. Your " +
                "menacing demeanor quickly persuades the guests to comply, and they hand over their valuables. Leaving " +
                "triumphantly, you cynically toss a gold coin to the shaken birthday girl. Your loot amounts to {goldLooted} " +
                "gold, a testament to your ability to instill fear and enrich yourself.",
                
                //Event Choice 4E
                "{=BirthdayParty_Event_Choice_4E}Opting for an aggressive approach, you command your men to blockade the " +
                "party, demanding valuables from the guests. When they hesitate, you seize a young woman from the group, " +
                "using her as leverage to underscore your determination. Your method is deliberately menacing yet not deadly, " +
                "meant to terrorize rather than injure. This forceful tactic swiftly convinces the guests to yield their " +
                "possessions. Satisfied with your success, you derisively toss a gold coin to the anguished birthday girl " +
                "as you depart. Counting your gains, you find yourself {goldLooted} gold richer, having effectively spread " +
                "fear and secured considerable wealth."
            };
            
            private static readonly List<string> eventMsg1 = new List<string>
            {
                "{=BirthdayParty_Event_Msg_1A}{heroName} gave {goldGiven} to the girl and gained {influenceGain} influence for defeating bandits.",
                "{=BirthdayParty_Event_Msg_1B}{heroName} donated {goldGiven} and earned {influenceGain} influence for bravery against bandits.",
                "{=BirthdayParty_Event_Msg_1C}{heroName} bestowed {goldGiven}, gaining {influenceGain} influence for their valor.",
                "{=BirthdayParty_Event_Msg_1D}{heroName} presented {goldGiven}, increasing {influenceGain} influence post-bandit victory.",
                "{=BirthdayParty_Event_Msg_1E}{heroName} contributed {goldGiven}, earning {influenceGain} influence for overcoming bandits."
            };
            
            private static readonly List<string> eventMsg2 = new List<string>
            {
                "{=BirthdayParty_Event_Msg_2A}{heroName} offered {goldGiven} to the girl.",
                "{=BirthdayParty_Event_Msg_2B}{heroName} presented {goldGiven} to the girl.",
                "{=BirthdayParty_Event_Msg_2C}{heroName} handed {goldGiven} to the girl.",
                "{=BirthdayParty_Event_Msg_2D}{heroName} bestowed {goldGiven} upon the girl.",
                "{=BirthdayParty_Event_Msg_2E}{heroName} donated {goldGiven} to the girl."
            };
            
            private static readonly List<string> eventMsg3 = new List<string>
            {
                "{=BirthdayParty_Event_Msg_3A}{heroName}'s party ignored the birthday celebrations.",
                "{=BirthdayParty_Event_Msg_3A}{heroName}'s party bypassed the birthday event.",
                "{=BirthdayParty_Event_Msg_3B}{heroName}'s group overlooked the birthday festivities.",
                "{=BirthdayParty_Event_Msg_3C}{heroName}'s entourage disregarded the birthday party.",
                "{=BirthdayParty_Event_Msg_3D}{heroName}'s company skipped the birthday celebrations.",
                "{=BirthdayParty_Event_Msg_3E}{heroName}'s team paid no heed to the birthday gathering."
            };
            
            private static readonly List<string> eventMsg4 = new List<string>
            { 
            "{=BirthdayParty_Event_Msg_4A}{heroName} plundered the party, gaining {goldLooted} gold.",
            "{=BirthdayParty_Event_Msg_4B}{heroName} pillaged the celebration, securing {goldLooted} gold.",
            "{=BirthdayParty_Event_Msg_4C}{heroName} looted the party, acquiring {goldLooted} gold.",
            "{=BirthdayParty_Event_Msg_4D}{heroName} ransacked the festivity, netting {goldLooted} gold.",
            "{=BirthdayParty_Event_Msg_4E}{heroName} commandeered the event, earning {goldLooted} gold."
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


    public class BirthdayPartyData : RandomEventData
    {
        public BirthdayPartyData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new BirthdayParty();
        }
    }
}