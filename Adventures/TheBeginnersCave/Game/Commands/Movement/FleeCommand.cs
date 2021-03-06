﻿
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		public override void PrintCalmDown()
		{
			gOut.Print("What are you fleeing from?");
		}

		public override void PrintNoPlaceToGo()
		{
			gOut.Print("There's no place to run!");
		}

		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterNumberOfExitsCheck)
			{
				// another classic Eamon moment...

				var mimicMonster = gMDB[7];

				Debug.Assert(mimicMonster != null);

				if (mimicMonster.IsInRoom(gActorRoom))
				{
					gOut.Print("You are held fast by the mimic and cannot flee!");

					NextState = Globals.CreateInstance<IMonsterStartState>();

					GotoCleanup = true;
				}
			}

			base.PlayerProcessEvents(eventType);
		}
	}
}
