# OUTLOUD_Test_proj

2 версії API. Перша використовує авторизації на основі збережених в БД токенів юзерів. Друга використовує Bearer авторизацію.
В кожній версії є два контролера: Login відповідає за верифікацію та отримання токену з бази даних; RSSFeed відповідає за взаємодію з RSS.

DB: MySQL.
Сорока підключення в appsettings.json. Версія бд в Models/ApplicationContext.cs

Нижче наведено дані з якими ініціалізується БД.

````
List<RSS> rssList = new()
{
    new RSS { Title = "1 feed", Content = "Description of the 1 feed", Link = url, Id = 1, Created = DateTime.Now, IsRead = false },
    new RSS { Title = "2 feed", Content = "Description of the 2 feed", Link = url, Id = 2, Created = DateTime.Now.AddDays(-1), IsRead = false },
    new RSS { Title = "3 feed", Content = "Description of the 3 feed", Link = url, Id = 3, Created = DateTime.Now.AddDays(-2), IsRead = false },
    new RSS { Title = "4 feed", Content = "Description of the 4 feed", Link = url, Id = 4, Created = DateTime.Now.AddDays(-3), IsRead = false }
};

List<User> userList = new()
{
    new User { Id = 1, Login = "FirstUser", Password = "qwe" },
    new User { Id = 2, Login = "SecondUser", Password = "asd" }
};
````
Many-to-many:
````
modelBuilder.Entity<User>()
    .HasMany(p => p.RSSList)
    .WithMany(p => p.Users)
    .UsingEntity(j => 
        j.HasData(
            new { UsersId = 1, RSSListId = 1 },
            new { UsersId = 1, RSSListId = 2 },
            new { UsersId = 1, RSSListId = 3 },
            new { UsersId = 1, RSSListId = 4 },
            new { UsersId = 2, RSSListId = 1 },
            new { UsersId = 2, RSSListId = 2 },
            new { UsersId = 2, RSSListId = 3 },
            new { UsersId = 2, RSSListId = 4 }
));
````

Пояснення щодо ендпоінтів:
````
SetRssAsRead
  Приймає link.
  Link не є обов'язковим.
  При його відсутності всі RSS юзера становляться IsRead = false;
````

````
GetUnreadRSSWithDate
  Приймає date, addDays.
  Date можна залишити по замовчуванню.
  addDays змінює дату на необхідну кільіксть днів: може бути позитивним та негативним.
````

````
AddRss
  Приймає link.
  Інші поля моделі задаються по замовчуванню.
  Додається тільки для авторизованого юзера.
````

Для старіших версій .net я бачив приклади візуалізації RSS. Нажаль, з .net7.0 аналогів не знайшов.
