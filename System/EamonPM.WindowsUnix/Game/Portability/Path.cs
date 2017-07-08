
// Path.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class Path : IPath
	{
		public virtual char DirectorySeparatorChar
		{
			get
			{
				return System.IO.Path.DirectorySeparatorChar;
			}
		}

		protected virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}

		public virtual bool EqualPaths(string path1, string path2)
		{
			return string.Equals(NormalizePath(path1), NormalizePath(path2), System.IO.Path.DirectorySeparatorChar == '\\' ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		}

		public virtual string Combine(string path1, string path2)
		{
			return System.IO.Path.Combine(NormalizePath(path1), NormalizePath(path2));
		}

		public virtual string GetDirectoryName(string path)
		{
			return System.IO.Path.GetDirectoryName(NormalizePath(path));
		}

		public virtual string GetExtension(string path)
		{
			return System.IO.Path.GetExtension(NormalizePath(path));
		}

		public virtual string GetFileName(string path)
		{
			return System.IO.Path.GetFileName(NormalizePath(path));
		}

		public virtual string GetFileNameWithoutExtension(string path)
		{
			return System.IO.Path.GetFileNameWithoutExtension(NormalizePath(path));
		}
	}
}
