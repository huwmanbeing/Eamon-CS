﻿
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheSubAquanLaboratory.Game.Helpers
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

		protected virtual bool ValidateFoodButtonPushes()
		{
			return Record.FoodButtonPushes >= 0 && Record.FoodButtonPushes <= 2;
		}

		protected virtual bool ValidateFlood()
		{
			return Record.Flood >= 0 && Record.Flood <= 2;
		}

		protected virtual bool ValidateFloodLevel()
		{
			return Record.FloodLevel >= 0 && Record.FloodLevel <= 11;
		}

		protected virtual bool ValidateElevation()
		{
			return Record.Elevation >= 0 && Record.Elevation <= 4;
		}

		protected virtual bool ValidateEnergyMaceCharge()
		{
			return Record.EnergyMaceCharge >= 0 && Record.EnergyMaceCharge <= 120;
		}

		protected virtual bool ValidateLaserScalpelCharge()
		{
			return Record.LaserScalpelCharge >= 0 && Record.LaserScalpelCharge <= 40;
		}

		protected virtual bool ValidateQuestValue()
		{
			return Record.QuestValue >= 0 && Record.QuestValue <= 1250;
		}

		protected virtual bool ValidateFakeWallExamines()
		{
			return Record.FakeWallExamines >= 0 && Record.FakeWallExamines <= 2;
		}

		protected virtual bool ValidateLabRoomsSeen()
		{
			return Record.LabRoomsSeen >= 0 && Record.LabRoomsSeen <= 45;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"FoodButtonPushes",
				"Flood",
				"FloodLevel",
				"Elevation",
				"EnergyMaceCharge",
				"LaserScalpelCharge",
				"QuestValue",
				"FakeWallExamines",
				"LabRoomsSeen",
			});
		}
	}
}
