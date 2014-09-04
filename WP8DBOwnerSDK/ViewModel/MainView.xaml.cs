using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DBOwnerLib;
using System.Windows.Media.Imaging;
using DBOwnerLib.Events;
using System.Windows.Media;
using WP8DBOwnerSDK.Model;
using System.Windows.Input;
using System.Diagnostics;

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class MainViewPage : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;
        String chooseNow = "";
        String name = "";

        public MainViewPage()
        {
            InitializeComponent();

            //取当前登录者的用户信息
            dbOwnerSDK.DBOAuthGetUserInfo(DBOwner_GetUserInfoCompletedHandler);

            dbOwnerSDK.DBOwnerPushViewPage("MainView");

            //dbOwnerSDK.addAdFullScreen();
        }

        private void load_envent(object sender, RoutedEventArgs e) 
        {
            headPortrait.Source = new BitmapImage(new Uri("/Assets/loading.gif", UriKind.RelativeOrAbsolute));
        }

        public void DBOwner_GetUserInfoCompletedHandler(object sender, AuthUsersCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                NavigationService.GoBack();
            }
            else
            {
                if (e.Result.error != null)
                {
                    MessageBox.Show("错误代码：" + e.Result.error);
                }
                else if (e.Result.status == "success")
                {
                    name = e.Result.data.name;
                    UserName.Text = e.Result.data.name;
                    UserEmail.Text = e.Result.data.location;
                    headPortrait.Source = new BitmapImage(new Uri(e.Result.data.ico.b, UriKind.RelativeOrAbsolute));

                    List<AuthMethods> source = new List<AuthMethods>();
                    source.Add(new AuthMethods("当前登录者用户信息", "/users/show"));
                    source.Add(new AuthMethods("退出登录", "/users/signout"));
                    source.Add(new AuthMethods("判断是否过期", "/users/istimeout"));
                    source.Add(new AuthMethods("刷新access_token", "/users/fresh_token"));
                    source.Add(new AuthMethods("返回已授权AppID列表", "/users/getapplist"));
                    source.Add(new AuthMethods("指定用户名用户信息", "/users/show_by_name"));
                    source.Add(new AuthMethods("指定用户user_id用户信息", "/users/show_by_userid"));
                    source.Add(new AuthMethods("发送短信息", "/content/send_msg"));
                    source.Add(new AuthMethods("获取新短信", "/content/get_new_msg"));
                    source.Add(new AuthMethods("获取已读短信", "/content/get_read_msg"));
                    source.Add(new AuthMethods("获取已发送短息", "/content/get_send_msg"));
                    source.Add(new AuthMethods("获取已删除短息", "/content/get_del_msg"));
                    source.Add(new AuthMethods("删除短信息", "/content/del_msg"));
                    DBOwner_way.ItemsSource = source;
                }
                else
                {
                    MessageBox.Show(e.Result.status);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DBprogressBar.Visibility = System.Windows.Visibility.Collapsed;
            chooseWrap.Visibility = System.Windows.Visibility.Visible;
            intrWrap.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void choose_Click(object sender, RoutedEventArgs e)
        {
            AuthMethods selectedPerson = ((sender as Button).DataContext as AuthMethods);

            chooseMethod.Text = selectedPerson.title;
            chooseNow = selectedPerson.content;

            DBprogressBar.Visibility = System.Windows.Visibility.Visible;
            chooseWrap.Visibility = System.Windows.Visibility.Collapsed;
            intrWrap.Visibility = System.Windows.Visibility.Visible;
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            switch (chooseNow)
            {
                case "/users/show":
                    NavigationService.Navigate(new Uri("/ViewModel/UsersShowView.xaml", UriKind.Relative));
                    break;
                case "/users/signout":
                    //退出当前登录状态
                    NavigationService.Navigate(new Uri("/ViewModel/UsersSignoutView.xaml", UriKind.Relative));
                    break;
                case "/users/istimeout":
                    //判断是否过期 
                    NavigationService.Navigate(new Uri("/ViewModel/UsersIstimeoutView.xaml", UriKind.Relative));
                    break;
                case "/users/fresh_token":
                    NavigationService.Navigate(new Uri("/ViewModel/UsersFreshTokenView.xaml", UriKind.Relative));
                    break;
                case "/users/getapplist":
                    NavigationService.Navigate(new Uri("/ViewModel/UsersGetapplistView.xaml", UriKind.Relative));
                    break;
                case "/users/show_by_name":
                    NavigationService.Navigate(new Uri("/ViewModel/UsersInfoByNameView.xaml?name=" + name, UriKind.Relative));
                    break;
                case "/users/show_by_userid":
                    NavigationService.Navigate(new Uri("/ViewModel/UsersInfoByUserID.xaml", UriKind.Relative));
                    break;
                case "/content/send_msg":
                    NavigationService.Navigate(new Uri("/ViewModel/ContentSendMsgView.xaml", UriKind.Relative));
                    break;
                case "/content/get_new_msg":
                    NavigationService.Navigate(new Uri("/ViewModel/ContentGetMsgListView.xaml?type=new", UriKind.Relative));
                    break;
                case "/content/get_read_msg":
                    NavigationService.Navigate(new Uri("/ViewModel/ContentGetMsgListView.xaml?type=read", UriKind.Relative));
                    break;
                case "/content/get_send_msg":
                    NavigationService.Navigate(new Uri("/ViewModel/ContentGetMsgListView.xaml?type=send", UriKind.Relative));
                    break;
                case "/content/get_del_msg":
                    NavigationService.Navigate(new Uri("/ViewModel/ContentGetMsgListView.xaml?type=del", UriKind.Relative));
                    break;
                case "/content/del_msg":
                    NavigationService.Navigate(new Uri("/ViewModel/ContentDelMsgView.xaml", UriKind.Relative));
                    break;
                default:
                    MessageBox.Show("请先选择方法，或您选择的方法不存在");
                    break;
            }
        }

        /// <summary>
        /// 全屏广告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdFullClick(object sender, RoutedEventArgs e)
        {
            dbOwnerSDK.addAdFullScreen(this);
        }

        /// <summary>
        /// 插屏广告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdInsertClick(object sender, RoutedEventArgs e)
        {
            dbOwnerSDK.addAdInsertScreen(this);
        }

        /// <summary>
        /// 横幅广告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdBannerUpClick(object sender, RoutedEventArgs e)
        {
            dbOwnerSDK.addAdBanner(this, "up");
        }

        /// <summary>
        /// 全屏广告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdBannerDownClick(object sender, RoutedEventArgs e)
        {
            dbOwnerSDK.addAdBanner(this, "down");
        }
    }
}