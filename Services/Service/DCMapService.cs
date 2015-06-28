using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface;
using Core.Models;
using Services.BulkLoad;
using Services.Interface;
using Newtonsoft.Json;

namespace Services.Service
{
    public class DCMapService : BaseService, IDCMapService
    {
        public DCMapService(ILogger logger) : base(logger)
        {
            ThrowIfIsInitialized();
            IsInitialized = true;
        }

        public void MapDriveRecursive()
        {
            // Start with drives if you have to search the entire computer. 
            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(dr);
                MapFiles mfs = new MapFiles();

                // Here we skip the drive if it is not ready to be read. This 
                // is not necessarily the appropriate action in all scenarios. 
                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }
                try
                {
                    System.IO.DirectoryInfo rootDir = di.RootDirectory;
                    DirectoryInfo fucksake = new DirectoryInfo(@"C:\Temp");
                    mfs = WalkDirectoryTree(fucksake);
                }
                catch (Exception ex)
                {
                    // This code just writes out the message and continues to recurse. 
                    logger.DebugFormat("Unable to process {1}{0}Error: {2}"
                        , Environment.NewLine
                        , GetThisMethodName()
                        , ex.Message);
                }

                //using (BulkLoadFiles blc = new BulkLoadFiles(logger))
                //{
                //    var dtc = blc.ConfigureDataTable();
                //    dtc = blc.LoadDataTableWithFiles(mfs, dtc);
                //    blc.BulkCopy<MapFiles>(dtc, "DCMaperContext");
                //}
                
                #region ioccontainer
                //var dt = IOCContainer.Instance.Get<BulkLoadFiles>().ConfigureDataTable();

                //dt = IOCContainer.Instance.Get<BulkLoadFiles>().LoadDataTableWithTradingVolume(MapFile, dt);

                //if (dt == null)
                //{
                //    logger.DebugFormat("{0}No data returned on LoadDataTableWithTradingVolume", Environment.NewLine);
                //}
                //else
                //{
                //    success = IOCContainer.Instance.Get<BulkLoadFiles>().BulkCopy<MapFile>(dt, "ETFContext");
                //}

                //return success;
                #endregion ioccontainer
                break;
            }

            // Write out all the files that could not be processed.
            Console.WriteLine("Files with restricted access:");
            //foreach (string s in log)
            //{
            //    Console.WriteLine(s);
            //}
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private MapFiles WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            logger.DebugFormat("{0}mapping directory: {1}", Environment.NewLine, root.FullName);
            MapFiles mfs = new MapFiles();
            try
            {
                System.IO.FileInfo[] files = null;
                System.IO.DirectoryInfo[] subDirs = null;

                // First, process all the files directly under this folder 
                try
                {
                    files = root.GetFiles("*.*");
                }
                #region catch errors
                // This is thrown if even one of the files requires permissions greater 
                // than the application provides. 
                catch (UnauthorizedAccessException ex)
                {
                    // This code just writes out the message and continues to recurse. 
                    logger.DebugFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                        , Environment.NewLine
                        , GetThisMethodName()
                        , root.FullName
                        , ex.Message);

                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    logger.DebugFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                        , Environment.NewLine
                        , GetThisMethodName()
                        , root.FullName
                        , ex.Message);
                }
                catch (System.IO.PathTooLongException ex)
                {
                    // This code just writes out the message and continues to recurse. 
                    logger.DebugFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                        , Environment.NewLine
                        , GetThisMethodName()
                        , root.Name
                        , ex.Message);
                    //Console.ReadKey();
                }
                catch (Exception ex)
                {
                    // This code just writes out the message and continues to recurse. 
                    logger.DebugFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                        , Environment.NewLine
                        , GetThisMethodName()
                        , root.FullName
                        , ex.Message);
                    // Console.ReadKey();
                }
                #endregion catch errors

                if (files != null)
                {
                    foreach (System.IO.FileInfo fi in files)
                    {
                        try
                        {
                            MapFile mf = new MapFile().LoadData(fi);
                            string json = JsonConvert.SerializeObject(mf);
                            mfs.Add(mf);
                        }
                        #region catch errors
                        catch (System.IO.PathTooLongException ex)
                        {
                            // This code just writes out the message and continues to recurse. 
                            logger.DebugFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                                , Environment.NewLine
                                , GetThisMethodName()
                                , fi.Name
                                , ex.Message);
                            //Console.ReadKey();
                        }
                        catch (Exception ex)
                        {
                            // This code just writes out the message and continues to recurse. 
                            logger.DebugFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                                , Environment.NewLine
                                , GetThisMethodName()
                                , fi.Name
                                , ex.Message);
                            // Console.ReadKey();
                        }
                        #endregion catch errors
                    }

                    // Now find all the subdirectories under this directory.
                    subDirs = root.GetDirectories();
                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        // Resursive call for each subdirectory.
                        var more = WalkDirectoryTree(dirInfo);
                        mfs.AddRange(more);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.DebugFormat("Unable to process {1}{0}root: {2}{0}Error: {3}"
                    , Environment.NewLine
                    , GetThisMethodName()
                    , root.FullName
                    , ex.Message);
            }
            using (BulkLoadFiles blc = new BulkLoadFiles(logger))
            {
                var dtc = blc.ConfigureDataTable();
                dtc = blc.LoadDataTableWithFiles(mfs, dtc);
                blc.BulkCopy<MapFiles>(dtc, "DCMaperContext");
            }
            return mfs;
        }

        public void MapDrive(string drive)
        {
            ThrowIfNotInitialized();

            try
            {
                logger.DebugFormat("{0}mapping drive: {1}", Environment.NewLine, drive);

                DriveInfo di = new DriveInfo(drive + ":\\");
                var root = di.RootDirectory;
                string[] directories = Directory.GetDirectories(root.ToString());

                MapFiles mfs = GetMapFiles(directories);

                for (int i = 0; i < directories.Count(); i++)
                {
                    try
                    {
                        var directory = directories[i];
                        var dirs = Directory.GetDirectories(directory);
                        var files = Directory.GetFiles(directory);
                        var dinfo = new DirectoryInfo(directory);

                        foreach (var file in files)
                        {
                            FileInfo info = new FileInfo(file);
                            MapFile mf = new MapFile().LoadData(info);
                            string json = JsonConvert.SerializeObject(mf);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                        , Environment.NewLine
                        , GetThisMethodName()
                        , drive
                        , ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                                        , Environment.NewLine
                                        , GetThisMethodName()
                                        , drive
                                        , ex.Message);
            }
        }

        private static MapFiles GetMapFiles(string[] directories)
        {
            MapFiles mfs = new MapFiles();

            return mfs;
        }
    }
}
