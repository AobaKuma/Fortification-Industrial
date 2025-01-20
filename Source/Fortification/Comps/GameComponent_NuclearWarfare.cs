using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Fortification
{
    public class GameComponent_NuclearWarfare : GameComponent
    {
        public GameComponent_NuclearWarfare(Game game)
        {
        }
        public int nuclearWeaponInStock = 0;
        public int nuclearWeaponDetonated = 0;
        private bool isNuclearWar = false;
        public  bool NuclearWarReached => !isNuclearWar && nuclearWeaponDetonated > 3;

        readonly String letter_init = "核鎮攝";
        readonly String letter_first = "核威脅";
        readonly String letter_second = "核陰雲";
        readonly String letter_third = "核競賽";
        readonly String letter_forth = "核制裁";

        readonly String init = "很顯然你已經獲取了一枚核武器，但在這顆星球上的其他勢力顯然尚未知曉這一點。你可以在任何地點引爆核彈來向世界展示這份力量。";     
        readonly String first = "第一枚核彈成功的引爆並完成了它的設計目的，很快邊緣世界上的陣營都會知曉[陣營名稱]所掌握的力量，並重新審視交惡帶來的風險。\n\n你應當開始建設核武庫，每一顆庫存中的核彈都能在戰爭的談判桌壓上一枚籌碼。妥善保存這份力量，你便能夠在這顆星球上維持著由核武所帶來的和平。";
        readonly String second = "第二枚核彈摧毀的不只是目標，也同時正在破壞著星球上脆弱的信任。切記，核彈只有在躺在發射井中才能發揮它維繫和平的作用。\n\n或許一兩枚核彈還在他們接受的範圍，但我們無從知道其他陣營的底線。";
        readonly String third = "第三枚核彈的引爆很顯然刺激了星球上本地陣營的神經。\n\n一些勢力重新啟用了他們封存的核武庫與生產設施，引發了連鎖反應式的核武軍備競賽，很顯然這會使得那些不被監管的核彈流落到黑市與海盜的手中。";
        readonly String forth = "核閃光再一次的照耀於這顆星球上，但這一次...他們不再只是觀望了。\n\n如今你的陣營被視為一個不穩定的核威脅，所有陣營都拒絕向你貿易，並且雖然不會在明面上與你敵對，但他們可能會向你的敵人暗中提供核武與賞金。\n這些陣營的目的是制裁你的核儲備並維持陣營之間的核威嚇平衡，因此現在核武庫中每一枚核彈都將為你的殖民地帶來更多威脅，你可以將所有核武銷毀以表示對於和平的誠意。";

        public void NuclearWeaponUsed()//每次使用都會觸發
        {
            nuclearWeaponDetonated++;
            if (isNuclearWar) return;
            TriggerLetter();
            if (NuclearWarReached)
            {
                isNuclearWar = true;
            }
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();
        }
        private void TriggerLetter()
        {
            switch (nuclearWeaponDetonated)
            {
                case 1:
                    Find.LetterStack.ReceiveLetter(letter_first.Translate(), first.Translate(), LetterDefOf.PositiveEvent);
                    break;
                case 2:
                    Find.LetterStack.ReceiveLetter(letter_second.Translate(), second.Translate(), LetterDefOf.NeutralEvent);
                    break;
                case 3:
                    Find.LetterStack.ReceiveLetter(letter_third.Translate(), third.Translate(), LetterDefOf.NegativeEvent);
                    break;
                case 4:
                    Find.LetterStack.ReceiveLetter(letter_forth.Translate(), forth.Translate(), LetterDefOf.NegativeEvent);
                    break;
            }
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            if (isNuclearWar && nuclearWeaponInStock == 0)
            { 
            
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref nuclearWeaponInStock, "nuclearWeaponInStock", 0);
            Scribe_Values.Look(ref nuclearWeaponDetonated, "nuclearWeaponDetonated", 0);
            Scribe_Values.Look(ref isNuclearWar, "isNuclearWar", false);
        }
    }
}
