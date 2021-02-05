using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class CSVManager
{
    private static string uuid = System.Guid.NewGuid().ToString();
    private static string reportDirectoryName = "Report";
    private static string reportFileName = "report.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = new string[15]{
                "episcodecount",
                "reward",
                "WheelAction1",
                "WheelAction2",
                "WheelAction3",
                "WheelAction4",
                "Targetlocalpostionx",
                "Targetlocalpostiony",
                "Targetlocalpostionz",
                "Vlocalpostionx",
                "Vlocalpostiony",
                "Vlocalpostionz",
                "Vvelocityx",
                "Vvelocityy",
                "Vvelocityz",

            };
    private static string timeStampHeader = "time stamp";

    #region Interactions

    public static void AppendToReport(List<string[]> strings , string filePath)
    {
        //VerifyDirectory();
        //VerifyFile();
        foreach (string[] s in strings)
        { 
            using (StreamWriter sw = File.AppendText(filePath))
        {
            
            string finalString = GetTimeStamp();
            for (int i = 0; i < s.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += s[i];
            }
            finalString += reportSeparator ;
            sw.WriteLine(finalString);
        }
        }
    }
    //public static void AppendToReport(string[] strings, string filePath)
    //{
    //    //VerifyDirectory();
    //    //VerifyFile();
    //    using (StreamWriter sw = File.AppendText(filePath))
    //    {
    //        string finalString = GetTimeStamp();
    //        for (int i = 0; i < strings.Length; i++)
    //        {
    //            if (finalString != "")
    //            {
    //                finalString += reportSeparator;
    //            }
    //            finalString += strings[i];
    //        }
    //        finalString += reportSeparator;
    //        sw.WriteLine(finalString);
    //    }
    //}

    public static string CreateReport(string agentname)
    {
        string filepath;
        filepath = GetFilePath(agentname);
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(filepath))
        {
            string finalString = timeStampHeader;
            for (int i = 0; i < reportHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportHeaders[i];
            }
            finalString += reportSeparator;
            sw.WriteLine(finalString);
            return filepath;
        }
    }

    #endregion


    #region Operations

    static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    //static void VerifyFile()
    //{
    //    string file = GetFilePath();
    //    if (!File.Exists(file))
    //    {
    //        CreateReport();
    //    }
    //}

    #endregion


    #region Queries

    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName + "/" + uuid;
    }

    static string GetFilePath(string agentname)
    {
        return GetDirectoryPath() + "/" + System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss--") + agentname+ uuid + ".csv";
    }

    static string GetTimeStamp()
    {
        return System.DateTime.Now.ToString("o");
    }

    #endregion

}
