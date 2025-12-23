INCLUDE GlobalInkVariables.ink

VAR knotProgression = -> Start

-> knotProgression

=== Start ===
Grandson how are you?
    +[good]
    Oh that's wonderful to hear
    ->MissingRing
    +[bad]
    Oh man that sucks
    ->MissingRing
    
===MissingRing===
We need to leave soon
it's getting dangerous
the kings road has fallen
but we need the ring
the activation ring for our mech
it's the only way we'll make it
you must retrieve it from the bandits who stole it
do you think you're up for it?
    +[Of course]
    ~knotProgression = -> WaitingForRing
    ->inBetween
    
===inBetween===
Anyways...
->WaitingForRing


===WaitingForRing===
- have you got the ring?
{hasRing:
    Oh wonderful!
    ->test
  - else:
    Sigh, hopefully it'll turn up soon
    ->inBetween
}

===test===
~knotProgression = -> test
I'm glad you're alright
We will depart soon
Come back in an hour
I will have everything ready
mhm?
+[I found the ring in your shed]
    What?
    how curious
    I mustve lost it
    what were you even doing back there
    Anyways it doesn't matter
    come back in an hour
    we're getting out of here
->END


