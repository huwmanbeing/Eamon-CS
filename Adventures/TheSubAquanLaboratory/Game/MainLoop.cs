﻿
// MainLoop.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IMainLoop))]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			// End of game specials

			Globals.Out.Write("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

			Globals.Engine.PrintEffectDesc(71);

			Globals.In.KeyPress(Globals.Buf);

			// Calculate number of lab rooms explored

			var rooms = Globals.Database.RoomTable.Records.Where(r => r.Zone == 2).ToList();

			var seenCount = rooms.Count(r => r.Seen);

			gameState.QuestValue += (long)Math.Round(((double)seenCount / (double)rooms.Count) * 350);

			// 100% of quest = base reward of 1250

			Globals.Out.Write("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

			if (gameState.QuestValue > 0)
			{
				var reward = (long)Math.Round((double)gameState.QuestValue * ((double)Globals.Character.GetStats(Enums.Stat.Charisma) / (double)10));

				if (reward > 3000)
				{
					reward = 3000;
				}

				Globals.Character.HeldGold += reward;

				Globals.Engine.PrintEffectDesc(78);

				Globals.Out.Write("{0}The mayor then calculates the value of the information you have presented him with and pays you {1} gold pieces.{0}", Environment.NewLine, reward);
			}
			else
			{
				Globals.Engine.PrintEffectDesc(76);
			}

			Globals.In.KeyPress(Globals.Buf);

			base.Shutdown();
		}
	}
}
