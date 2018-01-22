using System.Web.Script.Serialization;
using System.Diagnostics;
using System.IO;
using System;

namespace WIXInstaller.Services
{
    public class JSONSerialization<TModel> where TModel : class
    {
        public static void Serialization(TModel Model, string FilePath, string FileName)
        {
            try
            {
                File.WriteAllText
                (
                    FilePath + FileName,
                    new JavaScriptSerializer().Serialize(Model)
                );
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);                
            }
        }
        public static TModel DeSerialization(string FilePath)
        {
            return new JavaScriptSerializer().Deserialize<TModel>(File.ReadAllText(FilePath));
        }
        public static void DeSerialization(TModel Model, string FilePath)
        {
            try
            {
                Model =  new JavaScriptSerializer().Deserialize<TModel>(File.ReadAllText(FilePath));
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }
    }
}