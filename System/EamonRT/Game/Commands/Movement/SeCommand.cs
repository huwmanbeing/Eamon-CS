﻿
// SeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SeCommand : Command, ISeCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Southeast;
			});
		}

		public SeCommand()
		{
			SortOrder = 80;

			IsDarkEnabled = true;

			Name = "SeCommand";

			Verb = "se";

			Type = CommandType.Movement;
		}
	}
}
