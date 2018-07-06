﻿
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheTempleOfNgurct.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IGameState>))]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper
	{
		public virtual new Framework.IGameState Record
		{
			get
			{
				return (Framework.IGameState)base.Record;
			}

			set
			{
				if (base.Record != value)
				{
					base.Record = value;
				}
			}
		}

		protected virtual bool ValidateWanderingMonster()
		{
			return Record.WanderingMonster >= 12 && Record.WanderingMonster <= 27;
		}

		protected virtual bool ValidateDwLoopCounter()
		{
			return Record.DwLoopCounter >= 0 && Record.DwLoopCounter <= 16;
		}

		protected virtual bool ValidateWandCharges()
		{
			return Record.WandCharges >= 0 && Record.WandCharges <= 5;
		}

		protected virtual bool ValidateRegenerate()
		{
			return Record.Regenerate >= 0 && Record.Regenerate <= 5;
		}

		protected virtual bool ValidateKeyRingRoomUid()
		{
			return Record.KeyRingRoomUid >= 0 && Record.KeyRingRoomUid <= 59;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"WanderingMonster",
				"DwLoopCounter",
				"WandCharges",
				"Regenerate",
				"KeyRingRoomUid",
			});
		}
	}
}
