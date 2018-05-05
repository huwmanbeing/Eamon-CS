﻿
// Artifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;

namespace WrenholdsSecretVigil.Game
{
	[ClassMappings(typeof(Eamon.Framework.IArtifact))]
	public class Artifact : Eamon.Game.Artifact, Framework.IArtifact
	{
		public override bool IsReadyableByCharacter()
		{
			// Only one-eyed ogre can wield large tree limb

			return Uid != 7 && base.IsReadyableByCharacter();
		}

		public override bool IsReadyableByMonsterUid(long monsterUid)
		{
			// Only one-eyed ogre can wield large tree limb

			return Uid != 7 || monsterUid == 3 ? base.IsReadyableByMonsterUid(monsterUid) : false;
		}

		public override string GetBrokenDesc()
		{
			// Swallower shark

			return Uid == 31 ? "(mangled)" : base.GetBrokenDesc();
		}

		public virtual bool IsBuriedInRoomUid(long roomUid)
		{
			return Location == (roomUid + 4000);
		}

		public virtual bool IsBuriedInRoom(Eamon.Framework.IRoom room)
		{
			Debug.Assert(room != null);

			return IsBuriedInRoomUid(room.Uid);
		}
	}
}
