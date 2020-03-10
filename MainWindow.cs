using Hamastar.DefaultConfig;
using Hamastar.DefaultEnum;
using Hamastar.Navigation;
using Hamastar.NavigationHandler;
using Hamastar.SilverlightBook;
using Hamastar.Utility;
using Hamastar.Utility.Culture;
using Hamastar.ViewCompoments;
using MeetingHubLibrary;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

public class MainWindow : Window, IComponentConnector
{
	private Timer LaunchTimer;

	private PopUpDialog PopUpDialog;

	private bool _contentLoaded;

	public MainWindow()
	{
		InitializeComponent();
		base.WindowStyle = WindowStyle.None;
		base.Left = SystemParameters.PrimaryScreenWidth / 2.0;
		base.Top = SystemParameters.PrimaryScreenHeight / 2.0;
		base.Width = 0.0;
		base.Height = 0.0;
		base.Opacity = 0.0;
		if (MeetingHubLibrary.Config.EnableAppExpireCheck && DateTime.Compare(DateTime.Now, MeetingHubLibrary.Config.ExpireDateTime) == 1)
		{
			MessageBox.Show("使用期限已過");
			Application.Current.Shutdown();
		}
		base.Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		Image image = Utilities.LoadGifImage($"pack://application:,,,/MeetingHubLibrary;Component/Images/Project/Launch/{Hamastar.DefaultConfig.Config.Language}/loading.gif");
		image.Width = 400.0;
		image.Height = 300.0;
		PopUpDialog = new PopUpDialog(image);
		PopUpDialog.Left = SystemParameters.PrimaryScreenWidth / 2.0 - 200.0;
		PopUpDialog.Top = SystemParameters.PrimaryScreenHeight / 2.0 - 150.0;
		PopUpDialog.Show();
		LaunchTimer = new Timer(Launch, null, 300, 300);
		base.Closing += OnClosing;
	}

	private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
	{
		if (NavigationController.Controller.ContainsViewController(typeof(SilverlightViewController)))
		{
			NotificationCenter.defaultCenter.postNotification(this, "ExitBook", null);
			cancelEventArgs.Cancel = true;
		}
	}

	private void Launch(object state)
	{
		LaunchTimer.Dispose();
		base.Dispatcher.BeginInvoke((Action)delegate
		{
			try
			{
				CulturesHelper.CurrentCulture = Hamastar.DefaultConfig.Config.Language.ToCultureInfo();
				CulturesHelper.ChangedCulterHandler = (EventHandler)Delegate.Combine(CulturesHelper.ChangedCulterHandler, new EventHandler(ChangedCulterHandler));
				CulturesHelper culturesHelper = new CulturesHelper();
			}
			catch (Exception ex)
			{
				Utilities.AppLog.Error("Load culturesHelper occour error:" + ex.StackTrace);
			}
		});
	}

	private void ChangedCulterHandler(object sender, EventArgs eventArgs)
	{
		CulturesHelper.ChangedCulterHandler = (EventHandler)Delegate.Remove(CulturesHelper.ChangedCulterHandler, new EventHandler(ChangedCulterHandler));
		Utilities.InitialLoger();
		Utilities.CopySqlite(Hamastar.DefaultConfig.Config.RootDirectory);
		NavigationHandler handler = NavigationHandler.Handler;
		base.Title = $"{Hamastar.DefaultConfig.Config.ProjectName}  v{Hamastar.DefaultConfig.Config.AppVersion:F1}";
		base.Icon = new BitmapImage(new Uri("pack://application:,,,/MeetingHubLibrary;Component/Images/Project/Icon/" + MeetingHubLibrary.Config.IconName, UriKind.RelativeOrAbsolute));
		NavigationController.Controller.ReturnButtonText = "返回";
		MainViewController viewController = new MainViewController();
		NavigationController.Controller.PushViewController(viewController);
		base.Left = SystemParameters.PrimaryScreenWidth / 2.0 - 512.0;
		base.Top = SystemParameters.PrimaryScreenHeight / 2.0 - 384.0;
		base.Width = 1024.0;
		base.Height = 768.0;
		base.Opacity = 1.0;
		base.WindowStyle = WindowStyle.SingleBorderWindow;
		base.WindowState = WindowState.Maximized;
		PopUpDialog.Close();
	}
