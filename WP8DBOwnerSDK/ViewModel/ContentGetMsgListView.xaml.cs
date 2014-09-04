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
using WP8DBOwnerSDK.Model;
using DBOwnerLib.Events;
using DBOwnerLib.Helper;

namespace WP8DBOwnerSDK.ViewModel
{
    public partial class ContentGetMsgListView : PhoneApplicationPage
    {

        DBOwnerSDK dbOwnerSDK = App.dbOwnerSDK;

        public ContentGetMsgListView()
        {
            InitializeComponent();
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ///取用户未读短信息列表 
            List<APIParameter> param = new List<APIParameter>();
            param.Add(new APIParameter("pagesize", "10000"));
            param.Add(new APIParameter("page", "1"));

            switch (this.NavigationContext.QueryString["type"])
            {
                case "new":
                    methodTitle.Text = "未读短信息列表 ";
                    methodName.Text = "/content/get_new_msg";

                    dbOwnerSDK.DBOwnerAuthContentNewMsgList(param, DBOwner_ContentNewMsgListCompletedHandler);
                    break;
                case "read":
                    methodTitle.Text = "已读短信息列表";
                    methodName.Text = "/content/get_read_msg";

                    dbOwnerSDK.DBOwnerAuthContentReadMsgList(param, DBOwner_ContentNewMsgListCompletedHandler);
                    break;
                case "send":
                    methodTitle.Text = "已发送信息列表";
                    methodName.Text = "/content/get_send_msg";

                    dbOwnerSDK.DBOwnerAuthContentSendMsgList(param, DBOwner_ContentNewMsgListCompletedHandler);
                    break;
                case "del":
                    methodTitle.Text = "已删除信息列表";
                    methodName.Text = "/content/get_del_msg";

                    dbOwnerSDK.DBOwnerAuthContentDelMsgList(param, DBOwner_ContentNewMsgListCompletedHandler);
                    break;
                default:
                    MessageBox.Show("方法不存在");
                    NavigationService.GoBack();
                    break;
            }
        }

        /// <summary>
        ///  请求列表返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBOwner_ContentNewMsgListCompletedHandler(object sender, AuthContentMsgListCompletedEventArgs e)
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
                    if (e.Result.data.count > 0)
                    {
                        List<AuthMsgList> source = new List<AuthMsgList>();

                        for (int i = 0; i < e.Result.data.count; i++)
                        {
                            source.Add(new AuthMsgList(
                                    e.Result.data.record[i].id,
                                    e.Result.data.record[i].name,
                                    e.Result.data.record[i].theme,
                                    e.Result.data.record[i].content,
                                    UnixTimestampConverter.ConvertUtcFromTimestamp(long.Parse(e.Result.data.record[i].appendtime)).ToLocalTime().ToString()
                                ));
                        }

                        DBOwner_data.ItemsSource = source;
                    }
                    else
                    {
                        MessageBox.Show("没有信息");
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