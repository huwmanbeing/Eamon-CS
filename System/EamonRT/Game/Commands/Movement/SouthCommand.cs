﻿
// SouthCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SouthCommand : Command, ISouthCommand
	{
		public override void PlayerExecute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Enums.Direction.South;
			});
		}

		public SouthCommand()
		{
			SortOrder = 10;

			IsDarkEnabled = true;

			Name = "SouthCommand";

			Verb = "south";

			Type = Enums.CommandType.Movement;
		}
	}
}

/* EamonCsCodeTemplate

// SouthCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class SouthCommand : EamonRT.Game.Commands.SouthCommand, ISouthCommand
	{

	}
}
EamonCsCodeTemplate */
