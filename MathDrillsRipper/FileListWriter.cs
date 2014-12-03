﻿using System;
using System.IO;

namespace MathDrillsRipper
{
    public class FileListWriter : IDisposable
    {
        private readonly IConsole _console;
        private readonly object _lock = new object();
        private readonly StreamWriter _writer;

        public FileListWriter(string fileList, IConsole console)
        {
            _console = console;
            fileList = Path.GetFullPath(fileList);
            if (File.Exists(fileList))
            {
                File.Delete(fileList);
            }

            _writer = new StreamWriter(fileList) { AutoFlush = true };
        }

        public void WriteEntry(string baseUrl, string path)
        {
            string entry = path;
            if (!path.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                entry = baseUrl + entry;
            }

            _console.WriteWarning("---- Found pdf: {0}", entry);

            lock (_lock)
            {
                _writer.WriteLine(entry);
            }
        }

        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Flush();
                _writer.Dispose();
            }
        }
    }
}