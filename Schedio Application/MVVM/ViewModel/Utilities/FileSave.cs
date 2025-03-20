using Microsoft.Win32;
using Schedio_Application.MVVM.ViewModel.Commands;
using Schedio_Application.MVVM.ViewModel.WrapperClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{
    public class FileSave
    {
        // Commands
        public RelayCommand SaveCommand => new RelayCommand(execute => Save((FullDataWrapper)execute));

        // JsonOptions
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = 
            { 
                new SubjectConverter(), 
                new SubjectEntryConverter() 
            },
            WriteIndented = true
        };


        public FileSave() { }

        public bool Save(FullDataWrapper fullDataWrapper)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                DefaultExt = "json",
                AddExtension = true
            };
            string fullJsonData = JsonSerializer.Serialize(fullDataWrapper, options);
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName.ToString(), fullJsonData);
            }
            return true;
        }

        public bool SaveAs()
        {
            // TODO: 
            return true;
        }
    }
}
