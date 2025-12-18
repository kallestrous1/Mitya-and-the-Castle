VAR hasRing = false
VAR knotProgression = -> Start

-> knotProgression

=== Start ===
Hello grandson how are you?
    +[good]
    Oh that's wonderful to hear
    ->Sunflowers
    +[bad]
    Oh man that sucks
    ->Sunflowers
    
    
===Sunflowers===
The sunflowers look beautiful don't they
water water water 
and the sun yes the sun
we must leave soon
the mech
but oh my son the ring
bandits have stolen the activation ring
you must retrieve it
it's the only way we'll make it to the king's city
the king's road has fallen
the east isn't safe anymore
we need the mech
we need the ring
->WaitingForRing

===WaitingForRing===
~knotProgression = -> WaitingForRing
- have you found the ring?
{hasRing:
    Oh wonderful!
    we shall depart swiftly!
    ->test
  - else:
    Sigh, hopefully it'll turn up soon
    ->WaitingForRing
}

===test===
~knotProgression = -> test
mhm??
+[I found the ring in your shed]
    what oh that is mighty strange
    I must have misplaced it oh dear
    what were you even doing back there
    but we wil depart soon,
    come back in an hour, pack your things
    everything will be ready
->END


