using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Channels;
using System.Windows;
using DictionaryOfTerms.data.repository;
using DictionaryOfTerms.domain;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DictionaryOfTerms.app.view_model;

public class MainWindowViewModel : ViewModelBase, IRoutableViewModel, IScreen
{
    public ReactiveCommand<Unit, IRoutableViewModel> Add { get; }
    public static string DictionaryName { get; set; }
    private static string _dictionaryPath { get; set; }
    public ReactiveCommand<Unit, IRoutableViewModel> Delete { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> Edit { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }
    
    private AVLTree _tree = new();
    
    [Reactive] public ObservableCollection<string> AllTerms { get; set; } = new();

    private readonly FileManager _fileManager = new();

    private string _currentDefinitions;

    public string CurrentDefinitions
    {
        get => _currentDefinitions;
        set => _currentDefinitions = value ?? throw new ArgumentNullException(nameof(value));
    }

    private string _searchBar;
    
    public string SearchBar
    {
        get => _searchBar;
        set
        {
            _searchBar = value;
            if (value == "")
            {
                AllTerms.Clear();
                InsertToListView(_tree.Root);
            }
            else
            {
                AllTerms.Clear();
                var matchingTerms = _tree.SearchBySubstring(_searchBar);
                foreach (var term in matchingTerms)
                    AllTerms.Add(term);
            }
        }
    }

    private string _selectedTerm;
    
    public string SelectedTerm
    {
        get => _selectedTerm;
        set
        {
            _selectedTerm = value;
            var def = _tree.Search(_selectedTerm);
            _currentDefinitions = string.Join("\n\n", def.Select(x => x));
            OnPropertyChanged(nameof(CurrentDefinitions));
        }
    }

    public MainWindowViewModel()
    {
        
    }
    
    public MainWindowViewModel(AVLTree tree)
    {
        _tree = tree;
        
        InsertToListView(_tree.Root);
        SelectedTerm = _tree.Root.Key;
        
        Add = ReactiveCommand.CreateFromObservable(() =>
        {
            return Router.Navigate.Execute(new TermAddingViewModel(_tree));
        });
        
        Delete = ReactiveCommand.CreateFromObservable(() =>
        {
            if (SelectedTerm != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить термин '{SelectedTerm}'?", "Удалить?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _tree.Delete(SelectedTerm);
    
                    return Router.Navigate.Execute(new MainWindowViewModel(_tree));
                }
    
                return Router.Navigate.Execute(new MainWindowViewModel(_tree));
            }
    
            MessageBox.Show("Термин не выбран!");
            return Router.Navigate.Execute(new MainWindowViewModel(_tree));
        });
        
        Edit = ReactiveCommand.CreateFromObservable(() =>
        {
            if (SelectedTerm != null)
            {
                List<string> subStrings = CurrentDefinitions
                    .Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return Router.Navigate.Execute(new TermChangingViewModel(_tree, SelectedTerm, subStrings));
            }
        
            MessageBox.Show("Термин не выбран!");
            return Router.Navigate.Execute(new MainWindowViewModel(_tree));
        });

        Back = ReactiveCommand.CreateFromObservable(() =>
        {
            return Router.Navigate.Execute(new DictionaryChoiceViewModel());
        });

        List<Term> terms = new();
        InsertToTermList(tree.Root, terms);
        
        _fileManager.WriteTerms(terms, _dictionaryPath);
        
    }
    
    public MainWindowViewModel(string path, string name)
    {
        DictionaryName = name;
        _dictionaryPath = path;
        
        var terms = _fileManager.ReadTerms(_dictionaryPath);
        
        foreach (var term in terms)
            foreach (var definition in term.Definitions)
                _tree.Insert(term.Name, definition);
        
        InsertToListView(_tree.Root);
        
        if (_tree.Root != null)
            SelectedTerm = _tree.Root.Key;

        Add = ReactiveCommand.CreateFromObservable(() =>
        {
            return Router.Navigate.Execute(new TermAddingViewModel(_tree));
        });
        
        Delete = ReactiveCommand.CreateFromObservable(() =>
        {
            if (SelectedTerm != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить термин '{SelectedTerm}'?", "Удалить?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _tree.Delete(SelectedTerm);
    
                    return Router.Navigate.Execute(new MainWindowViewModel(_tree));
                }
    
                return Router.Navigate.Execute(new MainWindowViewModel(_tree));
            }
    
            MessageBox.Show("Термин не выбран!");
            return Router.Navigate.Execute(new MainWindowViewModel(_tree));
        });
        
        Edit = ReactiveCommand.CreateFromObservable(() =>
        {
            if (SelectedTerm != null)
            {
                List<string> subStrings = CurrentDefinitions
                    .Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return Router.Navigate.Execute(new TermChangingViewModel(_tree, SelectedTerm, subStrings));
            }
        
            MessageBox.Show("Термин не выбран!");
            return Router.Navigate.Execute(new MainWindowViewModel(_tree));
        });
        
        Back = ReactiveCommand.CreateFromObservable(() =>
        {
            return Router.Navigate.Execute(new DictionaryChoiceViewModel());
        });
    }


    private void InsertToListView(AVLNode node)
    {
        if (node != null)
        {
            InsertToListView(node.Left);
            AllTerms.Add(node.Key);
            InsertToListView(node.Right);
        }
    }

    private void InsertToTermList(AVLNode node, List<Term> terms)
    {
        if (node != null)
        {
            InsertToTermList(node.Left, terms);
            terms.Add(new Term(){Name = node.Key, Definitions = node.Definitions});
            InsertToTermList(node.Right, terms);
        }
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