﻿
// IEditRecordOneFieldMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	public interface IEditRecordOneFieldMenu<T> : IRecordMenu<T> where T : class, IHaveUid
	{
		T EditRecord { get; set; }

		IField EditField { get; set; }
	}
}
