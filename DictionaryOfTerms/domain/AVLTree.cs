using System;
using System.Collections.Generic;

namespace DictionaryOfTerms.domain;

public class AVLTree
{
    public AVLNode Root { get; private set; } // Корень дерева.

    // Метод для получения высоты узла.
    private int Height(AVLNode node)
    {
        if (node == null)
            return 0;
        return node.Height;
    }

    // Метод для получения баланса узла.
    private int GetBalance(AVLNode node)
    {
        if (node == null)
            return 0;
        return Height(node.Left) - Height(node.Right);
    }

    // Метод дляu поворота вправо.
    private AVLNode RotateRight(AVLNode y)
    {
        AVLNode x = y.Left;
        AVLNode T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

        return x;
    }

    // Метод для поворота влево.
    private AVLNode RotateLeft(AVLNode x)
    {
        AVLNode y = x.Right;
        AVLNode T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

        return y;
    }

    // Метод для вставки узла.
    public AVLNode Insert(AVLNode node, string key, string definition)
    {
        if (node == null)
            return new AVLNode(key, definition);

        if (string.Compare(key, node.Key) < 0)
            node.Left = Insert(node.Left, key, definition);
        else if (string.Compare(key, node.Key) > 0)
            node.Right = Insert(node.Right, key, definition);
        else
            node.Definitions.Add(definition);

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

        int balance = GetBalance(node);

        // Если узел не сбалансирован, то проверяем нарушения и выполняем повороты.
        if (balance > 1 && string.Compare(key, node.Left.Key) < 0)
            return RotateRight(node);

        if (balance < -1 && string.Compare(key, node.Right.Key) > 0)
            return RotateLeft(node);

        if (balance > 1 && string.Compare(key, node.Left.Key) > 0)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        if (balance < -1 && string.Compare(key, node.Right.Key) < 0)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    // Обертка для вставки.
    public void Insert(string key, string definition)
    {
        Root = Insert(Root, key, definition);
    }
    
    // Рекурсивная функция для удаления узла.
    private AVLNode DeleteNode(AVLNode root, string key)
    {
        // Шаг 1: Обычное удаление узла.
        if (root == null)
            return root;

        // Если ключ меньше значения корня, идем влево.
        if (string.Compare(key, root.Key) < 0)
            root.Left = DeleteNode(root.Left, key);

        // Если ключ больше значения корня, идем вправо.
        else if (string.Compare(key, root.Key) > 0)
            root.Right = DeleteNode(root.Right, key);

        // Если ключ равен значению корня, этот узел нужно удалить.
        else
        {
            // У узла нет потомков или только один потомок.
            if (root.Left == null || root.Right == null)
            {
                AVLNode temp = null;
                if (temp == root.Left)
                    temp = root.Right;
                else
                    temp = root.Left;

                // Узел без потомков.
                if (temp == null)
                {
                    temp = root;
                    root = null;
                }
                else // Узел с одним потомком.
                    root = temp; 
            }
            else
            {
                // Узел с двумя потомками: найдем минимальный узел в правом поддереве.
                AVLNode temp = MinValueNode(root.Right);

                // Копируем значения из найденного узла в текущий узел.
                root.Key = temp.Key;

                // Удаляем найденный узел.
                root.Right = DeleteNode(root.Right, temp.Key);
            }
        }

        // Если дерево состоит из одного узла, просто вернем его.
        if (root == null)
            return root;

        // Шаг 2: Обновляем высоту текущего узла.
        root.Height = 1 + Math.Max(Height(root.Left), Height(root.Right));

        // Шаг 3: Получаем фактор баланса узла, чтобы проверить нарушение баланса.
        int balance = GetBalance(root);

        // Если узел не сбалансирован, то проверяем нарушения и выполняем повороты.
        // Левый случай.
        if (balance > 1 && GetBalance(root.Left) >= 0)
            return RotateRight(root);

        // Правый случай.
        if (balance < -1 && GetBalance(root.Right) <= 0)
            return RotateLeft(root);

        // Лево-правый случай.
        if (balance > 1 && GetBalance(root.Left) < 0)
        {
            root.Left = RotateLeft(root.Left);
            return RotateRight(root);
        }

        // Право-левый случай.
        if (balance < -1 && GetBalance(root.Right) > 0)
        {
            root.Right = RotateRight(root.Right);
            return RotateLeft(root);
        }

        return root;
    }

    // Метод-обертка для удаления узла.
    public void Delete(string key)
    {
        Root = DeleteNode(Root, key);
    }

    // Метод для поиска узла с минимальным значением в дереве.
    private AVLNode MinValueNode(AVLNode node)
    {
        AVLNode current = node;

        // Ищем самый левый узел, он будет содержать минимальное значение.
        while (current.Left != null)
            current = current.Left;

        return current;
    }

    // Рекурсивная функция для поиска определений по ключу (точное совпадение).
    private List<string> SearchHelper(AVLNode node, string key)
    {
        if (node == null)
            return null;

        int compare = key.CompareTo(node.Key);
        if (compare == 0)
            return node.Definitions;
        if (compare < 0)
            return SearchHelper(node.Left, key);
        return SearchHelper(node.Right, key);
    }
    
    // Рекурсивная функция для поиска терминов по подстроке (все совпадения).
    private void SearchHelper(AVLNode node, string substr, List<string> results)
    {
        if (node != null)
        {
            if (node.Key.ToLower().Contains(substr.ToLower()))
            {
                results.Add(node.Key);
            }
            SearchHelper(node.Left, substr, results);
            SearchHelper(node.Right, substr, results);
        }
    }

    // Функция для поиска терминов по подстроке.
    public List<string> SearchBySubstring(string substr)
    {
        List<string> results = new List<string>();
        SearchHelper(Root, substr, results);
        return results;
    }

    // Метод для поиска определений по ключу.
    public List<string> Search(string key)
    {
        return SearchHelper(Root, key);
    }

    // Рекурсивная функция для печати дерева в консоль.
    private void PrintHelper(AVLNode node, string indent, bool last)
    {
        if (node != null)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "| ";
            }

            Console.WriteLine(node.Key);

            PrintHelper(node.Left, indent, false);
            PrintHelper(node.Right, indent, true);
        }
    }

    // Функция для печати дерева в консоль.
    public void Print()
    {
        PrintHelper(Root, "", true);
    }
}