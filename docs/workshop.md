Crypto workshop
================

There's a server that provides InternetTokens. Those are tokens with key-value data that you're
going to protect with encryption to either:
- Make sure the other guy can't read the contents
- Make sure the other guy can't forge a token that he shouldn't have

The format of the token: `email=<email>;role=<role>;secretMessage=<optional message>`

The email is input you can provide freely in the API; the role and message obviously not. If you
care, you can assume plain string encoding is UTF-8.


Challenge           | Description
------------------- | ------------
ECB decryption      | Decrypt a message without using the key!
Forge a hash        | Forge a token, abusing the inner working of the sha1 algorithm.
Side channel attack | Forge a token by comparing request times.


Challenge 1: ECB decryption
----------------------------
#### Part I: implement ECB

The AES algorithm encrypts just one block of 16 bytes (128 bits). So, if you want to encrypt
a message that's longer, you'll have to split it up and encrypt each block seperately. There's
different ways to do this (called modes of operation).

The easiest one is ECB (Electronic CodeBook). ECB mode doesn't require initialization vectors,
counters or anything else, it just encrypts the blocks as they are.
This can leak information, because if two blocks of the input happen to be the same, then they're
also the same in the (encrypted) output.

We don't care about that, and we'll 'protect' our token with AES in ECB mode anyways!

Hint: Look at TokenController.cs, InternetToken.cs and BlockCipher.cs to get you started.

#### Part II: decrypt the secret message

Once you've implemented this, you can send a POST to /token/new, with your favourite email address
to get a token. Just that is enough to decrypt it! No knowledge of the secret key is needed :)

Hints:
- Look at HackConsole.cs
- If you insert 32 A's as 'email address', you'll get two exactly the same blocks in the output right?
  Well, no, because your input isn't the first byte in the input - there's a small prefix first.
- Find a way to get the ciphertext (aka, the encrypted output) of 32 A's. If you add 16 more A's, you
  should have two the same blocks in the ciphertext now.
- What happends if you get the ciphertext of 31 A's, instead of 32? Is it still the same? Why?
  What do you need to do to make them the same anyways?
- Another way of thinking: encrypt a single block starting with 15 A's, and then encrypt a block
  with 16 A's (in a second run). Why are they different?
- Now decrypt the message.

Thanks to: http://cryptopals.com/sets/2/challenges/12


Challenge 2: Forge webtoken hash
---------------------------------
#### Part I: Implement SHA1 with a naive MAC

So, encrypting doesn't help. So we'll leave out the secret message and send the token in plaintext.
To make sure the token is valid, we'll add a digital signature with a message autentication code
(MAC).
The string we return after creating a new token now becomes: `<token>|<sha1(secret + token)`>

Hints:
- I already implemented SHA1 for you, see Sha1.cs, but do take a good look at how it works. You'll
  need it for part II.
- Use byte arrays as input for the sha1, not strings.

#### Part II: Modify the token and extend the signature

Append the admin role to your token and use the previous SHA1-MAC to generate a new signature to
validate your forged token.

Hints:
- If a token-string has two `role=<role>`'s, which one will it use?
- This is a bit annoying, because of the padding - you'll need to modify that a bit to include the
  right message length.
- You'll need to know the length of the previous hashed message. You know the length of the token,
  but do you know the length of the key? You can guess a few.
- Sha1 computes from left to right, so you can modify the sha1 hash a bit so that it continues from
  a previous hash, instead of starting from magic numbers.

Thanks to: https://cryptopals.com/sets/4/challenges/29

Fun fact: Sha1 is known to be insecure (don't ever use it anymore pls), but if you do this with
sha2, you're still vulnerable. The proper solution is to use a HMAC (for example https://jwt.io does
this), instead of just prefixing the secret to the hash.


Challenge 3: Sidechannel (timing) attack
-----------------------------------------
#### Note:
If you skipped challenge 2, you still need to secure the token with a sha1 hash. Part I of
challenge 2 contains some hints on that.

#### Part I: Implement SHA1 HMAC (optional) and a delayed check if the sha1 hash is valid.

We've seen from challenge 2 that using a straightforward MAC is insecure. So let's implemnt this
properly and use a HMAC. (This is optional - if you're short on time you can also just pretend that
your naive sha1-mac is secure).

Afterwards, implement an API endpoint that validates whether or not the sha1 hash is correct. Use an
early-exit strategy here: as soon as a byte in the given sha does not match the expected value,
return false right away without looking at the other bytes.
Also, add an artificial delay. After every byte checked, add a 50ms delay.

#### Part II: Find the correct signature for the following token:
`email=admin@example.com;role=admin`

Hints:
- Just try a hash, and use the API to validate wheter it's correct or not.
- Remember, that API has a timing leak :)

Too easy? See how small you can make your artificial delay and still 'break' it.

Thanks to: http://cryptopals.com/sets/4/challenges/31

Fun fact 1: exploiting timing leaks is really hard. Over the internet an attack like this may be
impossible. Still, attacks like these are being used IRL.

Fun fact 2: The Json Web Token standard (https://jwt.io) uses sha2 with an hmac. This is
algorithmically secure. But even if you make all the right choices, you can still make a mistake ;)
