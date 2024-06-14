namespace DictionaryOfTerms.domain;

// Класс, представляющий пользователя
public class User
{
    public string Login { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}