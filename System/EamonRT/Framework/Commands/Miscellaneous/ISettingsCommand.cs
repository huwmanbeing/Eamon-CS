﻿
// ISettingsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface ISettingsCommand : ICommand
	{
		/// <summary></summary>
		bool? VerboseRooms { get; set; }

		/// <summary></summary>
		bool? VerboseMonsters { get; set; }

		/// <summary></summary>
		bool? VerboseArtifacts { get; set; }

		/// <summary></summary>
		bool? MatureContent { get; set; }

		/// <summary></summary>
		long? PauseCombatMs { get; set; }
	}
}
