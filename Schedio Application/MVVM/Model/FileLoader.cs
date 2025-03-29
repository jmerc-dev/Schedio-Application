using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.Model
{
    public class FileLoader
    {
        private static string _FileExtension = ".json";

        private string? _FileContent;
        private string? _FileName;
        private string? _Path;

        public FileLoader(string path)
        {
            this._Path = path;

            if (!Path.GetExtension(path).Equals(_FileExtension, StringComparison.CurrentCultureIgnoreCase))
                throw new InvalidFileException(path);
        }
    }
}
