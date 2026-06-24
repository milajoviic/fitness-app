Aplikacija cija je namena pracenje treninga korisnika. 
Koristi Cassandra bazu podataka za skladistenje velike kolicine vremenski orijentisanih podataka o treninzima 
pri cemu pokriva evidenciju svakog treninga, 
detalja o konkretnim vezbama, pregledima, ishrani i napretku kroz vreme.

Jedan tip korisnika -- korisnik sam. Sam korisnik zapisuje i prati svoje treninge svaki dan i dane kad nemaju treninge (odmor). Korisnik moze 
da dodaje, brise i menja treninge. U treninzima dodaje vezbe, kilazu, ponavljanja, vreme pauze. Pored toga, unosi podatke o sebi i svom
zdravlju -- kilazu, obime delova tela, zdravstveno stanje + period menstruacije za zene (mozda poruke za delove meseca).
Dodatno, podaci o ishrani svakog dana plus dodaci u ishrani poput vitamina, kreatina, proteina itd.
Korisnik se registruje na aplikaciju i dodaje podatke poput godina, imena, prezimena, email adrese, pola (ako je zena, datum poslednje
menstruacije). Kad se registruje, moze da doda informacije poput postojanja hronicnih bolesti ili problema sa zdravljem. Pored toga,
moze da unosi i navodi gore navedene informacije o treninzima, ishrani itd. Informacije za jednog korisnika se pamte u bazi i kad se izloguje
iz aplikacije. Kad se ponovo uloguje, svi podaci su tu.