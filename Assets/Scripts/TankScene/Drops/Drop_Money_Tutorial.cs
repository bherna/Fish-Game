

public class Drop_Money_Tutorial : Drop_Money
{

    //override original to message controller tutorial


    protected override void Collect()
    {

        //only difference is we send message to tutorial
        //controller tutorial
        TutorialReaderParent.instance.CollectCoin();

        base.Collect();
    }
}
