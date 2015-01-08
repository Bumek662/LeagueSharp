using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Drawing;

namespace StrongAmumu
{
    class Program
    {
        private static string Name = "Strong Amumu";
        private static string Champion = "Amumu";
        private static Obj_AI_Hero Player = ObjectManager.Player;
        private static Spell Q;
        private static Spell W;
        private static Spell E;
        private static Spell R;
        private static List<Spell> SpellList = new List<Spell>();
        private static Menu Config;
        private static Orbwalking.Orbwalker Orbwalker;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += OnDraw;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != Champion) return;
            Game.PrintChat("<font color=\"#ff5500\">[{0}]</font><font color=\"#00aa00\"> - loaded!</font>", Name);
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 300);
            E = new Spell(SpellSlot.E, 350);
            R = new Spell(SpellSlot.R, 550);

            Q.SetSkillshot(250f, 90, 2000, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(250f, 550, float.MaxValue, false, SkillshotType.SkillshotCircle);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            Config = new Menu("Strong Amumu", "Config", true);
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);

            Config.AddSubMenu(targetSelectorMenu);
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("Packets", "Packet Cast")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("MinUlt", "Min. enemies to ult")).SetValue(new Slider(2, 1, 5));
            Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawEnable", "Enable Drawing").SetValue(true));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);

            Config.AddToMainMenu();

        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.Item("DrawEnable").GetValue<bool>())
            {

                if (Config.Item("DrawQ").GetValue<bool>())
                {
                    Drawing.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.Blue);
                }

                if (Config.Item("DrawR").GetValue<bool>())
                {
                    Drawing.DrawCircle(Player.Position, R.Range, System.Drawing.Color.Red);
                }

            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
        }

        private static void Combo()
        {
            var Target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);

            if (Target == null) return;

            var useQ = Config.Item("UseQCombo").GetValue<bool>();
            var useE = Config.Item("UseECombo").GetValue<bool>();
            var useR = Config.Item("UseRCombo").GetValue<bool>();
            var minR = Config.Item("MinUlt").GetValue<Slider>().Value;
            var packetCast = Config.Item("Packets").GetValue<bool>();

            if (useQ && Target.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.Cast(Target, packetCast);
            }

            if (useE && Target.IsValidTarget(E.Range) && E.IsReady())
            {
                E.Cast(packetCast);
            }

            if (useR && Target.IsValidTarget(R.Range) && Player.CountEnemysInRange(R.Range) >= minR && R.IsReady())
            {
                R.Cast(Player, packetCast);
            }
        }
    }
}
