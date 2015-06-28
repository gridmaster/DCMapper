// -----------------------------------------------------------------------
// <copyright file="BaseBulkLoad.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Core.Interface;
using Core.Models;

namespace Services.BulkLoad
{
    class BulkLoadFiles : BaseBulkLoad, IDisposable
    {
        private static readonly string[] ColumnNames = new string[]
            {
                "Attributes", "CreationTime", "CreationTimeUtc", "Exists", "Extension",
                "FullName", "LastAccessTime", "LastAccessTimeUtc", "LastWriteTime", "LastWriteTimeUtc", 
                "Name", "DirectoryName", "IsReadOnly", "Length"
            };

        public BulkLoadFiles(ILogger logger)
            : base(logger, ColumnNames)
        {
        }

        public DataTable LoadDataTableWithFiles(IEnumerable<MapFile> dStats, DataTable dt)
        {
            foreach (var value in dStats)
            {
                string sValue = value.Attributes + "~" + value.CreationTime + "~" + value.CreationTimeUtc + "~"
                                + value.Exists + "~" + value.Extension + "~" + value.FullName + "~" 
                                + value.LastAccessTime + "~" + value.LastAccessTimeUtc + "~" + value.LastWriteTime + "~" 
                                + value.LastWriteTimeUtc + "~" + value.Name + "~" + value.DirectoryName
                                + "~" + value.IsReadOnly + "~" + value.Length;

                DataRow row = dt.NewRow();

                row.ItemArray = sValue.Split('~');

                dt.Rows.Add(row);
            }

            return dt;
        }
        
        #region Implement IDisposable
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        //More Info

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~BulkLoadFiles()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }
        #endregion Implement IDisposable
    }
}
