using UnityEngine;

namespace IL2CPPBase
{
    public class Render : MonoBehaviour
    {
        private static GUIStyle StringStyle { get; set; } = new();
        
        public static float GetTextWidth(string text)
        {
            var content = new GUIContent(text);
            var size = StringStyle.CalcSize(content);
            return size.x;
        }
    }
}