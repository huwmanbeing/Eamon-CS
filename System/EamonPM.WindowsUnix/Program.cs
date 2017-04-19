﻿
// Program.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Eamon;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.PluginContext;
using static Eamon.Game.Plugin.PluginContextStack;

namespace EamonPM
{
	public class Program
	{
		public static string[] NextArgs { get; set; }

		protected static void LoadPortabilityClassMappings(IDictionary<Type, Type> classMappings)
		{
			Debug.Assert(classMappings != null);

			classMappings[typeof(ITextReader)] = typeof(Game.Portability.TextReader);

			classMappings[typeof(ITextWriter)] = typeof(Game.Portability.TextWriter);

			classMappings[typeof(IMutex)] = typeof(Game.Portability.Mutex);

			classMappings[typeof(ITransferProtocol)] = typeof(Game.Portability.TransferProtocol);

			classMappings[typeof(IDirectory)] = typeof(Game.Portability.Directory);

			classMappings[typeof(IFile)] = typeof(Game.Portability.File);

			classMappings[typeof(IPath)] = typeof(Game.Portability.Path);

			classMappings[typeof(ISharpSerializer)] = typeof(Game.Portability.SharpSerializer);

			classMappings[typeof(IThread)] = typeof(Game.Portability.Thread);
		}

		public static void ExecutePlugin(string[] args, bool enableStdio = true)
		{
			Debug.Assert(args != null);

			Debug.Assert(args.Length > 1);

			Debug.Assert(string.Equals(args[0], "-pfn", StringComparison.OrdinalIgnoreCase));

			var pluginFileName = System.IO.Path.GetFileNameWithoutExtension(args[1]);

			var plugin = Assembly.Load(pluginFileName);

			Debug.Assert(plugin != null);

			var typeName = string.Format("{0}.Program", pluginFileName);

			var type = plugin.GetType(typeName);

			Debug.Assert(type != null);

			var program = (IProgram)Activator.CreateInstance(type);

			Debug.Assert(program != null);

			program.EnableStdio = enableStdio;

			program.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			program.Main(args.Skip(2).ToArray());
		}

		public static void Main(string[] args)
		{
			RetCode rc;

			try
			{
				rc = RetCode.Success;

				PushConstants();

				PushClassMappings();

				ClassMappings.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

				ClassMappings.ResolvePortabilityClassMappings();

				if (args == null || args.Length < 2 || !string.Equals(args[0], "-pfn", StringComparison.OrdinalIgnoreCase))
				{
					rc = RetCode.InvalidArg;

					ClassMappings.Error.WriteLine("{0}Usage: EamonPM.WindowsUnix.exe -pfn PluginFileName [PluginArgs]", Environment.NewLine);

					goto Cleanup;
				}

				try
				{
					while (true)
					{
						if (args == null || args.Length < 2 || !string.Equals(args[0], "-pfn", StringComparison.OrdinalIgnoreCase))
						{
							goto Cleanup;
						}

						ExecutePlugin(args);

						args = NextArgs;

						NextArgs = null;
					}
				}
				catch (Exception ex)
				{
					rc = RetCode.Failure;

					ClassMappings.HandleException
					(
						ex,
						Constants.StackTraceFile,
						string.Format("{0}Error: Caught fatal exception; terminating program", Environment.NewLine)
					);

					goto Cleanup;
				}

			Cleanup:

				if (rc != RetCode.Success)
				{
					ClassMappings.Error.WriteLine("{0}{1}", Environment.NewLine, new string('-', (int)Constants.RightMargin));

					ClassMappings.Error.Write("{0}Press any key to continue: ", Environment.NewLine);

					ClassMappings.In.ReadKey(true);

					ClassMappings.Error.WriteLine();

					ClassMappings.Thread.Sleep(150);
				}

				ClassMappings.Out.CursorVisible = true;
			}
			catch (Exception)
			{
				rc = RetCode.Failure;

				// do nothing
			}
			finally
			{
				PopClassMappings();

				PopConstants();
			}

			Environment.Exit(rc == RetCode.Success ? 0 : -1);
		}
	}
}
