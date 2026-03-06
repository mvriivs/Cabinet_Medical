Medical Clinic Management System
Descriere Proiect
O aplicație web Full-Stack dezvoltată în mediul ASP.NET pentru digitalizarea operațiunilor dintr-un cabinet medical. Sistemul integrează managementul pacienților, programările și istoricul clinic al consultațiilor într-o arhitectură unitară.

Arhitectură Tehnologică
Limbaj de programare: C# (.NET Framework).

Tehnologie interfață: ASP.NET Web Forms, HTML5, CSS3.


Sistem baze de date: Microsoft SQL Server.

Acces date: ADO.NET folosind SqlConnection și SqlDataAdapter.

Descrierea Fișierelor

Cabinet_Medical_Maria_Salau.sln: Fișierul de soluție Visual Studio care gestionează structura întregului proiect și dependențele acestuia.

Login.aspx / Login.aspx.cs: Modulul de securitate care implementează autentificarea medicilor pe bază de Email și CNP, utilizând obiecte de tip Session pentru controlul accesului.

Menu.aspx / Menu.aspx.cs: Panoul central de navigare implementat cu un layout CSS Grid, oferind acces rapid către modulele Pacienți, Programări și Consultații.

Pacienti.aspx / Pacienti.aspx.cs: Interfața pentru gestionarea datelor pacienților, incluzând operații de inserare, actualizare și ștergere (CRUD), corelate cu medicul de familie asociat.

Consultatii.aspx / Consultatii.aspx.cs: Modulul pentru evidența clinică unde se înregistrează diagnosticul și parametrii vitali (tensiune, greutate, puls).

Statistici.aspx / Statistici.aspx.cs: Implementarea logicii de raportare avansată ce utilizează interogări SQL complexe, subcereri (Subqueries) și funcții de agregare.


script_cabinet.sql: Scriptul SQL pentru definirea schemei bazei de date relaționale, incluzând tabelele Medici, Pacienti, Programari, Consultatii, Retete și Tratamente.

Funcționalități Implementate

Management Relațional: Organizarea datelor într-o structură normalizată cu integritate referențială prin constrângeri SQL.

Securitate Sesiuni: Verificarea stării de autentificare la încărcarea fiecărei pagini protejate pentru a preveni accesul neautorizat.

Raportare Avansată: Interogări de tip JOIN între tabele multiple pentru generarea istoricului medical complet și a statisticilor de performanță.

Interfață Responsivă: Utilizarea unităților de măsură flexibile și a containerelor moderne (Flexbox/Grid) pentru adaptarea pe diferite rezoluții de ecran.
