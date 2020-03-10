using Hamastar.Utility;
using HamastarMeetingHub;
using MeetingHubLibrary;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

public class App : Application
{
	private static class NativeMethods
	{
		[DllImport("user32.dll")]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);
	}

	internal enum WindowShowStyle : uint
	{
		Hide = 0u,
		ShowNormal = 1u,
		ShowMinimized = 2u,
		ShowMaximized = 3u,
		Maximize = 3u,
		ShowNormalNoActivate = 4u,
		Show = 5u,
		Minimize = 6u,
		ShowMinNoActivate = 7u,
		ShowNoActivate = 8u,
		Restore = 9u,
		ShowDefault = 10u,
		ForceMinimized = 11u
	}

	private static readonly Semaphore singleInstanceWatcher;

	private static readonly bool createdNew;

	private bool _contentLoaded;

	static App()
	{
		HamastarMeetingHub.Config config = new HamastarMeetingHub.Config();
		if (!MeetingHubLibrary.Config.IsSingleInstanceEnable)
		{
			return;
		}
		singleInstanceWatcher = new Semaphore(0, 1, Assembly.GetExecutingAssembly().GetName().Name, out createdNew);
		if (createdNew)
		{
			return;
		}
		Process currentProcess = Process.GetCurrentProcess();
		Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
		foreach (Process process in processesByName)
		{
			if (process.Id != currentProcess.Id)
			{
				NativeMethods.SetForegroundWindow(process.MainWindowHandle);
				NativeMethods.ShowWindow(process.MainWindowHandle, WindowShowStyle.Restore);
				break;
			}
		}
		Environment.Exit(-2);
	}

	public App()
	{
		base.DispatcherUnhandledException += OnDispatcherUnhandledException;
	}

	private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
	{
		Utilities.AppLog.ErrorFormat("Occour UnhandledException: {0}", dispatcherUnhandledExceptionEventArgs.Exception);
		dispatcherUnhandledExceptionEventArgs.Handled = true;
	}
