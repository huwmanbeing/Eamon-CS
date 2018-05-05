﻿
// Artifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, Eamon.Framework.IArtifact
	{
		public override bool IsReadyableByMonsterUid(long monsterUid)
		{
			// Only player can wield fireball wand

			return Uid != 63 && base.IsReadyableByMonsterUid(monsterUid);
		}
	}
}
