# Zastosowanie syntetycznych danych w uczeniu maszynowym - detekcja i klasyfikacja obiektów

Repozytorium zawiera kody źródłowe opracowane w ramach pracy inżynierskiej, której celem było wygenerowanie syntetycznego zbioru danych w środowisku Unity oraz wykorzystanie go do trenowania modelu detekcji obiektów YOLOv8. Projekt koncentruje się na automatycznym generowaniu zróżnicowanych scen, ich randomizacji oraz eksporcie adnotacji zgodnych z formatem YOLO.

Udostępniony kod umożliwia odtworzenie kluczowych elementów procesu generowania danych syntetycznych, jednak repozytorium **nie stanowi kompletnego projektu Unity** i wymaga integracji z istniejącą sceną oraz pakietem Unity Perception.

---

## Zakres projektu

Projekt obejmuje:
- generowanie syntetycznych obrazów obiektów (owoców) w scenie 3D,
- losową modyfikację położenia, rotacji, skali oraz liczby instancji obiektów,
- randomizację oświetlenia oraz tła,
- automatyczne etykietowanie obiektów,
- konwersję adnotacji z formatu Unity Perception do formatu YOLO,
- przygotowanie danych do trenowania modelu YOLOv8.

--- 

## Opis kluczowych komponentów

### Randomizery Unity (C#)

Pliki w katalogu `unity/randomizers/` są przeznaczone do użycia z pakietem **Unity Perception** i dziedziczą po klasie `Randomizer`.

#### `ObjectRandomizer.cs`
- losuje liczbę instancji obiektów dla każdej klasy,
- losuje pozycję obiektów w przestrzeni widoku kamery,
- stosuje losową rotację i skalę zależną od klasy obiektu,
- wymusza minimalną odległość pomiędzy obiektami, aby zapobiec ich nachodzeniu.

#### `LightRandomizer.cs`
- modyfikuje jasność oraz odcień materiałów obiektów,
- realizuje symulację zmiennych warunków oświetleniowych,
- wykorzystuje przestrzeń barw HSV do kontrolowanej randomizacji.

#### `BackgroundRandomizer.cs`
- losowo zmienia tekstury tła sceny,
- umożliwia symulację różnych środowisk (np. kuchnie, blaty, stoły),
- zwiększa realizm i różnorodność wizualną danych syntetycznych.

---

### Skrypty pomocnicze (Python)

Pliki w katalogu `python/` służą do przetwarzania danych wygenerowanych przez Unity.

#### `json_to_yolo.py`
- konwertuje adnotacje wygenerowane przez Unity Perception (JSON)
  do formatu YOLO (`.txt`),
- przelicza współrzędne ramek ograniczających na postać znormalizowaną,
- generuje pliki etykiet kompatybilne z YOLOv8.

#### `utils.py`
- funkcje pomocnicze do obsługi ścieżek, walidacji danych
  oraz przetwarzania zbiorów obrazów.

---

## Wymagania środowiskowe

### Unity
- Unity 2021 LTS lub nowsze
- Pakiet **Unity Perception**
- Universal Render Pipeline (URP)

### Python
- Python ≥ 3.8
- `numpy`
- `opencv-python`
- `tqdm`

---

## Integracja z projektem Unity

1. Utwórz scenę w Unity zawierającą:
   - kamerę z komponentem `Perception Camera`,
   - etykietowane obiekty (`Labeling`),
   - światło kierunkowe,
   - obiekt tła (np. Quad).

2. Dodaj wybrane randomizery do `Scenario` w Unity Perception.

3. Skonfiguruj zakresy losowości w Inspector Unity.

4. Uruchom generowanie danych poprzez `Run in Editor` lub `Run in Player`.

---

## Uwagi końcowe

Repozytorium udostępnia kluczowe elementy implementacyjne, jednak nie zawiera kompletnego projektu Unity ani gotowej konfiguracji treningu YOLO. Kod należy traktować jako materiał badawczy oraz referencyjny, przeznaczony do dalszej adaptacji i rozbudowy.
