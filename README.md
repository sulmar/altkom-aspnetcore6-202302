## Podstawy
### Komendy CLI

#### Środowisko
- ``` dotnet --version ``` - wyświetlenie aktualnie używanej wersji SDK
- ``` dotnet --list-sdks ``` - wyświetlenie listy zainstalowanych SDK
- ``` dotnet new globaljson ``` - utworzenie pliku _global.json_
- ``` dotnet new globaljson --sdk-version {version} ``` - utworzenie pliku _global.json_ i ustawienie wersji SDK

#### Projekt
- ``` dotnet new --list ``` - wyświetlenie listy dostępnych szablonów
- ``` dotnet new {template} ``` - utworzenie nowego projektu na podstawie wybranego szablonu, np. console, web
- ``` dotnet new {template} -o {output} ``` - utworzenie nowego projektu w podanym katalogu
- ``` dotnet restore ``` - pobranie pakietów nuget powiązanych z projektem
- ``` dotnet build ``` - kompilacja projektu
- ``` dotnet run ``` - uruchomienie projektu
- ``` dotnet watch run ``` - uruchomienie projektu w trybie śledzenia zmian
- ``` dotnet run {app.dll}``` - uruchomienie aplikacji
- ``` dotnet test ``` - uruchomienie testów jednostkowych
- ``` dotnet watch test ``` - uruchomienie testów jednostkowych w trybie śledzenia zmian
- ``` dotnet add reference {library.csproj} ``` - dodanie odwołania do biblioteki
- ``` dotnet remove reference {library.csproj} ``` - usunięcie odwołania do biblioteki
- ``` dotnet list reference ``` - lista wszystkich odwołań w projekcie
- ``` dotnet clean ``` - wyczyszczenie wyniku kompilacji, czyli zawartości folderu pośredniego _obj_ oraz folderu końcowego _bin_

#### Rozwiązanie
- ``` dotnet new sln ``` - utworzenie nowego rozwiązania
- ``` dotnet new sln --name {name} ``` - utworzenie nowego rozwiązania o określonej nazwie
- ``` dotnet sln add {folder} ``` - dodanie projektu z folderu do rozwiązania
- ``` dotnet sln remove {folder} ``` - usunięcie projektu z folderu z rozwiązania
- ``` dotnet sln add {project.csproj} ``` - dodanie projektu do rozwiązania
- ``` dotnet sln remove {project.csproj} ``` - usunięcie projektu z rozwiązania
- ``` dotnet sln list ``` - lista projektów w rozwiązaniu

#### Secret Keys
- ``` dotnet user-secrets init ``` - utworzenie sekretów
- ``` dotnet user-secrets set "{key}" "{value}" ``` - ustawienie wartości klucza
- ``` dotnet user-secrets list ``` - wyświetlenie listy kluczy i wartości
- ``` dotnet user-secrets remove "{key}" ``` - usunięcie wskazanego klucza
- ``` dotnet user-secrets clear ``` - usunięcie wszystkich kluczy