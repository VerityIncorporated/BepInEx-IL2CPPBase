using UnityEngine;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using IL2CPPBase.Misc;
using IL2CPPBase.ModuleSystem;

namespace IL2CPPBase
{
    [BepInPlugin("IL2CPPBase", "IL2CPPBase", "1.0.0.0")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            Logs.Logger = Log;
            IL2CPPBase.Initialize(this);
        }
    }

    public class IL2CPPBase : MonoBehaviour
    {
        private static readonly GUIStyle WatermarkGUIStyle = new();
        private static readonly GUIStyle ArrayListGUIStyle = new();
        
        public static Harmony Harmony = new("IL2CPPBase");
        
        public static void Initialize(Plugin plugin)
        {
            var addComponent = plugin.AddComponent<IL2CPPBase>();
            addComponent.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(addComponent.gameObject);
        }
        
        private void Start()
        {
            ModuleManager.Initialize();
            foreach (var module in ModuleManager.Modules)
            {
                Logs.Logger.LogInfo($"{module.Name} was Initialized");
                module.OnCheatLoad();
            }
        }
        
        private void Update()
        {
            foreach (var module in ModuleManager.Modules.Where(module => Input.GetKeyDown(module.KeyBind)))
            {
                module.Toggle();
            }
                
            foreach (var module in ModuleManager.Modules.Where(m => m.Enabled))
            {
                module.OnUpdate();
            }
        }
        
        private void OnGUI()
        {
            ArrayList();
            Watermark();
            foreach (var module in ModuleManager.Modules.Where(m => m.Enabled))
            {
                module.OnGUI();
            }
        }
        
        private static void ArrayList()
        {
            var rainbow = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.30f, 1), 1, 1);
            ArrayListGUIStyle.alignment = TextAnchor.MiddleRight;
            ArrayListGUIStyle.normal.textColor = rainbow;
            ArrayListGUIStyle.fontSize = 20;
            
            var modules = ModuleManager.Modules.Where(module => module.Enabled && module.Name != "GUI");
            var orderedEnumerable = modules.OrderByDescending(x => Render.GetTextWidth(x.Name)).ToArray();

            var count = 0;
            foreach (var module in orderedEnumerable)
            {
                count += 20;
                GUI.Label(new Rect(1710, count, 200, 200), $"{module.Name}", ArrayListGUIStyle);
            }
        }

        private static void Watermark()
        {
            var rainbow = Color.HSVToRGB(Mathf.PingPong(Time.time * 0.30f, 1), 1, 1);
            WatermarkGUIStyle.alignment = TextAnchor.UpperLeft;
            WatermarkGUIStyle.normal.textColor = rainbow;
            WatermarkGUIStyle.fontSize = 25;
            
            GUI.Label(new Rect(10, 5, 200, 200), "IL2CPPBase by Verity", WatermarkGUIStyle);
        }
    }
}