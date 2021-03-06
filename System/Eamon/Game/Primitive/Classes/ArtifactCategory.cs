﻿
// ArtifactCategory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class ArtifactCategory : IArtifactCategory
	{
		[ExcludeFromSerialization]
		public virtual IArtifact Parent { get; set; }

		public virtual Enums.ArtifactType Type { get; set; }

		public virtual long Field1 { get; set; }

		public virtual long Field2 { get; set; }

		public virtual long Field3 { get; set; }

		public virtual long Field4 { get; set; }

		public virtual long Field5 { get; set; }

		public virtual bool IsOpenable()
		{
			return Type == Enums.ArtifactType.InContainer || Type == Enums.ArtifactType.Drinkable || Type == Enums.ArtifactType.Edible || Type == Enums.ArtifactType.Readable || Type == Enums.ArtifactType.DoorGate;
		}

		public virtual bool IsLockable()
		{
			return Type == Enums.ArtifactType.InContainer || Type == Enums.ArtifactType.DoorGate || Type == Enums.ArtifactType.BoundMonster;
		}

		public virtual bool IsBreakable()
		{
			return Type == Enums.ArtifactType.InContainer || Type == Enums.ArtifactType.DoorGate;
		}

		public virtual bool IsEffectExposer()
		{
			return Type == Enums.ArtifactType.Readable || Type == Enums.ArtifactType.DisguisedMonster;
		}

		public virtual bool IsMonsterExposer()
		{
			return Type == Enums.ArtifactType.BoundMonster || Type == Enums.ArtifactType.DisguisedMonster;
		}

		public virtual bool IsWeapon(Enums.Weapon weapon)
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Weapon), weapon));

			return IsWeapon01() && Field2 == (long)weapon;
		}

		public virtual bool IsWeapon01()
		{
			return Type == Enums.ArtifactType.Weapon || Type == Enums.ArtifactType.MagicWeapon;
		}

		public virtual bool IsOpen()
		{
			var result = false;

			if (Type == Enums.ArtifactType.InContainer)
			{
				result = Field2 == 1;
			}
			else if (Type == Enums.ArtifactType.Drinkable || Type == Enums.ArtifactType.Edible || Type == Enums.ArtifactType.Readable)
			{
				result = Field3 == 1;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				result = Field3 == 0;
			}

			return result;
		}

		public virtual void SetOpen(bool open)
		{
			if (Type == Enums.ArtifactType.InContainer)
			{
				Field2 = open ? 1 : 0;
			}
			else if (Type == Enums.ArtifactType.Drinkable || Type == Enums.ArtifactType.Edible || Type == Enums.ArtifactType.Readable)
			{
				Field3 = open ? 1 : 0;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				Field3 = open ? 0 : 1;
			}
		}

		public virtual void SetKeyUid(long artifactUid)
		{
			Debug.Assert(artifactUid >= -2);         // -2=Broken

			if (Type == Enums.ArtifactType.InContainer)
			{
				Field1 = artifactUid;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				Field2 = artifactUid;
			}
			else if (Type == Enums.ArtifactType.BoundMonster)
			{
				if (artifactUid == -2)
				{
					artifactUid = 0;
				}

				Field2 = artifactUid;
			}
		}

		public virtual void SetBreakageStrength(long strength)
		{
			Debug.Assert(gEngine.IsArtifactFieldStrength(strength));

			if (Type == Enums.ArtifactType.InContainer)
			{
				Field2 = strength;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				Field3 = strength;
			}
		}

		public virtual void SetFirstEffect(long effectUid)
		{
			Debug.Assert(effectUid > 0);

			if (Type == Enums.ArtifactType.Readable)
			{
				Field1 = effectUid;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				Field2 = effectUid;
			}
		}

		public virtual void SetNumEffects(long numEffects)
		{
			Debug.Assert(numEffects > 0);

			if (Type == Enums.ArtifactType.Readable)
			{
				Field2 = numEffects;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				Field3 = numEffects;
			}
		}

		public virtual void SetMonsterUid(long monsterUid)
		{
			Debug.Assert(monsterUid > 0);

			if (Type == Enums.ArtifactType.BoundMonster || Type == Enums.ArtifactType.DisguisedMonster)
			{
				Field1 = monsterUid;
			}
		}

		public virtual long GetKeyUid()
		{
			long result = 0;

			if (Type == Enums.ArtifactType.InContainer)
			{
				result = Field1;
			}
			else if (Type == Enums.ArtifactType.DoorGate || Type == Enums.ArtifactType.BoundMonster)
			{
				result = Field2;
			}

			return result;
		}

		public virtual long GetBreakageStrength()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.InContainer)
			{
				result = gEngine.IsArtifactFieldStrength(Field2) ? Field2 : 0L;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				result = gEngine.IsArtifactFieldStrength(Field3) ? Field3 : 0L;
			}

			return result;
		}

		public virtual long GetFirstEffect()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.Readable)
			{
				result = Field1;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				result = Field2;
			}

			return result;
		}

		public virtual long GetNumEffects()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.Readable)
			{
				result = Field2;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				result = Field3;
			}

			return result;
		}

		public virtual long GetMonsterUid()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.BoundMonster || Type == Enums.ArtifactType.DisguisedMonster)
			{
				result = Field1;
			}

			return result;
		}

		public ArtifactCategory()
		{
			Type = Enums.ArtifactType.None;
		}
	}
}
