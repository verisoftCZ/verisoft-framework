#Get endpointy

**.../Records/{NázevČíselníku}/{stringValue}/Detail**
- Vrátí detail pro danou StringValue a všechny tenanty a překlady (stejný contract jako pro POST)

**.../Records/{NázevČíselníku}**
- Filtrovaná paged data zejména pro FE - vrací se jen záznamy, které jsou validní pro daného tenanta (bez forbidden - vysvětleno v dalším bodu) s jediným překladem na záznam dle požadovaného jazyka
- Forbidden záznamy existují z důvodu zpětné kompatibility - v tomto endpointu nejsou nabízeny, aby je uživatel nemohl vybrat.
-- Uživatel zvolí jako preferovanou formu komunikace Fax, ale ten později přestane být aktuální a nechceme jej dále nabízet jako možnou volbu, přesto, když detail uživatele zobrazím, chceme, aby byl fax stále viditelný, ale pro nové záznamy jej neumožníme nastavit.

**.../Records/{NázevČíselníku}/{stringValue}**
- Jeden záznam, který je validní pro daného tenanta (vč. forbidden - vysvětleno v dalším bodu) s jediným překladem na záznam dle požadovaného jazyka (optional query param languageId)
- Forbidden záznamy se vrací z důvodu zpětné kompatibility
-- Uživatel zvolí jako přeferovanou formu komunikace Fax, ale ten později přestane být aktuální a nechceme jej dále nabízet jako možnou volbu, přesto, když detail uživatele zobrazím, chceme, aby byl fax stále viditelný, ale pro nové záznamy jej neumožníme nastavit.

**.../Records/{NázevČíselníku}/DetailList**
- Filtrovaná Paged data s detaily pro každou StringValue (kolekce stejných contractů jako pro POST)

**.../Records/{NázevČíselníku}/{stringValue}/IsAvailable**
- Boolean, zda je daná stringValue k dispozici (existuje pro daného tenanta a není forbidden)
- Slouží k BE validaci vstupů