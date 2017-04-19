﻿
// InputArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class InputArgs : IInputArgs
	{
		public virtual bool EditRec { get; set; }

		public virtual bool EditField { get; set; }

		public virtual Enums.FieldDesc FieldDesc { get; set; }

		public virtual StringBuilder Buf { get; set; }

		public virtual IValidateArgs Vargs { get; set; }

		public InputArgs()
		{
			Buf = new StringBuilder(Constants.BufSize);

			Vargs = Globals.CreateInstance<IValidateArgs>();
		}
	}
}
