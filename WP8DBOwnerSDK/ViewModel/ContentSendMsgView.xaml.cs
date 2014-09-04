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
    public partial class ContentSendMsgView : PhoneApplicationPage
    {
        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public ContentSendMsgView()
        {
            InitializeComponent();

            methodTitle.Text = "发送短信息";
            methodName.Text = "/content/send_msg";
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

        public void cancel_Click(object sender, RoutedEventArgs e)
        {
            DBOwner_accepter.Text = "";
            DBOwner_theme.Text = "";
            DBOwner_content.Text = "";
        }

        public void add_Click(object sender, RoutedEventArgs e)
        {
            string accepter = DBOwner_accepter.Text;
            string theme = DBOwner_theme.Text;
            string content = DBOwner_content.Text;

            if (string.IsNullOrEmpty(accepter))
            {
                MessageBox.Show("接收人不能为空");
            }
            else if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("内容不能为空");
            }
            else
            {
                ///发送短信息
                List<APIParameter> param = new List<APIParameter>();
                param.Add(new APIParameter("accepter", accepter));
                param.Add(new APIParameter("theme", theme));
                param.Add(new APIParameter("content", content));
                dbOwnerSDK.DBOwnerAuthContentSendMsg(param, DBOwner_ContentSendMsgCompletedHandler);

                DBOwner_accepter.Text = "";
                DBOwner_theme.Text = "";
                DBOwner_content.Text = "";
            }
        }

        /// <summary>
        /// 发送信息事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBOwner_ContentSendMsgCompletedHandler(object sender, AuthContentSendMsgCompletedEventArgs e)
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
                    MessageBox.Show("发送成功");
                }
                else
                {
                    MessageBox.Show(e.Result.msg);
                }
            }
        }
    }
}