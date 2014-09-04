using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP8DBOwnerSDK.Model
{
    public class AuthMethods
    {
        public string title { get; set;}

        public string content { get; set;}

        public AuthMethods(string title, string content)
        {
            this.title = title;
            this.content = content;
        }
    }

    public class AuthAppList
    {
        public string AppID { get; set; }

        public string aName { get; set; }

        public string aIcoCode { get; set; }

        public string apppermissions { get; set; }

        public AuthAppList(string AppID, string aName, string aIcoCode, string apppermissions)
        {
            this.AppID = AppID;
            this.aName = aName;
            this.aIcoCode = aIcoCode;
            this.apppermissions = apppermissions;
        }
    }

    public class AuthMsgList
    {
        public string id { get; set; }

        public string name { get; set; }

        public string theme { get; set; }

        public string content { get; set; }

        public string time { get; set; }

        public AuthMsgList(string id, string name, string theme, string content, string time)
        {
            this.id = id;
            this.name = name;
            this.theme = theme;
            this.content = content;
            this.time = time;
        }
    }
}
