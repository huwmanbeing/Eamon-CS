﻿
// IHealCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Commands;

namespace EamonRT.Framework.Commands
{
	public interface IHealCommand : ICommand
	{
		bool CastSpell { get; set; }
	}
}
