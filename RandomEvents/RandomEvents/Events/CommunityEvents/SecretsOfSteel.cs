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
using TaleWorlds.ObjectSystem;

namespace Bannerlord.RandomEvents.Events.CommunityEvents
{
    public sealed class SecretsOfSteel : BaseEvent
    {
        private readonly bool eventDisabled;

        public SecretsOfSteel() : base(ModSettings.RandomEvents.SecretsOfSteelData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
			
            eventDisabled = ConfigFile.ReadBoolean("SecretsOfSteel", "EventDisabled");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            return eventDisabled == false && Hero.MainHero.GetSkillValue(DefaultSkills.Crafting) >= 120 && MobileParty.MainParty.MemberRoster.TotalRegulars >= 50;
        }

        public override bool CanExecuteEvent()
        {

            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            var mainHero = Hero.MainHero;

            var heroName = mainHero.FirstName;
            
            var eventTitle = new TextObject("{=SecretsOfSteel_Title}Secrets Of Steel").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty);

            var smithingLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Crafting);

            var canHelp = false;

            var smithingAppendedText = "";

            var option1AppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                canHelp = true;

                //Fakes a smithing level
                smithingLevel = MBRandom.RandomInt(100, 275);

                smithingAppendedText = new TextObject("{=SecretsOfSteel_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();
            }
            else
            {
                if (smithingLevel >= 100)
                {
                    canHelp = true;
                                    
                    smithingAppendedText = new TextObject("{=SecretsOfSteel_Smithing_Appended_Text}[Smithing - lvl 100]")
                        .ToString();
                }

                option1AppendedText = new TextObject("{=SecretsOfSteel_Option1_Appended_Text} Maybe if you were better in Smithing you could.").ToString();
            }

            var eventDescription = new TextObject(
                    "{=SecretsOfSteel_Event_Desc}While you party is travelling near {closestSettlement}, you come across a secluded forge with an old blacksmith in it. You hear him calling out to you so you decide to " +
                    "check what he wants. He explains to you that he once was one of the greatest blacksmiths in all of Calradia, but now he has become way to old to work alone. He explains to you that he had an " +
                    "apprentice, but he left a few days ago and said he would never come back. He also explains that he has an order to fill, but he cannot do it without help. He looks you straight in the eyes and asks " +
                    "if you could consider helping an old man on his final order. He also explains that you will be well rewarded. What do you do ?")
                    .SetTextVariable("closestSettlement", closestSettlement.Name.ToString())
                    .ToString();
            
            var eventOption1 = new TextObject("{=SecretsOfSteel_Event_Option_1}Decline").ToString();
            var eventOption1Hover = new TextObject("{=SecretsOfSteel_Event_Option_1_Hover}You cannot assist him.{option1AppendedText}").SetTextVariable("option1AppendedText", option1AppendedText).ToString();
            
            var eventOption2 = new TextObject("{=SecretsOfSteel_Event_Option_2}[Smithing] Accept").ToString();
            var eventOption2Hover = new TextObject("{=SecretsOfSteel_Event_Option_2_Hover}Agree to help him.{smithingAppendedText}").SetTextVariable("smithingAppendedText",smithingAppendedText).ToString();

