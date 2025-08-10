INCLUDE GlobalInkVariables.ink
EXTERNAL triggerGameEvent(ShopName)

VAR knotProgression = -> Start

-> knotProgression

===Start===
Hey buddy do you want to buy anything?
    +[yes]
    take a look!
    ->Shop
    +[no]
    maybe next time!
    ~ knotProgression = ->Start
    ->Goodbye

===Shop===
~ triggerGameEvent("OpenLittleWizardShop")
Here's all my wares:
    ~ knotProgression = ->Start
->Goodbye

===Goodbye===
~ triggerGameEvent("CloseShop")
GoodBye!

->Start

    -> END
