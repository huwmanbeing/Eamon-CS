
// TextWriter.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework.Portability;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

namespace EamonPM.Game.Portability
{
	public class TextWriter : ITextWriter
	{
		protected virtual StringBuilder Buf { get; set; }

		protected virtual StringBuilder Buf01 { get; set; }

		public virtual bool EnableOutput { get; set; }

		public virtual bool ResolveUidMacros { get; set; }

		public virtual bool WordWrap { get; set; }

		public virtual bool Stdout { get; set; }

		public virtual Encoding Encoding
		{
			get
			{
				return Stdout ? Console.Out.Encoding : Console.Error.Encoding;
			}
		}

		public virtual bool CursorVisible
		{
			get
			{
				return Console.CursorVisible;
			}

			set
			{
				Console.CursorVisible = value;
			}
		}

		public virtual void SetCursorPosition(Coord coord)
		{
			Debug.Assert(coord != null);

			if (EnableOutput && Stdout)
			{
				Console.SetCursorPosition(coord.X, coord.Y);
			}
			else
			{
				// do nothing
			}
		}

		public virtual void SetWindowTitle(string title)
		{
			Debug.Assert(title != null);

			if (EnableOutput && Stdout)
			{
				Console.Title = title;
			}
			else
			{
				// do nothing
			}
		}

		public virtual void SetWindowSize(long width, long height)
		{
			if (EnableOutput && Stdout)
			{
				Console.SetWindowSize((int)width, (int)height);
			}
			else
			{
				// do nothing
			}
		}

		public virtual void SetBufferSize(long width, long height)
		{
			if (EnableOutput && Stdout)
			{
				Console.SetBufferSize((int)width, (int)height);
			}
			else
			{
				// do nothing
			}
		}

		public virtual Coord GetCursorPosition()
		{
			return new Coord()
			{
				X = Console.CursorLeft,
				Y = Console.CursorTop
			};
		}

		public virtual long GetLargestWindowWidth()
		{
			return Console.LargestWindowWidth;
		}

		public virtual long GetLargestWindowHeight()
		{
			return Console.LargestWindowHeight;
		}

		public virtual long GetWindowHeight()
		{
			return Console.WindowHeight;
		}

		public virtual long GetBufferHeight()
		{
			return Console.BufferHeight;
		}

		public virtual void Write(object value)
		{
			Write("{0}", value);
		}

		public virtual void Write(string value)
		{
			Write("{0}", value);
		}

		public virtual void Write(decimal value)
		{
			Write("{0}", value);
		}

		public virtual void Write(double value)
		{
			Write("{0}", value);
		}

		public virtual void Write(float value)
		{
			Write("{0}", value);
		}

		public virtual void Write(long value)
		{
			Write("{0}", value);
		}

		public virtual void Write(uint value)
		{
			Write("{0}", value);
		}

		public virtual void Write(int value)
		{
			Write("{0}", value);
		}

		public virtual void Write(bool value)
		{
			Write("{0}", value);
		}

		public virtual void Write(char[] buffer)
		{
			Debug.Assert(buffer != null);

			Write("{0}", buffer.ToString());
		}

		public virtual void Write(char value)
		{
			Write("{0}", value);
		}

		public virtual void Write(ulong value)
		{
			Write("{0}", value);
		}

		public virtual void Write(string format, object arg0)
		{
			Write(format, new object[] { arg0 });
		}

		public virtual void Write(string format, params object[] arg)
		{
			Debug.Assert(format != null);

			Buf.SetFormat(format, arg);

			if (ResolveUidMacros && Globals?.Engine != null)
			{
				Buf01.Clear();

				var rc = Globals.Engine.ResolveUidMacros(Buf.ToString(), Buf01, true, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Buf.SetFormat("{0}", Buf01);
			}

			if (WordWrap && Globals?.Engine != null)
			{
				Globals.Engine.WordWrap(Buf.ToString(), Buf);
			}

			if (EnableOutput)
			{
				if (Stdout)
				{
					Console.Out.Write("{0}", Buf);
				}
				else
				{
					Console.Error.Write("{0}", Buf);
				}
			}
		}

		public virtual void Write(string format, object arg0, object arg1)
		{
			Write(format, new object[] { arg0, arg1 });
		}

		public virtual void Write(char[] buffer, int index, int count)
		{
			Debug.Assert(buffer != null);

			Write("{0}", buffer.ToString().Substring(index, count));
		}

		public virtual void Write(string format, object arg0, object arg1, object arg2)
		{
			Write(format, new object[] { arg0, arg1, arg2 });
		}

		public virtual void WriteLine()
		{
			WriteLine("{0}", "");
		}

		public virtual void WriteLine(object value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(string value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(decimal value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(float value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(ulong value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(double value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(uint value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(int value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(bool value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(char[] buffer)
		{
			Debug.Assert(buffer != null);

			WriteLine("{0}", buffer.ToString());
		}

		public virtual void WriteLine(char value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(long value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(string format, object arg0)
		{
			WriteLine(format, new object[] { arg0 });
		}

		public virtual void WriteLine(string format, params object[] arg)
		{
			Debug.Assert(format != null);

			Buf.SetFormat(format, arg);

			if (ResolveUidMacros && Globals?.Engine != null)
			{
				Buf01.Clear();

				var rc = Globals.Engine.ResolveUidMacros(Buf.ToString(), Buf01, true, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Buf.SetFormat("{0}", Buf01);
			}

			if (WordWrap && Globals?.Engine != null)
			{
				Globals.Engine.WordWrap(Buf.ToString(), Buf);
			}

			if (EnableOutput)
			{
				if (Stdout)
				{
					Console.Out.WriteLine("{0}", Buf);
				}
				else
				{
					Console.Error.WriteLine("{0}", Buf);
				}
			}
		}

		public virtual void WriteLine(char[] buffer, int index, int count)
		{
			Debug.Assert(buffer != null);

			WriteLine("{0}", buffer.ToString().Substring(index, count));
		}

		public virtual void WriteLine(string format, object arg0, object arg1)
		{
			WriteLine(format, new object[] { arg0, arg1 });
		}

		public virtual void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			WriteLine(format, new object[] { arg0, arg1, arg2 });
		}

		public TextWriter()
		{
			Buf = new StringBuilder(Constants.BufSize);

			Buf01 = new StringBuilder(Constants.BufSize);

			EnableOutput = true;

			ResolveUidMacros = true;

			WordWrap = true;

			Stdout = true;
		}
	}
}
