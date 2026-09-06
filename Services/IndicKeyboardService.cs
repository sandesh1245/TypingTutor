using System;
using System.Collections.Generic;
using System.Linq;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class IndicKeyboardService
    {
        private static IndicKeyboardService? _instance;
        public static IndicKeyboardService Instance => _instance ??= new IndicKeyboardService();

        // Master dictionaries: (Language, Shift, KeyChar) -> Resulting Indic String
        private readonly Dictionary<(PassageLanguage, bool, char), string> _keyMaps = new();

        // Reverse lookup: (Language, IndicChar) -> (PhysicalKeyChar, NeedsShift)
        private readonly Dictionary<(PassageLanguage, char), (char, bool)> _reverseMaps = new();

        public bool IsInAppTransliteratorEnabled { get; set; } = true;

        public IndicKeyboardService()
        {
            InitializeRemingtonGail();
            InitializeDevanagariInscript();
            InitializeMarathiInscript();
            InitializePunjabiRaavi();
            InitializeGujaratiInscript();
            InitializeBengaliInscript();
            InitializeTamilInscript();
            InitializeTeluguInscript();
            InitializeKrutiDev();
        }

        public bool TryTranslate(char physicalChar, bool isShift, PassageLanguage lang, out string result)
        {
            if (lang == PassageLanguage.English || !IsInAppTransliteratorEnabled)
            {
                result = physicalChar.ToString();
                return false;
            }

            char lower = char.ToLowerInvariant(physicalChar);

            if (_keyMaps.TryGetValue((lang, isShift, lower), out var mapped))
            {
                result = mapped;
                return true;
            }

            result = physicalChar.ToString();
            return false;
        }

        public bool TryGetPhysicalKey(char indicChar, PassageLanguage lang, out char physicalKey, out bool needsShift)
        {
            if (_reverseMaps.TryGetValue((lang, indicChar), out var entry))
            {
                physicalKey = entry.Item1;
                needsShift = entry.Item2;
                return true;
            }

            physicalKey = indicChar;
            needsShift = false;
            return false;
        }

        private void Register(PassageLanguage lang, char key, bool shift, string indicOutput)
        {
            char k = char.ToLowerInvariant(key);
            _keyMaps[(lang, shift, k)] = indicOutput;

            if (indicOutput.Length == 1)
            {
                _reverseMaps[(lang, indicOutput[0])] = (k, shift);
            }
        }

        #region Hindi Remington Gail (Mangal)
        private void InitializeRemingtonGail()
        {
            var lang = PassageLanguage.Hindi_Remington;

            // Number Row (Unshifted)
            Register(lang, '`', false, "`");
            Register(lang, '1', false, "1");
            Register(lang, '2', false, "2");
            Register(lang, '3', false, "3");
            Register(lang, '4', false, "4");
            Register(lang, '5', false, "5");
            Register(lang, '6', false, "6");
            Register(lang, '7', false, "7");
            Register(lang, '8', false, "8");
            Register(lang, '9', false, "9");
            Register(lang, '0', false, "0");
            Register(lang, '-', false, "-");
            Register(lang, '=', false, "ृ"); // ri matra

            // Number Row (Shifted)
            Register(lang, '`', true, "~");
            Register(lang, '1', true, "!");
            Register(lang, '2', true, "@");
            Register(lang, '3', true, "#");
            Register(lang, '4', true, "$");
            Register(lang, '5', true, "%");
            Register(lang, '6', true, "^");
            Register(lang, '7', true, "&");
            Register(lang, '8', true, "*");
            Register(lang, '9', true, "(");
            Register(lang, '0', true, ")");
            Register(lang, '-', true, "ऋ");
            Register(lang, '=', true, "्");

            // Top Row (Unshifted)
            Register(lang, 'q', false, "ु");
            Register(lang, 'w', false, "ू");
            Register(lang, 'e', false, "म");
            Register(lang, 'r', false, "त");
            Register(lang, 't', false, "ज");
            Register(lang, 'y', false, "ल");
            Register(lang, 'u', false, "न");
            Register(lang, 'i', false, "प");
            Register(lang, 'o', false, "व");
            Register(lang, 'p', false, "च");
            Register(lang, '[', false, "ख");
            Register(lang, ']', false, ",");
            Register(lang, '\\', false, "?");

            // Top Row (Shifted)
            Register(lang, 'q', true, "फ");
            Register(lang, 'w', true, "ँ");
            Register(lang, 'e', true, "म्");
            Register(lang, 'r', true, "त्");
            Register(lang, 't', true, "झ");
            Register(lang, 'y', true, "भ");
            Register(lang, 'u', true, "न्");
            Register(lang, 'i', true, "प्");
            Register(lang, 'o', true, "व्");
            Register(lang, 'p', true, "छ");
            Register(lang, '[', true, "ख्");
            Register(lang, ']', true, "।");
            Register(lang, '\\', true, "।");

            // Home Row (Unshifted)
            Register(lang, 'a', false, "ो");
            Register(lang, 's', false, "े");
            Register(lang, 'd', false, "्");
            Register(lang, 'f', false, "ि");
            Register(lang, 'g', false, "ह");
            Register(lang, 'h', false, "ी");
            Register(lang, 'j', false, "र");
            Register(lang, 'k', false, "ा");
            Register(lang, 'l', false, "स");
            Register(lang, ';', false, "य");
            Register(lang, '\'', false, "श");

            // Home Row (Shifted)
            Register(lang, 'a', true, "ौ");
            Register(lang, 's', true, "ै");
            Register(lang, 'd', true, "क");
            Register(lang, 'f', true, "थ");
            Register(lang, 'g', true, "भ");
            Register(lang, 'h', true, "क्ष");
            Register(lang, 'j', true, "श्र");
            Register(lang, 'k', true, "ज्ञ");
            Register(lang, 'l', true, "स्");
            Register(lang, ';', true, "ष्");
            Register(lang, '\'', true, "ष्");

            // Bottom Row (Unshifted)
            Register(lang, 'z', false, "्र");
            Register(lang, 'x', false, "ग");
            Register(lang, 'c', false, "ब");
            Register(lang, 'v', false, "अ");
            Register(lang, 'b', false, "इ");
            Register(lang, 'n', false, "द");
            Register(lang, 'm', false, "उ");
            Register(lang, ',', false, "ए");
            Register(lang, '.', false, "।");
            Register(lang, '/', false, "ध");

            // Bottom Row (Shifted)
            Register(lang, 'z', true, "र्");
            Register(lang, 'x', true, "घ");
            Register(lang, 'c', true, "ब्");
            Register(lang, 'v', true, "ट");
            Register(lang, 'b', true, "ठ");
            Register(lang, 'n', true, "ड");
            Register(lang, 'm', true, "ढ");
            Register(lang, ',', true, "ण");
            Register(lang, '.', true, "ऋ");
            Register(lang, '/', true, "ध्");
        }
        #endregion

        #region Devanagari Inscript (Hindi & Marathi Official)
        private void InitializeDevanagariInscript()
        {
            var lang = PassageLanguage.Hindi_Inscript;

            // Number row
            Register(lang, '1', false, "1"); Register(lang, '1', true, "ऍ");
            Register(lang, '2', false, "2"); Register(lang, '2', true, "ॅ");
            Register(lang, '3', false, "3"); Register(lang, '3', true, "्र");
            Register(lang, '4', false, "4"); Register(lang, '4', true, "र्");
            Register(lang, '5', false, "5"); Register(lang, '5', true, "ज्ञ");
            Register(lang, '6', false, "6"); Register(lang, '6', true, "त्र");
            Register(lang, '7', false, "7"); Register(lang, '7', true, "क्ष");
            Register(lang, '8', false, "8"); Register(lang, '8', true, "श्र");
            Register(lang, '9', false, "9"); Register(lang, '9', true, "(");
            Register(lang, '0', false, "0"); Register(lang, '0', true, ")");
            Register(lang, '-', false, "-"); Register(lang, '-', true, "ः");
            Register(lang, '=', false, "ृ"); Register(lang, '=', true, "ऋ");

            // Top Row
            Register(lang, 'q', false, "ौ"); Register(lang, 'q', true, "औ");
            Register(lang, 'w', false, "ै"); Register(lang, 'w', true, "ऐ");
            Register(lang, 'e', false, "ा"); Register(lang, 'e', true, "आ");
            Register(lang, 'r', false, "ी"); Register(lang, 'r', true, "ई");
            Register(lang, 't', false, "ू"); Register(lang, 't', true, "ऊ");
            Register(lang, 'y', false, "ब"); Register(lang, 'y', true, "भ");
            Register(lang, 'u', false, "ह"); Register(lang, 'u', true, "ङ");
            Register(lang, 'i', false, "ग"); Register(lang, 'i', true, "घ");
            Register(lang, 'o', false, "द"); Register(lang, 'o', true, "ध");
            Register(lang, 'p', false, "ज"); Register(lang, 'p', true, "झ");
            Register(lang, '[', false, "ड"); Register(lang, '[', true, "ढ");
            Register(lang, ']', false, "़"); Register(lang, ']', true, "ञ");
            Register(lang, '\\', false, "ॉ"); Register(lang, '\\', true, "ऑ");

            // Home Row
            Register(lang, 'a', false, "ो"); Register(lang, 'a', true, "ओ");
            Register(lang, 's', false, "े"); Register(lang, 's', true, "ए");
            Register(lang, 'd', false, "्"); Register(lang, 'd', true, "अ");
            Register(lang, 'f', false, "ि"); Register(lang, 'f', true, "इ");
            Register(lang, 'g', false, "ु"); Register(lang, 'g', true, "उ");
            Register(lang, 'h', false, "प"); Register(lang, 'h', true, "फ");
            Register(lang, 'j', false, "र"); Register(lang, 'j', true, "ऱ");
            Register(lang, 'k', false, "क"); Register(lang, 'k', true, "ख");
            Register(lang, 'l', false, "त"); Register(lang, 'l', true, "थ");
            Register(lang, ';', false, "च"); Register(lang, ';', true, "छ");
            Register(lang, '\'', false, "ट"); Register(lang, '\'', true, "ठ");

            // Bottom Row
            Register(lang, 'z', false, "ॆ"); Register(lang, 'z', true, "ऎ");
            Register(lang, 'x', false, "ं"); Register(lang, 'x', true, "ँ");
            Register(lang, 'c', false, "म"); Register(lang, 'c', true, "ण");
            Register(lang, 'v', false, "न"); Register(lang, 'v', true, "ऩ");
            Register(lang, 'b', false, "व"); Register(lang, 'b', true, "ऴ");
            Register(lang, 'n', false, "ल"); Register(lang, 'n', true, "ळ");
            Register(lang, 'm', false, "स"); Register(lang, 'm', true, "श");
            Register(lang, ',', false, ","); Register(lang, ',', true, "ष");
            Register(lang, '.', false, "."); Register(lang, '.', true, "।");
            Register(lang, '/', false, "य"); Register(lang, '/', true, "य़");
        }
        #endregion

        #region Marathi Inscript
        private void InitializeMarathiInscript()
        {
            var lang = PassageLanguage.Marathi;
            // Copies Devanagari Inscript with special Marathi keys
            var inscriptEntries = _keyMaps.Where(kvp => kvp.Key.Item1 == PassageLanguage.Hindi_Inscript).ToList();
            foreach (var kvp in inscriptEntries)
            {
                _keyMaps[(lang, kvp.Key.Item2, kvp.Key.Item3)] = kvp.Value;
                if (kvp.Value.Length == 1)
                {
                    _reverseMaps[(lang, kvp.Value[0])] = (kvp.Key.Item3, kvp.Key.Item2);
                }
            }
            // Explicit Marathi letter ळ on Shift+N
            Register(lang, 'n', true, "ळ");
            Register(lang, '.', true, "।");
        }
        #endregion

        #region Punjabi Raavi (Gurmukhi)
        private void InitializePunjabiRaavi()
        {
            var lang = PassageLanguage.Punjabi_Raavi;

            Register(lang, 'q', false, "ੌ"); Register(lang, 'q', true, "ਔ");
            Register(lang, 'w', false, "ੈ"); Register(lang, 'w', true, "ਐ");
            Register(lang, 'e', false, "ਾ"); Register(lang, 'e', true, "ਆ");
            Register(lang, 'r', false, "ੀ"); Register(lang, 'r', true, "ਈ");
            Register(lang, 't', false, "ੂ"); Register(lang, 't', true, "ਊ");
            Register(lang, 'y', false, "ਬ"); Register(lang, 'y', true, "ਭ");
            Register(lang, 'u', false, "ਹ"); Register(lang, 'u', true, "ਙ");
            Register(lang, 'i', false, "ਗ"); Register(lang, 'i', true, "ਘ");
            Register(lang, 'o', false, "ਦ"); Register(lang, 'o', true, "ਧ");
            Register(lang, 'p', false, "ਜ"); Register(lang, 'p', true, "ਝ");
            Register(lang, '[', false, "ਡ"); Register(lang, '[', true, "ਢ");
            Register(lang, ']', false, "਼"); Register(lang, ']', true, "ਞ");

            Register(lang, 'a', false, "ੋ"); Register(lang, 'a', true, "ਓ");
            Register(lang, 's', false, "ੇ"); Register(lang, 's', true, "ਏ");
            Register(lang, 'd', false, "੍"); Register(lang, 'd', true, "ਅ");
            Register(lang, 'f', false, "ਿ"); Register(lang, 'f', true, "ਇ");
            Register(lang, 'g', false, "ੁ"); Register(lang, 'g', true, "ਉ");
            Register(lang, 'h', false, "ਪ"); Register(lang, 'h', true, "ਫ");
            Register(lang, 'j', false, "ਰ"); Register(lang, 'j', true, "ੜ");
            Register(lang, 'k', false, "ਕ"); Register(lang, 'k', true, "ਖ");
            Register(lang, 'l', false, "ਤ"); Register(lang, 'l', true, "ਥ");
            Register(lang, ';', false, "ਚ"); Register(lang, ';', true, "ਛ");
            Register(lang, '\'', false, "ਟ"); Register(lang, '\'', true, "ਠ");

            Register(lang, 'x', false, "ਂ"); Register(lang, 'x', true, "ੰ");
            Register(lang, 'c', false, "ਮ"); Register(lang, 'c', true, "ਣ");
            Register(lang, 'v', false, "ਨ"); Register(lang, 'v', true, "ਨ");
            Register(lang, 'b', false, "ਵ"); Register(lang, 'b', true, "ਵ");
            Register(lang, 'n', false, "ਲ"); Register(lang, 'n', true, "ਲ਼");
            Register(lang, 'm', false, "ਸ"); Register(lang, 'm', true, "ਸ਼");
            Register(lang, '.', false, "."); Register(lang, '.', true, "।");
            Register(lang, '/', false, "ਯ"); Register(lang, '/', true, "ਯ");
        }
        #endregion

        #region Gujarati Inscript
        private void InitializeGujaratiInscript()
        {
            var lang = PassageLanguage.Gujarati;

            Register(lang, 'q', false, "ૌ"); Register(lang, 'q', true, "ઔ");
            Register(lang, 'w', false, "ૈ"); Register(lang, 'w', true, "ઐ");
            Register(lang, 'e', false, "ા"); Register(lang, 'e', true, "આ");
            Register(lang, 'r', false, "ી"); Register(lang, 'r', true, "ઈ");
            Register(lang, 't', false, "ૂ"); Register(lang, 't', true, "ઊ");
            Register(lang, 'y', false, "બ"); Register(lang, 'y', true, "ભ");
            Register(lang, 'u', false, "હ"); Register(lang, 'u', true, "ઙ");
            Register(lang, 'i', false, "ગ"); Register(lang, 'i', true, "ઘ");
            Register(lang, 'o', false, "દ"); Register(lang, 'o', true, "ધ");
            Register(lang, 'p', false, "જ"); Register(lang, 'p', true, "ઝ");
            Register(lang, '[', false, "ડ"); Register(lang, '[', true, "ઢ");
            Register(lang, ']', false, "઼"); Register(lang, ']', true, "ઞ");

            Register(lang, 'a', false, "ો"); Register(lang, 'a', true, "ઓ");
            Register(lang, 's', false, "ે"); Register(lang, 's', true, "એ");
            Register(lang, 'd', false, "્"); Register(lang, 'd', true, "અ");
            Register(lang, 'f', false, "િ"); Register(lang, 'f', true, "ઇ");
            Register(lang, 'g', false, "ુ"); Register(lang, 'g', true, "ઉ");
            Register(lang, 'h', false, "પ"); Register(lang, 'h', true, "ફ");
            Register(lang, 'j', false, "ર"); Register(lang, 'j', true, "ર");
            Register(lang, 'k', false, "ક"); Register(lang, 'k', true, "ખ");
            Register(lang, 'l', false, "ત"); Register(lang, 'l', true, "થ");
            Register(lang, ';', false, "ચ"); Register(lang, ';', true, "છ");
            Register(lang, '\'', false, "ટ"); Register(lang, '\'', true, "ઠ");

            Register(lang, 'x', false, "ં"); Register(lang, 'x', true, "ઁ");
            Register(lang, 'c', false, "મ"); Register(lang, 'c', true, "ણ");
            Register(lang, 'v', false, "ન"); Register(lang, 'v', true, "ન");
            Register(lang, 'b', false, "વ"); Register(lang, 'b', true, "વ");
            Register(lang, 'n', false, "લ"); Register(lang, 'n', true, "ળ");
            Register(lang, 'm', false, "સ"); Register(lang, 'm', true, "શ");
            Register(lang, ',', false, ","); Register(lang, ',', true, "ષ");
            Register(lang, '.', false, "."); Register(lang, '.', true, "।");
            Register(lang, '/', false, "ય"); Register(lang, '/', true, "ય");
        }
        #endregion

        #region Bengali Inscript
        private void InitializeBengaliInscript()
        {
            var lang = PassageLanguage.Bengali;

            Register(lang, 'q', false, "ৌ"); Register(lang, 'q', true, "ঔ");
            Register(lang, 'w', false, "ৈ"); Register(lang, 'w', true, "ঐ");
            Register(lang, 'e', false, "া"); Register(lang, 'e', true, "আ");
            Register(lang, 'r', false, "ী"); Register(lang, 'r', true, "ঈ");
            Register(lang, 't', false, "ূ"); Register(lang, 't', true, "ঊ");
            Register(lang, 'y', false, "ব"); Register(lang, 'y', true, "ভ");
            Register(lang, 'u', false, "হ"); Register(lang, 'u', true, "ঙ");
            Register(lang, 'i', false, "গ"); Register(lang, 'i', true, "ঘ");
            Register(lang, 'o', false, "দ"); Register(lang, 'o', true, "ধ");
            Register(lang, 'p', false, "জ"); Register(lang, 'p', true, "ঝ");
            Register(lang, '[', false, "ড"); Register(lang, '[', true, "ঢ");
            Register(lang, ']', false, "়"); Register(lang, ']', true, "ঞ");

            Register(lang, 'a', false, "ো"); Register(lang, 'a', true, "ও");
            Register(lang, 's', false, "ে"); Register(lang, 's', true, "এ");
            Register(lang, 'd', false, "্"); Register(lang, 'd', true, "অ");
            Register(lang, 'f', false, "ি"); Register(lang, 'f', true, "ই");
            Register(lang, 'g', false, "ু"); Register(lang, 'g', true, "উ");
            Register(lang, 'h', false, "প"); Register(lang, 'h', true, "ফ");
            Register(lang, 'j', false, "র"); Register(lang, 'j', true, "র");
            Register(lang, 'k', false, "ক"); Register(lang, 'k', true, "খ");
            Register(lang, 'l', false, "ত"); Register(lang, 'l', true, "থ");
            Register(lang, ';', false, "চ"); Register(lang, ';', true, "ছ");
            Register(lang, '\'', false, "ট"); Register(lang, '\'', true, "ঠ");

            Register(lang, 'x', false, "ং"); Register(lang, 'x', true, "ঁ");
            Register(lang, 'c', false, "ম"); Register(lang, 'c', true, "ণ");
            Register(lang, 'v', false, "ন"); Register(lang, 'v', true, "ন");
            Register(lang, 'b', false, "ব"); Register(lang, 'b', true, "ব");
            Register(lang, 'n', false, "ল"); Register(lang, 'n', true, "ল");
            Register(lang, 'm', false, "স"); Register(lang, 'm', true, "শ");
            Register(lang, ',', false, ","); Register(lang, ',', true, "ষ");
            Register(lang, '.', false, "."); Register(lang, '.', true, "।");
            Register(lang, '/', false, "য"); Register(lang, '/', true, "য়");
        }
        #endregion

        #region Tamil Inscript
        private void InitializeTamilInscript()
        {
            var lang = PassageLanguage.Tamil;

            Register(lang, 'q', false, "ௌ"); Register(lang, 'q', true, "ஔ");
            Register(lang, 'w', false, "ை"); Register(lang, 'w', true, "ஐ");
            Register(lang, 'e', false, "ா"); Register(lang, 'e', true, "ஆ");
            Register(lang, 'r', false, "ீ"); Register(lang, 'r', true, "ஈ");
            Register(lang, 't', false, "ூ"); Register(lang, 't', true, "ஊ");
            Register(lang, 'u', false, "ஹ"); Register(lang, 'u', true, "ங");
            Register(lang, 'a', false, "ோ"); Register(lang, 'a', true, "ஓ");
            Register(lang, 's', false, "ே"); Register(lang, 's', true, "ஏ");
            Register(lang, 'd', false, "்"); Register(lang, 'd', true, "அ");
            Register(lang, 'f', false, "ி"); Register(lang, 'f', true, "இ");
            Register(lang, 'g', false, "ு"); Register(lang, 'g', true, "உ");
            Register(lang, 'h', false, "ப"); Register(lang, 'h', true, "ப");
            Register(lang, 'j', false, "ர"); Register(lang, 'j', true, "ற");
            Register(lang, 'k', false, "க"); Register(lang, 'k', true, "க");
            Register(lang, 'l', false, "த"); Register(lang, 'l', true, "த");
            Register(lang, ';', false, "ச"); Register(lang, ';', true, "ச");
            Register(lang, '\'', false, "ட"); Register(lang, '\'', true, "ட");
            Register(lang, 'c', false, "ம"); Register(lang, 'c', true, "ண");
            Register(lang, 'v', false, "ந"); Register(lang, 'v', true, "ன");
            Register(lang, 'b', false, "வ"); Register(lang, 'b', true, "வ");
            Register(lang, 'n', false, "ல"); Register(lang, 'n', true, "ள");
            Register(lang, 'm', false, "ஸ"); Register(lang, 'm', true, "ஷ");
            Register(lang, '/', false, "ய"); Register(lang, '/', true, "ழ");
        }
        #endregion

        #region Telugu Inscript
        private void InitializeTeluguInscript()
        {
            var lang = PassageLanguage.Telugu;

            Register(lang, 'q', false, "ౌ"); Register(lang, 'q', true, "ఔ");
            Register(lang, 'w', false, "ై"); Register(lang, 'w', true, "ఐ");
            Register(lang, 'e', false, "ా"); Register(lang, 'e', true, "ఆ");
            Register(lang, 'r', false, "ీ"); Register(lang, 'r', true, "ఈ");
            Register(lang, 't', false, "ూ"); Register(lang, 't', true, "ఊ");
            Register(lang, 'y', false, "బ"); Register(lang, 'y', true, "భ");
            Register(lang, 'u', false, "హ"); Register(lang, 'u', true, "ఙ");
            Register(lang, 'i', false, "గ"); Register(lang, 'i', true, "ఘ");
            Register(lang, 'o', false, "ద"); Register(lang, 'o', true, "ధ");
            Register(lang, 'p', false, "జ"); Register(lang, 'p', true, "ఝ");
            Register(lang, 'a', false, "ో"); Register(lang, 'a', true, "ఓ");
            Register(lang, 's', false, "ే"); Register(lang, 's', true, "ఏ");
            Register(lang, 'd', false, "్"); Register(lang, 'd', true, "అ");
            Register(lang, 'f', false, "ి"); Register(lang, 'f', true, "ఇ");
            Register(lang, 'g', false, "ు"); Register(lang, 'g', true, "ఉ");
            Register(lang, 'h', false, "ప"); Register(lang, 'h', true, "ఫ");
            Register(lang, 'j', false, "ర"); Register(lang, 'j', true, "ఱ");
            Register(lang, 'k', false, "క"); Register(lang, 'k', true, "ఖ");
            Register(lang, 'l', false, "త"); Register(lang, 'l', true, "థ");
            Register(lang, ';', false, "చ"); Register(lang, ';', true, "ఛ");
            Register(lang, '\'', false, "ట"); Register(lang, '\'', true, "ఠ");
            Register(lang, 'x', false, "ం"); Register(lang, 'x', true, "ఁ");
            Register(lang, 'c', false, "మ"); Register(lang, 'c', true, "ణ");
            Register(lang, 'v', false, "న"); Register(lang, 'v', true, "న");
            Register(lang, 'b', false, "వ"); Register(lang, 'b', true, "వ");
            Register(lang, 'n', false, "ల"); Register(lang, 'n', true, "ళ");
            Register(lang, 'm', false, "స"); Register(lang, 'm', true, "శ");
            Register(lang, ',', false, ","); Register(lang, ',', true, "ష");
            Register(lang, '/', false, "య"); Register(lang, '/', true, "య");
        }
        #endregion

        #region Hindi Kruti Dev 010
        private void InitializeKrutiDev()
        {
            var lang = PassageLanguage.Hindi_Kruti;

            // Kruti Dev Maps standard keys directly
            Register(lang, 'd', false, "क");
            Register(lang, 'k', false, "ा");
            Register(lang, 's', false, "े");
            Register(lang, 'j', false, "र");
            Register(lang, 'e', false, "म");
            Register(lang, 'r', false, "त");
            Register(lang, 'u', false, "न");
            Register(lang, 'l', false, "स");
            Register(lang, 'g', false, "ह");
            Register(lang, 'h', false, "ी");
            Register(lang, 'f', false, "ि");
            Register(lang, 'b', false, "इ");
            Register(lang, 'c', false, "ब");
            Register(lang, 'v', false, "अ");
            Register(lang, 'x', false, "ग");
            Register(lang, 'p', false, "च");
            Register(lang, 't', false, "ज");
            Register(lang, 'y', false, "ल");
            Register(lang, 'i', false, "प");
            Register(lang, 'o', false, "व");
            Register(lang, 'a', false, "ं");

            Register(lang, 'd', true, "क्");
            Register(lang, 'k', true, "ज्ञा");
            Register(lang, 's', true, "ै");
            Register(lang, 'y', true, "भ");
            Register(lang, 'u', true, "न्");
            Register(lang, 'r', true, "त्");
            Register(lang, 'e', true, "म्");
            Register(lang, 'l', true, "स्");
            Register(lang, 'x', true, "घ");
            Register(lang, 'c', true, "ब्");
            Register(lang, 'i', true, "प्");
            Register(lang, 'o', true, "व्");
            Register(lang, 'p', true, "छ");
            Register(lang, 't', true, "झ");
            Register(lang, 'f', true, "थ");
            Register(lang, 'g', true, "भ");
            Register(lang, 'v', true, "ट");
            Register(lang, 'b', true, "ठ");
            Register(lang, 'n', true, "ड");
            Register(lang, 'm', true, "ढ");
        }
        #endregion
    }
}
