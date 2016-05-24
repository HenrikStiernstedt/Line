

Liner
========================

Syfte och mål:
Rita linjer mellan parvisa punkter. Linjerna får aldrig korsas.

Förslag 1:
Antag att linjer får mötas i hörnen. Det gör att man kan rekursivt dela upp varje skärning i två nya linjer via någon av ändpunkterna på den linje man korsat.

Förslag 2:
Utgå från förslag 1 ovan, men utöka med lite padding kring hörnen så att linjerna inte korsar. Kan leda till knepiga situationer där linjerna inte får plats. Oklart hur det skall lösas.

Problem:
Vad händer om en linje inte går att rita ut utan att den korsar befintlig linje?

Förslag 1: 

Rita inte ut linjen. Visa för användaren att det inte går.

Förslag 2:
Flytta på befintlig linje, så att den istället får runda den nya linjens ena ändpunkt istället. Rekursera... Det borde kunna gå, tycker jag, men kräver lite analys. 

Källor och ideér:
-----------------

* Få igång en WPF-app och rita linjer vid musevents:
http://stackoverflow.com/questions/16037753/wpf-drawing-on-canvas-with-mouse-events

* Algoritm för att se om linjer korsar varandra:
http://dotnetbyexample.blogspot.se/2013/09/utility-classes-to-check-if-lines-andor.html


Arbetslogg:
-----------
2016-05-16 Måndag: Första dagen. fått en WPF-applikation som ritar ut linjer.
2016-05-20 fredag: Ett första utkast som föutsätter att varje linje får korsas i ändpunkterna. Korsar dock även sammansatta lijner i skarvarna.
2016-05-22 söndag: Andra utkast som försöker runda befintliga linjer med marginaler. Algoritmen har dock svårt at avgöra när den kört in i en återvändsgränd.
2016-05-23 måndag: Sammanställning av läget. Test av att spara befintlig väg. Tittar dock inte alltid på rätt linje och misslyckas därmed ibland.
2016-05-24 tisdag: Planen är att välja det andra alternativet runt ett hinder om det första misslyckas. 