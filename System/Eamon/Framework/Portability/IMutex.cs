﻿
// IMutex.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Portability
{
	public interface IMutex
	{
		void CreateAndWaitOne();
	}
}
