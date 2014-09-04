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
using DBOwnerLib.Helper;
using DBOwnerLib.Events;
using WP8DBOwnerSDK.Model;

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class UsersFreshTokenView : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public UsersFreshTokenView()
        {
            InitializeComponent();

            methodTitle.Text = "当前登录者用户信息";
            methodName.Text = "/users/show";
            DBOwner_RefreshToken.Text = dbOwnerSDK.GetRefreshToken();
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

        public void Seach_Click(object sender, RoutedEventArgs e)
        {
            DBprogressBar.Visibility = System.Windows.Visibility.Visible;

            string refresh_token = DBOwner_RefreshToken.Text;

            if (string.IsNullOrEmpty(refresh_token))
            {
                MessageBox.Show("refresh_token不能为空");
            }
            else
            {
                ///刷新授权值
                List<APIParameter> param = new List<APIParameter>();
                param.Add(new APIParameter("refresh_token", refresh_token));
                dbOwnerSDK.DBOAuthUserRefreshToken(param, DBOwner_UserRefreshTokenCompletedHandler);
            }
        }

        private void DBOwner_UserRefreshTokenCompletedHandler(object sender, AuthUserRefreshTokenCompletedEventArgs e)
        {
            DBprogressBar.Visibility = System.Windows.Visibility.Collapsed;

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
                else if (e.Result.state == true)
                {
                    DBprogressBar.Visibility = System.Windows.Visibility.Collapsed;

                    List<AuthMethods> source = new List<AuthMethods>();
                    source.Add(new AuthMethods("access_token", e.Result.access_token));
                    source.Add(new AuthMethods("refresh_token", e.Result.refresh_token));
                    source.Add(new AuthMethods("user_id", e.Result.user_id));
                    DBOwner_data.ItemsSource = source;
                }
                else
                {
                    MessageBox.Show(e.Result.msg);
                }
            }
        }
    }
}