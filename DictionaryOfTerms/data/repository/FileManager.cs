using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using DictionaryOfTerms.domain;
using Newtonsoft.Json;

namespace DictionaryOfTerms.data.repository;


// Класс, осуществляющий чтение и запись терминов из файла.
public class FileManager
{
    // Метод для чтения терминов из JSON файла
    public List<Term> ReadTerms(string filePath)
    {
        List<Term> terms = new List<Term>();

        try
        {
            string jsonText = File.ReadAllText(filePath);
            if(!string.IsNullOrEmpty(jsonText)) terms = JsonConvert.DeserializeObject<List<Term>>(jsonText);
            //return terms;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading terms from {filePath}: {ex.Message}");
        }

        return terms;
    }
    public List<Dictionary> ReadDictionary(string filePath)
    {
        List<Dictionary> dictionaries = new List<Dictionary>();

        try
        {
            string jsonText = File.ReadAllText(filePath);
            dictionaries = JsonConvert.DeserializeObject<List<Dictionary>>(jsonText);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading dictionaries from {filePath}: {ex.Message}");
        }

        return dictionaries;
    }
    
    // Метод для записи терминов в JSON файл
    public void WriteTerms(List<Term> terms, string filePath)
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(terms, Formatting.Indented);
            File.WriteAllText(filePath, jsonText);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing terms to {filePath}: {ex.Message}");
        }
    }

    public void WriteDictionary(Dictionary dictionary)
    {
        var allDictionaries = File.ReadAllText(
            "C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json");
        var dictionariesList = JsonConvert.DeserializeObject<List<Dictionary>>(allDictionaries);
        dictionariesList?.Add(dictionary);
        var jsonText = JsonConvert.SerializeObject(dictionariesList, Formatting.Indented);
        File.WriteAllText("C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json", jsonText);
    }
    //Метод для записи словаря в JSON файл
    public void WriteDictionary(string dictionaryName, string filePath)
    {
        try
        {
            Dictionary newDictionary = new Dictionary();
            newDictionary.Name = dictionaryName;
            newDictionary.Path = filePath + dictionaryName+".json";
            var allDictionaries = File.ReadAllText(
                "C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json");
            var dictionariesList = JsonConvert.DeserializeObject<List<Dictionary>>(allDictionaries);
            dictionariesList?.Add(newDictionary);
            var jsonText = JsonConvert.SerializeObject(dictionariesList, Formatting.Indented);
            File.WriteAllText("C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json", jsonText);
            File.Create(
                "C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\" +
                dictionaryName + ".json");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing terms to {filePath}: {ex.Message}");
        }
    }

    public void DeleteDictionaryWithoutDeletingFile(string dictionary)
    {
        var allDictionaries = File.ReadAllText(
            "C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json");
        var dictionariesList = JsonConvert.DeserializeObject<List<Dictionary>>(allDictionaries);
        Dictionary d = dictionariesList.Single(s => s.Name == dictionary);
        dictionariesList.Remove(d);
        var jsonText = JsonConvert.SerializeObject(dictionariesList, Formatting.Indented);
        File.WriteAllText("C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json", jsonText);

    }
    

    // Удаление словаря из JSON файла
    public void DeleteDictionary(string dictionary)
    {
        var allDictionaries = File.ReadAllText(
            "C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json");
        var dictionariesList = JsonConvert.DeserializeObject<List<Dictionary>>(allDictionaries);
        Dictionary d = dictionariesList.Single(s => s.Name == dictionary);
        dictionariesList.Remove(d);
        File.Delete(d.Path);
        var jsonText = JsonConvert.SerializeObject(dictionariesList, Formatting.Indented);
        File.WriteAllText("C:\\Users\\IRONIN\\RiderProjects\\DictionaryOfTerms-RGZ\\DictionaryOfTerms\\data\\data_set\\dictionaries.json", jsonText);

    }
    // Метод для чтения пользователя из JSON файла
    public List<User> ReadUser(string filePath)
    {
        List<User> users = new List<User>();

        try
        {
            string jsonText = File.ReadAllText(filePath);
            users = JsonConvert.DeserializeObject<List<User>>(jsonText);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading terms from {filePath}: {ex.Message}");
        }

        return users;
    }

    // Метод для записи пользователя в JSON файл
    public void WriteUser(List<User> users, string filePath)
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, jsonText);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing user to {filePath}: {ex.Message}");
        }
    }
}

