using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DBOwnerLib.Events;
using DBOwnerLib;

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class UsersSignoutView : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public UsersSignoutView()
        {
            InitializeComponent();

            methodTitle.Text = "退出当前登录";
            methodName.Text = "/users/signout";
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// 退出返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //退出当前登录
            dbOwnerSDK.DBAuthUserSigout(DBOwner_UserSigoutCompletedHandler);
        }

        /// <summary>
        /// 退出返回回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBOwner_UserSigoutCompletedHandler(object sender, AuthUserSigoutCompletedEventArgs e)
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
                    MessageBox.Show(e.Result.error);
                }
                else if (e.Result.state == true)
                {
                    //MessageBox.Show(e.Result.status);
                    MessageBox.Show("成功退出");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show(e.Result.msg);
                }
            }
        }
    }
}