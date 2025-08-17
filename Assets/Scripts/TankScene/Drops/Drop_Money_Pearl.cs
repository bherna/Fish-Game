

public class Drop_Money_Pearl : Drop_Money
{
 
    protected override void Collect()
    {
        //message cherry that pearl was picked up
        Controller_Pets.instance.Annoucement_Init(Event_Type.pearlCollected, null);

        //this has a destroy method, so run last
        base.Collect();
    }
}
