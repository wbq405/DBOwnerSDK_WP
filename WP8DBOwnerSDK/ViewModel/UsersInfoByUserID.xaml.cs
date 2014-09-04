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
    public partial class UsersInfoByUserID : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public UsersInfoByUserID()
        {
            InitializeComponent();

            methodTitle.Text = "查询指定user_id用户信息";
            methodName.Text = "/users/show_by_show_by_userid";
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DBOwner_UserID.Text = dbOwnerSDK.GetUserID();
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
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Seach_Click(object sender, RoutedEventArgs e)
        {
            DBprogressBar.Visibility = System.Windows.Visibility.Visible;

            string user_id = DBOwner_UserID.Text;

            if (string.IsNullOrEmpty(user_id))
            {
                MessageBox.Show("user_id不能为空");
            }
            else
            {
                //取当前登录者的用户信息
                List<APIParameter> param = new List<APIParameter>();
                param.Add(new APIParameter("user_id", user_id));
                dbOwnerSDK.DBOwnerAuthGetUserInfoByID(param, DBOwner_GetUserInfoCompletedHandler);
            }
        }

        /// <summary>
        /// 指定用户名查询用户信息处理
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
                else if (e.Result.state == true)
                {

                    DBprogressBar.Visibility = System.Windows.Visibility.Collapsed;

                    List<AuthMethods> source = new List<AuthMethods>();
                    source.Add(new AuthMethods("用户名", e.Result.data.name));
                    source.Add(new AuthMethods("邮箱", e.Result.data.email));
                    source.Add(new AuthMethods("用户UserID", e.Result.data.id));
                    source.Add(new AuthMethods("头像大", e.Result.data.ico.b));
                    source.Add(new AuthMethods("头像中", e.Result.data.ico.m));
                    source.Add(new AuthMethods("头像小", e.Result.data.ico.s));
                    source.Add(new AuthMethods("地址", e.Result.data.location));
                    source.Add(new AuthMethods("性别", (System.Convert.ToInt32(e.Result.data.sex) == 0) ? "男" : "女"));
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