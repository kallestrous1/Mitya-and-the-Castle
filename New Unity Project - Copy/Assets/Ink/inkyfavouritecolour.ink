VAR hasRing = false
VAR knotProgression = -> Start

-> knotProgression

=== Start ===
Hello grandson how are you?
    +[good]
    Oh that's wonderful to hear
    ->MissingRing
    +[bad]
    Oh man that sucks
    ->MissingRing
    
===MissingRing===
It seems I've lost my ring while I was out gathering mushrooms, would you go find it for me?
    +[Of course]
    ->WaitingForRing

===WaitingForRing===
~knotProgression = -> WaitingForRing
- have you found my ring?
{hasRing:
    Oh wonderful!
    ->test
  - else:
    Sigh, hopefully it'll turn up soon
    ->MissingRing
}

===test===
~knotProgression = -> test
Mhm what a wonderful day we're having isnt it?
->END


