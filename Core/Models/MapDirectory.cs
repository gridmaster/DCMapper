using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class MapDirectory : BaseInfo
    {
        public MapDirectory Parent { get; set; }
        public MapDirectory Root { get; set; }

        public MapDirectory LoadData(DirectoryInfo fi)
        {
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
            return this;
        }
    }
}
