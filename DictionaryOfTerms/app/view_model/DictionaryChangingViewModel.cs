using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Windows;
using DictionaryOfTerms.data.repository;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DictionaryOfTerms.app.view_model;

public class DictionaryChangingViewModel : ViewModelBase, IRoutableViewModel, IScreen
{
    public ReactiveCommand<Unit, IRoutableViewModel> Done { get; }
    [Reactive] public string DictionaryName { get; set; }
    
    private readonly FileManager _fileManager = new();
    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }

    public DictionaryChangingViewModel()
    {
        
    }

    public DictionaryChangingViewModel(string name)
    {
        DictionaryName = name;
        string oldName = name;
        
        Done = ReactiveCommand.CreateFromObservable(() =>
        {
            if (string.IsNullOrEmpty(DictionaryName) || DictionaryName == " ")
            {
                MessageBox.Show("Вы не ввели название словаря!");
            }

            if (DictionaryName != null)
            {
                var dictionaries = _fileManager.ReadDictionary(
                    "C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json");
                var dict = dictionaries.Single(s => s.Name == oldName);

                dict.Name = DictionaryName;
                
                _fileManager.WriteDictionary(dict);
                _fileManager.DeleteDictionaryWithoutDeletingFile(oldName);
                
                return Router.Navigate.Execute(new DictionaryChoiceViewModel());
            }
            return Router.Navigate.Execute(new DictionaryAddingViewModel());
        });
        
        Back = ReactiveCommand.CreateFromObservable(() =>
        {
            return Router.Navigate.Execute(new DictionaryChoiceViewModel());
        });
    }

    public event PropertyChangingEventHandler? PropertyChanging;
    public void RaisePropertyChanging(PropertyChangingEventArgs args)
    {
        throw new System.NotImplementedException();
    }

    public void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        throw new System.NotImplementedException();
    }

    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new();
}