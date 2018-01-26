﻿
// IInputArgs.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Args
{
	public interface IInputArgs
	{
		bool EditRec { get; set; }

		bool EditField { get; set; }

		Enums.FieldDesc FieldDesc { get; set; }

		StringBuilder Buf { get; set; }

		IValidateArgs Vargs { get; set; }
	}
}
