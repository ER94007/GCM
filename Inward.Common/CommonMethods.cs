//using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Inward.Common
{
    public static class CommonMethods
    {
        public enum ResponseMsgType
        {
            success = 1,
            error = 2,
            warning = 3,
            info = 4
        }
        public enum exceptionlogfrom
        {
            WEB = 1,
            API = 2,
        }
        public enum APIMethod
        {
            GET = 1,
            POST = 2,
        }
        public enum PermissionConstant
        {
            //IsNone = 0,
            //IsInsert = 1,
            //IsUpdate = 2,
            //IsDelete = 3,
            //IsView = 4,

            Admin = -1,
            StateLevel = 1,
            DistrictLevel = 2,
            TalukaLevel = 3,
            BlockLevel = 4,
            SejaLevel = 5,
            VillageLevel = 6,
            AnaganwadiLevel = 7,
        }

        //Status Code for Api Response
        public enum StatusCode
        {
            Bad_Request = 400,
            Sucess = 200,
            Internal_Server_Error = 500,
            Not_Found = 404,
            Unauthorized = 401
        }

        public enum Status
        {
            [Description("Success")]
            Success = 1,
            [Description("Not Found")]
            Not_Found = 2,
            [Description("Fail")]
            Fail = 3,
            [Description("Unauthorized")]
            Unauthorized = 4

        }

        public enum Message
        {

            [Description("Token Generated Successfully")]
            Token_Success = 3,

            [Description("User Successfully Login")]
            User_Login = 4,

            [Description("Invalid Credentials")]
            Login_Failed = 5,

            [Description("Inserted Successfully")]
            Employee_Reservation = 6,

            [Description("Not Inserted")]
            Employee_Reservation_Failed = 7,

            [Description("Employee Listed")]
            Employee_List = 8,

            [Description("Employee Listed Failed")]
            Employee_List_Failed = 7,
        }

        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description ?? string.Empty;
        }
        // Return IP address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        // Set temp data to necessary type
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        // Get temp data to necessary type
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }

        // convert list to datatable
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

     
        public static string ConcatString(string strtext1, string strtext2, string seperator)
        {
            return string.Concat(strtext1, seperator, strtext2);

        }
    }
}
