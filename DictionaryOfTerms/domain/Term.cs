using System.Collections.Generic;

namespace DictionaryOfTerms.domain;

// Класс, представляющий термин с определением для удобного обмена данными при сохранении информации в файл.
public class Term
{
    public string Name { get; set; }
    public List<string> Definitions { get; set; }
}