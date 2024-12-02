# Projekt
## NX
* Monorepo
* Jak začít projekt
* Kód v knihovnách
## Struktura projektu
* Pomocí NX
* Inspirace z Enterprise Angular Architecture od Manfred Steir
* Složky apps a libs
* Apps je pouze shell
* adresar libs obsahuje moduly
* modul členěna na
  * api - veřejně přístupné api
  * domain - entity, services
  * feature-.. - jednotlivé feature
  * feature-shell 
  * state - state management
  * ui - prezentační a znovupoužitelné prvky pro modul
## Architektura
### Dumb vs Containers
Dělení na komponenty
#### Dump (prezentační komponenty)
* Co nejvíce v prezentačních komponentách
* Závislé na vstupech a výstupech

#### Container componenty
### Behavior v directives
### State a view facade
* Použití NgRx
* Typizované úlohy jsou odstíněny pomocí wrapperu v knihovnách
* Požití view facade
### Verisoft knihovny
* Libs
   * Core
   * State
   * UI 
* v NPM
