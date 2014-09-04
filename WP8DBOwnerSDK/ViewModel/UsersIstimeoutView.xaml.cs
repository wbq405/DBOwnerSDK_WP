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

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class UsersIstimeoutView : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public UsersIstimeoutView()
        {
            InitializeComponent();

            methodTitle.Text = "登录是否过期";
            methodName.Text = "/users/istimeout";
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
        /// 判断是否过期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //判读是否过期
            dbOwnerSDK.DBOAuthGetUserIstimeout(DBOwner_UserIstimeoutCompletedHandler);
        }

        /// <summary>
        /// 判断是否过期回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBOwner_UserIstimeoutCompletedHandler(object sender, AuthUserIstimeoutCompletedEventArgs e)
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
                    //MessageBox.Show(e.Result.id);
                    if (int.Parse(e.Result.id) > 0)
                    {
                        MessageBox.Show("正常使用");
                    }
                    else
                    {
                        MessageBox.Show("已经过期");
                    }
                }
                else
                {
                    MessageBox.Show(e.Result.msg);
                }
            }
        }
    }
}