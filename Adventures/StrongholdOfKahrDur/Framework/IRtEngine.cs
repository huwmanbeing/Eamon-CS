﻿
// IRtEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace StrongholdOfKahrDur.Framework
{
	public interface IRtEngine : EamonRT.Framework.IRtEngine
	{
		bool SpellReagentsInCauldron(Eamon.Framework.IArtifact artifact);
	}
}
