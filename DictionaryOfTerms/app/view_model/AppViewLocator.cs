using DictionaryOfTerms.app.view;
using ReactiveUI;

namespace DictionaryOfTerms.app.view_model;

public class AppViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        return viewModel switch
        {
            StartWindowViewModel context => new StartWindow{DataContext = context},
            MainWindowViewModel context => new AdminMainWindow {DataContext = context},
            TermAddingViewModel context => new TermAddingWindow{DataContext = context},
            TermChangingViewModel context => new TermChangingWindow{DataContext = context},
            DictionaryAddingViewModel context => new DictionaryAddingWindow{DataContext = context},
            DictionaryChoiceViewModel context => new DictionaryChoice {DataContext = context},
            DictionaryChangingViewModel context => new DictionaryChanging() {DataContext = context}
        };
    }
}