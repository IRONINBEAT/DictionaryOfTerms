using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Windows;
using DictionaryOfTerms.data.repository;
using DictionaryOfTerms.domain;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DictionaryOfTerms.app.view_model;

public class DictionaryChoiceViewModel : ViewModelBase, IRoutableViewModel, IScreen
{
    public ReactiveCommand<Unit, IRoutableViewModel> Add { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> Delete { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> Edit { get; }
    
    public ReactiveCommand<Unit, IRoutableViewModel> Open { get; }

    private List<Dictionary> DictionariesList = new();

    [Reactive] public ObservableCollection<string> AllDictionaries { get; set; } = new();

    private readonly FileManager _fileManager = new();

    private string _dictionary;

    public string Dictionary
    {
        get => _dictionary;
        set => _dictionary = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DictionaryChoiceViewModel()
    {
        var dictionaries = _fileManager.ReadDictionary("C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json");

        foreach (var dictionary in dictionaries)
        {
            AllDictionaries.Add(dictionary.Name);
            DictionariesList.Add(dictionary);
        }

        //MessageBox.Show(DictionariesList.Count.ToString());
        Add = ReactiveCommand.CreateFromObservable(() =>
        {
            return Router.Navigate.Execute(new DictionaryAddingViewModel());
        });
        
        Delete = ReactiveCommand.CreateFromObservable(() =>
        {
            if (Dictionary != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить словарь '{Dictionary}'?", "Удалить?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _fileManager.DeleteDictionary(Dictionary);
                    return Router.Navigate.Execute(new DictionaryChoiceViewModel());
                }
                return Router.Navigate.Execute(new DictionaryChoiceViewModel());
            }

            MessageBox.Show("Словарь не выбран!");
            return Router.Navigate.Execute(new DictionaryChoiceViewModel());
        });
        Open = ReactiveCommand.CreateFromObservable(() =>
        {
            if (Dictionary != null)
            {
                var dictionary = DictionariesList.Single(s => s.Name == _dictionary);
                string path = dictionary.Path;
                return Router.Navigate.Execute(new MainWindowViewModel(path, dictionary.Name));
            }

            MessageBox.Show("Словарь не выбран!");
            return Router.Navigate.Execute(new DictionaryChoiceViewModel());

        });

        Edit = ReactiveCommand.CreateFromObservable(() =>
        {
            if (_dictionary != null)
                return Router.Navigate.Execute(new DictionaryChangingViewModel(_dictionary));
            MessageBox.Show("Словарь не выбран!");
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