            var eventButtonText1 = new TextObject("{=SecretsOfSteel_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=SecretsOfSteel_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            
            if (canHelp)
            {
                inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            }

            var craftingSkillValue = Hero.MainHero.GetSkillValue(DefaultSkills.Crafting);
            
            var charcoal = MBObjectManager.Instance.GetObject<ItemObject>("charcoal");
            var steel = MBObjectManager.Instance.GetObject<ItemObject>("ironIngot4");
            var fineSteel = MBObjectManager.Instance.GetObject<ItemObject>("ironIngot5");
            var thamaskeneSteel = MBObjectManager.Instance.GetObject<ItemObject>("ironIngot6");

            var randomCharcoal = MBRandom.RandomInt(5, 20);
            var randomSteel = MBRandom.RandomInt(5, 20);
            var randomFineSteel = MBRandom.RandomInt(2, 10);
            var randomThamaskeneSteel = MBRandom.RandomInt(2, 10);

            var eventOptionAText = new TextObject(
                    "{=SecretsOfSteel_Event_Choice_1}You tell him that you are unable to help him. He looks back to you and smiles and says that he understands. He then gets up, enters the house and closes the door. " +
                    "You and your men proceed to leave.")
                .ToString();
            
            var eventOptionBTextOutcome1 = new TextObject(
                    "{=SecretsOfSteel_Event_Choice_2_Outcome_1}You agree to help him. You send your men to {closestSettlement} and tells them to wait for you there. You and the blacksmith gets to work. The hours pass " +
                    "but after some time you are ready to start tempering your blades. You heat the first one and lower it into the oil. You hear the metal clinging and making a lot of noise under the oil. You pull it " +
                    "up only to realize the entire blade is cracked and useless. You try again, but same result. And again, but same result again and before you know it you've ruined all of your blades. You walk over " +
                    "to the blacksmith and apologize for you bad work. To your surprise he just smiles at you and says that he appreciated the help nonetheless. He informs you to take some charcoal with you as a payment " +
                    "as he does not have any gold. You pick up {randomCharcoal} pieces of charcoal and stuff them in your satchel. You and the blacksmith then part ways.")
                .SetTextVariable("closestSettlement", closestSettlement.Name.ToString())
                .SetTextVariable("randomCharcoal",randomCharcoal)
                .ToString();
            
            var eventOptionBTextOutcome2 = new TextObject(
                    "{=SecretsOfSteel_Event_Choice_2_Outcome_2}You agree to help him. You send your men to {closestSettlement} and tells them to wait for you there. You and the blacksmith gets to work. The hours pass " +
                    "but after some time you have all your blades tempered and you are halfway through the sharpening of the blades. The blacksmith comes over to you and pick up one of the sharpened blades. " +
                    "He thoroughly inspects the blade. Suddenly he tell you to stop. He informs you that something is wrong with the sharpening of the blades. He informs you that he will take over the finishing of " +
                    "the blades as you have done all the hard work. He also informs you to take some pieces of steel with you as a payment as he does not have any gold. You pick up {randomSteel} pieces of steel " +
                    "and stuff them in your satchel. You and the blacksmith then part ways.")
                .SetTextVariable("closestSettlement", closestSettlement.Name.ToString())
                .SetTextVariable("randomSteel",randomSteel)
                .ToString();
            
            var eventOptionBTextOutcome3 = new TextObject(
                    "{=SecretsOfSteel_Event_Choice_2_Outcome_3}You agree to help him. You send your men to {closestSettlement} and tells them to wait for you there. You and the blacksmith gets to work. The hours pass " +
                    "but after some time you have all your blades tempered and you are halfway through the sharpening of the blades. The blacksmith comes over to you and pick up one of the sharpened blades. " +
                    "He thoroughly inspects the blade. He informs you that your work is excellent. He puts the blade back down and help you finish. After a few more hours you are finally done. The blacksmith tells " +
                    "you that he does not know how he can ever repay your kindness. You tell him you were glad to help him. He informs you to take some pieces of Fine Steel and some pieces " +
                    "of Thamaskene Steel with you as a payment as he does not have any gold. You pick up {randomFineSteel} pieces of Fine Steel and {randomThamaskeneSteel} pieces of Thamaskene Steel and stuff them in " +
                    "your satchel. You and the blacksmith then part ways.")
                .SetTextVariable("closestSettlement", closestSettlement.Name.ToString())
                .SetTextVariable("randomFineSteel",randomFineSteel)
                .SetTextVariable("randomThamaskeneSteel",randomThamaskeneSteel)
                .ToString();


            var eventMsg1 =new TextObject(
                    "{=SecretsOfSteel_Event_Msg_1}{heroName} declined to help the old blacksmith.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=SecretsOfSteel_Event_Msg_2}{heroName} agreed to help the blacksmith, but destroyed all the blades during the tempering process.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=SecretsOfSteel_Event_Msg_3}{heroName} agreed to help the blacksmith, but made mistakes during the sharpening.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=SecretsOfSteel_Event_Msg_4}{heroName} agreed to help the blacksmith and they successfully made all the blades.")
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
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                                break;
                            case "b" when smithingLevel >= 100 && smithingLevel <= 125:
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBTextOutcome1, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(charcoal, randomCharcoal);
                                
                                Hero.MainHero.AddSkillXp(DefaultSkills.Crafting,craftingSkillValue * 1.0075f);

                                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                                break;
                            case "b" when smithingLevel >= 126 && smithingLevel <= 175:
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBTextOutcome2, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(steel, randomSteel);
                                
                                Hero.MainHero.AddSkillXp(DefaultSkills.Crafting,craftingSkillValue * 1.015f);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                                break;
                            
                            case "b" when smithingLevel >= 176:
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBTextOutcome3, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(fineSteel, randomFineSteel);
                                MobileParty.MainParty.ItemRoster.AddToCounts(thamaskeneSteel, randomThamaskeneSteel);
                                
                                Hero.MainHero.AddSkillXp(DefaultSkills.Crafting,craftingSkillValue * 1.0225f);
                                
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
                MessageBox.Show(
                    $"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }


    public class SecretsOfSteelData : RandomEventData
    {
        public SecretsOfSteelData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new SecretsOfSteel();
        }
    }
}