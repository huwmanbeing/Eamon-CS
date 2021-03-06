﻿
// NeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NeCommand : Command, INeCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Northeast;
			});
		}

		public NeCommand()
		{
			SortOrder = 60;

			IsDarkEnabled = true;

			Name = "NeCommand";

			Verb = "ne";

			Type = CommandType.Movement;
		}
	}
}
