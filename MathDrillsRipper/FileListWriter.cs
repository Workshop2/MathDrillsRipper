using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MathDrillsRipper
{
    public class FileListWriter : IDisposable
    {
        private readonly IConsole _console;
        private readonly object _lock = new object();
        private readonly StreamWriter _writer;
        private readonly HashSet<string> _cached = new HashSet<string>();

        public int TotalLinesWritten
        {
            get
            {
                lock (_lock)
                {
                    return _cached.Count;
                }
            }
        }

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

        public void WriteEntry(string baseUrl, IEnumerable<string> paths)
        {
            lock (_lock)
            {
                foreach (string path in paths)
                {
                    string entry = path;
                    if (!path.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                    {
                        entry = baseUrl + entry;
                    }
                    
                    if (_cached.Contains(entry))
                    {
                        return;
                    }

                    _console.WriteInfo("---- Found pdf: {0}", entry);
                    _writer.WriteLine(entry);
                    _cached.Add(entry);
                }
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