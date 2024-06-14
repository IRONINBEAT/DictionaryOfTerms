using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Windows;
using DictionaryOfTerms.data.repository;
using DictionaryOfTerms.domain;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DictionaryOfTerms.app.view_model;

public class DictionaryAddingViewModel : ViewModelBase, IRoutableViewModel, IScreen
{
    
    public ReactiveCommand<Unit, IRoutableViewModel> Done { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }


    [Reactive] public string DictionaryName { get; set; }
    //[Reactive] public string FileName { get; set; }
    
    private readonly FileManager _fileManager = new();
    public DictionaryAddingViewModel()
    {
        
        Done = ReactiveCommand.CreateFromObservable(() =>
        {
            if (string.IsNullOrEmpty(DictionaryName) || DictionaryName == " ")
            {
                MessageBox.Show("Вы не ввели название словаря!");
            }

            //if (string.IsNullOrEmpty(FileName) || FileName == " ")
            //{
            //    MessageBox.Show("Вы не ввели имя файла!");
            //}

            //if (tree.Search(Term) != null)
            //{
            //    MessageBox.Show("Такой термин уже есть в словаре!");
            //}
            if (DictionaryName != null)// && FileName != null)
            {
                _fileManager.WriteDictionary(DictionaryName, "C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\");
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