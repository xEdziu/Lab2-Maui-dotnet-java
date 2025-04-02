# Aplikacja do zarządzania użytkownikami i postami w .NET MAUI
> Autor: Adrian Goral

## Wstęp
Celem projektu jest stworzenie aplikacji w technologii .NET MAUI, która umożliwia zarządzanie użytkownikami i ich postami. Projekt obejmuje integrację z zewnętrznym API, obsługę lokalnej bazy danych SQLite, implementację graficznego interfejsu użytkownika (GUI) oraz testowanie aplikacji.

## Opis funkcjonalności
Aplikacja pozwala użytkownikowi na:
1. Pobieranie danych użytkowników i postów z zewnętrznego API (`https://jsonplaceholder.typicode.com`).
2. Przeglądanie listy użytkowników oraz ich postów.
3. Dodawanie nowych użytkowników.
4. Usuwanie istniejących użytkowników wraz z ich postami.
5. Przechowywanie danych w lokalnej bazie SQLite.

## Struktura projektu
Projekt składa się z następujących komponentów:
1. **Modele danych**:
   - [`User`](MauiApp1/User.cs) - Klasa reprezentująca użytkownika pobranego z API.
   - [`Post`](MauiApp1/Post.cs) - Klasa reprezentująca post pobrany z API.
   - [`UserEntity`](MauiApp1/UserEntity.cs) - Klasa reprezentująca użytkownika w lokalnej bazie danych.
   - [`PostEntity`](MauiApp1/PostEntity.cs) - Klasa reprezentująca post w lokalnej bazie danych.
   - [`ApiState`](MauiApp1/ApiState.cs) - Klasa przechowująca stan pobierania danych.

2. **Baza danych**:
   - [`AppDbContext`](dotnetLab2/AppDbContext.cs) - Klasa kontekstu bazy danych SQLite, zarządzająca tabelami użytkowników, postów i stanu API.

3. **Interfejs użytkownika (GUI)**:
   - [`MainPage.xaml`](MauiApp1/MainPage.xaml) - Główna strona aplikacji, zawierająca formularz dodawania użytkownika oraz listę użytkowników.
   - [`AppShell.xaml`](MauiApp1/AppShell.xaml) - Struktura nawigacji w aplikacji.

4. **Logika aplikacji**:
   - [`MainPageViewModel`](MauiApp1/ViewModels/MainPageViewModel.cs) - Klasa ViewModel implementująca logikę pobierania i zarządzania danymi użytkowników.
   - [`MainPage.xaml.cs`](MauiApp1/MainPage.xaml.cs) - Klasa kodu zaplecza dla głównej strony aplikacji, odpowiedzialna za interakcję z GUI.

5. **Konfiguracja aplikacji**:
   - [`MauiProgram.cs`](MauiApp1/MauiProgram.cs) - Konfiguracja aplikacji .NET MAUI, w tym rejestracja usług i bazy danych.

## Działanie programu
Po uruchomieniu aplikacji **konsolowej** użytkownik może:
1. Przeglądać listę użytkowników i ich postów.
2. Dodawać nowych użytkowników za pomocą formularza.
3. Usuwać użytkowników wraz z ich postami.

Po uruchomieniu aplikacji **okienkowej**, użytkownik może:
1. Przeglądać listę użytkowników w interfejsie graficznym.
2. Dodawać nowych użytkowników za pomocą formularza w GUI.
3. Usuwać użytkowników z listy.

Dane są pobierane z API podczas pierwszego uruchomienia aplikacji konsolowej i zapisywane w lokalnej bazie SQLite. W przypadku braku danych w bazie, aplikacja automatycznie pobiera je z API.

## Wykorzystane technologie
- .NET 8.0
- MAUI
- C#
- SQLite (Entity Framework Core)
- XAML (do tworzenia GUI)

## Podsumowanie
Projekt stanowi przykład aplikacji mobilnej stworzonej w technologii .NET MAUI, która integruje się z zewnętrznym API, obsługuje lokalną bazę danych SQLite i oferuje graficzny interfejs użytkownika. Aplikacja jest rozszerzalna i może być rozwijana o dodatkowe funkcjonalności, takie jak testy jednostkowe czy zaawansowane opcje filtrowania danych.