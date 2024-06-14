using System.Collections.Generic;

namespace DictionaryOfTerms.domain;

// Класс, представляющий узел дерева.
public class AVLNode
{
    public string Key; // Ключ узла, который определяет его положение в дереве.
    public List<string> Definitions; // Список определений, связанных с данным ключом.
    public int Height; // Высота узла в дереве.
    public AVLNode Left; // Ссылка на левого потомка узла.
    public AVLNode Right; // Ссылка на правого потомка узла.

    public AVLNode(string key, string definition)
    {
        Key = key; 
        Definitions = new List<string>();
        Definitions.Add(definition); 
        Height = 1;
    }
}
