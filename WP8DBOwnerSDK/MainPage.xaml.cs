using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP8DBOwnerSDK.Resources;
using DBOwnerLib;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using DBOwnerLib.Events;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using DBOwnerLib.Services;

namespace WP8DBOwnerSDK
{
    public partial class MainPage : PhoneApplicationPage
    {
        //声明参数
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            CreateTileContent();

            dbOwnerSDK.DBOwnerPushViewPage("MainPage");
        }


        /// 更新Titl内容
        private void  CreateTileContent () {
            // 创建应用程序 Tile 的 Demo
            ShellTile applicationTile = ShellTile.ActiveTiles.First();

            StandardTileData newTile = new StandardTileData
            {
                Title = "",
                BackTitle = "DBOwnerSDK WP8",
                BackContent = "DBOwnerSDK集Auth登录、信息推送、广告展示。",
                BackgroundImage = new Uri("/Assets/DBOwner336X336.png", UriKind.Relative),
                BackBackgroundImage = new Uri("/Assets/DBOwnerBG336X336.png", UriKind.Relative)
            };

            applicationTile.Update(newTile);
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auth_Click(object sender, RoutedEventArgs e) 
        {
            //ToastPrompts toastPrompt = new ToastPrompts();
            //toastPrompt.Show("aaaaabbbbb");
            
            if (dbOwnerSDK.IsAccessTokenValid())
            {
                NavigationService.Navigate(new Uri("/ViewModel/mainViewPage.xaml", UriKind.Relative));
            }
            else
            {
                dbOwnerSDK.DBOAuthLogin(this, DBOwner_LoginCompletedHandler);
            }
        }

        /// <summary>
        /// 登录成功后处理
        /// </summary>
        public void DBOwner_LoginCompletedHandler(object sender, AuthLoginCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                NavigationService.Navigate(new Uri("/ViewModel/MainView.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        /// <summary>
        /// 登录初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            ProgressIndicator progress = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true,
                Text = "Downloading details..."
            };
            SystemTray.SetProgressIndicator(this, progress);
            SystemTray.BackgroundColor = System.Windows.Media.Colors.Red;
            SystemTray.ForegroundColor = System.Windows.Media.Colors.Blue;
             */
           // SystemTray.IsVisible = false;
        }

        /// <summary>
        /// 横竖屏处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            /*
            // 如果是横向的  
            if (e.Orientation == PageOrientation.Landscape ||
                e.Orientation == PageOrientation.LandscapeLeft ||
                e.Orientation == PageOrientation.LandscapeRight)
            {
                LandscapeChangeEvent();
            }
            // 如果是纵向  
            else if (e.Orientation == PageOrientation.Portrait ||
                e.Orientation == PageOrientation.PortraitDown ||
                e.Orientation == PageOrientation.PortraitUp)
            {
                PortraitChangeEvent();
            }
            //默认纵向
            else
            {
                PortraitChangeEvent();
            } 
            */
        }

        /// <summary>
        /// 横屏改变页面元素
        /// </summary>
        private void LandscapeChangeEvent() 
        {
            ChangeBackgroundImage("DBOwnerBG_H.png");
        }

        /// <summary>
        /// 竖屏改变页面元素
        /// </summary>
        private void PortraitChangeEvent()
        {
            ChangeBackgroundImage("DBOwnerBG_V.png");
        }

        /// <summary>
        /// 改变背景图
        /// </summary>
        /// <param name="imgName"></param>
        private void ChangeBackgroundImage(string imgName)
        {
            // Create an ImageBrush.
            ImageBrush berriesBrush = new ImageBrush();
            berriesBrush.ImageSource =
                new BitmapImage(
                    new Uri(@"Assets\" + imgName, UriKind.Relative)
                );

            // Use the brush to paint the button's background.
            LayoutRoot.Background = berriesBrush;
        }
    }
}