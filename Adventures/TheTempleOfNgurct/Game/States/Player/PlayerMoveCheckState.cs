﻿
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterBlockingArtifactCheck)
			{
				if (Globals.GameState.R2 == -33)
				{
					Globals.Out.Print("The oak door is locked from the inside!");
				}
				else if (Globals.GameState.R2 == -55)
				{
					Globals.Out.Print("The cell door is locked from the outside!");
				}

				// Down the sewage chute

				else if (Globals.GameState.R2 == -60)
				{
					Globals.Engine.PrintEffectDesc(24);

					Globals.GameState.Die = 1;

					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;
				}
				else
				{
					base.ProcessEvents(eventType);
				}
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
