using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Prototype.Providers
{
    public static class Extension
    {
        // New Option 03/2016

        static Regex regex = new Regex(@"\p{Lu}\p{Ll}*");
        public static bool IsOnCapitals(this string _text)
        {
            return regex.IsMatch(_text);
        }
        public static IEnumerable<string> SplitOnCapitals(this string _text)
        {
            foreach (Match match in regex.Matches(_text))
            {
                yield return match.Value;
            }
        }

        public static string UpperFirst(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string IPClient
        {
            get
            {
                var clientIp = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(2).ToString();
                return clientIp;
            }
        }
        public static string IPAddress
        {
            get
            {
                foreach (System.Net.IPAddress _address in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
                {
                    if (_address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return _address.ToString();
                    }
                }
                return string.Empty;
            }
        }

        public static int GetDayEndOfMonth(int _year, int _month)
        {
            if (_month == 2)
            {
                return ((_year % 4) == 0) ? 29 : 28;
            }
            else if (_month == 4 || _month == 6 || _month == 9 || _month == 11)
            {
                return 30;
            }
            else
            {
                return 31;
            }
        }

        public static string GetPasswordMD5(this string pwd)
        {
            string epassword = null;
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(pwd);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            epassword = s.ToString();
            return epassword;
        }

        public static string EncryptMD5(this string _password)
        {
            return _password;
        }

        public static string GenerateRunning(string PreFix, int LastRunning, int LengthCode)
        {
            //Method สำหรับสร้างเลขที่รายการ

            int add_number = LastRunning + 1;
            string new_numlist = PreFix + add_number.ToString().PadLeft(LengthCode,'0');  //'000' ถ้าตัวเลขที่แปลงมีค่าไม่ถึงหลัก จะเติม 0 ไว้ด้านหน้าแทนตัวเลข

            return new_numlist;
        }

        public static int GenerateRunning(int? LastRunning)
        {
            //Method สำหรับสร้างเลขที่รายการ

            if (LastRunning.HasValue)
            {
                LastRunning++;
                return LastRunning.Value;
            }
            else
            {
                return 1;
            }
        }

        public static string SubStringOutOfLength(this string _strValue, int maxLength)
        {
            string val = _strValue;

            if ((val != null) && (val != string.Empty) && (val.Length > maxLength))
            {
                val = _strValue.Trim().Substring(0, maxLength);
            }

            return val;
        }

        public static string SplitZeroDegit(decimal textNumber)
        {
            // # คือถ้าตัวเลขมีค่าก็จะโชว์ค่าตัวเลขถ้าตัวหลังมีค่าเป้น 0 ก็จะไม่แสดง ค่า เพราะ 0 ตัวหลังสุดไม่มีค่า
            var value = textNumber.ToString("#,##0.######");
            return value;
        }

        public static string AddGuot(this string _string)
        {
            return "\"" + _string + "\"";

        }

        public static string AddSpace(this string _string)
        {
            string value = _string;
            char[] array = _string.ToCharArray();

            for (int i = 0; i < array.Length - 1; i++)
            {
                if (i > 0)
                {
                    if (char.IsUpper((char)array[i]))
                    {
                        value = value.Insert(i, " ");
                        i++;
                    }
                }
            }

            return value;
        }

        public static string ToHashed(this string _string)
        {
            Byte[] passwordByte = Encoding.Unicode.GetBytes(_string);
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            Byte[] passwordHash = sha1.ComputeHash(passwordByte);
            return Convert.ToBase64String(passwordHash);

        }

        public static bool NotNull(this object _object)
        {
            return _object.IsNull().Inverse();
        }
        public static bool NotNull(this string _object)
        {
            return !string.IsNullOrEmpty(_object);
        }

        public static bool IsNull(this object _object)
        {
            if (_object == null)
                return true;
            if (_object.GetType() == typeof(System.DBNull))
                return true;
            return false;
        }

        public static bool IsEmpty(this object _object)
        {
            if (_object.IsNull())
                return true;
            else if (_object.ToString().Trim() == string.Empty)
                return true;
            else
                return false;
        }
        public static bool NotEmpty(this object _object)
        {
            return _object.IsEmpty().Inverse();
        }

        public static bool IsNumber(this object _object)
        {
            try
            {
                decimal _val = Convert.ToDecimal(_object);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool NotNumber(this object _object)
        {
            return _object.IsNumber().Inverse();
        }

        public static bool Inverse(this bool _value)
        {
            return !_value;
        }

        public static DateTime? GetDateOnly(this DateTime? _datetime)
        {
            if (_datetime.IsNull())
                return null;
            return _datetime.Value.Date;
        }
        public static DateTime? GetTimeOnly(this DateTime? _datetime)
        {
            if (_datetime.IsNull())
                return null;
            return new DateTime(1900, 1, 1, _datetime.Value.Hour, _datetime.Value.Minute, 0);
        }
        public static DateTime? ToDateTime(this object _object)
        {
            try
            {
                return Convert.ToDateTime(_object);
            }
            catch
            {
                return null;
            }
        }

        public static decimal ToDecimal(this object _object)
        {
            try
            {
                return Convert.ToDecimal(_object);
            }
            catch
            {
                return 0;
            }
        }
        public static int ToInteger(this object _object)
        {
            try
            {
                return Convert.ToInt32(_object);
            }
            catch
            {
                return 0;
            }
        }

        public static bool ToBoolean(this object _object)
        {
            try
            {
                return Convert.ToBoolean(_object);
            }
            catch
            {
                return false;
            }
        }
        public static bool IsBoolean(this object _object)
        {
            try
            {
                var _bool = Convert.ToBoolean(_object);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool NotBoolean(this object _object)
        {
            return !_object.IsBoolean();
        }
    }
}
