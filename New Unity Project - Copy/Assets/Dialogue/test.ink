INCLUDE testtwo.ink
-> main

=== main ===
Hey!
I'm bob

Do you kin shinji?
    +[yes]
     -> chosen("yea")
    +[no]
     -> chosen("no")
    +[idk]
     -> chosen("idk")

=== chosen(choice) ===
You said {choice}? I'm sure you do lol

-> testtwo


=== testtwo ===
what's ur favourite colour?
    +[red]
     -> decision("blue")
    +[yellow]
     -> decision("blue")
    +[green]
     -> decision("blue")

=== decision(choice) ===
{choice}!? normie ass colour tf

-> END