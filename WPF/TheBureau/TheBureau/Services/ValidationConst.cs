namespace TheBureau.Services
{
    public static class ValidationConst
    {
        public const string LettersHyphenRegex = @"^[а-яА-Я-]+$";
        public const string LettersHyphenDigitsRegex = @"^[а-яА-Я0-9-]+$";
        public const string LoginRegex = @"^[a-zA-Z0-9]+$";
        public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string ContactNumberRegex = @"^375((17|25|29|33|44))[0-9]{7}$";

        public const string SomethingWentWrong = "Что-то пошло не так...";
        public const string PasswordLengthExceeded = "Пароль должен быть от 5 до 20 символов";
        public const string WrongLoginOrPassword = "Неверный логин или пароль";

        public const string LoginEmpty = "Введите логин";
        public const string LoginLengthExceeded = "Превышена максимальная длина логина";
        public const string IncorrectLoginStructure = "Логин может состоять лишь из букв латинского алфавита и цифр";
        
        public const string FieldCannotBeEmpty = "Поле не может быть пустым";
        public const string EmailLengthExceeded = "Превышена максимальная длина адреса почты (255)";
        public const string NameLengthExceeded = "Длина поля должна находиться в пределах от 2 до 20 символов";
        public const string CommentLengthExceeded = "Максимальное количество символов в комментарии к заявке - 200";
        public const string QuantityExceeded = "Количество оборудования не может превышать 100";
        public const string IncorrectExceeded = "Номер корпуса не может быть содержать более 10 символов";
        public const string MaxLength30 = "Максимальная длина поля- 30 символов";
        
        public const string IncorrectEmailStructure = "Некорректная структура адреса электронной почты";
        public const string IncorrectNumberStructure = "Номер телефона должен начинаться с 375, иметь один из зональных кодов: 17, 25, 29, 33, 44 и содержать 12 цифр";
        public const string IncorrectFirstname = "Имя может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectSurname = "Фамилия может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectPatronymic = "Отчество может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectCity = "Название города может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectStreet = "Название улицы может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectHouse = "Номер дома должен быть от 1 до 300";
        public const string IncorrectCorpus = "Номер корпуса может состоять лишь из букв кириллицы и знака \"-\"";
        public const string IncorrectFlat = "Номер квартиры должен быть от 1 до 1011";
        public const string IncorrectPassword = "Длина пароля должна быть от 8 до 40 символов";
        
        // public const string EmailRegex = "[.\\-_a-zA-Z0-9]+@([a-zA-Z0-9][\\-a-zA-Z0-9]+\\.)+[a-zA-Z]{2,6}";

    }
}