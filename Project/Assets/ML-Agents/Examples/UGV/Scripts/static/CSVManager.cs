using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class CSVManager
{
    private static string uuid = System.Guid.NewGuid().ToString();
    private static string reportDirectoryName = "Report";
    private static string reportFileName = "report.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = new string[]{
                "deltatime",
                "stepcount_currentEP",
                "totalstepcount",
                "completedEpisodes",
                "cumaltiveRewardInCurrentEP",
                "wheelAction1",
                "wheelAction2",
                "wheelAction3",
                "wheelAction4",
                "wheeltorque1",
                "wheeltorque2",
                "wheeltorque3",
                "wheeltorque4",
                "legAction1",
                "legAction2",
                "legAction3",
                "legAction4",
                "legRotate1",
                "legRotate2",
                "legRotate3",
                "legRotate4",
                "velcoityToGoal",
                "vehicleVelocity_x",
                "vehicleVelocity_y",
                "vehicleVelocity_z",
                "goalVelocity_x",
                "goalVelocity_y",
                "goalVelocity_z",
                "vehicleRotation_x",
                "vehicleRotation_y",
                "vehicleRotation_z",
                "vehicleRotation_w",
                "TargetRealtivePosition_x",
                "TargetRealtivePosition_y",
                "TargetRealtivePosition_z",
                "legRotation_1",
                "legRotation_2",
                "legRotation_3",
                "legRotation_4",
         


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
