﻿
// EffectDbTable.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.DataStorage.Generic;

namespace Eamon.Game.DataStorage
{
	[ClassMappings(typeof(IDbTable<IEffect>))]
	public class EffectDbTable : DbTable<IEffect>
	{

	}
}
