# Földrengések – adatbázis feladat (Digitális kultúra, 2024. május)

Ez a repository a **„Földrengések”** adatbázis-kezelési feladat megoldásához készült.  A feladat megoldását ASP.NET Core MVC projektben valósítjuk meg, emiatt a napló adattábla idegen kulcsának (telepid) nevét TelepulesID-nek neveztem el a névkonvenció miatt (bár Entity Framework Annotation segítségével is megoldható lenne, de így találtam egyszerűbbnek).
A feladat magyarországi, **1995–2020** közötti kiindulású földrengések adataira épül, amelyek két UTF-8 kódolású, tabulátorral tagolt szövegfájlban érkeznek.

## Forrásfájlok

- `naplo.txt` – földrengés-napló
- `telepules.txt` – településtörzs

> Mindkét fájl **tabulátorral tagolt**, **UTF-8** kódolású, és **az első sor mezőneveket tartalmaz**.

## Cél

1. Hozz létre egy `renges` nevű adatbázist. (Megjegyzés: nálam az adatbázis neve inkább: )
2. Importáld a két szövegfájlt `naplo` és `telepules` táblákba. (Megjegyzés: az ASP.NET Core MVC névkonvenciói miatt naplok és telepulesek táblaneveket adok meg.) 
3. Állítsd be a megfelelő adattípusokat és elsődleges kulcsokat.
4. Készítsd el a feladatban kért lekérdezéseket, és mentsd el őket a megadott neveken.

## Adatmodell

### `naplok` tábla

Mezők: `id, datum, ido, telepulesid, magnitudo, intenzitas`

- `id` – földrengés azonosító (szám), **elsődleges kulcs**
- `datum` – dátum
- `ido` – időpont
- `telepulesid` – a legközelebbi település azonosítója (szám)
- `magnitudo` – valós szám, **2 tizedes**, üres lehet ha ismeretlen
- `intenzitas` – valós szám, **2 tizedes**, üres lehet ha ismeretlen

### `telepulesek` tábla

Mezők: `id, nev, varmegye`

- `id` – település azonosító (szám), **elsődleges kulcs**
- `nev` – település neve (szöveg)
- `varmegye` – vármegye (szöveg)

### Kapcsolat (logikai)

- `naplok.telepulesid` → `telepulesek.id`

## Feladatok / Lekérdezések

A lekérdezéseket a zárójelben megadott néven kell létrehozni a metódusneveket, vagy a feladatok sorszámával jelölhetjük őket.

### 2) Somogy vármegye települései (2somogy)
Listázd ábécérendben az adatbázisban szereplő **Somogy vármegyei települések nevét**, és **csak** ezt az egy mezőt jelenítsd meg.

### 3) Földrengések száma vármegyénként (3darabszam)
Add meg, hogy az egyes vármegyékhez **hány feljegyzett földrengés** tartozik.  
A lista legyen **darabszám szerint csökkenő**, és jelenjen meg a **vármegye neve** is.

### 4) Legnagyobb magnitúdójú földrengés (4legnagyobb)
Írasd ki a **legnagyobb magnitúdójú** földrengés:
- településnevét
- dátumát
- időpontját
- magnitúdóját

Ha több ilyen rekord van, elég egyet megjeleníteni (de az összes is jó).

### 5) „Alig érzékelhető” rengések 2022-ben (5alig)
A **2,0 és 3,0 közötti (beleértve a határokat)** intenzitású rengések „alig érzékelhetők”.  
Add meg, hogy **2022-ben** mely településeken volt ilyen földmozgás.

A listában jelenjen meg:
- település neve
- dátum
- intenzitás

Rendezés: **dátum szerint növekvő**.

### 6) A 3 legaktívabb év (6aktivevek)
Sorold fel azt a **három évet**, amikor a legtöbb **3,0-nál nagyobb intenzitású** földrengés volt.

A listában jelenjen meg:
- évszám
- a feltételnek megfelelő rengések száma

Rendezés: **darabszám szerint csökkenő**.
