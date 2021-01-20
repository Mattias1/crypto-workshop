Crypto workshop
================

Modern cryptography is so strong that you'll need more computing power than that there are atoms in
the universe to break it. Well ... when properly implemented that is. That, however, is not an easy
thing to do.
Hence the advice to use well known, audited, open source standards and libraries.

In this workshop we'll deliberately ignore that advice, naively implement 'unbreakable crypto'
ourselves and then break it.

I got these challenges from https://cryptopals.com/, the code and text provided here is just a way
to get you up to speed quickly so that you can do a few of the challenges in workshop format.


What do you need to participate
--------------------------------
- Be able to write and compile C# code (C#8, asp.net core 3.1)
- Have a bit of knowledge about cryptography. If you don't know the difference between encryption
  and hashing or don't like manipulating byte arrays, maybe this is not for you.


Setup
------
The provided code consists of two parts:
- The secure server. This is where the crypto is implemented and exposed via safe API's.
  It runs at http://localhost:5000.
- The attack platform. This is just a unit test project that starts the server for you to do API calls.

You can find the challenges in [docs/workshop.md](/docs/workshop.md)

Goodluck and have fun!




OLD STUFF IN DUTCH:
====================


TODO's:
--------
- Do 1st challenge, get a tester???
- Readme's
- Sha1 extension
- Timing attack


Deelname eisen / huiswerk:
---------------------------
- Wees niet bang voor byte-arrays
- Weet dat encryptie algoritmes (nouja, degene die we hier gaan gebruiken, AES) in blokken werken
- Ken de begrippen "plaintext" en "ciphertext"
- Weet het verschil tussen een encryptie-algoritme en een hash-algoritme
- Kan C#8 en asp.net core 3.1 compileren (aka, installeer visual studio of vs-code oid)


Intro:
-------
## Maak en kraak je eigen crypto

In deze workshop gaan we op een naieve manier zelf onze crypto in elkaar zetten, en daarna ontdekken
waarom dat misschien niet zo'n goed idee was ;)

### Voordat je inschrijft:
- Dit is erg technisch! Als je niet het verschil weet tussen encryptie en hashing dan is dit
  waarschijnlijk niet voor jou.
- Zorg dat je voor deelname je ontwikkel omgeving gereed hebt: je moet C# 8 en asp.net core 3.1
  kunnen compileren. Op windows kan dat makkelijk door de laatste visual studio te installeren. Op
  linx/mac kan dat bijvoorbeeld via vs code (let op, je hebt daar wel plugins voor nodig!)
- Maximaal 20 inschrijvingen

**Let op! Deze duurt tot 16:00 uur!**
