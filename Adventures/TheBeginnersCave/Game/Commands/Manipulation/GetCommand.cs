﻿
// GetCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IGetCommand))]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		protected override void PlayerFinishParsing()
		{
			CommandParser.ParseName();

			if (string.Equals(CommandParser.ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				GetAll = true;
			}
			else if ((ActorRoom.Uid == 4 || ActorRoom.Uid == 20 || ActorRoom.Uid == 22) && CommandParser.ObjData.Name.IndexOf("torch", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				Globals.Out.WriteLine("{0}All torches are bolted to the wall and cannot be removed.", Environment.NewLine);

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsInRoom(ActorRoom),
					a => a.IsEmbeddedInRoom(ActorRoom)
				};

				CommandParser.ObjData.ArtifactNotFoundFunc = PrintCantVerbThat;

				PlayerResolveArtifact();
			}
		}
	}
}
