# Crypto workshop

Wat leuke cryptopals-challenges met wat code om je op weg te helpen.

Deelname eisen / huiswerk:
---------------------------
- Wees niet bang voor byte-arrays
- Weet dat encryptie algoritmes (nouja, degene die we hier gaan gebruiken, AES) in blokken werken
- Ken de begrippen "plaintext" en "ciphertext"
- Weet het verschil tussen een encryptie-algoritme en een hash-algoritme
- Kan C#7 compileren (aka, installeer visual studio 2017 of de laatste vs-code)

ECB decryption:
----------------
Inleiding: er is een programma dat een geheim bericht verstuurt, waar je wat opmerkingen aan de voorkant aan toe kan
voegen als je wilt

Opdracht: vind de inhoud van het geheime bericht zonder de sleutel te weten

Formaat van het bericht: `<opmerkingen><het geheime bericht>`
Detail: Het is ge-encrypt met AES in ECB mode, gepad met 0-bytes.

Hints:
- AES encrypt de tekst in blokken van 16 bytes; ECB mode betekend dat twee dezelfde blokken plaintext naar dezelfde
  ciphertext ge-encrypt worden.
- Als de userinput 15 bytes lang is, wat stelt de 16e byte dan voor?
- Decrypt 1 byte per 'keer'
- Kan je de lengte van het geheime bericht te weten komen? (Evt. met padding bytes)
- Maak je niet druk om de encryptie-sleutel.

Met dank aan: http://cryptopals.com/sets/2/challenges/12

Forge webtoken hash:
---------------------
Inleiding: er is een webtoken { user=henk, role=normaluser }, met de bijbehorende hash (SHA1-HMAC) om te valideren of
er niet met de webtoken is gerommeld

Opdracht: forge een webtoken met role=admin en de bijbehorende hash.

Detail: Een HMAC is een hash gebaseerd op het webtoken en een geheime sleutel. Dit is in principe de juiste manier om
het webtoken te verifieren (behalve dat je wss geen SHA-1 meer wilt gebruiken), maar het timingleak maakt hem dus stuk.

Hints:
- De hash wordt gecontroleerd met een foreach waarin hij direct `false` returned als hij fout is. Daardoor
  lekt de hash-check informatie. Als de eerste byte van de hash fout is, is de 'verstreken tijd' waarschijnlijk kleiner
  dan dat als de tweede byte fout is.
- Er zijn soms 'latency problemen', waardoor een fout in de eerste byte toch wel langer kan duren dan een fout in de
  tweede byte
- (merk op: dit is een simulatie - in het echt zijn timing aanvallen een stuk moeilijker (maar nog steeds mogelijk!))

Met dank aan: http://cryptopals.com/sets/4/challenges/31
