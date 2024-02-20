using UnityEngine;
namespace PAG.Utility
{
    public static class PAGText
    {
        public static string GetColoredText(string text, Color32 color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}";
        }
    }
}