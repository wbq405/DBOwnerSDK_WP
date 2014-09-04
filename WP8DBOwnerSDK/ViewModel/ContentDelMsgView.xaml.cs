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

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class ContentDelMsgView : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public ContentDelMsgView()
        {
            InitializeComponent();

            methodTitle.Text = "删除短信息";
            methodName.Text = "/content/del_msg";
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

        public void del_Click(object sender, RoutedEventArgs e)
        {
            string id = DBOwner_id.Text;
            
            int type = (type1.IsChecked == true) ? 1 : 2;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("id不能为空");
            }
            else
            {
                ///发送短信息
                List<APIParameter> param = new List<APIParameter>();
                param.Add(new APIParameter("id", id));
                param.Add(new APIParameter("type", type.ToString()));
                dbOwnerSDK.DBOwnerAuthContentDelMsg(param, DBOwner_ContentDelMsgListCompletedHandler);

                DBOwner_id.Text = "";
            }
        }

        /// <summary>
        /// 删除事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBOwner_ContentDelMsgListCompletedHandler(object sender, AuthContentDelMsgCompletedEventArgs e)
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
                    if (e.Result.data.ident > 0)
                    {
                        MessageBox.Show("成功删除");
                    }
                    else
                    {
                        MessageBox.Show("删除失败");
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