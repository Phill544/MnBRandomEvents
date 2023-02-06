using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public sealed class PrisonerTransfer : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minPrisoners;
        private readonly int maxPrisoners;
        private readonly int minPricePrPrisoner;
        private readonly int maxPricePrPrisoner;

        public PrisonerTransfer() : base(ModSettings.RandomEvents.PrisonerTransferData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("PrisonerTransfer", "EventDisabled");
            minPrisoners = ConfigFile.ReadInteger("PrisonerTransfer", "MinPrisoners"); 
            maxPrisoners = ConfigFile.ReadInteger("PrisonerTransfer", "MaxPrisoners"); 
            minPricePrPrisoner = ConfigFile.ReadInteger("PrisonerTransfer", "MinPricePrPrisoner"); 
            maxPricePrPrisoner = ConfigFile.ReadInteger("PrisonerTransfer", "MaxPricePrPrisoner"); 
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minPrisoners != 0 || maxPrisoners != 0 || minPricePrPrisoner != 0 || maxPricePrPrisoner != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData();
        }

        public override void StartEvent()
        {
            if (GeneralSettings.DebugMode.IsActive())
            {
                var debugMsg = new TextObject(
                        "Starting “{randomEvent}” with the current values:\n\n" +
                        "Min Prisoners : {minPrisoners}\n" +
                        "Max Prisoners : {maxPrisoners}\n" +
                        "Min Price Pr Prisoner : {minPricePrPrisoner}\n" +
                        "Max Price Pr Prisoner : {maxPricePrPrisoner}\n\n" +
                        "To disable these messages make sure you set the DebugMode = false in the ini settings\n\nThe ini file is located here : \n{path}"
                    )
                    .SetTextVariable("randomEvent", randomEventData.eventType)
                    .SetTextVariable("minPrisoners", minPrisoners)
                    .SetTextVariable("maxPrisoners", maxPrisoners)
                    .SetTextVariable("minPricePrPrisoner", minPricePrPrisoner)
                    .SetTextVariable("maxPricePrPrisoner", maxPricePrPrisoner)
                    .SetTextVariable("path", ParseIniFile.GetTheConfigFile())
                    .ToString();
                
                InformationManager.ShowInquiry(new InquiryData("Debug Info", debugMsg, true, false, "Start Event", null, null, null), true);
            }

            var heroName = Hero.MainHero.FirstName.ToString();
            
            var closestSettlement = ClosestSettlements.GetClosestTownOrVillage(MobileParty.MainParty).ToString();
            
            var closestCastle = ClosestSettlements.GetClosestCastle(MobileParty.MainParty).ToString();

            var prisonerForTransfer = MBRandom.RandomInt(minPrisoners, maxPrisoners);

            var pricePrPrisoner = MBRandom.RandomInt(minPricePrPrisoner, maxPricePrPrisoner);

            var allCultures = Settlement.FindAll(s => s.IsTown || s.IsCastle || s.IsVillage).ToList();
            
            var random = new Random();
            var index = random.Next(allCultures.Count);

            var randomCulture = allCultures[index].Culture.Name.ToString();

            var eventTitle = new TextObject("{=PrisonerTransfer_Title}A Bunch of Prisoners").ToString();

            if (randomCulture == "Empire")
            {
                randomCulture = "The Empire";
            }

            var eventDescription = new TextObject(
                "{=PrisonerTransfer_Event_Desc}You and your men are traveling near {closestSettlement} when you are " +
                "approached by a band of mercenaries from {randomCulture}. They inform you that they were tasked with " +
                "capturing a bunch of soldiers who had deserted their duty. They were on their way to {closestCastle} " +
                "with the prisoners but they have not received payment for their job. They are offering you the chance " +
                "to buy some or all of the prisoners for a nice sum of {pricePrPrisoner} gold per prisoner. You look at " +
                "the prisoners and all you see are young recruits who can easily be trained and given a worthy cause to " +
                "fight for. What do you do?")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("randomCulture", randomCulture)
                .SetTextVariable("closestCastle", closestCastle)
                .SetTextVariable("pricePrPrisoner", pricePrPrisoner)
                .ToString();

            var eventOption1 = new TextObject("{=PrisonerTransfer_Event_Option_1}Buy all the prisoners").ToString();
            var eventOption1Hover = new TextObject("{=PrisonerTransfer_Event_Option_1_Hover}You could use every man").ToString();
            
            var eventOption2 = new TextObject("{=PrisonerTransfer_Event_Option_2}Buy some prisoners").ToString();
            var eventOption2Hover = new TextObject("{=PrisonerTransfer_Event_Option_2_Hover}You don't need everyone").ToString();
            
            var eventOption3 = new TextObject("{=PrisonerTransfer_Event_Option_3}Decline").ToString();
            var eventOption3Hover = new TextObject("{=PrisonerTransfer_Event_Option_3_Hover}You really don't need any more men").ToString();
            
            var eventButtonText1 = new TextObject("{=PrisonerTransfer_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=PrisonerTransfer_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
            };

            var eventOptionAText = new TextObject(
                "{=PrisonerTransfer_Event_Choice_1}You tell the mercenary captain that you will take all the prisoners " +
                "off his hands. You haggle for a few minutes over the price and end up paying {totalGold} gold for " +
                "them.\n\n{pricePrPrisoner} gold * {prisonerForTransfer} men = {totalGold} gold in total.\n\n Once the " +
                "negotiations have been concluded and the mercenaries have left you make sure to feed the prisoners. " +
                "They cannot thank you enough for taking them in and it does not take long for all the prisoners to " +
                "swear their allegiance to you.")
                .SetTextVariable("totalGold", pricePrPrisoner * prisonerForTransfer)
                .SetTextVariable("pricePrPrisoner", pricePrPrisoner)
                .SetTextVariable("prisonerForTransfer", prisonerForTransfer)
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=PrisonerTransfer_Event_Choice_2}You tell him you will take some of his men. He agrees and lets you inspect the men.")
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=PrisonerTransfer_Event_Choice_3}You tell him that you don't have room for anyone but you wish " +
                    "him luck on his journeys. You watch on as the mercenaries and the prisoners disappears over a small " +
                    "hill in the distance.")
                .ToString();
            
            
            var eventMsg1 =new TextObject(
                    "{=PrisonerTransfer_Event_Msg_1}{heroName} bought {prisonerForTransfer} prisoners for {totalGold} gold.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("totalGold", pricePrPrisoner * prisonerForTransfer)
                .SetTextVariable("prisonerForTransfer", prisonerForTransfer)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=PrisonerTransfer_Event_Msg_2}{heroName} is thinking about buying some prisoners.")
                .SetTextVariable("heroName", heroName)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=PrisonerTransfer_Event_Msg_3}{heroName} did not buy any prisoners.")
                .SetTextVariable("heroName", heroName)
                .ToString();



            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1,
                eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            Hero.MainHero.AddSkillXp(DefaultSkills.Trade, 40);
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null,
                                    null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;

                        case "b":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null,
                                    null), true);

                            var troopRoster = TroopRoster.CreateDummyTroopRoster();

                            var cultureClass = ClosestSettlements.GetClosestAny(MobileParty.MainParty).Culture.ToString();

                            foreach (var characterObject in CharacterObject.All)
                            {
                                if (characterObject.StringId.Contains("recruit") &&
                                    !characterObject.StringId.Contains("vigla")
                                    && characterObject.Culture.ToString() == randomCulture ||
                                    (characterObject.StringId.Contains("footman") &&
                                     !characterObject.StringId.Contains("vlandia")
                                     && !characterObject.StringId.Contains("aserai") &&
                                     characterObject.Culture.ToString() == randomCulture) ||
                                    (characterObject.StringId.Contains("volunteer")
                                     && characterObject.StringId.Contains("battanian") &&
                                     characterObject.Culture.ToString() == randomCulture))
                                {
                                    troopRoster.AddToCounts(characterObject, prisonerForTransfer);
                                }
                            }

                            PartyScreenManager.OpenScreenAsReceiveTroops(troopRoster, leftPartyName: new TextObject("{cultureclass} Prisoners").SetTextVariable("cultureclass", cultureClass));

                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;

                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null,
                                    null), true);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;

                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                },

                null);

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


    public class PrisonerTransferData : RandomEventData
    {
        public PrisonerTransferData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new PrisonerTransfer();
        }
    }
}