using UnityEngine;
using UnityEngine.InputSystem;

public class WidgetCredits : MenuBase
{
    //Hey. Welcome to the Credits script.
    //My name is Paul Credits, you can call me Credits. It's sorta my whole deal.
    //We like to have fun here in the Credits script. I kinda run the place, so
    //I do things a little different, in my own way.

    //I don't have any methods I personally run, that stuff is all inherited from my dad.
    //His name is Calvin Canvas, he's sort of a big deal here. Not my vibe, though.

    //Normally I would have plenty of responsibilities and connections to keep up with.
    //I just have to display all this text, and no one really cares about the credits,
    //Unless they're in them, or someone they know is. So I keep a chill vibe about the place.

    //Well, thanks for stopping by, grab a beer on the way out.
    //Do me a solid and keep this script on the down low?

    //ACTUAL CODING NECESSARY. SORRY PAUL.
    //Ah nuts guys, that was my dang dad Calvin Canvas. Turns out I have some actual code to run now.
    //Growing up sucks, but I have a job to do now, so let's hop to it.

    FeFunctions feFunctions;

    private void Start()
    {
        feFunctions = GetComponentInParent<FeFunctions>();
    }

    private void Update()
    {
        if (Gamepad.current.bButton.wasPressedThisFrame && GameManager.gm.curScene == 0)
        {
            feFunctions.ActiveButtonsAll(true); //buttons are interactible again
            feFunctions.ReselectNewGame(); //Controller can navigate buttons again
            Destroy(this.gameObject);
            //BackButtonPressed();
        }
    }
}
