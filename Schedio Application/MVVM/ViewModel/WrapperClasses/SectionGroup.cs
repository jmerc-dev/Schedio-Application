using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.WrapperClasses
{
    public class SectionGroup
    {
        private int _IdCounter;
        private ObservableCollection<ClassSection>? _Sections { get; set; }

        public int IdCounter
        {
            get => _IdCounter;
            set => _IdCounter = value;
        }

        public ObservableCollection<ClassSection>? Sections
        {
            get => _Sections;
            set
            {
                _Sections = value;
                if (_Sections == null)
                    throw new NullReferenceException();
                _Sections.CollectionChanged += SectionAddedInCollection;
            }
        }

        private void SectionAddedInCollection(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    throw new NullReferenceException();
                if (e.NewItems.Count == 1)
                {
                    if (e.NewItems[0] == null)
                        throw new NullReferenceException();

                    ClassSection? newSection = (ClassSection?)e.NewItems[0];

                    if (newSection == null)
                        throw new NullReferenceException();
                    newSection.ID = Interlocked.Increment(ref _IdCounter);
                }
            }
        }
    }
}
