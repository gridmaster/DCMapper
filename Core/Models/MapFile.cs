using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class MapFile : BaseInfo
    {
        public MapDirectory Directory { get; set; }
        public string DirectoryName { get; set; }
        public bool IsReadOnly { get; set; }
        public long Length { get; set; }

        public MapFile LoadData(FileInfo fi)
        {
            DirectoryName = fi.DirectoryName;
            IsReadOnly = fi.IsReadOnly;
            Length = fi.Length;

            FullName = fi.FullName;
            Attributes = fi.Attributes.ToString();
            Name = fi.Name;
            CreationTime = fi.CreationTime;
            CreationTimeUtc = fi.CreationTimeUtc;
            Exists = fi.Exists;
            Extension = fi.Extension;
            LastAccessTime = fi.LastAccessTime;
            LastAccessTimeUtc = fi.LastAccessTimeUtc;
            LastWriteTime = fi.LastWriteTime;
            LastWriteTimeUtc = fi.LastWriteTimeUtc;

            try
            {
                Directory = new MapDirectory().LoadData(fi.Directory);
                if (fi.Directory.Parent != null)
                    Directory.Parent = new MapDirectory().LoadData(fi.Directory.Parent);
                Directory.Root = new MapDirectory().LoadData(fi.Directory.Root);
            }
            catch (System.IO.PathTooLongException ex)
            {
                Console.WriteLine(ex.Message);
                // This code just writes out the message and continues to recurse. 
                //logger.DebugFormat("Unable to process {1}{0}drive: {2}{0}Error: {3}"
                //    , Environment.NewLine
                //    , "LoadData"
                //    , FullName
                //    , ex.Message);
            }


            return this;
        }
    }
}
