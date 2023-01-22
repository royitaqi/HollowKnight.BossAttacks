using System;
using System.Collections.Generic;
using BossAttacks.Utils;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace BossAttacks
{
    internal class Debugger
    {
        public static Debugger Instance;

        public void Load()
        {
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= OnHeroUpdate;

            foreach (var hook in _hooks)
            {
                hook.Dispose();
            }
            _hooks.Clear();
        }

        private void OnHeroUpdate()
        {
            var leftKey = KeyCode.Alpha1;
            var rightKey = KeyCode.Alpha2;
            var attackKey = KeyCode.Alpha3;

            if (Input.GetKeyDown(leftKey))
            {
                InputUtils.Load();
                InputUtils.PressControllerDirection("left");
                this.LogModTEMP($"fake left: {_leftDown}");
            }
            if (Input.GetKeyUp(leftKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseControllerDirection("left");
                this.LogModTEMP($"fake left: {_leftDown}");
            }
            if (Input.GetKeyDown(rightKey))
            {
                InputUtils.Load();
                InputUtils.PressControllerDirection("right");
                this.LogModTEMP($"fake right: {_rightDown}");
            }
            if (Input.GetKeyUp(rightKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseControllerDirection("right");
                this.LogModTEMP($"fake right: {_rightDown}");
            }
            if (Input.GetKeyDown(attackKey))
            {
                InputUtils.Load();
                InputUtils.PressControllerButton("attack");
                this.LogModTEMP($"fake attack: {_attackDown}");
            }
            if (Input.GetKeyUp(attackKey))
            {
                InputUtils.Load();
                InputUtils.ReleaseControllerButton("attack");
                this.LogModTEMP($"fake attack: {_attackDown}");
            }


            //if (Input.GetKeyDown(leftKey))
            //{
            //    Initialize();
            //    _leftDown = true;
            //    this.LogModTEMP($"fake left: {_leftDown}");
            //}
            //if (Input.GetKeyUp(leftKey))
            //{
            //    Initialize();
            //    _leftDown = false;
            //    this.LogModTEMP($"fake left: {_leftDown}");
            //}
            //if (Input.GetKeyDown(rightKey))
            //{
            //    Initialize();
            //    _rightDown = true;
            //    this.LogModTEMP($"fake right: {_rightDown}");
            //}
            //if (Input.GetKeyUp(rightKey))
            //{
            //    Initialize();
            //    _rightDown = false;
            //    this.LogModTEMP($"fake right: {_rightDown}");
            //}
            //if (Input.GetKeyDown(attackKey))
            //{
            //    Initialize();
            //    _attackDown = true;
            //    this.LogModTEMP($"fake attack: {_attackDown}");
            //}
            //if (Input.GetKeyUp(attackKey))
            //{
            //    Initialize();
            //    _attackDown = false;
            //    this.LogModTEMP($"fake attack: {_attackDown}");
            //}
        }

        private Dictionary<object, string> GetFieldNamesGeneric<AxisInputControl>()
        {
            var fieldNames = new Dictionary<object, string>();
            foreach (var field in typeof(HeroActions).GetFields())
            {
                if (field.FieldType.IsSubclassOf(typeof(AxisInputControl)))
                {
                    fieldNames[field.GetValue(_inputHandler.inputActions)] = field.Name;
                    this.LogModTEMP($"Found {typeof(AxisInputControl).Name.Substring(0, 7)} field: {field.Name}");
                }
            }
            return fieldNames;
        }

        private void HookPropertyGeneric<AxisInputControl, T>(Dictionary<object, string> fieldNames, Func<string, T, T> overrideFunc) where AxisInputControl : class
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
                        ret = overrideFunc(key, ret);
                        this.LogModTEMP($"~ {typeof(AxisInputControl).Name.Substring(0, 7)} ~ {key}: {ret}");
                        return ret;
                    };
                    _hooks.Add(new Hook(getter, genericHook));
                    this.LogModTEMP($"Hooked {typeof(AxisInputControl).Name.Substring(0, 7)} ~ {prop.Name}");
                }
            }
        }

        private bool OneAxisBoolOverrides(string key, bool dft)
        {
            if (key == "attack.WasPressed")
            {
                bool v = _attackDown | dft;
                this.LogModTEMP($"Overriding attack.WasPressed to {v}");
                return v;
            }
            return dft;
        }

        private int OneAxisIntOverrides(string key, int dft)
        {
            return dft;
        }

        private float OneAxisFloatOverrides(string key, float dft)
        {
            if (key == "left.Value")
            {
                float v = _leftDown ? 1 : dft;
                this.LogModTEMP($"Overriding left.Value to {v}");
                return v;
            }
            if (key == "right.Value")
            {
                float v = _rightDown ? 1 : dft;
                this.LogModTEMP($"Overriding right.Value to {v}");
                return v;
            }
            return dft;
        }

        private bool TwoAxisBoolOverrides(string key, bool dft)
        {
            return dft;
        }

        private void Initialize()
        {
            if (_inputHandler == null)
            {
                _inputHandler = typeof(HeroController).GetField("inputHandler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(HeroController.instance) as InputHandler;
                if (_inputHandler == null)
                {
                    return;
                }
            }

            // Get all the input actions in HeroActions. Create a map from action instance to string name. The hook will print the name of the actions being queried.
            if (_hooks.Count == 0)
            {
                var oneAxisFieldNames = GetFieldNamesGeneric<OneAxisInputControl>();
                var twoAxisFieldNames = GetFieldNamesGeneric<TwoAxisInputControl>();
                HookPropertyGeneric<OneAxisInputControl, bool>(oneAxisFieldNames, OneAxisBoolOverrides);
                HookPropertyGeneric<OneAxisInputControl, int>(oneAxisFieldNames, OneAxisIntOverrides);
                HookPropertyGeneric<OneAxisInputControl, float>(oneAxisFieldNames, OneAxisFloatOverrides);
                HookPropertyGeneric<TwoAxisInputControl, bool>(twoAxisFieldNames, TwoAxisBoolOverrides);
                HookPropertyGeneric<TwoAxisInputControl, int>(twoAxisFieldNames, (_, ret) => ret);
                HookPropertyGeneric<TwoAxisInputControl, float>(twoAxisFieldNames, (_, ret) => ret);
                HookPropertyGeneric<TwoAxisInputControl, Vector2>(twoAxisFieldNames, (_, ret) => ret);
            }
        }

        private InputHandler _inputHandler;
        private List<Hook> _hooks = new();
        // fake input
        private bool _leftDown;
        private bool _rightDown;
        private bool _attackDown;
    }
}
