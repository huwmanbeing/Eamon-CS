﻿
// IAfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	public interface IAfterPlayerMoveState : IState
	{
		Enums.Direction Direction { get; set; }

		IArtifact Artifact { get; set; }
	}
}
