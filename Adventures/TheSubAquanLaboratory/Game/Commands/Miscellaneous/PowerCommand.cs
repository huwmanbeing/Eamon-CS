﻿
// PowerCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using TheSubAquanLaboratory.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IPowerCommand))]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		protected virtual bool IsActorRoomInLab()
		{
			return ActorRoom.Uid == 18 || ActorRoom.Zone == 2;
		}

		protected override void PrintSonicBoom()
		{
			Globals.Engine.PrintEffectDesc(80 + (IsActorRoomInLab() ? 1 : 0));
		}

		protected override void PlayerProcessEvents()
		{
			var rl = Globals.Engine.RollDice01(1, 100, 0);

			if (rl < 11)
			{
				// 10% Chance of raising the dead

				var found = Globals.RtEngine.ResurrectDeadBodies();

				if (found)
				{
					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 21)
			{
				// 10% Chance of stuff vanishing

				var found = Globals.RtEngine.MakeArtifactsVanish(a => a.IsInRoom(ActorRoom) && !a.IsUnmovable() && a.Uid != 82 && a.Uid != 89);

				if (found)
				{
					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 31)
			{
				// 10% Chance of cracking dome

				if (IsActorRoomInLab())
				{
					Globals.Engine.PrintEffectDesc(44);

					Globals.GameState.Die = 1;

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 41)
			{
				// 10% Chance of insta-heal (tm)

				if (ActorMonster.DmgTaken > 0)
				{
					ActorMonster.DmgTaken = 0;

					Globals.RtEngine.CheckEnemies();

					Globals.Out.Write("{0}All of your wounds are suddenly healed!{0}", Environment.NewLine);

					Globals.Buf.SetFormat("{0}You are ", Environment.NewLine);

					ActorMonster.AddHealthStatus(Globals.Buf);

					Globals.Out.Write("{0}", Globals.Buf);

					GotoCleanup = true;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 101)
			{
				// 60% Chance of boom over lake/in lab or fortune cookie

				base.PlayerProcessEvents();
			}

		Cleanup:

			;
		}
	}
}
