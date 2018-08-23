﻿
// SeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
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
				x.Direction = Enums.Direction.Southeast;
			});
		}

		public SeCommand()
		{
			SortOrder = 80;

			IsDarkEnabled = true;

			Name = "SeCommand";

			Verb = "se";

			Type = Enums.CommandType.Movement;
		}
	}
}

/* EamonCsCodeTemplate

// SeCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class SeCommand : EamonRT.Game.Commands.SeCommand, ISeCommand
	{

	}
}
EamonCsCodeTemplate */
