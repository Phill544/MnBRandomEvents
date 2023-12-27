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
    public sealed class FallenSoldierFamily : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minFamilyCompensation;
        private readonly int maxFamilyCompensation;
        private readonly int minGoldLooted;
        private readonly int maxGoldLooted;
        private readonly int minRogueryLevel;

        public FallenSoldierFamily() : base(ModSettings.RandomEvents.FallenSoldierFamilyData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("FallenSoldierFamily", "EventDisabled");
            minFamilyCompensation = ConfigFile.ReadInteger("FallenSoldierFamily", "MinFamilyCompensation");
            maxFamilyCompensation = ConfigFile.ReadInteger("FallenSoldierFamily", "MaxFamilyCompensation");
            minGoldLooted = ConfigFile.ReadInteger("FallenSoldierFamily", "MinGoldLooted");
            maxGoldLooted = ConfigFile.ReadInteger("FallenSoldierFamily", "MaxGoldLooted");
            minRogueryLevel = ConfigFile.ReadInteger("FallenSoldierFamily", "MinRogueryLevel");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minFamilyCompensation != 0 || maxFamilyCompensation != 0 || minGoldLooted != 0 || maxGoldLooted != 0 || minRogueryLevel != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement != null && (MobileParty.MainParty.CurrentSettlement.IsTown || MobileParty.MainParty.CurrentSettlement.IsVillage)  && MobileParty.MainParty.MemberRoster.TotalRegulars >= 100;
        }

        public override void StartEvent()
        {
            var heroName = Hero.MainHero.FirstName;
            
            var currentSettlement = MobileParty.MainParty.CurrentSettlement.Name;
            
            var roguerySkill = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
            
            var plotEvil = false;
            
            var rogueryAppendedText = "";
            
            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                
                plotEvil = true;
                rogueryAppendedText = new TextObject("{=FallenSoldier_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();

            }
            else
            {
                if (roguerySkill >= minRogueryLevel)
                {
                    plotEvil = true;
                    
                    rogueryAppendedText = new TextObject("{=FallenSoldier_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
            }
            
            
            var familyCompensation = MBRandom.RandomInt(minFamilyCompensation, maxFamilyCompensation);
            var goldLooted = MBRandom.RandomInt(minGoldLooted, maxGoldLooted);
            
            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();
            
            var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription())
                .SetTextVariable("currentSettlement", currentSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=FallenSoldier_Event_Option_1}Offer them compensation").ToString();
            var eventOption1Hover = new TextObject("{=FallenSoldier_Event_Option_1_Hover}They should be compensated").ToString();
            
            var eventOption2 = new TextObject("{=FallenSoldier_Event_Option_2}Explain that you owe them nothing").ToString();
            var eventOption2Hover = new TextObject("{=FallenSoldier_Event_Option_2_Hover}Not my problem!").ToString();
            
            var eventOption3 = new TextObject("{=FallenSoldier_Event_Option_3}Leave").ToString();
            var eventOption3Hover = new TextObject("{=FallenSoldier_Event_Option_3_Hover}You have a headache so you leave").ToString();
            
            var eventOption4 = new TextObject("{=FallenSoldier_Event_Option_4}[Roguery] Trick them").ToString();
            var eventOption4Hover = new TextObject("{=FallenSoldier_Event_Option_4_Hover}{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();
            
            var eventButtonText1 = new TextObject("{=FallenSoldier_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=FallenSoldier_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            if (plotEvil)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }
            
            
            var eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1())
                .SetTextVariable("familyCompensation", familyCompensation)
                .ToString();
            
            var eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2())
                .ToString();
            
            var eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3())
                .ToString();
            
            var eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4())
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();
            
            var eventMsg1 =new TextObject(EventTextHandler.GetRandomEventMessage1())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("familyCompensation", familyCompensation)
                .ToString();
            
            var eventMsg2 =new TextObject(EventTextHandler.GetRandomEventMessage2())
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg3 =new TextObject(EventTextHandler.GetRandomEventMessage3())
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg4 =new TextObject(EventTextHandler.GetRandomEventMessage4())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();
            

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-familyCompensation);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(+goldLooted);
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
                "{=FallenSoldier_Title_A}The Soldier's Family",
                "{=FallenSoldier_Title_B}Relatives of the Fallen",
                "{=FallenSoldier_Title_C}The Bereaved Kin",
                "{=FallenSoldier_Title_D}Household of the Lost Soldier",
                "{=FallenSoldier_Title_E}Next of Kin: A Soldier's Story",
                "{=FallenSoldier_Title_F}The Soldier's Descendants",
                "{=FallenSoldier_Title_G}The Lineage of a Soldier",
                "{=FallenSoldier_Title_H}Family Beyond the Battlefield"
            };
            
            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=FallenSoldier_Event_Desc_A}As you enjoy your drink in the cozy atmosphere of the local tavern in " +
                "{currentSettlement}, your solitude is interrupted by the approach of three individuals. A woman, " +
                "flanked by two young boys, hesitantly makes her way towards you. With a mixture of respect and " +
                "apprehension, they request a moment of your time. They reveal themselves to be the family of a " +
                "soldier who tragically lost his life under your command. The sorrow in their eyes is evident as " +
                "they explain their dire situation. With their primary breadwinner gone, they find themselves struggling " +
                "to sustain their farm. They stand before you, a picture of vulnerability, seeking financial assistance " +
                "to continue their livelihood in the wake of their loss. Faced with this poignant situation, you ponder " +
                "over the right course of action. Do you offer them the compensation they desperately need, or do you " +
                "have another solution in mind?",
                
                //Event Description B
                "{=FallenSoldier_Event_Desc_B}As you relax in the rustic ambiance of the local tavern in {currentSettlement}, " +
                "your quiet evening is interrupted by the arrival of three somber figures. A woman, visibly burdened by " +
                "life's hardships, accompanied by two young boys, approaches you with a sense of purpose. They politely ask " +
                "for your attention, revealing themselves to be the family of a soldier who valiantly fought and died under " +
                "your command. The pain in their eyes speaks volumes as they express their plight: with the loss of their " +
                "family's main provider, they are struggling to keep their farm from falling into ruin. Desperation is clear " +
                "in their voices as they humbly request financial assistance to help them through this challenging time. As " +
                "you absorb their story, a dilemma unfolds before you: how to respond to this heartfelt plea for help in " +
                "memory of a fallen soldier.",
                
                //Event Description C
                "{=FallenSoldier_Event_Desc_C}In the midst of your peaceful drink at the tavern in {currentSettlement}, " +
                "your solitude is pierced by the cautious approach of a small, forlorn group. A woman, her face etched with " +
                "lines of worry, and two young boys, who exude a sense of loss, stand before you. They hesitantly explain " +
                "that they are the family of a soldier who lost his life while serving under your leadership. Their struggle " +
                "is apparent as they share the dire straits they have found themselves in, now facing the daunting task of " +
                "maintaining their farm without their beloved. They look to you, their last hope, seeking compensation to " +
                "alleviate their financial burdens in these trying times. Confronted with their earnest appeal, you are left " +
                "to weigh the gravity of their situation against the demands of your own conscience.",
                
                //Event Description D
                "{=FallenSoldier_Event_Desc_D}As you enjoy a moment of respite with a drink in hand at the tavern in " +
                "{currentSettlement}, your tranquility is disrupted by the hesitant approach of three individuals. A woman, " +
                "whose eyes reflect a deep-seated sorrow, flanked by two young boys, requests a moment of your time. With " +
                "a voice tinged with grief, they reveal their identity as the family of a soldier who bravely served and " +
                "died under your command. Their current plight, struggling to sustain the farm that was once their soldier's " +
                "pride, is laid bare before you. They seek your aid, hoping for compensation to keep their lives afloat in " +
                "the wake of their irreplaceable loss. As their story of struggle and resilience unfolds, you find yourself " +
                "grappling with the decision of how to best assist them during their time of need.",
                
                //Event Description E
                "{=FallenSoldier_Event_Desc_E}Your evening at the tavern in {currentSettlement}, previously filled with " +
                "the simple pleasure of a drink, takes a poignant turn as three individuals approach you. Leading them is " +
                "a woman, her face a canvas of both strength and despair, accompanied by two young boys. With a respectful " +
                "demeanor, they request a moment to speak with you. They identify themselves as the family of a soldier who " +
                "once served under you and recently fell in battle. The harsh reality of their situation becomes evident as " +
                "they describe their struggle to keep their farm, now devoid of its caretaker. With a heartfelt plea, they " +
                "ask for your financial support, hoping it will provide some respite in their time of overwhelming hardship. " +
                "You are left to ponder their request, deeply aware of the impact your decision could have on their lives."
            };
            
            private static readonly List<string> eventChoice1 = new List<string>
            {
                //Event Choice 1A
                "{=FallenSoldier_Event_Choice_1A}You ask for the name and rank of the man who died. When she tells you his " +
                "name you do remember him and how he died. The soldier in question was executed by your hands as it was " +
                "discovered he was a traitor. The question you ask yourself now is if his entire family should suffer " +
                "from his mistake. They have spoken so warmly about him that you don't want to tell them the truth about " +
                "how he died so you make up a heroic story. Even though the family have no right for compensation, you agree " +
                "to pay them {familyCompensation} gold in compensation so they can keep their family farm. After you have " +
                "handed over they gold to them and they have left, you cannot help but wonder if you did the right thing " +
                "keeping the mother in the dark about her son's true nature. You end up drinking the night away.",
                
                //Event Choice 1B
                "{=FallenSoldier_Event_Choice_1B}You inquire about the name and rank of the deceased soldier. Upon hearing " +
                "his name, you recall him vividly – he was executed by your order for treason. Now, you're faced with a moral " +
                "dilemma: should his family suffer for his betrayal? They speak of him with such fondness that you decide " +
                "against revealing the bitter truth, instead, crafting a tale of his bravery. Though they aren't entitled to " +
                "compensation, you choose to give them {familyCompensation} gold to support their farm. As they depart with " +
                "the gold, you're left questioning whether keeping the mother uninformed of her son's betrayal was the right " +
                "choice. The rest of your night is spent in contemplative drinking, pondering over the complexities of " +
                "truth and compassion.",
                
                //Event Choice 1C
                "{=FallenSoldier_Event_Choice_1C}You request the name and details of the soldier who lost his life. " +
                "Recognizing the name, you remember his fate - he was a traitor whom you had to execute. This revelation " +
                "leaves you in a quandary; does his family deserve to bear the consequences of his actions? They hold him " +
                "in such high regard that you choose not to shatter their image of him, instead telling a story of his " +
                "heroism. Though they have no legal claim to compensation, you feel compelled to offer them " +
                "{familyCompensation} gold to save their farm. After they leave with the money, doubts linger about your " +
                "decision to keep the truth hidden, leading you to seek solace in drink as you reflect on the " +
                "night's events.",
                
                //Event Choice 1D
                "{=FallenSoldier_Event_Choice_1D}You question the woman about her husband's identity and his role in " +
                "your army. When she mentions his name, it strikes a chord – he was the soldier you had executed for " +
                "committing treason. This knowledge presents a difficult choice: should his family suffer for his " +
                "wrongdoing? Realizing they hold him in high esteem, you decide to spare them the painful truth, " +
                "inventing a story of his gallantry instead. Although there's no obligation for compensation, you hand " +
                "them {familyCompensation} gold to help maintain their farm. After they've gone, you're left with a heavy " +
                "heart, wondering if keeping the mother oblivious to her son's real actions was just. The night passes in " +
                "a blur as you drink away your uncertainties.",
                
                //Event Choice 1E
                "{=FallenSoldier_Event_Choice_1E}Curiously, you ask about the soldier's identity. When she reveals his " +
                "name, memories flood back – he was the one you had to execute for his treacherous acts. This recollection " +
                "forces you to confront a tough decision: is it fair for his family to pay for his mistake? Given their " +
                "affectionate memories of him, you opt not to disclose the harsh reality, instead weaving a narrative of " +
                "his valor. Despite no entitlement to compensation, you feel morally obliged to provide them " +
                "{familyCompensation} gold for their farm's upkeep. Once they leave with the gold, a sense of doubt creeps " +
                "in about your choice to conceal the truth from his mother, leading you to drown these thoughts in alcohol " +
                "as the night progresses."
            };
            
            private static readonly List<string> eventChoice2 = new List<string>
            {
                //Event Choice 2A
                "{=FallenSoldier_Event_Choice_2A}You ask for the name and rank of the man who died. When she tells you his " +
                "name you do remember him and how he died. The soldier in question was executed by your hands as it was " +
                "discovered he was a traitor. You tell the family that you cannot grant them compensation as it was " +
                "specified in his contract that the family left behind had no right to claim compensation. " +
                "The women starts to cry and begs you to help them. You decline to help them and leave the tavern.",
                
                //Event Choice 2B
                "{=FallenSoldier_Event_Choice_2B}You inquire about the fallen soldier's name and rank. Recognition dawns " +
                "as his name is mentioned, along with the memory of his execution - a traitor's end by your own hand. You " +
                "inform the family that compensation is not possible, as his military contract clearly stated no entitlement " +
                "for his family in such circumstances. The woman breaks down in tears, pleading for your assistance. Despite " +
                "her pleas, you firmly uphold the decision and exit the tavern, leaving the family to grapple with the " +
                "harsh realities of their situation.",
                
                //Event Choice 2C
                "{=FallenSoldier_Event_Choice_2C}Seeking details, you ask about the soldier's identity. His name triggers " +
                "memories; he was the one you executed for betrayal. You explain to the family that, according to his " +
                "service terms, they are ineligible for compensation. Overwhelmed with sorrow, the woman starts to weep " +
                "and implores you for aid. However, bound by the rules of the contract, you choose not to intervene and " +
                "make your way out of the tavern, leaving them in their moment of despair.",
                
                //Event Choice 2D
                "{=FallenSoldier_Event_Choice_2D}You question the family about the soldier's name and service rank. " +
                "Upon hearing his name, you recall his fate – a traitor executed under your command. You relay to the " +
                "family that, as per his agreement, they have no right to compensation. The woman's tears start to flow " +
                "as she desperately seeks your help. Despite her distress, you maintain your stance, unable to deviate " +
                "from the contract's stipulations, and depart from the tavern, leaving the family in their sorrowful state.",
                
                //Event Choice 2E
                "{=FallenSoldier_Event_Choice_2E}Curious, you ask for the soldier's identity. His name brings back the " +
                "memory of his execution for treachery. You inform the family that, under the terms of his enlistment, " +
                "they are not entitled to any compensation. The woman, stricken with grief, begins to cry and " +
                "earnestly requests your help. Despite her emotional plea, you stand by the contract's terms, " +
                "declining to provide assistance, and take your leave from the tavern, immersed in the complexity " +
                "of duty and compassion."
                
            };
            
            private static readonly List<string> eventChoice3 = new List<string>
            {
                //Event Choice 3A
                "{=FallenSoldier_Event_Choice_3A}Feeling drained and unwilling to engage in such a heavy conversation, " +
                "you bluntly inform the family that you have no obligation to provide them with any assistance. The " +
                "weight of their gaze lingers on you as you stand up, signaling your desire to leave the tavern. Their " +
                "faces, etched with disappointment and growing despair, fade into the background as you step out, " +
                "seeking to distance yourself from the situation. As you exit, the air of the tavern suddenly feels " +
                "heavier, the burden of their unmet hopes lingering in your mind even as you walk away.",
                
                //Event Choice 3B
                "{=FallenSoldier_Event_Choice_3B}Overwhelmed by the emotional weight of the encounter, you quickly " +
                "assert that you bear no responsibility for their financial woes. The atmosphere turns tense as you " +
                "rise from your seat, ready to depart from the tavern. Their faces, a mix of shock and sadness, watch" +
                " you as you stride out, leaving behind a silence filled with unspoken pleas and dashed hopes. The " +
                "heaviness of the situation stays with you, a silent companion as you leave the tavern behind.",
                
                //Event Choice 3C
                "{=FallenSoldier_Event_Choice_3C}Feeling unequipped to handle this situation, you firmly state that you " +
                "owe them nothing. The family's expressions shift to one of disbelief as you get up, indicating your " +
                "intent to leave. As you make your way out of the tavern, their forlorn looks linger in your mind, " +
                "their unfulfilled expectations casting a shadow over your departure. The door closes behind you, but " +
                "the echo of their plight resonates, leaving you with a sense of unresolved tension.",
                
                //Event Choice 3D
                "{=FallenSoldier_Event_Choice_3D}Lacking the patience for such a confrontation, you dismissively tell " +
                "them that they are not entitled to any compensation from you. The sorrow in their eyes deepens as you " +
                "stand and head towards the tavern's exit. Their silent stares follow you, heavy with unmet needs and " +
                "sorrow. As you step outside, the murmur of the tavern fades, but the imprint of their disappointed " +
                "faces remains, a stark reminder of the encounter as you walk away.",
                
                //Event Choice 3E
                "{=FallenSoldier_Event_Choice_3E}Devoid of the energy to engage in this emotionally charged conversation, " +
                "you tersely inform them of your inability to help. The family's expressions turn to a blend of grief " +
                "and disbelief as you rise to leave. Walking out of the tavern, you can feel their eyes on your back, " +
                "heavy with the weight of unfulfilled hopes. The door shuts behind you, but the heaviness of the moment " +
                "lingers, casting a pall over your steps as you distance yourself from the tavern and their plight."
            };
            
            private static readonly List<string> eventChoice4= new List<string>
            {
                //Event Choice 4A
                "{=FallenSoldier_Event_Choice_4A}Upon inquiring about the name and rank of the deceased soldier, you " +
                "recognize him as the man you executed for treason. Curiously, you ask the family about the location " +
                "of their farm, assuring them of your visit the next day. After politely excusing yourself, you leave" +
                " the tavern. The next day, you and your men approach the farm with a sinister agenda. Rather than " +
                "offering aid, you command your men to obliterate the farm and eliminate its inhabitants. Observing the " +
                "chaos, you see your men forcefully escort the family outside, their hands bound. They then proceed to " +
                "ransack the farmhouse, extracting valuables before setting it ablaze. You watch dispassionately as the " +
                "farmhouse is engulfed in flames and your men carry out the execution of the entire family. After " +
                "the grim task is completed, you signal your men to regroup and depart towards your main encampment. " +
                "Back at camp, your men report that they acquired {goldLooted} gold from the looting, a cold testament " +
                "to the day's ruthless actions.",
                
                //Event Choice 4B
                "{=FallenSoldier_Event_Choice_4B}You inquire about the soldier's details. Recognizing his name, you recall " +
                "his fate as a traitor executed by your command. Curious, you ask about their farm's location, promising " +
                "to visit the next day. After excusing yourself, you leave the tavern. The next day, you arrive at their " +
                "farm, not with aid, but a grim plan. You command your men to raze the farm and eliminate the family. As the " +
                "flames engulf the farmhouse, you oversee the capture and execution of the family. Your men emerge with " +
                "valuables, including {goldLooted} gold, which they report back at camp. The grim task completed, you return " +
                "to your main party, the day's events leaving a stark mark on your memory.",
                
                //Event Choice 4C
                "{=FallenSoldier_Event_Choice_4C}After learning the soldier's name, memories of his execution for treachery " +
                "flood back. You feign interest in their farm's whereabouts, stating you'll visit soon. Politely excusing " +
                "yourself, you exit the tavern. The following dawn, you and your men reach the farm, harboring dark " +
                "intentions. You coldly order the destruction of the farm and the execution of its inhabitants. You stand " +
                "watch as your men bind the family, pillage the house, and set it ablaze. The family's execution is carried " +
                "out under your watchful eye. Your men later inform you at camp that they secured {goldLooted} gold " +
                "from the looting.",
                
                //Event Choice 4D
                "{=FallenSoldier_Event_Choice_4D}Recognizing the name of the soldier you executed for betrayal, you express " +
                "interest in visiting their farm. You leave the tavern, promising to arrive the next day. Upon arrival at " +
                "the farm, your intentions are far from benevolent. Instead, you instruct your men to set fire to the farm " +
                "and eliminate its residents. You oversee the operation as your men forcibly remove the family, loot their " +
                "possessions, and execute them one by one. The farmhouse is reduced to ashes. Back at camp, your men report " +
                "a loot of {goldLooted} gold from the raid.",
                
                //Event Choice 4E
                "{=FallenSoldier_Event_Choice_4E}Upon hearing the soldier's name, you remember executing him for treason. " +
                "You ask about the location of their farm and assure them of your visit the next day. However, your intentions " +
                "are malevolent. Arriving at the farm, you command your men to annihilate it and its owners. You watch impassively " +
                "as they drag the family out, ransack the house for valuables, and then set it ablaze. The execution of the " +
                "family is carried out under your command. Later, at your camp, your men inform you of the {goldLooted} gold " +
                "they acquired from the looting."
            };
            
            private static readonly List<string> eventMsg1 = new List<string>
            {
                "{=FallenSoldier_Event_Msg_1A}{heroName} gave the family {familyCompensation} gold in compensation.",
                "{=FallenSoldier_Event_Msg_1B}{heroName} handed over {familyCompensation} gold to the soldier's family as compensation.",
                "{=FallenSoldier_Event_Msg_1C}{heroName} provided the grieving family {familyCompensation} gold for their loss.",
                "{=FallenSoldier_Event_Msg_1D}{heroName} presented {familyCompensation} gold to the bereaved family.",
                "{=FallenSoldier_Event_Msg_1E}{heroName} compensated the family with {familyCompensation} gold."
            };
            
            private static readonly List<string> eventMsg2 = new List<string>
            {
                "{=FallenSoldier_Event_Msg_2A}{heroName} denied the family compensation.",
                "{=FallenSoldier_Event_Msg_2B}{heroName} refused to grant the family any compensation.",
                "{=FallenSoldier_Event_Msg_2C}{heroName} declined the family's request for compensation.",
                "{=FallenSoldier_Event_Msg_2D}Despite the family's plea, {heroName} did not offer them compensation.",
                "{=FallenSoldier_Event_Msg_2E}{heroName} withheld compensation from the soldier's family."
            };
            
            private static readonly List<string> eventMsg3 = new List<string>
            {
                "{=FallenSoldier_Event_Msg_3A}{heroName} could not be bothered to listen.",
                "{=FallenSoldier_Event_Msg_3B}{heroName} showed no interest in hearing their story.",
                "{=FallenSoldier_Event_Msg_3C}{heroName} disregarded their pleas, unwilling to listen.",
                "{=FallenSoldier_Event_Msg_3D}{heroName} chose not to pay attention to their words.",
                "{=FallenSoldier_Event_Msg_3E}{heroName} remained uninterested and did not entertain their appeal."
            };
            
            private static readonly List<string> eventMsg4 = new List<string>
            { 
                "{=FallenSoldier_Event_Msg_4A}{heroName}'s party looted {goldLooted} gold from the farmhouse and killed everyone.",
                "{=FallenSoldier_Event_Msg_4B}{heroName}'s men plundered {goldLooted} gold and eliminated all occupants of the farmhouse.",
                "{=FallenSoldier_Event_Msg_4C}Led by {heroName}, the group seized {goldLooted} gold and eradicated everyone in the farmhouse.",
                "{=FallenSoldier_Event_Msg_4D}Under {heroName}'s command, the unit extracted {goldLooted} gold from the farmhouse, leaving no survivors.",
                "{=FallenSoldier_Event_Msg_4E}{heroName}'s forces raided, acquiring {goldLooted} gold and dispatching all inhabitants of the farmhouse."
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


    public class FallenSoldierFamilyData : RandomEventData
    {

        public FallenSoldierFamilyData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new FallenSoldierFamily();
        }
    }
}