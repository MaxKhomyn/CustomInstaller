using System;
using System.Diagnostics;
using System.Text;

namespace WIXInstaller.Model.Installation
{
    public delegate void SetTitles();
    class Activation
    {
        static SetTitles Email { get; set; }
        static SetTitles ActivationKey { get; set; }

        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }


        public static bool Activate(string EMail, string Key, SetTitles email, SetTitles activationKey)
        {
            Email = email;
            ActivationKey = activationKey;
            return Activate(EMail, Key);
        }
        public static bool Activate(string EMail, string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                //MessageBox.Show("Активуйте свій продукт у налаштуваннях!");
                //ActivationKey();
                return false;
            }
            try
            {
                string[] stringTobyte = Key.Split('-');
                byte[] bytes = new byte[stringTobyte.Length];

                for (int i = 0; i < stringTobyte.Length; i++)
                {
                    string tmp = stringTobyte[i];
                    if (tmp[0] == '0') tmp = tmp.Substring(1, 2);

                    bytes[i] = Convert.ToByte(tmp);
                }

                var DecryptString = Encoding.UTF8.GetString(bytes);

                string[] DateEmail = DecryptString.Split('|');

                if (EMail != DateEmail[1])
                {
                    Email();
                    return false;
                }
                else if (!ValidateDate(DateEmail[0]))
                {
                    //MessageBox.Show("У вас вийшов термін придатності користування програмою. Цей ключ не є дійсним!");
                    //ActivationKey();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //ActivationKey();
                //MessageBox.Show("Проблеми з активацією продукту!");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        static bool ValidateDate(string date)
        {
            string[] LastDate = date.Split('/');

            string Day = DateTime.Now.ToString("dd");
            string Month = DateTime.Now.ToString("MM");
            string Year = DateTime.Now.ToString("yy");

            int NowDays = GetNumberOfDays(int.Parse(Year), int.Parse(Month), int.Parse(Day));
            int LastDays = GetNumberOfDays(int.Parse(LastDate[2]), int.Parse(LastDate[1]), int.Parse(LastDate[0]));

            if ((NowDays - LastDays) <= 1) return true;//Чи не вийшов термін ліцензії

            return false;
        }

        public static string Encrypting(string Text)
        {
            var bytes = Encoding.UTF8.GetBytes(Text);
            string EncryptString = bytes[0].ToString();
            string oneByte = "";

            for (int i = 1; i < bytes.Length; i++)
            {
                oneByte = bytes[i].ToString();

                if (oneByte.Length < 3) oneByte = "0" + oneByte;
                EncryptString += "-" + oneByte;
            }
            return EncryptString;
        }
        public static string Decrypting(string Key)
        {
            string[] stringTobyte = Key.Split('-');
            byte[] bytes = new byte[stringTobyte.Length];

            for (int i = 0; i < stringTobyte.Length; i++)
            {
                string tmp = stringTobyte[i];
                if (tmp[0] == '0') tmp = tmp.Substring(1, 2);

                bytes[i] = Convert.ToByte(tmp);
            }

            var DecryptString = Encoding.UTF8.GetString(bytes);
            return DecryptString;
        }

        static int GetNumberOfDays(int year, int month, int day)
        {
            int iterator = year * 365;
            for (int i = 1; i <= month; i++)
            {
                iterator += GetNumberOfDaysInMonth(i);
            }
            iterator += day;
            return iterator;
        }
        static int GetNumberOfDaysInMonth(int Month)
        {
            return 28 + (Month + Month / 8) % 2 + 2 % Month + 1 / Month * 2;
        }
    }
}