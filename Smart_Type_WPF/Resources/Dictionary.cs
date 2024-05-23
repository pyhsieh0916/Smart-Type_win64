using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Type_WPF.Windows
{
    public partial class Dictionary_spell
    {
        public Dictionary<char, string> spell_Data { get { return _spell_Data; } }
        public Dictionary_spell() 
        {
            Dictionary_spell_Data();
        }
        private Dictionary<char, string> _spell_Data = new Dictionary<char, string>();
        public void Dictionary_spell_Data()
        {
            //建表
            _spell_Data.Add('q', "ㄆ");
            _spell_Data.Add('a', "ㄇ");
            _spell_Data.Add('z', "ㄈ");
            _spell_Data.Add('w', "ㄊ");
            _spell_Data.Add('s', "ㄋ");
            _spell_Data.Add('x', "ㄌ");
            _spell_Data.Add('e', "ㄍ");
            _spell_Data.Add('d', "ㄎ");
            _spell_Data.Add('c', "ㄏ");
            _spell_Data.Add('r', "ㄐ");
            _spell_Data.Add('f', "ㄑ");
            _spell_Data.Add('v', "ㄒ");
            _spell_Data.Add('t', "ㄔ");
            _spell_Data.Add('g', "ㄕ");
            _spell_Data.Add('b', "ㄖ");
            _spell_Data.Add('y', "ㄗ");
            _spell_Data.Add('h', "ㄘ");
            _spell_Data.Add('n', "ㄙ");
            _spell_Data.Add('u', "ㄧ");
            _spell_Data.Add('j', "ㄨ");
            _spell_Data.Add('m', "ㄩ");
            _spell_Data.Add('i', "ㄛ");
            _spell_Data.Add('k', "ㄜ");
            _spell_Data.Add(',', "ㄝ");
            _spell_Data.Add('o', "ㄟ");
            _spell_Data.Add('l', "ㄠ");
            _spell_Data.Add('.', "ㄡ");
            _spell_Data.Add('p', "ㄣ");
            _spell_Data.Add(';', "ㄤ");
            _spell_Data.Add('/', "ㄥ");
            _spell_Data.Add('-', "ㄦ");
            _spell_Data.Add('0', "ㄢ");
            _spell_Data.Add('9', "ㄞ");
            _spell_Data.Add('8', "ㄚ");
            _spell_Data.Add('7', "˙");
            _spell_Data.Add('6', "ˊ");
            _spell_Data.Add('5', "ㄓ");
            _spell_Data.Add('4', "ˋ");
            _spell_Data.Add('3', "ˇ");
            _spell_Data.Add('2', "ㄉ");
            _spell_Data.Add('1', "ㄅ");
            _spell_Data.Add(' ', " ");
            _spell_Data.Add('ㄅ', "ㄅ");
            _spell_Data.Add('ㄆ', "ㄆ");
            _spell_Data.Add('ㄇ', "ㄇ");
            _spell_Data.Add('ㄈ', "ㄈ");
            _spell_Data.Add('ㄉ', "ㄉ");
            _spell_Data.Add('ㄊ', "ㄊ");
            _spell_Data.Add('ㄋ', "ㄋ");
            _spell_Data.Add('ㄌ', "ㄌ");
            _spell_Data.Add('ㄍ', "ㄍ");
            _spell_Data.Add('ㄎ', "ㄎ");
            _spell_Data.Add('ㄏ', "ㄏ");
            _spell_Data.Add('ㄐ', "ㄐ");
            _spell_Data.Add('ㄑ', "ㄑ");
            _spell_Data.Add('ㄒ', "ㄒ");
            _spell_Data.Add('ㄔ', "ㄔ");
            _spell_Data.Add('ㄕ', "ㄕ");
            _spell_Data.Add('ㄖ', "ㄖ");
            _spell_Data.Add('ㄗ', "ㄗ");
            _spell_Data.Add('ㄘ', "ㄘ");
            _spell_Data.Add('ㄙ', "ㄙ");
            _spell_Data.Add('ㄧ', "ㄧ");
            _spell_Data.Add('ㄨ', "ㄨ");
            _spell_Data.Add('ㄩ', "ㄩ");
            _spell_Data.Add('ㄟ', "ㄟ");
            _spell_Data.Add('ㄠ', "ㄠ");
            _spell_Data.Add('ㄡ', "ㄡ");
            _spell_Data.Add('ㄛ', "ㄛ");
            _spell_Data.Add('ㄜ', "ㄜ");
            _spell_Data.Add('ㄝ', "ㄝ");
            _spell_Data.Add('ㄣ', "ㄣ");
            _spell_Data.Add('ㄤ', "ㄤ");
            _spell_Data.Add('ㄥ', "ㄥ");
            _spell_Data.Add('ㄦ', "ㄦ");
        } 
    }
}
