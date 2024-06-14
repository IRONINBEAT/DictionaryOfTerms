using System;
using System.Collections.Generic;
using System.ComponentModel;
using DictionaryOfTerms.data.repository;
using DictionaryOfTerms.domain;
using ReactiveUI;

namespace DictionaryOfTerms.app.view_model;

public class StartWindowViewModel : ViewModelBase, IRoutableViewModel, IScreen
{
    public StartWindowViewModel()
    {
        Router.Navigate.Execute(new DictionaryChoiceViewModel());
    }

    // static string HashPassword(string password)
    // {
    //     // Генерация соли и хеширование пароля
    //     var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
    //     Console.WriteLine(hashedPassword);
    //     return hashedPassword;
    // }

    public event PropertyChangedEventHandler? PropertyChanged;
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