using System;
using System.Collections.Generic;
using InControl;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace BossAttacks.Utils
{
    internal static class InputUtils
    {
        #region Controller Overrides
        internal static void Load()
        {
            if (InputHandler == null)
            {
                InputHandler = typeof(HeroController).GetField("inputHandler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(HeroController.instance) as InputHandler;
                if (InputHandler == null)
                {
                    return;
                }
            }

            // Get all the input actions in HeroActions. Create a map from action instance to string name. The hook will print the name of the actions being queried.
            if (Hooks.Count == 0)
            {
                var oneAxisFieldNames = GetFieldNamesGeneric<OneAxisInputControl>();
                var twoAxisFieldNames = GetFieldNamesGeneric<TwoAxisInputControl>();
                HookPropertyGeneric<OneAxisInputControl, bool>(oneAxisFieldNames, ApplyControllerOverride);
                HookPropertyGeneric<OneAxisInputControl, int>(oneAxisFieldNames, ApplyControllerOverride);
                HookPropertyGeneric<OneAxisInputControl, float>(oneAxisFieldNames, ApplyControllerOverride);
                HookPropertyGeneric<TwoAxisInputControl, bool>(twoAxisFieldNames, ApplyControllerOverride);
                HookPropertyGeneric<TwoAxisInputControl, int>(twoAxisFieldNames, ApplyControllerOverride);
                HookPropertyGeneric<TwoAxisInputControl, float>(twoAxisFieldNames, ApplyControllerOverride);
                //HookPropertyGeneric<TwoAxisInputControl, Vector2>(twoAxisFieldNames, ApplyControllerOverride);
            }
        }

        internal static void Unload()
        {
            InputHandler = null;

            foreach (var hook in Hooks)
            {
                hook.Dispose();
            }
            Hooks.Clear();

            ControllerBoolOverrides.Clear();
            ControllerIntOverrides.Clear();
            ControllerFloatOverrides.Clear();
            KeyboardOverrides.Clear();
        }

        private static Dictionary<object, string> GetFieldNamesGeneric<AxisInputControl>()
        {
            var fieldNames = new Dictionary<object, string>();
            foreach (var field in typeof(HeroActions).GetFields())
            {
                if (field.FieldType.IsSubclassOf(typeof(AxisInputControl)))
                {
                    fieldNames[field.GetValue(InputHandler.inputActions)] = field.Name;
                    typeof(InputUtils).LogModTEMP($"Found {typeof(AxisInputControl).Name.Substring(0, 7)} field: {field.Name}");
                }
            }
            return fieldNames;
        }

        private static void HookPropertyGeneric<AxisInputControl, T>(Dictionary<object, string> fieldNames, Func<string, T, T> applyOverride) where AxisInputControl : class
        {
            foreach (var prop in typeof(AxisInputControl).GetProperties())
            {
                if (prop.PropertyType == typeof(T))
                {
                    var getter = typeof(AxisInputControl).GetMethod($"get_{prop.Name}");
                    ModAssert.AllBuilds(getter != null, $"getter for {prop.Name} should be nonnull");
                    var genericHook = (Func<AxisInputControl, T> orig, AxisInputControl self) =>
                    {
                        // original value
                        var ret = orig(self);

                        // if not one of the interesting actions, return original value
                        if (!fieldNames.ContainsKey(self))
                        {
                            return ret;
                        }

                        // override value
                        var key = $"{fieldNames[self]}.{prop.Name}";
                        ret = applyOverride(key, ret);
                        typeof(InputUtils).LogModTEMP($"~ {typeof(AxisInputControl).Name.Substring(0, 7)} ~ {key}: {ret}");
                        return ret;
                    };
                    Hooks.Add(new Hook(getter, genericHook));
                    typeof(InputUtils).LogModTEMP($"Hooked {typeof(AxisInputControl).Name.Substring(0, 7)} ~ {prop.Name}");
                }
            }
        }

        private static InputHandler InputHandler;
        private static readonly List<Hook> Hooks = new();

        //internal static void OverrideControllerInput(string key, bool value)
        //{
        //}
        //internal static void OverrideControllerInput(string key, int value)
        //{
        //}
        //internal static void OverrideControllerInput(string key, float value)
        //{
        //}
        internal static bool ApplyControllerOverride(string key, bool dft)
        {
            if (ControllerBoolOverrides.ContainsKey(key))
            {
                return ControllerBoolOverrides[key] | dft;
            }
            return dft;
        }
        internal static int ApplyControllerOverride(string key, int dft)
        {
            if (ControllerIntOverrides.ContainsKey(key))
            {
                return ControllerIntOverrides[key];
            }
            return dft;
        }
        internal static float ApplyControllerOverride(string key, float dft)
        {
            if (ControllerFloatOverrides.ContainsKey(key))
            {
                return ControllerFloatOverrides[key];
            }
            return dft;
        }
        internal static readonly Dictionary<string, bool> ControllerBoolOverrides = new();
        internal static readonly Dictionary<string, int> ControllerIntOverrides = new();
        internal static readonly Dictionary<string, float> ControllerFloatOverrides = new();
        #endregion

        #region Keyboard Overrides
        //internal static void OverrideKeyboardInput(KeyCode key, bool value)
        //{
        //    KeyboardOverrides[key] = value;
        //}

        internal static bool GetKeyDown(KeyCode key)
        {
            if (KeyboardOverrides.ContainsKey(key))
            {
                KeyboardOverrides.Remove(key);
                return KeyboardOverrides[key];
            }
            return Input.GetKeyDown(key);
        }
        internal static readonly Dictionary<KeyCode, bool> KeyboardOverrides = new();
        #endregion
    }
}
