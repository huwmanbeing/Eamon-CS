﻿
// IAnalyseAllRecordInterdependenciesMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IAnalyseAllRecordInterdependenciesMenu : IMenu
	{
		IAnalyseRecordInterdependenciesMenu01<IHaveUid>[] AnalyseMenus { get; set; }

		IList<IField> SkipFields { get; set; }

		bool ModifyFlag { get; set; }

		bool ExitFlag { get; set; }
	}
}
