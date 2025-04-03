using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Converter_Contexts;
using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
using Schedio_Application.MVVM.ViewModel.JsonConverters;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using Schedio_Application.MVVM.ViewModel.WrapperClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.Model
{
    public class FileLoader
    {
        private static string _FileExtension = ".json";
        private string? _FileContent;
        private string _Path;

        private RoomTypesConverterContext _RoomConverterContext = new();
        private PeopleConverterContext _PeopleConverterContext = new();
        private SectionsConverterContext _SectionConverterContext = new();

        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public FileLoader(string path)
        {
            this._Path = path;

            if (!Path.GetExtension(path).Equals(_FileExtension, StringComparison.CurrentCultureIgnoreCase))
                throw new InvalidFileException(path);

            // For room types reference resolving
            options.Converters.Add(new RoomTypesConverter(_RoomConverterContext));
            options.Converters.Add(new RoomsConverter(_RoomConverterContext));
            
            // For subject reference resolving
            options.Converters.Add(new PeopleConverter(_PeopleConverterContext));
            options.Converters.Add(new SectionsConverter(_SectionConverterContext));
            options.Converters.Add(new SubjectConverter(_SectionConverterContext, _PeopleConverterContext));

        }

        public FullDataWrapper? Execute()
        {
            _FileContent = File.ReadAllText(_Path);
            FullDataWrapper? fullData = JsonSerializer.Deserialize<FullDataWrapper>(_FileContent, options);

            if (fullData == null || fullData.Identifier == null)
                throw new FileFormatException("File is either corrupted or invalid");

            Trace.WriteLineIf(_PeopleConverterContext.PeopleMap == null, "peopleMap is null");
            Trace.WriteLineIf(_SectionConverterContext.ClassSectionMap == null, "sectionMap is null");
            try
            {
                //foreach (ClassSection cs in fullData.SectionsGroup.Sections)
                //{
                //    Trace.WriteLine($"{cs.ID}: {cs.Name}");

                //    foreach (Subject s in cs.Subjects)
                //    {
                //        Trace.WriteLine($"\t{s.ID}: {s.Name} Personnel: {s.PersonnelID}, OwnerSection: {s.OwnerSectionID}");
                //        Trace.WriteLineIf(s.OwnerSection == null, "\t\tNo ownersection");
                //        Trace.WriteLineIf(s.AssignedPerson == null, "\t\tNo AssignedPerson");
                //    }
                //}
            }
            catch (Exception ex)
            {
                new MBox(ex.Message).ShowDialog();
                return null;
            }


            if (fullData.Identifier.Name != null && !fullData.Identifier.Name.Equals(FileHashKey.Key))
            {
                throw new FileFormatException("Please import schedio file only");
            }

            return fullData;
        }
        
    }
}
