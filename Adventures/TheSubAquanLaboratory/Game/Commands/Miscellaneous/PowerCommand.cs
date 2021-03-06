﻿
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public virtual bool IsActorRoomInLab()
		{
			return gActorRoom.Uid == 18 || gActorRoom.Zone == 2;
		}

		public override void PrintSonicBoom()
		{
			gEngine.PrintEffectDesc(80 + (IsActorRoomInLab() ? 1 : 0));
		}

		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterPlayerSpellCastCheck)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				if (rl < 11)
				{
					// 10% Chance of raising the dead

					var found = gEngine.ResurrectDeadBodies(gActorRoom);

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

					var found = gEngine.MakeArtifactsVanish(gActorRoom, a => a.IsInRoom(gActorRoom) && !a.IsUnmovable() && a.Uid != 82 && a.Uid != 89);

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
						gEngine.PrintEffectDesc(44);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
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

					if (gActorMonster.DmgTaken > 0)
					{
						gActorMonster.DmgTaken = 0;

						gOut.Print("All of your wounds are suddenly healed!");

						Globals.Buf.SetFormat("{0}You are ", Environment.NewLine);

						gActorMonster.AddHealthStatus(Globals.Buf);

						gOut.Write("{0}", Globals.Buf);

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

					base.PlayerProcessEvents(eventType);
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}

		Cleanup:

			;
		}
	}
}
