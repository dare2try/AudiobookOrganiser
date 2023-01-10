﻿using AudiobookOrganiser.Business.Tasks;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using static System.Extensions;

namespace AudiobookOrganiser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConsoleEx.WriteColouredLine(ConsoleColor.Green, "--------------------------------------");
            ConsoleEx.WriteColouredLine(ConsoleColor.Green, "- Audiobook Organiser -");
            ConsoleEx.WriteColouredLine(ConsoleColor.Green, "--------------------------------------");

            if (args.Contains("-audible-sync"))
            {
                SyncFromAudibleCli.Run();
                ConsoleEx.WriteColoured(ConsoleColor.Green, "\n\nDONE!");
            }
            else
            {
                foreach (var library in LibraryRootPaths)
                {
                    ConsoleEx.WriteColouredLine(ConsoleColor.Cyan, $"\n\nLIBRARY {library}\n\n");
                    ReorganiseFilesAlreadyInLibrary.Run(library);
                }

                SyncFromOpenAudibleDownloads.Run();
                ConvertExistingMp3ToM4b.Run();
                CheckAndRewriteTags.Run();

                ConsoleEx.WriteColoured(ConsoleColor.Green, "\n\nDONE!");

                // Give time to see output before closing console
                Thread.Sleep(3000);
            }
        }

        internal static string[] LibraryRootPaths { get; set; }
            = ConfigurationManager.AppSettings["LibraryRootPaths"].Trim().Split(';');
        internal static string OpenAudibleDownloadsFolderMp3Path { get; set; }
            = ConfigurationManager.AppSettings["OpenAudibleDownloadsFolderMp3Path"].Trim();
        internal static string OpenAudibleDownloadsFolderM4bPath { get; set; }
            = ConfigurationManager.AppSettings["OpenAudibleDownloadsFolderM4bPath"].Trim();
        internal static string OpenAudibleBookListPath { get; set; }
            = ConfigurationManager.AppSettings["OpenAudibleBookListPath"].Trim();
        internal static bool SyncFromOpenAudibleDownloadsFolder { get; set; }
            = ConfigurationManager.AppSettings["SyncFromOpenAudibleDownloadsFolder"].Trim().ToLower().In("yes", "true", "1");
        internal static string OutputDirectoryName { get; set; }
            = ConfigurationManager.AppSettings["OutputDirectoryName"].Trim();
        internal static string AudibleCliSyncPath { get; set; }
            = ConfigurationManager.AppSettings["AudibleCliSyncPath"].Trim();
        internal static string FfMpegPath { get; set; }
            = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ffmpeg.exe");
        internal static string FfProbePath { get; set; }
            = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ffprobe.exe");
        internal static bool LibFDK_AAC_EncodingEnabled { get; set; }
           = ConfigurationManager.AppSettings["LibFDK_AAC_EncodingEnabled"].Trim().ToLower().In("yes", "true", "1");
    }
}
