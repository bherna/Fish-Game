using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PetDescription 
{
    



    private static Dictionary<PetNames, string> P_D = new Dictionary<PetNames, string>{
            {PetNames.Charlie, "Charlie is a grounded individual that always gives a helping hand whenever trouble arises. He uses his whistle to get guppys attention when things get too chaotic."},
            {PetNames.DrCrabs, "Dr Crabs is a money hungry individual. If you let coins touch the bottom of the tank, he will take them for himself. But don’t be worried, unlike his relative he does exchange the coins he collects for a special medicine that's hard to come by."},
            {PetNames.WhiteKnight, "Null"},
            {PetNames.Cherry, "A beautiful quahog clam, Cherry is passionate about crafting the shiniest pearls. She pours her heart into each shimmering creation, delighting in the thought that others will admire her work. In the peaceful depths she calls home, Cherry is dedicated to spreading beauty through her radiant, carefully crafted treasures. "},
            {PetNames.TinyOctopus, "Null"},
            {PetNames.Athos, "Athos, the young and fierce, was once content to live in solitude, avoiding the power struggles of his reef. However, when a ruthless barracuda threatened his school, Athos could no longer stay out of the fight. Using his speed and cunning, he outmaneuvered the predator and, despite a serious wound, drove it away, saving the reef. His bravery and wisdom earned him the title of First of the Swords, a leader not by desire, but by the respect he earned in battle."},
            {PetNames.Porthos, "Porthos, the vibrant, was always the life of the reef, his dazzling colors and infectious laughter bringing joy wherever he swam. But when a fierce storm tore through the reef, leaving destruction in its wake, Porthos knew it was time to put his playful spirit aside. He rallied the fish with his boundless energy and optimism, inspiring them to rebuild and restore what had been lost. In the end, it wasn’t just his colors that made him shine—it was his bold heart and unwavering hope that helped the reef recover. "},
            {PetNames.Aramis, "Aramis swam in slow circles, his tail flowing like the cape of a betrayed prince. The filter hummed a tragic tune as he paused by the glass, staring into his reflection.\n“How could you?” he whispered—to himself.\nHe had forgotten feeding time. Again.\nThe guilt was unbearable. He floated dramatically to the gravel, buried his face in the bubbles, and vowed, “Never again shall I nap through the flake rain.”\nA single air bubble escaped—his only tear."},
            {PetNames.Khalid, "Khalid can still take damage, even though he’s technically immortal—he just doesn’t die. Thankfully, he uses that immortality for good by loyally defending the tank. Only downside? He has no idea how to fight. Immortal body, mortal combat skills."},
            {PetNames.Mary, "Mary is a tortured refugee from a dark mutations lab, where a cruel curse binds her to an unending cycle of childbirth. Each new life is a reminder of her torment, a painful burden that haunts her every moment. Yet, despite the agony that engulfs her, she harbors no hatred for her children; they are both her captives and her solace in a world that has stripped her of choice."},
            {PetNames.Salt, "Don’t be turned off by the idea of giving your little guppies some salt. It’s a great source of electrolytes and helps maintain the fluid balance they need to keep swimming at their best. (Also, apparently known to the state of California to cause [cancer], [birth defects], or [other reproductive harm]—but hey, what isn’t?)"}

        };


    
    public static string GetPetDesc(PetNames petname){
        return P_D[petname];
    }
}
