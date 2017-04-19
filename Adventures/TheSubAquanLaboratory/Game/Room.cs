﻿
// Room.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings(typeof(Eamon.Framework.IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override long GetDirs(long index)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			if (gameState != null)        // null in EamonDD; non-null in EamonRT
			{
				if (Uid == 2)
				{
					var artifact = Globals.ADB[83];

					return artifact != null && artifact.IsInLimbo() && index == 1 ? 17 : base.GetDirs(index);
				}
				else if (Uid == 10)
				{
					return gameState.Flood != 0 && (index == 5 || index == 7 || index == 8) ? -20 : base.GetDirs(index);
				}
				else if (Uid == 43)
				{
					var artifact = Globals.ADB[16];

					return artifact != null && artifact.IsInLimbo() && index == 4 ? 9 : base.GetDirs(index);
				}
				else
				{
					return base.GetDirs(index);
				}
			}
			else
			{
				return base.GetDirs(index);
			}
		}
	}
}
