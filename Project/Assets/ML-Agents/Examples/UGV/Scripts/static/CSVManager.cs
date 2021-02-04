using UnityEngine;
using System.IO;

public static class CSVManager
{

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

    public static void AppendToReport(string[] strings , string filePath)
    {
        //VerifyDirectory();
        //VerifyFile();
        using (StreamWriter sw = File.AppendText(filePath))
        {
            string finalString = GetTimeStamp();
            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
            }
            finalString += reportSeparator ;
            sw.WriteLine(finalString);
        }
    }

    public static string CreateReport()
    {
        string filepath;
        filepath = GetFilePath();
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

    static void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateReport();
        }
    }

    #endregion


    #region Queries

    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName;
    }

    static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv";
    }

    static string GetTimeStamp()
    {
        return System.DateTime.Now.ToString("o");
    }

    #endregion

}
