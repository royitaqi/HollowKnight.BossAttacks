using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossAttacks.Modules;
using BossAttacks.Utils;
using Modding;
using Newtonsoft.Json;
using UnityEngine;

namespace BossAttacks
{
    internal class Debugger
    {
        public void Load()
        {
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= OnHeroUpdate;
        }
        
        private void OnHeroUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                PrintAllBooleanOptions();
            }

            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    var options = GetAllBooleanOptions();
                    options[i].Item3.Value = !options[i].Item3.Value;
                    PrintAllBooleanOptions();
                }
            }
        }

        private void PrintAllBooleanOptions()
        {
            var options = GetAllBooleanOptions();
            this.LogModTEMP("All boolean options:");
            for (int i = 0; i < Math.Min(options.Length, 9); i++)
            {
                var tuple = options[i];
                this.LogModTEMP($"  {i + 1} - {tuple.Item1.Name}/{tuple.Item2}: {tuple.Item3.Value}");
            }
        }

        private Tuple<Module, string, Option<bool>>[] GetAllBooleanOptions()
        {
            return ModuleManager
                .Instance
                .GetLoadedModules()
                .OrderBy(m => m.Name)
                .SelectMany(m =>
                    m.BooleanOptions
                        .OrderBy(kvp => kvp.Key)
                        .Select(kvp => new Tuple<Module, string, Option<bool>>(m, kvp.Key, kvp.Value))
                )
                .ToArray();
        }

    }
}
