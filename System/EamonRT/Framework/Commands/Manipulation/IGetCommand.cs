﻿
// IGetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IGetCommand : ICommand
	{
		/// <summary></summary>
		bool GetAll { get; set; }

		/// <summary></summary>
		bool OmitWeightCheck { get; set; }
	}
}
