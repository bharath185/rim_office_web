using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web.Mvc;


namespace OfficeConnect_Web.Models
{
    public class LogsModel
    {
        ////public static void LogErrorToFile(Exception ex, string errorName)
        ////{
        ////    try
        ////    {
        ////        // 1. Folder for logs
        ////        string logDirectory = @"E:\New Office Connect\Hosting\LogFile";
        ////        if (!Directory.Exists(logDirectory))
        ////        {
        ////            Directory.CreateDirectory(logDirectory);
        ////        }

        ////        // 2. File name per day
        ////        string logFile = Path.Combine(logDirectory, $"LogFile_{DateTime.Now:yyyyMMdd}.txt");


        ////        // 3. Append error to file
        ////        using (StreamWriter writer = new StreamWriter(logFile, true))
        ////        {
        ////            writer.WriteLine("-------------------------------------------------");
        ////            writer.WriteLine($"Error Heading : {errorName}");
        ////            writer.WriteLine($"Date/Time    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        ////            writer.WriteLine("Error Message : " + ex.Message);
        ////            writer.WriteLine("Stack Trace   : " + ex.StackTrace);

        ////            if (ex.InnerException != null)
        ////            {
        ////                writer.WriteLine("Inner Exception: " + ex.InnerException.Message);
        ////                writer.WriteLine("Inner Stack Trace: " + ex.InnerException.StackTrace);
        ////            }

        ////            writer.WriteLine("-------------------------------------------------");
        ////            writer.WriteLine();
        ////        }
        ////    }
        ////    catch
        ////    {
        ////        // Prevent logging failure from crashing app
        ////    }
        ////}

        public static void LogErrorToFile(Exception ex, string errorName)
        {
            try
            {
                // 1. Folder for logs
                string logDirectory = @"E:\New Office Connect\Hosting\LogFile";
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // 2. File name per day
                string logFile = Path.Combine(logDirectory, $"LogFile_{DateTime.Now:yyyyMMdd}.txt");

                // 3. Append error to file
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine("-------------------------------------------------");
                    writer.WriteLine($"Error Heading : {errorName}");
                    writer.WriteLine($"Date/Time    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine("Error Message : " + ex.Message);
                    writer.WriteLine("Stack Trace   : " + ex.StackTrace);

                    if (ex.InnerException != null)
                    {
                        writer.WriteLine("Inner Exception: " + ex.InnerException.Message);
                        writer.WriteLine("Inner Stack Trace: " + ex.InnerException.StackTrace);
                    }

                    writer.WriteLine("-------------------------------------------------");
                    writer.WriteLine();
                }
            }
            catch
            {
                // Prevent logging failure from crashing app
            }
        }

        // Add this new method for success logging
        public static void LogSuccess(string message, string successName)
        {
            try
            {
                // 1. Folder for logs
                string logDirectory = @"E:\New Office Connect\Hosting\LogFile";
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // 2. File name per day
                string logFile = Path.Combine(logDirectory, $"LogFile_{DateTime.Now:yyyyMMdd}.txt");

                // 3. Append success to file
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine("-------------------------------------------------");
                    writer.WriteLine($"Success Heading : {successName}");
                    writer.WriteLine($"Date/Time       : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"Success Message : {message}");
                    writer.WriteLine("-------------------------------------------------");
                    writer.WriteLine();
                }
            }
            catch
            {
                // Prevent logging failure from crashing app
            }
        }

        // Optional: Create a separate log file for success messages only
        public static void LogSuccessToSeparateFile(string message, string successName)
        {
            try
            {
                // 1. Folder for logs
                string logDirectory = @"E:\New Office Connect\Hosting\LogFile";
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // 2. Separate file for success logs
                string logFile = Path.Combine(logDirectory, $"SuccessLog_{DateTime.Now:yyyyMMdd}.txt");

                // 3. Append success to file
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine("-------------------------------------------------");
                    writer.WriteLine($"Success Heading : {successName}");
                    writer.WriteLine($"Date/Time       : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"Success Message : {message}");
                    writer.WriteLine("-------------------------------------------------");
                    writer.WriteLine();
                }
            }
            catch
            {
                // Prevent logging failure from crashing app
            }
        }
        public static void LogInfo(string message, string category = "Info")
        {
            try
            {
                string logPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Logs/");

                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                string logFile = Path.Combine(logPath, $"Info_{DateTime.Now:yyyyMMdd}.log");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {category} | {message}{Environment.NewLine}";

                File.AppendAllText(logFile, logEntry);
            }
            catch
            {
                // Silently fail if logging fails
            }
        }
    }
}