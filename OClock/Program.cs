using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using System.Drawing;
namespace OClock
{
    class Program
    {
        public static string Time;
        private static void Main(string[] args)
        {
            
            CustomEvents.Game.OnGameLoad += Game_OnLoad;
            Game.OnGameUpdate += Game_OnUpdate;
            Drawing.OnDraw += DrawNew;
            Game.PrintChat(Drawing.Height.ToString());
        }
        public static void Game_OnLoad(EventArgs args)
        {
            Game.PrintChat("<font color=\"#1eff00\">O'clock by Bumek662</font> - <font color=\"#00BFFF\">Loaded</font>" + Time);

        }

        public static void DrawNew(EventArgs args)
        {
        }

        public static void Game_OnUpdate(EventArgs args)
        {
                Time = DateTime.Now.ToString("HH:mm:ss");
                Drawing.DrawText(Drawing.Width - 100, 75, System.Drawing.Color.Goldenrod, Time);
        }

    }
}
