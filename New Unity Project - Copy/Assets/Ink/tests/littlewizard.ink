INCLUDE GlobalInkVariables.ink

VAR knotProgression = -> Start

-> knotProgression

===Start===
Hey buddy do you want to buy anything?
    +[yes]
    take a look!
        ~ openShop = true
    ->Shop
    +[no]
    maybe next time!
    ~ knotProgression = ->Start
    ->Goodbye

===Shop===
Here's all my wares:
    ~ knotProgression = ->Start
        ~ openShop = false
->Goodbye

===Goodbye===
GoodBye!
->Start

    -> END
