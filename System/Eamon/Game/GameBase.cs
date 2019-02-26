﻿
// GameBase.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Game
{
	public abstract class GameBase : IGameBase
	{
		#region Public Properties

		#region Interface IGameBase

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		public virtual string Name { get; set; }

		public virtual string Desc { get; set; }

		public virtual string[] Synonyms { get; set; }

		public virtual bool Seen { get; set; }

		public virtual ArticleType ArticleType { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public abstract void Dispose(bool disposing);

		public void Dispose()      // virtual intentionally omitted
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Interface IGameBase

		public virtual void SetParentReferences()
		{
			// do nothing
		}

		public virtual string GetPluralName(string fieldName, StringBuilder buf)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName) || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			buf.Clear();

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetPluralName01(StringBuilder buf)
		{
			return GetPluralName("Name", buf);
		}

		public virtual string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName) || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			buf.Clear();

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName("Name", ArticleType.None, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName("Name", ArticleType, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName("Name", ArticleType.The, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf.Clear();

		Cleanup:

			return rc;
		}

		#endregion

		#region Class GameBase

		public GameBase()
		{
			IsUidRecycled = true;

			Name = "";

			Desc = "";
		}

		#endregion

		#endregion
	}
}
