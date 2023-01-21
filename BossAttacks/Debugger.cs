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
                Initialize();
                _leftDown = true;
                this.LogModTEMP($"fake left: {_leftDown}");
            }
            if (Input.GetKeyUp(leftKey))
            {
                Initialize();
                _leftDown = false;
                this.LogModTEMP($"fake left: {_leftDown}");
            }
            if (Input.GetKeyDown(rightKey))
            {
                Initialize();
                _rightDown = true;
                this.LogModTEMP($"fake right: {_rightDown}");
            }
            if (Input.GetKeyUp(rightKey))
            {
                Initialize();
                _rightDown = false;
                this.LogModTEMP($"fake right: {_rightDown}");
            }
            if (Input.GetKeyDown(attackKey))
            {
                Initialize();
                _attackDown = true;
                this.LogModTEMP($"fake attack: {_attackDown}");
            }
            if (Input.GetKeyUp(attackKey))
            {
                Initialize();
                _attackDown = false;
                this.LogModTEMP($"fake attack: {_attackDown}");
            }
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

        private void HookPropertyGeneric<AxisInputControl, T>(Dictionary<object, string> fieldNames, Func<AxisInputControl, string, T, T> overrideFunc) where AxisInputControl : class
        {
            foreach (var prop in typeof(AxisInputControl).GetProperties())
            {
                if (prop.PropertyType == typeof(T))
                {
                    var getter = typeof(AxisInputControl).GetMethod($"get_{prop.Name}");
                    ModAssert.AllBuilds(getter != null, $"getter for {prop.Name} should be nonnull");
                    var genericHook = (Func<AxisInputControl, T> orig, AxisInputControl self) =>
                    {
                        // original
                        var ret = orig(self);

                        // override
                        ret = overrideFunc.Invoke(self, prop.Name, ret);

                        if (fieldNames.ContainsKey(self))
                        {
                            this.LogModTEMP($"~ {typeof(AxisInputControl).Name.Substring(0, 7)} ~ {fieldNames[self]}.{prop.Name}: {ret}");
                        }
                        return ret;
                    };
                    _hooks.Add(new Hook(getter, genericHook));
                    this.LogModTEMP($"Hooked {typeof(AxisInputControl).Name.Substring(0, 7)} ~ {prop.Name}");
                }
            }
        }

        private bool OneAxisBoolOverrides(OneAxisInputControl self, string propName, bool dft)
        {
            if (self == _inputHandler?.inputActions?.attack && propName == "WasPressed")
            {
                this.LogModTEMP("Overriding attack.WasPressed");
                return _attackDown;
            }
            return dft;
        }

        private int OneAxisIntOverrides(OneAxisInputControl self, string propName, int dft)
        {
            return dft;
        }

        private float OneAxisFloatOverrides(OneAxisInputControl self, string propName, float dft)
        {
            if (self == _inputHandler?.inputActions?.left && propName == "Value")
            {
                float v = _leftDown ? 1 : 0;
                this.LogModTEMP($"Overriding left.Value to {v}");
                return v;
            }
            if (self == _inputHandler?.inputActions?.right && propName == "Value")
            {
                float v = _rightDown ? 1 : 0;
                this.LogModTEMP($"Overriding right.Value to {v}");
                return v;
            }
            return dft;
        }

        private bool TwoAxisBoolOverrides(TwoAxisInputControl self, string propName, bool dft)
        {
            //if (self == _inputHandler?.inputActions?.moveVector && propName == "WasPressed")
            //{
            //    this.LogModTEMP($"Overriding moveVector.WasPressed to {_leftDown}");
            //    return _leftDown;
            //}
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
                HookPropertyGeneric<TwoAxisInputControl, int>(twoAxisFieldNames, (_, _, ret) => ret);
                HookPropertyGeneric<TwoAxisInputControl, float>(twoAxisFieldNames, (_, _, ret) => ret);
                HookPropertyGeneric<TwoAxisInputControl, Vector2>(twoAxisFieldNames, (_, _, ret) => ret);
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
