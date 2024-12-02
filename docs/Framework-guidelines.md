# Struktura projektu 
   
/                   root složka repozitáře 
└── build/          skripty a konfigurace pro build 
└── deploy/         skripty a konfigurace pro deploy 
├── docs/           dokumentace 
├── src/            všechny zdrojové kódy 
│   └── apps/       frontend aplikace 
│   └── infra/      infrastrukturní komponenty 
│   └── services/   backend mikroslužby 
│   └── shared/     sdílené knihovny a komponenty 
│   └── tests/      testovací projekty

**build** -  složka obsahuje skripty (bash, powershell) a yaml konfigurace pro build a deploy 
**doc** – složka obsahující dokumentaci k projektu (pro vývojáře), ostatní dokumentace jsou na sdílených úložištích projektu mimo tuto strukturu 
**src** – složka obsahující všechny zdrojové kódy mikroservis, tzn. že vytváříme takzvané mono-repo neboli jednotný repositář obsahující zdrojové kódy více mikroservis v jednotné struktuře.    Na této úrovni držíme i jednotlivé .sln
**apps** – složka obsahující zdrojové kódy pro frontend projekty. Může obsahovat jednu nebo více aplikací v závislosti na projektu 
**infra** – složka obsahující infrastrukturní komponenty projektu, jako je například .net aspire nebo docker-compose složící k možnosti spustit si lokální instance mikroservis. Obsahuje také sdílené infrastukruní zdrojové kódy sloužící při startu mikroservis nebo jiných komponent systému 
**services** – jednotlivé backend mikroservisy 
**shared** – sdílené knihovny pro back-end a front-end mikroservisy 
**tests** – testovací projekty pro back-end mikroservisy

