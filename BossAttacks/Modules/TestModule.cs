//using UnityEngine.SceneManagement;
//using HutongGames.PlayMaker;
//using BossAttacks.Utils;
//using SFCore.Utils;
//using System.Linq;
//using System;
//using System.Collections.Generic;
//using HutongGames.PlayMaker.Actions;

//namespace BossAttacks.Modules;

///**
// * Make the ROLL JUMP to be done without the initial throwing and the finishing diving.
// */
//internal class TestModule : Module
//{
//    public override string Name => "TestModule";
//    protected override bool OnLoad(Scene scene)
//    {
//        _options.Add("Test 1", new Option<bool> { Value = false });
//        _options.Add("Test 2", new Option<bool> { Value = false });
//        _options.Add("Test 3", new Option<bool> { Value = false });
//        _options.Add("Test 4", new Option<bool> { Value = false });
//        return true;
//    }

//    protected override void OnUnload()
//    {
//        // Turn all options off when the module is unloaded.
//        foreach (var kvp in _options)
//        {
//            kvp.Value.Value = false;
//        }
//    }
//}
