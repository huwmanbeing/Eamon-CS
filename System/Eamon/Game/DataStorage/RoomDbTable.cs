﻿
// RoomDbTable.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.DataStorage.Generic;

namespace Eamon.Game.DataStorage
{
	[ClassMappings(typeof(IDbTable<IRoom>))]
	public class RoomDbTable : DbTable<IRoom>
	{

	}
}
