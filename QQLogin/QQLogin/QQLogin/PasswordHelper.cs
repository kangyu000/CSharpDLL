using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace QQLogin
{
    public static class PasswordHelper
    {
        public static string EncyptMD5_3_16(string s)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            byte[] buffer = mD.ComputeHash(bytes);
            byte[] buffer2 = mD.ComputeHash(buffer);
            byte[] array = mD.ComputeHash(buffer2);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString().ToUpper();
        }
        public static string smethod_0(string s)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString().ToUpper();
        }
        public static byte[] EncyptMD5Bytes(string s)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            return mD.ComputeHash(bytes);
        }
        public static string smethod_1(byte[] s)
        {
            MD5 mD = MD5.Create();
            byte[] array = mD.ComputeHash(s);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString().ToUpper();
        }
        public static string EncryptQQWebMd5(string s)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append("\\x");
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
        public static string EncryptOld(string password, string verifyCode)
        {
            return smethod_0(EncyptMD5_3_16(password) + verifyCode.ToUpper());
        }
        public static string GetPassword(string qq, string password, string verifyCode)
        {
            return Encrypt((long)Convert.ToInt64(qq), password, verifyCode);
        }
        public static string Encrypt(long qq, string password, string verifyCode)
        {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.PushByteArray(EncyptMD5Bytes(password));
            byteBuffer.PushInt(0);
            byteBuffer.PushInt((uint)qq);
            EncryptQQWebMd5(password);
            byte[] s = byteBuffer.ToByteArray();
            string str = smethod_1(s);
            return smethod_0(str + verifyCode.ToUpper());
        }

    }
}
