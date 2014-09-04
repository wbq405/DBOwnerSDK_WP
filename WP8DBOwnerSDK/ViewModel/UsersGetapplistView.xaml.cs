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
using DBOwnerLib.Model;
using DBOwnerLib.Events;
using System.Collections.ObjectModel;
using WP8DBOwnerSDK.Model;

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class UsersGetapplistView : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public UsersGetapplistView()
        {
            InitializeComponent();

            methodTitle.Text = "授权的应用列表";
            methodName.Text = "/users/getapplist";

            dbOwnerSDK.DBOwnerAuthUserGetapplist(DBOwner_UserGetapplistCompletedHandler);
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

        private void DBOwner_UserGetapplistCompletedHandler(object sender, AuthUserGetapplistCompletedEventArgs e)
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
                    if (e.Result.list.Count > 0)
                    {
                        List<AuthAppList> source = new List<AuthAppList>();

                        for (int i = 0; i < int.Parse(e.Result.list.Count.ToString()); i++)
                        {
                            source.Add(new AuthAppList(
                                    e.Result.list[i].AppID,
                                    e.Result.list[i].aName,
                                    e.Result.list[i].aIcoCode,
                                    e.Result.list[i].apppermissions
                                ));
                        }

                        DBprogressBar.Visibility = System.Windows.Visibility.Collapsed;

                        DBOwner_data.ItemsSource = source;
                    }
                    else
                    {
                        MessageBox.Show("没有授权的应用");
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