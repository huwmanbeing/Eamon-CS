﻿
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace StrongholdOfKahrDur.Framework
{
	/// <summary></summary>
	public interface IEngine : EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		/// <param name="cauldronArtifact"></param>
		/// <returns></returns>
		bool SpellReagentsInCauldron(IArtifact cauldronArtifact);
	}
}
