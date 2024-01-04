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
    public sealed class SupernaturalEncounter : BaseEvent
    {
        private readonly bool eventDisabled;

        public SupernaturalEncounter() : base(ModSettings.RandomEvents.SupernaturalEncounterData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("SupernaturalEncounter", "EventDisabled");
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
            return HasValidEventData() && GeneralSettings.SupernaturalEvents.IsDisabled() == false && MobileParty.MainParty.CurrentSettlement == null && CurrentTimeOfDay.IsNight && MobileParty.MainParty.MemberRoster.TotalRegulars >= 50;
        }

        public override void StartEvent()
        {

            var heroName = Hero.MainHero.FirstName.ToString();
            
            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();
            
            var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription()).ToString();
            
            var eventOption1 = new TextObject("{=SupernaturalEncounter_Event_Option_1}Follow the apparition").ToString();
            var eventOption1Hover = new TextObject("{=SupernaturalEncounter_Event_Option_1_Hover}What could she possibly want?").ToString();
            
            var eventOption2 = new TextObject("{=SupernaturalEncounter_Event_Option_2}Try talking to her").ToString();
            var eventOption2Hover = new TextObject("{=SupernaturalEncounter_Event_Option_2_Hover}Seems like the logical choice").ToString();
            
            var eventOption3 = new TextObject("{=SupernaturalEncounter_Event_Option_3}Scream and run away").ToString();
            var eventOption3Hover = new TextObject("{=SupernaturalEncounter_Event_Option_3_Hover}I swear I'm not crazy!").ToString();
            
            var eventOption4 = new TextObject("{=SupernaturalEncounter_Event_Option_4}Ignore it").ToString();
            var eventOption4Hover = new TextObject("{=SupernaturalEncounter_Event_Option_4_Hover}It's just a dream anyway").ToString();
            
            var eventButtonText1 = new TextObject("{=SupernaturalEncounter_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=SupernaturalEncounter_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };
            
            var eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1())
                .ToString();
            
            var eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2())
                .ToString();
            
            var eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3())
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4())
                .ToString();
            
            var eventMsg1 = new TextObject(EventTextHandler.GetRandomEventMessage1())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg2 = new TextObject(EventTextHandler.GetRandomEventMessage2())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg3 = new TextObject(EventTextHandler.GetRandomEventMessage3())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg4 = new TextObject(EventTextHandler.GetRandomEventMessage4())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_POS_Outcome));
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
                MessageBox.Show($"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
        
        private static class EventTextHandler
        {
            private static readonly Random random = new Random();
            
            private static readonly List<string> eventTitles = new List<string>
            {
                "{=SupernaturalEncounter_Title_A}A Supernatural Encounter" ,
                "{=SupernaturalEncounter_Title_B}The Maiden's Ghost" ,
                "{=SupernaturalEncounter_Title_C}Whispers of the Lost Girl" ,
                "{=SupernaturalEncounter_Title_D}Apparition of a Young Maiden" ,
                "{=SupernaturalEncounter_Title_E}Spectral Visitation" ,
                "{=SupernaturalEncounter_Title_F}Echoes of a Young Spirit" ,
                "{=SupernaturalEncounter_Title_G}The Phantom Maiden" ,
                "{=SupernaturalEncounter_Title_H}Haunting of the Young Girl"
            };
            
            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=SupernaturalEncounter_Event_Desc_A}In the dead of night, a chilling presence disturbs your slumber in " +
                "the tent. You awaken to see the ghostly figure of a young woman, her eyes fixed on you with an eerie, " +
                "unspoken plea. As she drifts towards the exit, she pauses and looks back, her ethereal form casting a " +
                "haunting silhouette. A sense of ominous foreboding washes over you, suggesting she desires you to follow." ,
                
                //Event Description B
                "{=SupernaturalEncounter_Event_Desc_B}A sudden coldness envelops your tent, rousing you from sleep to face " +
                "a spectral young woman. Her gaze is sorrowful yet piercing, filled with silent stories of yesteryears. " +
                "She moves ghostly towards the entrance, halting momentarily to glance back, her form almost beckoning. " +
                "The air grows heavy, hinting that she seeks your pursuit into the unknown." ,
                
                //Event Description C
                "{=SupernaturalEncounter_Event_Desc_C}Under the moon's ghostly light, your sleep is interrupted by a spectral " +
                "vision. A young woman's apparition, shrouded in an aura of mystery, stands before you. She begins to fade " +
                "away, stopping at the tent's threshold to cast a lingering, haunting look in your direction. " +
                "An inexplicable urge to follow her grips you, as if drawn to uncover her spectral tale." ,
                
                //Event Description D
                "{=SupernaturalEncounter_Event_Desc_D}Your peaceful night is shattered by the apparition of a " +
                "young woman, her ghostly presence exuding a spine-chilling aura. She stares deeply, an unspoken " +
                "sorrow in her eyes, before slowly drifting towards the exit. Yet, she hesitates, turning back with " +
                "a look that transcends time, inviting you to follow her into the night's eerie embrace." ,
                
                //Event Description E
                "{=SupernaturalEncounter_Event_Desc_E}As the night whispers through your tent, you're startled awake by " +
                "an unsettling apparition. A young woman, her figure spectral and otherworldly, gazes at you with eyes " +
                "that pierce the veil between life and death. She moves towards the exit, only to pause and glance back " +
                "hauntingly, her silent gaze compelling you to follow her into the shrouded mysteries of the night."
            };
            
            private static readonly List<string> eventChoice1 = new List<string>
            {
                //Event Choice 1A
                "{=SupernaturalEncounter_Event_Choice_1A}Led by an inexplicable force, you cautiously follow the ethereal " +
                "apparition, with a handful of your men, equally entranced, trailing behind. The ghostly woman guides you " +
                "through the silent night, her spectral form gliding effortlessly until she reaches a lone, ancient tree " +
                "in a mist-enshrouded meadow. There, in a haunting display, she vanishes, leaving behind a cold, eerie " +
                "energy. Your group, enveloped in a mix of awe and fear, returns to camp, the silence between you speaking " +
                "volumes. Gathering around the dying embers of the campfire, you debate the significance of this " +
                "otherworldly encounter. Resolving to unearth the mystery, you lead a somber expedition back to the " +
                "tree at dawn. After painstakingly digging for what seems like an eternity, the shocking revelation of " +
                "a human skeleton buried beneath sends a shiver of dread through everyone. With a newfound respect for " +
                "the spectral maiden, you and your men carefully gather the bones and perform a proper burial, hoping " +
                "this act will grant her troubled spirit the peace it desperately seeks.",
                
                //Event Choice 1B
                "{=SupernaturalEncounter_Event_Choice_1B}Intrigued yet apprehensive, you and several unnerved men follow the " +
                "ghostly woman’s path into the darkness. She glides silently, her figure casting an otherworldly glow, " +
                "leading you to a solitary, gnarled tree in a desolate meadow. As you reach the spot, she fades into the " +
                "night, her presence leaving a chilling void. The haunting image lingers as you retreat to camp, the " +
                "unsettling encounter hanging heavily in the air. Around the campfire, whispers of the supernatural swirl " +
                "among your men, igniting a mixture of curiosity and dread. Determined to confront the unknown, you lead " +
                "a return to the eerie site at first light. Methodically excavating the earth beneath the tree, your heart " +
                "pounds with each shovel of soil. The grim discovery of a human skeleton buried in the cold ground confirms " +
                "the ghostly maiden's silent message. With a deep sense of solemnity, you and your men reverently gather " +
                "the remains, giving them a respectful burial. As you lay the bones to rest, a silent prayer is offered, " +
                "hoping to soothe the unrestful spirit that led you here.",
                
                //Event Choice 1C
                "{=SupernaturalEncounter_Event_Choice_1C}With a sense of foreboding, you follow the haunting figure of the young " +
                "woman, her ghostly visage a beacon in the moonlit night. A few of your bravest men accompany you, their faces " +
                "etched with a mix of fascination and fear. She leads you through the whispering fields to a solitary tree, " +
                "standing stark against the night sky. In a moment that feels suspended in time, she vanishes, her disappearance " +
                "leaving an indelible mark on your souls. The eerie journey back to camp is filled with hushed conversations " +
                "and uneasy glances. Unable to shake the ghostly encounter from your mind, you decide to delve deeper into the " +
                "mystery. As the first light of dawn breaks, you return to the haunting site, a sense of determination driving " +
                "your actions. The excavation reveals a buried secret - a human skeleton, its resting place undisturbed " +
                "until now. The air grows heavy with the realization of what lies before you. In a ceremony filled with " +
                "gravity, you carefully lay the remains to rest, each shovel of earth a step towards bringing peace to the " +
                "restless spirit that guided you.",
                
                //Event Choice 1D
                "{=SupernaturalEncounter_Event_Choice_1D}Compelled by the spectral maiden, you venture into the unknown, " +
                "your heart racing as you traverse the shadowy landscape. A few men, their expressions a mix of curiosity " +
                "and trepidation, follow closely. The apparition leads you to a bleak meadow, stopping beneath a forlorn " +
                "tree that seems to echo her loneliness. As she dissipates, a chilling breeze sweeps through, solidifying " +
                "the surreal experience. You return to camp in silence, each man lost in his thoughts, the ghostly image " +
                "etched in your minds. Gathering your senses, you convene with your men, the air thick with theories and " +
                "apprehensions. As the night gives way to dawn, you are drawn back to the tree, a resolve to uncover the " +
                "truth guiding your actions. Your efforts uncover a startling truth - a skeletal remnant hidden beneath " +
                "the earth. The discovery sends a wave of realization through the group. In a respectful and solemn act, " +
                "you bury the remains, hoping this gesture of honor will appease the spirit that led you to this " +
                "poignant discovery.",
                
                //Event Choice 1E
                "{=SupernaturalEncounter_Event_Choice_1E}Following the ghostly apparition, you tread cautiously, the night " +
                "air heavy with a sense of impending revelation. Accompanied by several men, their faces a canvas of intrigue " +
                "and unease, you are led to an isolated tree in a moonlit meadow. The spectral figure stops, her presence a " +
                "poignant reminder of life’s ephemeral nature. Then, as if dissolving into the night, she disappears, leaving a " +
                "void that seems to echo her silent cry. The return to camp is a journey of contemplation, the apparition's " +
                "lingering image igniting a flurry of whispered speculations. With the break of dawn, a unanimous decision to " +
                "return to the site is made, a collective need to unearth the truth driving you. The excavation at the tree’s " +
                "base reveals a haunting discovery - the skeletal remains of a once-living being. The solemn act of uncovering " +
                "the bones is met with a reverent silence. You carefully bury the remains, each motion a tribute to the young " +
                "spirit that brought you to this fateful spot, in the hope that your actions will bring solace to her wandering soul."
            };
            
            private static readonly List<string> eventChoice2 = new List<string>
            {
                //Event Choice 2A
                "{=SupernaturalEncounter_Event_Choice_2A}Gathering your courage, you manage to whisper a tentative 'hello' " +
                "into the stillness of the night. In response, the ghostly woman emits a bone-chilling shriek, a sound so " +
                "piercing and unearthly it seems to reverberate through the very fabric of the air. In an instant, she vanishes " +
                "before your eyes, leaving behind a haunting echo of her cry. The sudden silence that follows is almost as " +
                "terrifying as the scream itself. You find yourself paralyzed with fear, unable to move or speak. The rest " +
                "of the night is spent in your bed, shaking uncontrollably, every shadow and rustle of the wind magnified into " +
                "a thousand frightful possibilities. The image of her tormented face and the memory of her harrowing scream " +
                "linger with you, ensuring that sleep remains an elusive, distant dream for the remainder of the night.",
                
                //Event Choice 2B
                "{=SupernaturalEncounter_Event_Choice_2B}Summoning your bravest whisper, you murmur 'hello' into the midnight " +
                "silence. The spectral woman answers with a shriek that cuts through the night, a sound so eerie and otherworldly " +
                "it seems to warp the air around you. She vanishes instantly, her scream echoing in your ears. Overwhelmed by " +
                "fear, you're frozen, unable to utter a word. The night drags on, every shadow and breeze turning into terrifying " +
                "specters in your imagination. The vision of her anguished face and the chilling sound of her cry haunt you, " +
                "making sleep an impossible feat.",
                
                //Event Choice 2C
                "{=SupernaturalEncounter_Event_Choice_2C}With hesitant bravery, you utter a soft 'hello' into the night's quiet. " +
                "The ghostly figure responds with a scream that chills your bones, an unearthly howl vibrating through the " +
                "atmosphere. In a blink, she's gone, her wail lingering in the air. Rooted to the spot in terror, you can't" +
                " move or speak. The remainder of the night is a blur of fear, every minor noise or shadow appearing monstrous " +
                "and threatening. Her pained expression and the memory of her eerie scream stay with you, banishing any hope of sleep.",
                
                //Event Choice 2D
                "{=SupernaturalEncounter_Event_Choice_2D}Bracing yourself, you gently say 'hello' into the calm of the night. " +
                "The ghost-like woman unleashes a scream so chilling and surreal it seems to shake the very air. Then, she's " +
                "nowhere to be seen, her cry echoing hauntingly. Stricken with fear, you stand petrified, speechless. The rest " +
                "of the night is a cascade of terror, with every little sound or movement becoming a source of unspeakable horror. " +
                "Her tortured visage and the echoing scream remain etched in your mind, driving away any chance of sleep.",
                
                //Event Choice 2E
                "{=SupernaturalEncounter_Event_Choice_2E}With a trembling voice, you breathe out a 'hello' into the still night. " +
                "The apparition responds with a scream that's bone-chillingly unearthly, echoing as if from another realm. She " +
                "disappears abruptly, her scream resonating in the silence. You're left paralyzed with fear, unable to utter another " +
                "word. The night turns into a long ordeal of terror, with every shadow and whisper of wind seeming ominous and " +
                "full of dread. The image of her suffering face and her terrifying scream linger with you, " +
                "ensuring a night devoid of sleep."
                
            };
            
            private static readonly List<string> eventChoice3 = new List<string>
            {
                //Event Choice 3A
                "{=SupernaturalEncounter_Event_Choice_3A}Panicked as if chased by a horde of Sea Raiders, you leap from your bed, " +
                "arms flailing, and bolt out the door like a shot arrow. Your mad sprint through the camp is a blur of tents and " +
                "startled faces. As you skid to a halt by the campfire, you're a wild-eyed, breathless mess, hastily explaining " +
                "about a ghostly apparition in your tent. Your men burst into uncontrollable laughter. One of them, a veteran with " +
                "a scarred face, mimics your terrified expression, 'Did the ghost threaten to steal your boots?' he jests, while " +
                "another, a younger recruit, starts a playful chant, 'Brave {heroName}, scared of the dark!' They laugh heartily, " +
                "clinking their ale mugs, as you stand there, your fear slowly turning into a sheepish grin. The night continues " +
                "with exaggerated reenactments of your 'brave escape', your men finding endless amusement in your uncharacteristic dash.",
                
                //Event Choice 3B
                "{=SupernaturalEncounter_Event_Choice_3B}You catapult out of your bed as if a bandit lord was on your heels, your feet " +
                "barely touching the ground in your frantic escape. The entire camp seems to whirl past as you sprint straight to the " +
                "campfire. Gasping for air, you recount the terrifying encounter with what seemed like a ghost. The men, fall into fits " +
                "of laughter. 'Was the ghost armed? Did you win the duel?' one asks between chuckles. Another, a grizzled sergeant, pretends " +
                "to be frightened by his shadow, sending the group into another uproar. Jokes about recruiting ghosts for your next " +
                "siege or sending you on a 'ghost patrol' fill the air. As the night wears on, your tale of terror becomes a campfire " +
                "legend, embellished with each retelling, much to the delight of your troops.",
                
                //Event Choice 3C
                "{=SupernaturalEncounter_Event_Choice_3C}With a shriek that echoes through the camp, you leap from your bed as if " +
                "it were on fire and make a beeline for the door, sprinting as though leading a desperate charge against Swadian knights. " +
                "You stumble into the campfire area, a mix of fear and confusion. Out of breath, you explain about a ghostly " +
                "figure in your tent. The men erupt in laughter. One of them, a jester at heart, grabs a blanket and starts " +
                "floating around, imitating your ghost. 'Look out, our commander will desert us for a ghost' he cackles. Another, " +
                "pats your back, 'Don’t worry, we’ll stand guard tonight, lest the scary ghost returns!' he says with a wink. " +
                "The rest of the evening is filled with light-hearted banter and mock ghost stories, each more outrageous than " +
                "the last, as you join in the laughter, your initial fear dissolving into the warm camaraderie of your men.",
                
                //Event Choice 3D
                "{=SupernaturalEncounter_Event_Choice_3D}In a frenzy that rivals a villager escaping raiders, you jump from " +
                "your bed and hurtle through the camp. Dodging tents and a snoozing hound, you arrive at the campfire, a picture " +
                "of wide-eyed panic. Breathlessly, you recount your ghostly encounter, expecting some empathy. Instead, your " +
                "comrades erupt with laughter. One jokes heartily, 'Scared more than a peasant at tax time!' while another " +
                "breaks into a playful song about ghostly pursuits. The night turns into an uproar of jests and light-hearted " +
                "teasing, suggestions of a ghost hunt adding to the merriment. As the fire crackles and stars shimmer, " +
                "laughter fills the air, your spectral scare now a favorite tale among your fellow soldiers.",
                
                //Event Choice 3E
                "{=SupernaturalEncounter_Event_Choice_3E}Like a deer startled by an arrow, you bound from your bed and race " +
                "through the camp with the speed of a fleeing looter. Your dramatic entrance at the campfire, where your men " +
                "lounge leisurely, is met with surprise. Out of breath, you recount your ghostly encounter, expecting a mix " +
                "of awe and concern. Instead, your men can't hold back their laughter. A gruff soldier knight jests, " +
                "'Did you challenge the ghost to a duel, or were you too busy running?' Another playfully peers into the " +
                "darkness, pretending to spot a whole army of ghosts. The mood lightens as they tease you about " +
                "enlisting ghosts for your next campaign. Amidst chuckles and friendly jibes, your eerie experience turns " +
                "into a source of mirth, bringing a light-hearted end to the evening under the starlit Calradian sky."
            };
            
            private static readonly List<string> eventChoice4= new List<string>
            {
                //Event Choice 4A
                "{=SupernaturalEncounter_Event_Choice_4A}As the first light of dawn filters through your tent, you stir awake, " +
                "feeling unusually refreshed. The events of the eerie night seem like distant echoes, almost as if they belonged " +
                "to another life. Stretching your arms, you welcome the calmness that has settled over you. The ghostly encounter " +
                "that had sent chills down your spine now feels like a fragment of a forgotten dream, its details blurred and its " +
                "fearsome impact faded. As you prepare for the day, there's a newfound lightness in your step, the worries of the " +
                "night dissolved in the morning's gentle embrace.",
                
                //Event Choice 4B
                "{=SupernaturalEncounter_Event_Choice_4B}You awaken to the morning sun casting gentle rays across your bedding, " +
                "a sense of deep restfulness enveloping you. Strangely, the terrors of the previous night have vanished from " +
                "your memory, as if swept away by the comforting light of day. You sit up, feeling rejuvenated, the ghostly " +
                "visitation now nothing more than a shadow, lost in the recesses of your mind. As you step out of your tent, " +
                "the fresh air fills your lungs, and you find yourself smiling, ready to face the new day with vigor, the " +
                "night's fears locked away in the vault of forgotten dreams.",
                
                //Event Choice 4C
                "{=SupernaturalEncounter_Event_Choice_4C}You open your eyes to the soft glow of dawn, feeling an unexpected " +
                "sense of peace. The frightful encounter with the ghostly apparition that had haunted your night seems strangely " +
                "absent from your thoughts. It's as if the morning light has washed away the remnants of fear, leaving behind " +
                "a clear, tranquil mind. You rise, feeling lighter and more composed than you have in days. The memory of the " +
                "night's specter now feels like a distant, untroubling thought, barely a whisper in the back of your mind as " +
                "you prepare to greet the day with renewed energy and a clear head.",
                
                //Event Choice 4D
                "{=SupernaturalEncounter_Event_Choice_4D}As the morning sun peeks through the fabric of your tent, you wake " +
                "up feeling surprisingly well-rested. The harrowing events of the night, which had once seemed so vivid and " +
                "terrifying, now feel like they never happened. It's as though the light of day has a power to erase the fears " +
                "of the dark. You stretch, feeling revitalized, the memory of the ghostly figure fading like mist in the morning " +
                "sun. With a light heart, you step outside, ready to embrace the day's challenges, the echoes of the night's terror " +
                "now just a fleeting, inconsequential memory.",
                
                //Event Choice 4E
                "{=SupernaturalEncounter_Event_Choice_4E}The chirping of birds outside your tent gently coaxes you from sleep, " +
                "and you awaken to a morning bathed in soft, golden light. Remarkably, you feel thoroughly rested, the turmoil " +
                "of the previous night's ghostly encounter seemingly erased from your consciousness. It's as if the night's " +
                "shadows, which had once loomed so large and menacing, have been dispelled by the reassuring light of day. You " +
                "lie there for a moment, basking in the serene quiet, the memory of the spectral visitor now fading like a " +
                "half-remembered dream. As you rise to greet the day, there's a spring in your step, a sense of liberation from " +
                "the night's fright, as if it were a tale from a distant past, no longer holding any sway over your " +
                "thoughts or emotions."
            };
            
            private static readonly List<string> eventMsg1 = new List<string>
            {
                "{=SupernaturalEncounter_Msg_1A}{heroName} followed a ghostly guide and unearthed a skeleton, honoring the spirit with a solemn burial.",
                "{=SupernaturalEncounter_Msg_1B}{heroName}'s night expedition unveiled a buried skeleton, laid to rest after a respectful ceremony.",
                "{=SupernaturalEncounter_Msg_1C}Guided by an apparition, {heroName} discovered ancient bones and performed a dignified burial.",
                "{=SupernaturalEncounter_Msg_1D}{heroName} responded to a spectral call, uncovering and interring old remains with reverence.",
                "{=SupernaturalEncounter_Msg_1E}A spectral encounter led {heroName} to a hidden skeleton, granting peace to the spirit with a proper burial."
            };
            
            private static readonly List<string> eventMsg2 = new List<string>
            {
                "{=SupernaturalEncounter_Msg_2A}{heroName}'s greeting was met with a ghost's shriek, leading to a night of dread and restlessness.",
                "{=SupernaturalEncounter_Msg_2B}{heroName}'s whisper into the darkness returned a haunting scream, creating a night of terror.",
                "{=SupernaturalEncounter_Msg_2C}{heroName}'s soft call was answered by a chilling wail, enveloping the night in fear.",
                "{=SupernaturalEncounter_Msg_2D}{heroName}'s gentle words were echoed by a ghostly scream, casting a shadow of horror over the night.",
                "{=SupernaturalEncounter_Msg_2E}{heroName}'s tentative hello provoked an unearthly scream, filling the night with lingering fright."
            };
            
            private static readonly List<string> eventMsg3 = new List<string>
            {
                "{=SupernaturalEncounter_Msg_3A}{heroName}'s frantic escape from a ghostly apparition turns into campfire humor among the troops.",
                "{=SupernaturalEncounter_Msg_3B}{heroName}'s night of ghostly terror becomes a source of laughter and legendary tales at the campfire.",
                "{=SupernaturalEncounter_Msg_3C}After a comedic escape from a supposed ghost, {heroName} finds humor and camaraderie around the campfire.",
                "{=SupernaturalEncounter_Msg_3D}{heroName}'s ghostly encounter quickly transforms into amusing jests and light-hearted banter among the soldiers.",
                "{=SupernaturalEncounter_Msg_3E}{heroName}'s startled sprint from a ghost leads to an evening of laughter and playful teasing by the campfire."
            };
            
            private static readonly List<string> eventMsg4 = new List<string>
            { 
                "{=SupernaturalEncounter_Msg_4A}{heroName} awakens refreshed, the ghostly night's fears dissolving in the dawn's light.",
                "{=SupernaturalEncounter_Msg_4B}Morning's light brings peace to {heroName}, erasing the haunting memories of the night.",
                "{=SupernaturalEncounter_Msg_4C}Dawn's soft glow clears {heroName}'s mind of the night's terrors, bringing unexpected tranquility.",
                "{=SupernaturalEncounter_Msg_4D}{heroName} rises at sunrise, feeling renewed, the ghostly encounters now just distant echoes.",
                "{=SupernaturalEncounter_Msg_4E}Awakening to a golden morning, {heroName} finds the ghostly fears of the night faded like a forgotten dream."
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


    public class SupernaturalEncounterData : RandomEventData
    {
        public SupernaturalEncounterData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new SupernaturalEncounter();
        }
    }
}