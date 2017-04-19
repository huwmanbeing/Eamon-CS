﻿
// IMonsterBattleState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.States;

namespace EamonRT.Framework.States
{
	public interface IMonsterBattleState : IState
	{
		bool ReadyCommandCalled { get; set; }

		long MemberNumber { get; set; }
	}
}
