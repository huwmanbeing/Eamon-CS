﻿
// Monster.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using ARuncibleCargo.Framework;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(Eamon.Framework.IMonster))]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		protected override bool HasHumanNaturalAttackDescs()
		{
			// Use appropriate natural attack descriptions for humans

			return (Uid > 3 && Uid < 13) || Uid == 15 || (Uid > 21 && Uid < 24) || (Uid > 24 && Uid < 40);
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			rc = base.BuildPrintedFullDesc(buf, showName);

			// Pookas

			if (Globals.Engine.IsSuccess(rc) && Uid < 4)
			{
				var gameState = Globals.GameState as IGameState;

				Debug.Assert(gameState != null);

				if (!gameState.GetPookaMet(Uid - 1))
				{
					var effect = Globals.EDB[3 + Uid];

					Debug.Assert(effect != null);

					buf.AppendFormat("{0}{1}{0}", Environment.NewLine, effect.Desc);

					gameState.SetPookaMet(Uid - 1, true);
				}
			}

			return rc;
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			var room = Globals.RDB[roomUid];

			Debug.Assert(room != null);

			// Nobody can flee into a water room

			if (fleeing && room.CastTo<IRoom>().IsWaterRoom())
			{
				return false;
			}

			// Putrid rats and enormous alligator can't leave sewer

			else if (Uid == 13 || Uid == 14)
			{
				return roomUid != 13 && roomUid != 29 && roomUid != 50 ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
			}
			else
			{
				return base.CanMoveToRoomUid(roomUid, fleeing);
			}
		}
	}
}
