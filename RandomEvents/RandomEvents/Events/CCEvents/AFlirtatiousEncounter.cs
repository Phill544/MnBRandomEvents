using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class AFlirtatiousEncounter : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minWomanAge;
        private readonly int maxWomanAge;
        private readonly float minRelationshipIncrease;
        private readonly float maxRelationshipIncrease;
        private readonly int minCharmLevel;

        public AFlirtatiousEncounter() : base(ModSettings.RandomEvents.AFlirtatiousEncounterData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("AFlirtatiousEncounter", "EventDisabled");
            minWomanAge = ConfigFile.ReadInteger("AFlirtatiousEncounter", "MinWomanAge");
            maxWomanAge = ConfigFile.ReadInteger("AFlirtatiousEncounter", "MaxWomanAge");
            minRelationshipIncrease = ConfigFile.ReadFloat("AFlirtatiousEncounter", "MinRelationshipIncrease");
            maxRelationshipIncrease = ConfigFile.ReadFloat("AFlirtatiousEncounter", "MaxRelationshipIncrease");
            minCharmLevel = ConfigFile.ReadInteger("AFlirtatiousEncounter", "MinCharmLevel");
            
            //Overrides the input
            if (minWomanAge < 16)
            {
                minWomanAge = 16;
            }
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minWomanAge != 0 || maxWomanAge != 0 || minRelationshipIncrease != 0 || maxRelationshipIncrease != 0 || minCharmLevel != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return GetTargetHero() != null && HasValidEventData() && IsCorrectTimeOfDay() && MobileParty.MainParty.CurrentSettlement != null;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

            if (GetTargetHero() != null)
            {
                var target = GetTargetHero();

                var currentRelationship = target.GetRelation(Hero.MainHero);

                var relationshipGainPercent = MBRandom.RandomFloatRanged(minRelationshipIncrease, maxRelationshipIncrease);

                var newRelationship = (int)Math.Round(Math.Floor(currentRelationship * relationshipGainPercent));

                var targetCulture = target.Culture.Name.ToString();

                var currentSettlement = Settlement.CurrentSettlement.Name.ToString();

                var theDemonym = Demonym.GetTheDemonym(targetCulture, true);

                var charmLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Charm);

                var canCharmTarget = false;

                var charmAppendedText = "";

                if (GeneralSettings.SkillChecks.IsDisabled())
                {
                    canCharmTarget = true;
                    charmAppendedText =
                        new TextObject("{=AFlirtatiousEncounter_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**")
                            .ToString();
                }
                else
                {
                    if (charmLevel >= minCharmLevel)
                    {
                        canCharmTarget = true;

                        charmAppendedText =
                            new TextObject("{=AFlirtatiousEncounter_Charm_Appended_Text}[Charm - lvl {minCharmLevel}]")
                                .SetTextVariable("minCharmLevel", minCharmLevel)
                                .ToString();
                    }
                }

                var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription())
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .SetTextVariable("Demonym", theDemonym)
                    .ToString();

                var eventOption1 = new TextObject("{=AFlirtatiousEncounter_Event_Option_1}Strike up a conversation")
                    .ToString();
                var eventOption1Hover =
                    new TextObject("{=AFlirtatiousEncounter_Event_Option_1_Hover}Have a nice chat with her").ToString();

                var eventOption2 = new TextObject("{=AFlirtatiousEncounter_Event_Option_2}Buy her a drink").ToString();
                var eventOption2Hover =
                    new TextObject("{=AFlirtatiousEncounter_Event_Option_2_Hover}Always a gentleman.").ToString();

                var eventOption3 =
                    new TextObject("{=AFlirtatiousEncounter_Event_Option_3}[Charm] Hit on her").ToString();
                var eventOption3Hover =
                    new TextObject("{=AFlirtatiousEncounter_Event_Option_3_Hover}She's cute!\n{charmAppendedText}")
                        .SetTextVariable("charmAppendedText", charmAppendedText).ToString();

                var eventOption4 = new TextObject("{=AFlirtatiousEncounter_Event_Option_4}Be rude").ToString();
                var eventOption4Hover =
                    new TextObject("{=AFlirtatiousEncounter_Event_Option_4_Hover}Seriously?").ToString();

                var eventButtonText1 = new TextObject("{=AFlirtatiousEncounter_Event_Button_Text_1}Choose").ToString();
                var eventButtonText2 = new TextObject("{=AFlirtatiousEncounter_Event_Button_Text_2}Done").ToString();

                var inquiryElements = new List<InquiryElement>();

                inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
                inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));

                if (canCharmTarget)
                {
                    inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
                }

                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));


                var eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1())
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .ToString();

                var eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2())
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3())
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .ToString();

                var eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4())
                    .SetTextVariable("name", target.FirstName)
                    .SetTextVariable("age", (int)Math.Round(target.Age))
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var eventMsg1 = new TextObject(EventTextHandler.GetRandomEventMessage1())
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var eventMsg2 = new TextObject(EventTextHandler.GetRandomEventMessage2())
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var eventMsg3 = new TextObject(EventTextHandler.GetRandomEventMessage3())
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var eventMsg4 = new TextObject(EventTextHandler.GetRandomEventMessage4())
                    .SetTextVariable("name", target.FirstName.ToString())
                    .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                    .ToString();

                var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1,
                    eventButtonText1, null,
                    elements =>
                    {
                        switch ((string)elements[0].Identifier)
                        {
                            case "a":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                                
                                CharacterRelationManager.SetHeroRelation(target, Hero.MainHero, newRelationship - 5);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                                break;
                            case "b":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);

                                CharacterRelationManager.SetHeroRelation(target, Hero.MainHero, newRelationship - 3);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                                break;
                            case "c":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);

                                CharacterRelationManager.SetHeroRelation(target, Hero.MainHero, newRelationship + 5);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));

                                break;
                            case "d":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);

                                CharacterRelationManager.SetHeroRelation(target, Hero.MainHero, newRelationship - 30);
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_NEG_Outcome));

                                break;
                            default:
                                MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                                break;
                        }
                    }, null, null);

                MBInformationManager.ShowMultiSelectionInquiry(msid, true);

                StopEvent();
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

        private Hero GetTargetHero()
        {
            var currentSettlement = Settlement.CurrentSettlement;

            // Selecting a random hero based on the defined criteria
            var targetHero = Campaign.Current.AliveHeroes
                .Where(HeroSelectionCriteria)
                .OrderByDescending(RandomSortOrder)
                .FirstOrDefault();

            return targetHero;

            // Filtering criteria for the hero selection
            bool HeroSelectionCriteria(Hero hero) 
            {
                return hero.CurrentSettlement == currentSettlement &&
                       hero.IsLord &&
                       hero != Hero.MainHero.Spouse &&
                       hero.Clan != Clan.PlayerClan &&
                       hero.IsFemale &&
                       hero.Age >= minWomanAge &&
                       hero.Age <= maxWomanAge;
            }

            // Random sorting function
            float RandomSortOrder(Hero hero) => MBRandom.RandomFloat;
        }

        private static bool IsCorrectTimeOfDay() => CurrentTimeOfDay.IsEvening || CurrentTimeOfDay.IsNight;

        private static class EventTextHandler
        {
            private static readonly Random random = new Random();
            
            private static readonly List<string> eventTitles = new List<string>
            {
                "{=AFlirtatiousEncounter_Title_A}Dusk's Desire",
                "{=AFlirtatiousEncounter_Title_B}Twilight Temptations",
                "{=AFlirtatiousEncounter_Title_C}Evening Enchantment",
                "{=AFlirtatiousEncounter_Title_D}Nightfall's Whisper",
                "{=AFlirtatiousEncounter_Title_E}Moonlit Mischief",
                "{=AFlirtatiousEncounter_Title_F}Starry Serenade",
                "{=AFlirtatiousEncounter_Title_G}Sundown Soirée",
                "{=AFlirtatiousEncounter_Title_H}A Flirtatious Encounter"
            };
            
            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=AFlirtatiousEncounter_Event_Desc_A}This evening you are enjoying a drink in the tavern at " +
                "{currentSettlement}, just relaxing. You take a look at the various guest in the tavern. There " +
                "is quite a diverse mix of people here tonight. Your eyes suddenly lock eyes with {Demonym} " +
                "woman who is standing across the room. She smiles back when you notice her and starts to make " +
                "her way over to you. How shall you proceed?",
                
                //Event Description B
                "{=AFlirtatiousEncounter_Event_Desc_B}Tonight, at the tavern in {currentSettlement}, you savor a " +
                "beverage, unwinding. Glancing around, you observe an eclectic assortment of patrons. Amidst them, " +
                "your gaze meets that of a {Demonym} lady positioned on the opposite side. She returns your gaze with" +
                " a smile and begins approaching you. What will be your next move?",
                
                //Event Description C
                "{=AFlirtatiousEncounter_Event_Desc_C}This evening, while enjoying a libation in {currentSettlement}'s " +
                "local tavern, you're at ease. Surveying the room, you see a variety of guests. Your attention is " +
                "caught by a {Demonym} woman across the space. She notices your look and smiles, then starts walking" +
                " towards you. What actions will you take?",
                
                //Event Description D
                "{=AFlirtatiousEncounter_Event_Desc_D}You're relishing a drink tonight in {currentSettlement}'s tavern, " +
                "simply chilling out. You scan the diverse crowd of the tavern. Suddenly, you and a {Demonym} woman " +
                "across the room lock eyes. Acknowledging your gaze, she smiles and begins moving in your direction. " +
                "What's your strategy going forward?",
                
                //Event Description E
                "{=AFlirtatiousEncounter_Event_Desc_E}In the tavern at {currentSettlement} this evening, you are " +
                "leisurely sipping your drink. Casually observing, you note the tavern's varied clientele. Then, your " +
                "eyes meet with those of a {Demonym} woman standing afar. She smiles in response to your notice and " +
                "commences her approach. What will be your course of action?"
            };
            
            private static readonly List<string> eventChoice1 = new List<string>
            {
                //Event Choice 1A
                "{=AFlirtatiousEncounter_Event_Choice_1A}When she gets over to you the two of you start talking. " +
                "You learn that her name is {name} and she's {age} years old. Both of you end up sitting and chatting " +
                "for a few hours but eventually you both go your separate ways, but not before sharing a warming hug. " +
                "As you watch her walk away into the night you are left wondering if you'll ever see her again.",
                
                //Event Choice 1B
                "{=AFlirtatiousEncounter_Event_Choice_1B}As she arrives, conversation sparks between you. She " +
                "introduces herself as {name}, aged {age}. Time flies as you both chat for hours, seated together. " +
                "Ultimately, the evening concludes with each of you parting ways, but only after a comforting hug. " +
                "Watching her depart into the night, you ponder the possibility of crossing paths again.",
                
                //Event Choice 1C
                "{=AFlirtatiousEncounter_Event_Choice_1C}Once she reaches you, the two of you engage in dialogue. " +
                "You discover her name is {name} and she's {age} years old. After several hours of seated conversation, " +
                "you both decide to go separate ways, marked by a warm hug. As she walks away into the darkness, " +
                "you're left contemplating a future encounter.",
                
                //Event Choice 1D
                "{=AFlirtatiousEncounter_Event_Choice_1D}Upon her approach, you both start a conversation. She reveals " +
                "her name as {name} and mentions she's {age} years old. The two of you spend hours talking and sitting " +
                "together, then eventually part, sharing a warm hug. As she disappears into the night, thoughts of " +
                "seeing her again linger in your mind.",
                
                //Event Choice 1E
                "{=AFlirtatiousEncounter_Event_Choice_1E}When she joins you, a chat ensues. You learn her name is {name}, " +
                "and she is {age} years old. Several hours pass in enjoyable conversation, and eventually, you both " +
                "separate, not without a warm hug. As she vanishes into the night, you are left wondering about a future " +
                "meeting."
            };
            
            private static readonly List<string> eventChoice2 = new List<string>
            {
                //Event Choice 2A
                "{=AFlirtatiousEncounter_Event_Choice_2A}When she gets over to you the two of you starts talking. You offer " +
                "to buy her a drink which she accepts. As the evening progresses the two of you get more and more drunk. " +
                "You manage to learn that her name is {name} and she's {age} years old. Eventually you are both so drunk " +
                "that you are thrown out of the bar.\\n\\nBoth of you giggle as you wobble down the streets of " +
                "{currentSettlement} while singing songs. Eventually the alcohol starts wearing off and you both decide it was " +
                "a lovely night. You give each other a tender hug before you go your separate ways. After a few seconds " +
                "you hear your name being called. “{heroName} wait!” You turn around and see {name} running back to you. " +
                "She stops just a few inches from you face and gives you a deep and tender kiss on the lips. “That was for " +
                "a lovely evening” she said. You kiss her back before saying you final goodbye for the night.",
                
                //Event Choice 2B
                "{=AFlirtatiousEncounter_Event_Choice_2B}As she approaches, you both initiate a conversation. Offering " +
                "her a drink, which she gladly accepts, leads to an increasingly tipsy evening. You learn her name is " +
                "{name}, and she's {age}. The night escalates to the point where you're both ejected from the bar, laughing " +
                "and staggering through {currentSettlement}'s streets, singing joyfully. As sobriety returns, you acknowledge " +
                "the wonderful time spent, sharing a gentle hug before parting. Suddenly, she calls out, “{heroName} wait!” and " +
                "runs back for a heartfelt kiss, thanking you for the evening. You reciprocate the kiss and bid a final goodnight.",
                
                //Event Choice 2C
                "{=AFlirtatiousEncounter_Event_Choice_2C}Upon her joining you, conversation flows, and you buy her a drink, " +
                "which she accepts. As the night unfolds, you both become increasingly inebriated. You ascertain her name " +
                "is {name}, and she's {age}. Eventually, your drunken antics lead to both of you being thrown out of the bar. " +
                "Laughing and swaying, you meander through {currentSettlement}, singing. As the alcohol wears off, you part ways " +
                "with a sweet hug. Then, hearing your name, you turn to find {name} rushing back for a passionate kiss, expressing " +
                "gratitude for a delightful evening. After kissing her in return, you bid your final farewell for the night.",
                
                //Event Choice 2D
                "{=AFlirtatiousEncounter_Event_Choice_2D}When she reaches you, the two of you start chatting. You offer her a" +
                " drink, which she happily accepts, and as the evening rolls on, both of you drink more. You discover she's " +
                "{name} and {age} years old. Eventually, your drunkenness results in being ousted from the bar. Stumbling and " +
                "giggling down the streets of {currentSettlement}, singing songs, you share a memorable night. As the effect of " +
                "the drinks lessens, you hug tenderly and part ways. But she calls out, “{heroName} wait!” and returns for a deep, " +
                "affectionate kiss, dedicating it to the lovely evening. After returning the kiss, you say your final " +
                "goodbyes for the night.",
                
                //Event Choice 2E
                "{=AFlirtatiousEncounter_Event_Choice_2E}She comes over, and you both engage in a chat. You treat her to a drink, " +
                "which she accepts, leading to both of you getting quite drunk. You learn her name is {name}, and her age is {age}. " +
                "The night culminates in both of you being kicked out of the bar amidst laughter. Singing along the streets of " +
                "{currentSettlement}, you enjoy the evening's buzz. As you both sober up, you share a fond hug and start to part " +
                "ways. Then, she calls your name, “{heroName} wait!” and rushes back for a deep, affectionate kiss, " +
                "thanking you for a wonderful evening. You kiss her back before parting for the night."
                
            };
            
            private static readonly List<string> eventChoice3 = new List<string>
            {
                //Event Choice 3A
                "{=AFlirtatiousEncounter_Event_Choice_3A}When she gets over to you the two of you starts talking. You tell her " +
                "that you don't often get noticed by such beautiful women. She says it's not often she find such handsome men in " +
                "town. With the tone being set for the event the two of you continue to talk (and naturally to hit on each other " +
                "as well...) She tells you that her name is {name} and she's {age} years old. After a few minutes the two of " +
                "you move away from the bar and go somewhere a bit more comfortable. There you both continue your flirtatious " +
                "behaviour. It does not take long before the two of you start kissing each other.\\n\\n You kiss her on the neck, " +
                "on the cheeks, on the lips and so on. She whispers to you “I know a more private place...” with fire in her eyes. " +
                "Wanna head there she asks. You tell her to lead the way. She takes your hand and leads you out into the street. " +
                "After a few minutes of walking you arrive at what is clearly {name}'s home in {currentSettlement}. As you enter " +
                "her house she turns to you and start kissing you passionately again with more lust that you can ever remember. After " +
                "just a few seconds all your clothes have been removed and you make your way towards {name}'s bed. \\n\\nThe rest " +
                "of the night fades out in ecstasy.",
                
                //Event Choice 3B
                "{=AFlirtatiousEncounter_Event_Choice_3B}As she comes over, you both start a meaningful conversation. You admit feeling " +
                "unaccustomed to attention from such beautiful women, which she counters by expressing her own rarity in encountering " +
                "handsome men in town. This sets a flirtatious tone for the evening. She introduces herself as {name}, and shares that " +
                "she's {age}. As minutes pass, you both shift from the bar to a more secluded spot, continuing with light-hearted and " +
                "flirtatious talk. The chemistry escalates quickly into kissing - first on the neck, then cheeks, and onto the lips. She " +
                "leans in, whispering about a more private place, her eyes sparkling with desire. At her invitation, you follow, hand in " +
                "hand, through the streets to her residence in {currentSettlement}. Inside her home, the intensity of your interaction " +
                "amplifies; her kisses become more passionate and filled with a lust more intense than you've ever known. Clothing is " +
                "swiftly discarded as you both make your way to her bed, the night gradually fading into a memorable haze of " +
                "ecstasy and connection.",
                
                //Event Choice 3C
                "{=AFlirtatiousEncounter_Event_Choice_3C}Upon her arrival, you engage in a warm conversation. You confess that it's a " +
                "rare delight to be noticed by such a stunning woman. She playfully replies, stating that it's equally uncommon for her " +
                "to meet such attractive men in her town. This exchange sets a charming and flirtatious mood for the night. She soon reveals " +
                "her name as {name} and that she's {age}. Before long, you both decide to move from the bar to a quieter, more intimate space, " +
                "where your flirtatious exchanges grow more intense. The kissing begins tenderly, moving from her neck to her cheeks and finally " +
                "her lips. In a low, enticing whisper, she mentions a more secluded place, her eyes conveying a fiery invitation. Agreeing " +
                "eagerly, you follow her through the streets until you reach her home in {currentSettlement}. Entering her abode, the atmosphere " +
                "turns even more passionate, her kisses deepening, filled with a fervor and lust that overwhelm your senses. Clothes are " +
                "quickly shed, and you find yourselves moving towards her bed, enveloped in the escalating intensity of the night, which " +
                "eventually dissolves into a euphoric blur of ecstasy.",
                
                //Event Choice 3D
                "{=AFlirtatiousEncounter_Event_Choice_3D}She approaches, and a conversation filled with undercurrents of attraction begins. " +
                "You remark on the rarity of receiving attention from such beautiful women, to which she responds that handsome men are not a " +
                "common sight in town. This playful banter sets a flirty and captivating tone for the evening. As time passes, she introduces " +
                "herself as {name}, and reveals her age as {age}. Gradually, you both migrate from the bustling bar to a quieter, more " +
                "comfortable spot, where your flirtatious interactions become more pronounced. Kisses ensue, starting gently on the neck, " +
                "moving to the cheeks, and culminating on the lips. Her voice, low and seductive, suggests a more private location, her eyes " +
                "glinting with unspoken promises. Intrigued and captivated, you follow her lead through the streets to her house in " +
                "{currentSettlement}. Upon entering, the passion escalates; her kisses are fervent, laden with a lust that seems unmatched. " +
                "In a swift, seamless motion, clothing is discarded, and you both head towards her bed. The remainder of the night " +
                "becomes a passionate journey, fading into a rhapsody of ecstatic experiences and intimate moments.",
                
                //Event Choice 3E
                "{=AFlirtatiousEncounter_Event_Choice_3E}When she reaches your side, a conversation filled with flirtatious undertones " +
                "unfolds. You express how unusual it is for you to catch the eye of such beautiful women, which she counters by mentioning " +
                "how seldom she encounters handsome men in town. This playful, flirty exchange sets the mood for the evening. She " +
                "introduces herself as {name} and shares that she's {age}. As the minutes tick by, you both transition from the noisy " +
                "bar to a quieter, more intimate area, continuing your playful and flirtatious dialogue. The connection between you deepens, " +
                "leading to a series of kisses, beginning with her neck, then her cheeks, and eventually her lips. In a whisper tinged " +
                "with excitement and desire, she suggests moving to a more private spot, her eyes dancing with anticipation. You nod " +
                "in agreement, and she guides you hand in hand through the streets until you arrive at her home in {currentSettlement}. " +
                "As you step inside, the intensity between you soars; her kisses become more passionate and imbued with a deep, raw " +
                "lust unlike any you've experienced before. Clothes are shed hastily as you both advance towards her bed, the night " +
                "seamlessly transitioning into an ecstatic, unforgettable blur of passion and intimacy."
            };
            
            private static readonly List<string> eventChoice4= new List<string>
            {
                //Event Choice 4A
                "{=AFlirtatiousEncounter_Event_Choice_4A}You cannot help but be a little annoyed as you'd rather drink in " +
                "peace. When she gets over to you she introduces herself as {name}. It does not take long for you to have " +
                "insulted her enough that she leaves you alone. You may have hurt her feelings but at least  you are able to " +
                "enjoy the night alone. A few hours later as you are about to leave you come across {name} again. You tell " +
                "her that you're sorry for what you said and you didn't mean it as you just wanted to be left alone. " +
                "She somehow manages to politely and kindly tell you to F**k off.",
                
                //Event Choice 4B
                "{=AFlirtatiousEncounter_Event_Choice_4B}Your irritation is palpable, preferring solitude over company. When " +
                "she arrives, she introduces herself as {name}. It's not long before your sharp words drive her away, " +
                "possibly wounding her. While you've secured your solitude, it's at the cost of her feelings. Hours later, as " +
                "you're leaving, you encounter {name} again. Apologizing, you explain that your harshness stemmed from a desire " +
                "for alone time, not malice. Surprisingly, she responds with a polite yet firm dismissal, eloquently " +
                "telling you to leave her alone in no uncertain terms.",
                
                //Event Choice 4C
                "{=AFlirtatiousEncounter_Event_Choice_4C}Feeling annoyed, you wish to savor your drink in solitude. As she " +
                "approaches, she greets you as {name}. However, your brusque demeanor quickly offends her, leading her to " +
                "retreat. Your actions ensure a peaceful night alone, though at the expense of her feelings. Later, as you " +
                "prepare to leave, you bump into {name} once more. You express regret for your earlier words, clarifying that " +
                "your intention was simply to be left alone. In an unexpectedly gracious manner, she firmly rejects your " +
                "apology, instructing you to go away with a refined yet clear directive.",
                
                //Event Choice 4D
                "{=AFlirtatiousEncounter_Event_Choice_4D}Annoyance bubbles within you, as you'd much rather drink undisturbed. " +
                "She comes over and introduces herself as {name}. Your curt remarks soon send her away, potentially hurting " +
                "her. Despite this, you achieve your goal of a solitary evening. Several hours later, you cross paths with {name} " +
                "again while leaving. Offering an apology, you admit that your rude comments were a misguided effort to be left " +
                "alone. Remarkably, she responds with a polite but unmistakable rejection, effectively telling you to " +
                "back off in the nicest possible way.",
                
                //Event Choice 4E
                "{=AFlirtatiousEncounter_Event_Choice_4E}A sense of annoyance lingers, as you prefer to enjoy your drink alone. " +
                "When she introduces herself as {name}, you don't take long to offend her, leading her to leave you in " +
                "peace. Although this may have upset her, it grants you the solitude you desired. Later, at the end of the " +
                "night, you encounter {name} again. Apologetically, you explain that your unkind words were just a misguided " +
                "attempt to have some quiet time. She, in turn, manages to convey a polite yet unequivocal dismissal, " +
                "cleverly telling you to go away without a hint of rudeness."
            };
            
            private static readonly List<string> eventMsg1 = new List<string>
            {
                "{=AFlirtatiousEncounter_Event_Msg_1A}{heroName} had a casual conversation with {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_1B}{heroName} engaged in a relaxed, easy-going chat with {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_1C}A casual and light-hearted dialogue unfolded between {heroName} and {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_1D}{heroName} found themselves in a pleasant, informal conversation with {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_1E}A laid-back exchange of words occurred between {heroName} and {name}."
            };
            
            private static readonly List<string> eventMsg2 = new List<string>
            {
                "{=AFlirtatiousEncounter_Event_Msg_2A}{heroName} got a lot closer to {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_2B}{heroName} significantly deepened their bond with {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_2C}The connection between {heroName} and {name} grew much stronger.",
                "{=AFlirtatiousEncounter_Event_Msg_2D}{heroName} and {name} developed a notably closer relationship.",
                "{=AFlirtatiousEncounter_Event_Msg_2E}A substantial increase in admiration was experienced between {heroName} and {name}."
            };
            
            private static readonly List<string> eventMsg3 = new List<string>
            {
                "{=AFlirtatiousEncounter_Event_Msg_3A}{heroName} got lucky with {name}!",
                "{=AFlirtatiousEncounter_Event_Msg_3B}{heroName} shared an intimate encounter with {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_3C}A passionate night unfolded between {heroName} and {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_3D}{heroName} experienced a romantic liaison with {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_3E}An amorous adventure occurred between {heroName} and {name}."
            };
            
            private static readonly List<string> eventMsg4 = new List<string>
            {
                "{=AFlirtatiousEncounter_Event_Msg_4A}{heroName} deeply insulted {name}!",
                "{=AFlirtatiousEncounter_Event_Msg_4B}{heroName} gravely offended {name} with harsh words.",
                "{=AFlirtatiousEncounter_Event_Msg_4C}A serious affront was made by {heroName} towards {name}.",
                "{=AFlirtatiousEncounter_Event_Msg_4D}{heroName} caused deep upset to {name} with their cutting remarks.",
                "{=AFlirtatiousEncounter_Event_Msg_4E}{heroName} delivered a profound insult to {name}."
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


    public class AFlirtatiousEncounterData : RandomEventData
    {
        public AFlirtatiousEncounterData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new AFlirtatiousEncounter();
        }
    }
}