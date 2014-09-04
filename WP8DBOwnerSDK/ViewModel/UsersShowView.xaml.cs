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
using DBOwnerLib.Events;
using System.Windows.Media.Imaging;
using WP8DBOwnerSDK.Model;

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class UsersShowView : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public UsersShowView()
        {
            InitializeComponent();

            methodTitle.Text = "当前登录者用户信息";
            methodName.Text = "/users/show";

            //获取当前登录者信息
            dbOwnerSDK.DBOAuthGetUserInfo(DBOwner_GetUserInfoCompletedHandler);
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// 获取当前登录者信息回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    DBprogressBar.Visibility = System.Windows.Visibility.Collapsed;

                    List<AuthMethods> source = new List<AuthMethods>();
                    source.Add(new AuthMethods("用户名", e.Result.data.name));
                    source.Add(new AuthMethods("邮箱", e.Result.data.email));
                    source.Add(new AuthMethods("用户UserID", e.Result.data.id));
                    source.Add(new AuthMethods("用户主页", e.Result.data.account));
                    source.Add(new AuthMethods("退出登录", e.Result.data.signout));
                    source.Add(new AuthMethods("头像大", e.Result.data.ico.b));
                    source.Add(new AuthMethods("头像中", e.Result.data.ico.m));
                    source.Add(new AuthMethods("头像小", e.Result.data.ico.s));
                    source.Add(new AuthMethods("地址", e.Result.data.location));
                    source.Add(new AuthMethods("性别", (System.Convert.ToInt32(e.Result.data.sex) == 0) ? "男" : "女"));
                    DBOwner_data.ItemsSource = source;
                }
                else
                {
                    MessageBox.Show(e.Result.status);
                }
            }
        }
    }
}