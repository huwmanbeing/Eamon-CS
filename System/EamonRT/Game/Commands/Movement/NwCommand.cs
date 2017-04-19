﻿
// NwCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NwCommand : Command, INwCommand
	{
		protected override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.Northwest;
			});
		}

		public override string GetPrintedVerb()
		{
			return Verb.ToUpper();
		}

		public NwCommand()
		{
			SortOrder = 70;

			IsDarkEnabled = true;

			Name = "NwCommand";

			Verb = "nw";

			Type = Enums.CommandType.Movement;
		}
	}
}
