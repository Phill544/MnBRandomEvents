using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.RandomEvents.Helpers
{
    public static class AttributeHelper
    {
        /// <summary>
        /// This method returns a read only list of the attributes available to the campaign via reflection.
        /// This should be safe to use with other mods that inject additional attributes.
        /// </summary>
        /// <returns></returns>
        public static MBReadOnlyList<CharacterAttribute> GetAllAttributes()
        {
            //using reflection lets us get attributes added by mods (bannerkings for example)
            var allAttrs = Campaign.Current.GetType()
                .GetProperty("AllCharacterAttributes", BindingFlags.Instance | BindingFlags.NonPublic);
            
            // ReSharper disable once PossibleNullReferenceException
            return (MBReadOnlyList<CharacterAttribute>)allAttrs.GetValue(Campaign.Current);
        }
    }
}