/***
 * 
 *    核心层： XML操作类
 *
 * 
 * 
 *    Description: 
 *    
 * 
 *    Date: 2015
 *    
 *    Version: 1.0
 *    
 *    Modify Recoder: 
 *   
 */
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

namespace kernel
{
    public class XmlOperation
    {
        private static XmlOperation _Instance = null;          //静态类对象

        /// <summary>
        /// 得到实例
        /// </summary>
        /// <returns></returns>
        public static XmlOperation GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new XmlOperation();
            }
            return _Instance;
        }


        /// <summary>  加密方法
        /// 加密方法
        /// 
        /// 描述： 加密和解密采用相同的key,具体值自己填，但是必须为32位
        /// </summary>
        /// <param name="toE"></param>
        /// <returns></returns>
        public string Encrypt(string toE)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578902223367877723456789012");
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toE);
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>  解密方法
        /// 解密方法
        /// 
        /// 描述： 加密和解密采用相同的key,具体值自己填，但是必须为32位
        /// </summary>
        /// <param name="toD"></param>
        /// <returns></returns>
        public string Decrypt(string toD)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578902223367877723456789012");
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] toEncryptArray = Convert.FromBase64String(toD);
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary> 序列化对象
        /// 序列化对象
        /// </summary>
        /// <param name="pObject">进行序列化的对象</param>
        /// <param name="ty">序列化对象的类型</param>
        /// <returns></returns>
        public string SerializeObject(object pObject, System.Type ty)
        {
            string XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(ty);
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, pObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }

        /// <summary> 反序列化对象
        /// 反序列化对象
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public object DeserializeObject(string pXmlizedString, System.Type ty)
        {
            XmlSerializer xs = new XmlSerializer(ty);
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return xs.Deserialize(memoryStream);
        }

        /// <summary> 创建XML文件
        /// 创建XML文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="strFileData">写入的文件数据</param>
        public void CreateXML(string fileName, string strFileData)
        {
            StreamWriter writer;                               //写文件流

            //string strWriteFileData = Encrypt(strFileData);  //是否加密处理
            string strWriteFileData = strFileData;             //写入的文件数据
            writer = File.CreateText(fileName);
            writer.Write(strWriteFileData);
            writer.Close();                                    //关闭文件流
        }

        /// <summary> 读取XML文件
        /// 读取XML文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public string LoadXML(string fileName)
        {
            StreamReader sReader;                              //读文件流
            string dataString;                                 //读出的数据字符串

            sReader = File.OpenText(fileName);
            dataString = sReader.ReadToEnd();
            sReader.Close();                                   //关闭读文件流
            //return Decrypt(dataString);                      //是否解密处理
            return dataString;
        }

        /// <summary>
        /// 判断是否存在文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool hasFile(String fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// UTF8字节数组转字符串
        /// </summary>
        /// <param name="characters"></param>
        /// <returns></returns>
        public string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// 字符串转UTF8字节数组
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        public byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}