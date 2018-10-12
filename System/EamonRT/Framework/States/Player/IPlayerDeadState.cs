﻿
// IPlayerDeadState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.States
{
	public interface IPlayerDeadState : IState
	{
		bool PrintLineSep { get; set; }
	}
}
