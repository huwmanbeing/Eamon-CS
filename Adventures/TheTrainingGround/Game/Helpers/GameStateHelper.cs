﻿
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheTrainingGround.Game.Helpers
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

		protected virtual bool ValidateGenderChangeCounter()
		{
			return Record.GenderChangeCounter >= 0 && Record.GenderChangeCounter <= 2;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"GenderChangeCounter",
			});
		}
	}
}
