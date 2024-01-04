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
    public sealed class OldRuins : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minMen;
        private readonly int maxMen;
        private readonly int maxMenToKill;
        private readonly int minGoldFound;
        private readonly int maxGoldFound;

        public OldRuins() : base(ModSettings.RandomEvents.OldRuinsData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("OldRuins", "EventDisabled");
            minMen = ConfigFile.ReadInteger("OldRuins", "MinMen");
            maxMen = ConfigFile.ReadInteger("OldRuins", "MaxMen");
            maxMenToKill = ConfigFile.ReadInteger("OldRuins", "MaxMenToKill");
            minGoldFound = ConfigFile.ReadInteger("OldRuins", "MinGoldFound");
            maxGoldFound = ConfigFile.ReadInteger("OldRuins", "MaxGoldFound");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minMen != 0 || maxMen != 0 || maxMenToKill != 0 || minGoldFound != 0 || maxGoldFound != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null  && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxMen;
        }

        public override void StartEvent()
        {
            var heroName = Hero.MainHero.FirstName;
            
            var eventTitle = new TextObject("{=OldRuins_Title}The old ruins").ToString();
            
            var manCount  = MBRandom.RandomInt(minMen, maxMen);

            var killedMen = MBRandom.RandomInt(1, maxMenToKill);

            var goldFound = MBRandom.RandomInt(minGoldFound, maxGoldFound);

            var goldForYou = goldFound / manCount ;
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var eventDescription = new TextObject(
                    "{=OldRuins_Event_Desc}You are traveling through the lands near {closestSettlement} when you come across the ruins of a small abandoned settlement. You don't remember there being any " +
                    "settlements out here on any map so you tell your men to set up camp nearby. You and {manCount} of your men decide to investigate it. \n\nWhen you enter the settlement it becomes apparent " +
                    "that it has been abandoned for some time. There is an old farmhouse, well, barn, and a small shack at the settlement. You ask your men where they want to check first but before " +
                    "they can answer a bolt of lightning rips through the clouds followed by a thunderous roar. You decide that you only have time to check one of the locations before you head back. Which will it be?")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("manCount", manCount)
                .ToString();
            
            var eventOption1 = new TextObject("{=OldRuins_Event_Option_1}The old farmhouse").ToString();
            var eventOption1Hover = new TextObject("{=OldRuins_Event_Option_1_Hover}Better to check the homestead").ToString();
            
            var eventOption2 = new TextObject("{=OldRuins_Event_Option_2}The old well").ToString();
            var eventOption2Hover = new TextObject("{=OldRuins_Event_Option_2_Hover}...It's a well").ToString();
            
            var eventOption3 = new TextObject("{=OldRuins_Event_Option_3}The barn").ToString();
            var eventOption3Hover = new TextObject("{=OldRuins_Event_Option_3_Hover}Barns always hold something interesting").ToString();
            
            var eventOption4 = new TextObject("{=OldRuins_Event_Option_4}The small shack").ToString();
            var eventOption4Hover = new TextObject("{=OldRuins_Event_Option_4_Hover}The shack might be interesting").ToString();
            
            var eventOption5 = new TextObject("{=OldRuins_Event_Option_5}None of them, just leave").ToString();
            var eventOption5Hover = new TextObject("{=OldRuins_Event_Option_5_Hover}You don't want to get wet").ToString();
            
            var eventButtonText1 = new TextObject("{=OldRuins_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=OldRuins_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover),
                new InquiryElement("e", eventOption5, null, true, eventOption5Hover)
            };
            
            var eventOptionAText = new TextObject(
                    "{=OldRuins_Event_Choice_1}You decide to check the farmhouse. You approach the door and push it" +
                    " inwards. It falls off and the entire building is beginning to creak. You push into the building" +
                    " and have your men scatter to search for anything interesting. You decide to check the living room." +
                    " Nothing special here except bugs and spiders. You head back out to make sure your horses are" +
                    " still there. As you head out of the building you hear the deafening sound of the entire building" +
                    " collapsing behind you.\n\nConfused you start to call out the names of your men but no one" +
                    " responds. You return to the main party and gathers some additional soldiers to help retrieve" +
                    " them men trapped. Sadly only 2 of them survived and the {killedMen} others perished from being crushed.")
                .SetTextVariable("killedMen", killedMen)
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=OldRuins_Event_Choice_2}You all decide to check the well as it's closest to you. You encircle it, look down and... Nothing... Just dirt. You then leave.")
                .ToString();

            var eventOptionCText = new TextObject(
                    "{=OldRuins_Event_Choice_3}The men want to check the barn so you agree. Once inside you can only see some skeletons from what you can only assume were farm animals. The men scatter " +
                    "and search the barn but they all come up empty handed. You proceed to leave.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=OldRuins_Event_Choice_4}You all agree to head to the shack. You pry the door open and inside you find a chest with a lock. After fidgeting for a couple of minutes you manage to break the lock. " +
                    "The chest is mostly filled with papers but all the way at the bottom you come across a rather hefty coin purse. You open it and to you amazement it's filled with lots of gold coins. " +
                    "You head back to camp where you split the content of the purse with your men. You found {goldFound} gold and there were {men} men who went with you so {goldFound} / {men} = {goldForYou}. ")
                .SetTextVariable("goldFound",goldFound)
                .SetTextVariable("men",manCount)
                .SetTextVariable("goldForYou",goldForYou)
                .ToString();
            
            var eventOptionEText = new TextObject(
                    "{=OldRuins_Event_Choice_5}You all agree that you don't want to get wet so you make your way back to camp.")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=OldRuins_Event_Msg_1}{heroName} lost {killedMen} men to a collapsing structure.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("killedMen", killedMen)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=OldRuins_Event_Msg_2}{heroName} received {goldForYou} gold after splitting {goldFound} gold with {manCount} men.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldForYou", goldForYou)
                .SetTextVariable("goldFound", goldFound)
                .SetTextVariable("manCount", manCount )
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            
                            MobileParty.MainParty.MemberRoster.KillNumberOfNonHeroTroopsRandomly(killedMen);
                            break;
                        
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(+goldForYou);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        case "e":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionEText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                },
                null, null);

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


    public class OldRuinsData : RandomEventData
    {

        public OldRuinsData(string eventType, float chanceWeight) : base(eventType,
            chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new OldRuins();
        }
    }
}