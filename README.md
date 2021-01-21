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
