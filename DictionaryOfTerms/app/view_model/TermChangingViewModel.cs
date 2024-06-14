using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Windows;
using DictionaryOfTerms.domain;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DictionaryOfTerms.app.view_model;

public class TermChangingViewModel: ViewModelBase, IRoutableViewModel, IScreen
{
    public ReactiveCommand<Unit, IRoutableViewModel> Done { get; }
    
    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }
    
    [Reactive] public string Term { get; set; }
    
    [Reactive] public string Definition { get; set; }


    public TermChangingViewModel()
    {
        
    }
    
    public TermChangingViewModel(AVLTree tree, string selectedTerm, List<string> definitions)
    {
        Term = selectedTerm;
        var buffDef = definitions;
        Definition = string.Join("|", definitions);
        
        Back = ReactiveCommand.CreateFromObservable(() =>
             {
                 return Router.Navigate.Execute(new MainWindowViewModel(tree));
             });
        
        Done = ReactiveCommand.CreateFromObservable(() =>
        {
            if (Term == null || Term == "" || Term == " ")
            {
                MessageBox.Show("Вы не ввели термин!");
                return Router.Navigate.Execute(new TermChangingViewModel(tree, selectedTerm, buffDef));
            }
            
            if (Definition == null || Definition == "" || Definition == " ")
            {
                MessageBox.Show("Вы не ввели определения!");
                return Router.Navigate.Execute(new TermChangingViewModel(tree, selectedTerm, buffDef));
            }
            
            
            var definitions = new List<string>();
        
            string[] parts = Definition.Split("|");
            foreach (string part in parts)
                definitions.Add(part);
            
            tree.Delete(Term);
            foreach (var def in definitions)
                tree.Insert(Term, def);
            
        
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

// public TermChangingViewModel(DictionaryNode root, BinaryDictionaryTree dictionaryTree, string term, List<string> definitions)
// {
//     Term = term;
//     var buffDef = definitions;
//     Definition = string.Join("|", definitions);
//     
//     
//     
//     Back = ReactiveCommand.CreateFromObservable(() =>
//     {
//         return Router.Navigate.Execute(new MainWindowViewModel());
//     });
//     
//     Done = ReactiveCommand.CreateFromObservable(() =>
//     {
//         if (Term == null || Term == "" || Term == " ")
//         {
//             MessageBox.Show("Вы не ввели термин!");
//             return Router.Navigate.Execute(new TermChangingViewModel(root, dictionaryTree, term, buffDef));
//         }
//         
//         if (Definition == null || Definition == "" || Definition == " ")
//         {
//             MessageBox.Show("Вы не ввели определения!");
//             return Router.Navigate.Execute(new TermChangingViewModel(root, dictionaryTree, term, buffDef));
//         }
//         
//         
//         var definitions = new List<string>();
//     
//         string[] parts = Definition.Split("|");
//         foreach (string part in parts)
//             definitions.Add(part);
//         
//         var insertingTermWithDef = new Dictionary<string, List<string>>()
//         {
//             { Term, definitions }
//         };
//
//         dictionaryTree.Remove(root, insertingTermWithDef);
//         
//         dictionaryTree.Insert(root, insertingTermWithDef);
//
//         // var fileManager = new FileManager();
//         // fileManager.GenerateUniqueIDAndWriteToFile(insertingTermWithDef);
//
//         return Router.Navigate.Execute(new MainWindowViewModel(root, dictionaryTree));
//
//     });
//     
// }