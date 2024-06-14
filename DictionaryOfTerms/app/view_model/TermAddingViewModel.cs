using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Windows;
using DictionaryOfTerms.domain;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DictionaryOfTerms.app.view_model;

public class TermAddingViewModel : ViewModelBase, IRoutableViewModel, IScreen
{
    public ReactiveCommand<Unit, IRoutableViewModel> Done { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }


    [Reactive] public string Term { get; set; }
    [Reactive] public string Definition { get; set; }
    

    public TermAddingViewModel()
    {

    }
    
    public TermAddingViewModel(AVLTree tree)
    {
        
        Done = ReactiveCommand.CreateFromObservable(() =>
        {
            if (string.IsNullOrEmpty(Term) || Term == " ")
            {
                MessageBox.Show("Вы не ввели термин!");
            }

            if (string.IsNullOrEmpty(Definition) || Definition == " ")
            {
                MessageBox.Show("Вы не ввели определения!");
            }

            if (tree.Search(Term) != null)
            {
                MessageBox.Show("Такой термин уже есть в словаре!");
            }
            if (Term != null && Definition != null)
            {
                string[] parts = Definition.Split("|");
                foreach (string part in parts)
                    tree.Insert(Term, part);
                return Router.Navigate.Execute(new MainWindowViewModel(tree));
            }
            return Router.Navigate.Execute(new TermAddingViewModel(tree));
        });
        
        Back = ReactiveCommand.CreateFromObservable(() =>
        {
            return Router.Navigate.Execute(new MainWindowViewModel(tree));
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