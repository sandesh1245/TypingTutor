using System;
using System.Collections.Generic;
using System.Linq;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class LessonService
    {
        private static LessonService? _instance;
        public static LessonService Instance => _instance ??= new LessonService();

        private readonly List<LessonModel> _lessons = new List<LessonModel>();

        public LessonService()
        {
            InitializeLessons();
        }

        public List<LessonModel> GetLessonsByLanguage(PassageLanguage lang)
        {
            return _lessons.Where(l => l.Language == lang).OrderBy(l => l.LessonNumber).ToList();
        }

        private void InitializeLessons()
        {
            // ==========================================
            // 1. ENGLISH QWERTY LESSONS
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "eng_les_01",
                Language = PassageLanguage.English,
                LessonNumber = 1,
                Category = "Home Row",
                Title = "Home Row Basics (ASDF JKL;)",
                Description = "Place your left hand fingers on A, S, D, F and right hand fingers on J, K, L, ;.",
                TargetKeys = "a s d f j k l ;",
                PracticeText = "asdf jkl; asdf jkl; a s d f j k l ; asdf jkl; asdf jkl; a s d f j k l ;",
                RecommendedFinger = "Left Pinky to Left Index / Right Index to Right Pinky"
            });

            _lessons.Add(new LessonModel
            {
                Id = "eng_les_02",
                Language = PassageLanguage.English,
                LessonNumber = 2,
                Category = "Home Row",
                Title = "Home Row Extension (G and H)",
                Description = "Extend left index finger to G and right index finger to H.",
                TargetKeys = "g h f d s a j k l ;",
                PracticeText = "fghj fghj asdfgh jkl;gh fghj asdfg hjkl; fghj fghj",
                RecommendedFinger = "Left Index for G, Right Index for H"
            });

            _lessons.Add(new LessonModel
            {
                Id = "eng_les_03",
                Language = PassageLanguage.English,
                LessonNumber = 3,
                Category = "Top Row",
                Title = "Top Row Fundamentals (QWER UIOP)",
                Description = "Reach up from home row to top row keys using designated fingers.",
                TargetKeys = "q w e r u i o p",
                PracticeText = "qwer uiop qwer uiop rad sad dad pad quad wear pear pure rare",
                RecommendedFinger = "Reach upward without moving wrist position"
            });

            _lessons.Add(new LessonModel
            {
                Id = "eng_les_04",
                Language = PassageLanguage.English,
                LessonNumber = 4,
                Category = "Top Row",
                Title = "Top Row Extension (T and Y)",
                Description = "Reach left index to T and right index to Y.",
                TargetKeys = "t y r u e i w o q p",
                PracticeText = "try type year true yet party story water write city quay",
                RecommendedFinger = "Left Index for T, Right Index for Y"
            });

            _lessons.Add(new LessonModel
            {
                Id = "eng_les_05",
                Language = PassageLanguage.English,
                LessonNumber = 5,
                Category = "Bottom Row",
                Title = "Bottom Row (ZXCV BNM)",
                Description = "Reach down from home row to bottom row keys.",
                TargetKeys = "z x c v b n m",
                PracticeText = "zxcv bnm zxcv bnm cab van box man zinc exam bank back",
                RecommendedFinger = "Left Hand for Z X C V, Right Hand for B N M"
            });

            // ==========================================
            // 2. HINDI REMINGTON GAIL LESSONS
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "hin_rem_les_01",
                Language = PassageLanguage.Hindi_Remington,
                LessonNumber = 1,
                Category = "Home Row (मात्राएँ व स्वर)",
                Title = "होम रो अभ्यास (अ क म त)",
                Description = "रेमिंगटन गेल लेआउट में होम रो की उंगलियों की स्थिति सीखें।",
                TargetKeys = "क म त न ल स य ह",
                PracticeText = "कमत कमत नकस नकस कमत नकस कमत नकस कमत नकस",
                RecommendedFinger = "बाएं व दाएं हाथ की उंगलियों को होम रो पर रखें"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_rem_les_02",
                Language = PassageLanguage.Hindi_Remington,
                LessonNumber = 2,
                Category = "Top Row (अक्षर अभ्यास)",
                Title = "टॉप रो अभ्यास (च ज ट ड)",
                Description = "ऊपरी पंक्ति के अक्षरों का अभ्यास करें।",
                TargetKeys = "च ज ट ड प फ ब भ",
                PracticeText = "चजटड पफबभ चजटड पफबभ चजटड पफबभ चजटड",
                RecommendedFinger = "तर्जनी व मध्यमा उंगली का सही प्रयोग करें"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_rem_les_03",
                Language = PassageLanguage.Hindi_Remington,
                LessonNumber = 3,
                Category = "Bottom Row (व्यंजन)",
                Title = "बॉटम रो अभ्यास (ग घ ध ण)",
                Description = "निचली पंक्ति के अक्षरों का अभ्यास करें।",
                TargetKeys = "ग घ ध ण थ द",
                PracticeText = "गघधण गघधण थदगघ थदगघ गघधण थदगघ",
                RecommendedFinger = "कनिष्ठिका व अनामिका से निचली पंक्ति की ओर बढ़ें"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_rem_les_04",
                Language = PassageLanguage.Hindi_Remington,
                LessonNumber = 4,
                Category = "Shift Keys (हलंत व मात्राएं)",
                Title = "शिफ्ट कीज व संयुक्ताक्षर अभ्यास",
                Description = "शिफ्ट के साथ आधे अक्षर और विशेष मात्राओं का संयोजन।",
                TargetKeys = "प्र क्त स्त त्र ज्ञ",
                PracticeText = "प्रक्रिया स्थिति प्रस्ताव राष्ट्रीय विकास संविधान",
                RecommendedFinger = "शिफ्ट की को विपरीत हाथ की कनिष्ठिका से दबाएं"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_rem_les_05",
                Language = PassageLanguage.Hindi_Remington,
                LessonNumber = 5,
                Category = "Sentence Speed",
                Title = "सरकारी परीक्षा गति अभ्यास",
                Description = "एसएससी व न्यायालय प्रारूप के हिंदी वाक्य।",
                TargetKeys = "वाक्य अभ्यास",
                PracticeText = "भारत एक विशाल लोकतान्त्रिक देश है जहां सभी नागरिकों को समान अधिकार प्राप्त हैं।",
                RecommendedFinger = "पूर्ण वाक्य प्रवाह व निरंतर गति बनाए रखें"
            });

            // ==========================================
            // 3. HINDI INSCRIPT / MANGAL LESSONS
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "hin_ins_les_01",
                Language = PassageLanguage.Hindi_Inscript,
                LessonNumber = 1,
                Category = "Home Row Vowels & Consonants",
                Title = "इन्स्क्रिप्ट होम रो (ो े ा ् ु प र क त च)",
                Description = "मानक इन्स्क्रिप्ट कीबोर्ड की होम रो मैपिंग।",
                TargetKeys = "े ा ् ु प र क त च",
                PracticeText = "परक तच परक तच परकतच परकतच परक तच",
                RecommendedFinger = "बाएं हाथ से स्वर व मात्राएं, दाएं हाथ से व्यंजन"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_ins_les_02",
                Language = PassageLanguage.Hindi_Inscript,
                LessonNumber = 2,
                Category = "Top Row Consonants",
                Title = "टॉप रो व्यंजन (ग द ज ड ़)",
                Description = "ऊपरी पंक्ति के व्यंजनों का सटीक अभ्यास।",
                TargetKeys = "ग द ज ड ब ह",
                PracticeText = "गदजड बह गदजड बह गदजड बह गदजड बह",
                RecommendedFinger = "तर्जनी व मध्यमा उंगलियों को ऊपर की पंक्ति पर ले जाएं"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_ins_les_03",
                Language = PassageLanguage.Hindi_Inscript,
                LessonNumber = 3,
                Category = "Bottom Row Letters",
                Title = "बॉटम रो (ण न व ल स म)",
                Description = "निचली पंक्ति के मानक व्यंजनों का अभ्यास।",
                TargetKeys = "ण न व ल स म",
                PracticeText = "नवल सम नवल सम नवलसम नवलसम नवल सम",
                RecommendedFinger = "हाथ की कलाई स्थिर रखते हुए नीचे की पंक्ति टाइप करें"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_ins_les_04",
                Language = PassageLanguage.Hindi_Inscript,
                LessonNumber = 4,
                Category = "Halant & Conjuncts",
                Title = "हलंत (D Key) व संयुक्ताक्षर निर्माण",
                Description = "D की का उपयोग कर आधे अक्षरों का निर्माण सीखें।",
                TargetKeys = "् क ख ग घ",
                PracticeText = "शक्ति मुक्ति भक्ति राष्ट्र न्याय स्पष्ट त्याग",
                RecommendedFinger = "व्यंजन के बाद D दबाकर अगला व्यंजन टाइप करें"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_ins_les_05",
                Language = PassageLanguage.Hindi_Inscript,
                LessonNumber = 5,
                Category = "Speed Sentences",
                Title = "एसएससी व रेलवे इन्स्क्रिप्ट अभ्यास",
                Description = "सरकारी परीक्षाओं हेतु पूर्ण वाक्य गति अभ्यास।",
                TargetKeys = "वाक्य अभ्यास",
                PracticeText = "भारतीय संविधान प्रत्येक नागरिक को न्याय स्वतंत्रता और समानता का अधिकार प्रदान करता है।",
                RecommendedFinger = "लयबद्ध टाइपिंग एवं बिना रुके निरंतर अभ्यास"
            });

            // ==========================================
            // 4. HINDI KRUTI DEV 010 LESSONS
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "hin_kru_les_01",
                Language = PassageLanguage.Hindi_Kruti,
                LessonNumber = 1,
                Category = "Home Row Finger Map",
                Title = "कृति देव 010 - होम रो फिंगर प्लेसमेंट",
                Description = "कृति देव फॉन्ट में होम रो कीज की स्थिति व उंगली संचालन।",
                TargetKeys = "k e js g f r",
                PracticeText = "k e js g f r k e js g f r k e js g f r",
                RecommendedFinger = "कृति देव कीज मैपिंग गाइड"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_kru_les_02",
                Language = PassageLanguage.Hindi_Kruti,
                LessonNumber = 2,
                Category = "Top Row Mapping",
                Title = "कृति देव - टॉप रो वर्णमाला (Q W E R T Y)",
                Description = "मात्राओं एवं मुख्य अक्षरों की टॉप रो मैपिंग।",
                TargetKeys = "q w e r t y u i o p",
                PracticeText = "qwer tyui op qwer tyui op qwer tyui",
                RecommendedFinger = "ऊपरी पंक्ति के वर्णों का सटीक अभ्यास"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_kru_les_03",
                Language = PassageLanguage.Hindi_Kruti,
                LessonNumber = 3,
                Category = "Bottom Row & Shift",
                Title = "कृति देव - बॉटम रो एवं शिफ्ट कीज",
                Description = "संयुक्त वर्ण व शिफ्ट के संयोजन का अभ्यास।",
                TargetKeys = "z x c v b n m",
                PracticeText = "zxcv bnm zxcv bnm zxcv bnm",
                RecommendedFinger = "शिफ्ट की के साथ आधे वर्ण टाइप करें"
            });

            _lessons.Add(new LessonModel
            {
                Id = "hin_kru_les_04",
                Language = PassageLanguage.Hindi_Kruti,
                LessonNumber = 4,
                Category = "Court Exam Drill",
                Title = "न्यायालयीन एवं राजस्व शब्द अभ्यास",
                Description = "इलाहाबाद हाई कोर्ट व राज्य आयोग परीक्षाओं हेतु।",
                TargetKeys = "न्यायालय शब्द",
                PracticeText = "न्यायालय आदेशानुसार प्रार्थी का प्रार्थना पत्र स्वीकार किया जाता है।",
                RecommendedFinger = "शुद्धता 95% से अधिक रखने का प्रयास करें"
            });

            // ==========================================
            // 5. MARATHI LESSONS (मराठी टंकलेखन)
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "mar_les_01",
                Language = PassageLanguage.Marathi,
                LessonNumber = 1,
                Category = "होम रो मूलभूत (Home Row)",
                Title = "मराठी होम रो (क म त न ल स य ह)",
                Description = "मराठी टंकलेखनासाठी होम रो वरील बोटांची योग्य जागा.",
                TargetKeys = "क म त न ल स य ह",
                PracticeText = "कमतन लसयह कमतन लसयह कमत नकस कमतन लसयह कमत नकस",
                RecommendedFinger = "डाव्या हाताची बोटे 'क म त न' आणि उजव्या हाताची 'ल स य ह' वर ठेवा"
            });

            _lessons.Add(new LessonModel
            {
                Id = "mar_les_02",
                Language = PassageLanguage.Marathi,
                LessonNumber = 2,
                Category = "कान्हा व मात्रा (Vowels & Matras)",
                Title = "स्वर व मात्रा अभ्यास (ा ो ौ े ै ु ू)",
                Description = "मराठी मात्रांचा अचूक वापर आणि कान्हा-वेलांटी सराव.",
                TargetKeys = "ा े ै ो ौ ु ू",
                PracticeText = "कमळ नमन सरळ वजन भारत शासन कायदा न्यायालय विकास",
                RecommendedFinger = "मात्रांसाठी योग्य बोट वापरून अचूकता वाढवा"
            });

            _lessons.Add(new LessonModel
            {
                Id = "mar_les_03",
                Language = PassageLanguage.Marathi,
                LessonNumber = 3,
                Category = "जोडाक्षरे (Compound Letters)",
                Title = "मराठी जोडाक्षरे व हलंत सराव",
                Description = "हलंत (्) वापरून जोडाक्षरे तयार करण्याचा सराव.",
                TargetKeys = "प्र क्त स्त त्र ज्ञ श्र",
                PracticeText = "महाराष्ट्र प्रशासन संस्था स्वातंत्र्य व्यक्ती न्याय प्रक्रिया",
                RecommendedFinger = "हलंत की योग्य क्रमाने दाबून जोडाक्षर बनवा"
            });

            _lessons.Add(new LessonModel
            {
                Id = "mar_les_04",
                Language = PassageLanguage.Marathi,
                LessonNumber = 4,
                Category = "शासकीय शब्द (Administrative)",
                Title = "मंत्रालय व महसूल कार्यालयीन शब्दसंग्रह",
                Description = "शासकीय टंकलेखन परीक्षेतील महत्त्वाचे शब्द.",
                TargetKeys = "शासकीय शब्द",
                PracticeText = "अर्जदार महसूल जिल्हा परिषद नगरपालिका ग्रामपंचायत अधिकारी",
                RecommendedFinger = "जलद व अचूक टंकलेखन गती वाढवा"
            });

            _lessons.Add(new LessonModel
            {
                Id = "mar_les_05",
                Language = PassageLanguage.Marathi,
                LessonNumber = 5,
                Category = "MPSC / GCC-TBC परीक्षा",
                Title = "मराठी परीक्षा परिच्छेद सराव",
                Description = "GCC-TBC 30 व 40 श.प्र.मि. गतीसाठी संपूर्ण वाक्य सराव.",
                TargetKeys = "संपूर्ण वाक्य",
                PracticeText = "महाराष्ट्र शासनाच्या विविध विभागांमध्ये लोकाभिमुख कारभारासाठी आधुनिक संगणक प्रणालीचा वापर केला जात आहे.",
                RecommendedFinger = "३० ते ४० शब्द प्रति मिनिट गतीचे लक्ष्य ठेवा"
            });

            // ==========================================
            // 6. PUNJABI RAAVI LESSONS (ਪੰਜਾਬੀ ਰਾਵੀ ਟਾਈਪਿੰਗ)
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "pun_les_01",
                Language = PassageLanguage.Punjabi_Raavi,
                LessonNumber = 1,
                Category = "ਹੋਮ ਰੋਅ (Home Row)",
                Title = "ਰਾਵੀ ਹੋਮ ਰੋਅ ਬੇਸਿਕਸ (ਕ ਤ ਚ ਪ ਰ ਸ ਹ)",
                Description = "ਪੰਜਾਬੀ ਰਾਵੀ ਫੌਂਟ ਵਿੱਚ ਹੋਮ ਰੋਅ ਦੀਆਂ ਕੁੰਜੀਆਂ ਦੀ ਸਥਿਤੀ।",
                TargetKeys = "ਕ ਤ ਚ ਪ ਰ ਸ ਹ",
                PracticeText = "ਕਤਚਪ ਰਸਹ ਕਤਚਪ ਰਸਹ ਕਤਚਪ ਰਸਹ ਕਤਚਪ ਰਸਹ",
                RecommendedFinger = "ਉਂਗਲਾਂ ਨੂੰ ਹੋਮ ਰੋਅ ਤੇ ਸਥਿਰ ਰੱਖੋ"
            });

            _lessons.Add(new LessonModel
            {
                Id = "pun_les_02",
                Language = PassageLanguage.Punjabi_Raavi,
                LessonNumber = 2,
                Category = "ਮਾਤਰਾਵਾਂ (Vowel Marks)",
                Title = "ਸਿਹਾਰੀ, ਬਿਹਾਰੀ, ਔਂਕੜ ਅਤੇ ਕੰਨਾ",
                Description = "ਪੰਜਾਬੀ ਮਾਤਰਾਵਾਂ ਦਾ ਸਹੀ ਪ੍ਰਯੋਗ ਸਿੱਖੋ।",
                TargetKeys = "ਾ ਿ ੀ ੁ ੂ ੇ ੈ ੋ ੌ",
                PracticeText = "ਪੰਜਾਬ ਸਰਕਾਰ ਵਿਕਾਸ ਸਿੱਖਿਆ ਸਕੂਲ ਅਦਾਲਤ ਸੇਵਾ ਨਿਯਮ",
                RecommendedFinger = "ਖੱਬੇ ਹੱਥ ਦੀਆਂ ਉਂਗਲਾਂ ਨਾਲ ਮਾਤਰਾਵਾਂ ਟਾਈਪ ਕਰੋ"
            });

            _lessons.Add(new LessonModel
            {
                Id = "pun_les_03",
                Language = PassageLanguage.Punjabi_Raavi,
                LessonNumber = 3,
                Category = "ਟਿੱਪੀ, ਬਿੰਦੀ ਅਤੇ ਅੱਧਕ",
                Title = "ਅੱਧਕ (ੱ), ਟਿੱਪੀ (ੰ) ਅਤੇ ਬਿੰਦੀ (ਂ)",
                Description = "ਪੰਜਾਬੀ ਵਿਆਕਰਣ ਦੇ ਵਿਸ਼ੇਸ਼ ਚਿੰਨ੍ਹਾਂ ਦਾ ਅਭਿਆਸ।",
                TargetKeys = "ੱ ੰ ਂ",
                PracticeText = "ਬੱਚਾ ਪੰਛੀ ਦਿੱਲੀ ਪੰਜਾਬੀ ਸੰਵਿਧਾਨ ਸੰਸਥਾ ਹੱਥ",
                RecommendedFinger = "ਸ਼ਿਫਟ ਕੁੰਜੀ ਦਾ ਸਹੀ ਪ੍ਰਯੋਗ ਕਰਕੇ ਅੱਧਕ ਲਗਾਓ"
            });

            _lessons.Add(new LessonModel
            {
                Id = "pun_les_04",
                Language = PassageLanguage.Punjabi_Raavi,
                LessonNumber = 4,
                Category = "PSSSB / Court Words",
                Title = "ਸਰਕਾਰੀ ਦਫ਼ਤਰੀ ਸ਼ਬਦਾਵਲੀ",
                Description = "ਪੀ.ਐਸ.ਐਸ.ਐਸ.ਬੀ. ਕਲਰਕ ਅਤੇ ਹਾਈ ਕੋਰਟ ਪ੍ਰੀਖਿਆਵਾਂ ਲਈ ਸ਼ਬਦ।",
                TargetKeys = "ਅਦਾਲਤੀ ਸ਼ਬਦ",
                PracticeText = "ਅਰਜ਼ੀਦਾਰ ਫੈਸਲਾ ਅਧਿਕਾਰੀ ਰਜਿਸਟਰਾਰ ਕਰਮਚਾਰੀ ਹੁਕਮਨਾਮਾ",
                RecommendedFinger = "ਸਪੀਡ ਵਧਾਉਣ ਲਈ ਨਿਰੰਤਰ ਰਿਦਮ ਬਣਾਈ ਰੱਖੋ"
            });

            _lessons.Add(new LessonModel
            {
                Id = "pun_les_05",
                Language = PassageLanguage.Punjabi_Raavi,
                LessonNumber = 5,
                Category = "Full Exam Passage",
                Title = "ਪੰਜਾਬੀ ਟਾਈਪਿੰਗ ਪਰੀਖਿਆ ਵਾਕ",
                Description = "30 ਸ਼ਬਦ ਪ੍ਰਤੀ ਮਿੰਟ ਦੀ ਰਫ਼ਤਾਰ ਲਈ ਵਾਕ ਅਭਿਆਸ।",
                TargetKeys = "ਪੂਰੇ ਵਾਕ",
                PracticeText = "ਪੰਜਾਬ ਇੱਕ ਖੁਸ਼ਹਾਲ ਸੂਬਾ ਹੈ ਜਿੱਥੋਂ ਦੇ ਮਿਹਨਤੀ ਕਿਸਾਨ ਦੇਸ਼ ਦੇ ਅੰਨ ਭੰਡਾਰ ਵਿੱਚ ਵੱਡਾ ਯੋਗਦਾਨ ਪਾਉਂਦੇ ਹਨ।",
                RecommendedFinger = "30 ਡਬਲਯੂ.ਪੀ.ਐਮ. ਦਾ ਟੀਚਾ ਹਾਸਲ ਕਰੋ"
            });

            // ==========================================
            // 7. GUJARATI LESSONS (ગુજરાતી ટાઈપિંગ)
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "guj_les_01",
                Language = PassageLanguage.Gujarati,
                LessonNumber = 1,
                Category = "હોમ રો (Home Row)",
                Title = "ગુજરાતી ઇન્સ્ક્રિપ્ટ હોમ રો (ક ત ચ પ ર સ હ)",
                Description = "ગુજરાતી કીબોર્ડમાં હોમ રો ની આંગળીઓની સાચી સ્થિતિ.",
                TargetKeys = "ક ત ચ પ ર સ હ",
                PracticeText = "કતચપ રસહ કતચપ રસહ કતચપ રસહ કતચપ રસહ",
                RecommendedFinger = "ડાબા અને જમણા હાથની આંગળીઓને હોમ રો પર રાખો"
            });

            _lessons.Add(new LessonModel
            {
                Id = "guj_les_02",
                Language = PassageLanguage.Gujarati,
                LessonNumber = 2,
                Category = "માત્રાઓ (Matras)",
                Title = "કાનો, માત્રા, હ્રસ્વ અને દીર્ઘ",
                Description = "ગુજરાતી સ્વરચિહ્નો (ા ે ૈ ો ૌ િ ી ુ ૂ) નો અભ્યાસ.",
                TargetKeys = "ા ે ૈ ો ૌ િ ી ુ ૂ",
                PracticeText = "ગુજરાત સરકાર વિકાસ શિક્ષણ અદાલત નિયમ સેવા",
                RecommendedFinger = "ડાબા હાથથી સ્વરચિહ્નો ચોકસાઈપૂર્વક ટાઈપ કરો"
            });

            _lessons.Add(new LessonModel
            {
                Id = "guj_les_03",
                Language = PassageLanguage.Gujarati,
                LessonNumber = 3,
                Category = "જોડાક્ષર (Conjuncts)",
                Title = "હલંત અને જોડાક્ષર નિર્માણ",
                Description = "હલંત (્) નો ઉપયોગ કરીને જોડાક્ષર બનાવવાની રીત.",
                TargetKeys = "પ્ર ક્ત સ્ત ત્ર શ્ર",
                PracticeText = "રાષ્ટ્રીય સ્વાતંત્ર્ય પ્રક્રિયા ન્યાય પત્ર વ્યવહાર",
                RecommendedFinger = "હલંત કી દબાવીને જોડાક્ષર ટાઈપ કરો"
            });

            _lessons.Add(new LessonModel
            {
                Id = "guj_les_04",
                Language = PassageLanguage.Gujarati,
                LessonNumber = 4,
                Category = "GPSC / High Court",
                Title = "સરકારી વહીવટી શબ્દભંડોળ",
                Description = "જીપીએસસી અને હાઈકોર્ટ કલાર્ક પરીક્ષા માટે શબ્દો.",
                TargetKeys = "વહીવટી શબ્દો",
                PracticeText = "અરજદાર પંચાયત મ્યુનિસિપલ કલેક્ટર ન્યાયાધીશ હુકમ",
                RecommendedFinger = "ચોકસાઈ 95% ઉપર જાળવો"
            });

            _lessons.Add(new LessonModel
            {
                Id = "guj_les_05",
                Language = PassageLanguage.Gujarati,
                LessonNumber = 5,
                Category = "Exam Paragraph",
                Title = "સંપૂર્ણ વાક્ય પરીક્ષા અભ્યાસ",
                Description = "પરીક્ષાની ગતિ વધારવા માટે ફકરાનું ટાઈપિંગ.",
                TargetKeys = "સંપૂર્ણ વાક્ય",
                PracticeText = "ગુજરાત રાજ્ય દેશમાં ઔદ્યોગિક અને આર્થિક વિકાસના ક્ષેત્રે અગ્રણી સ્થાન ધરાવે છે.",
                RecommendedFinger = "સતત લયબદ્ધ ટાઈપિંગ કરો"
            });

            // ==========================================
            // 8. BENGALI LESSONS (বাংলা টাইপিং)
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "ben_les_01",
                Language = PassageLanguage.Bengali,
                LessonNumber = 1,
                Category = "হোম রো (Home Row)",
                Title = "বাংলা ইনস্ক্রিপ্ট হোম রো (ক ত চ প র স হ)",
                Description = "বাংলা কিবোর্ডে হোম রো আঙুলের অবস্থান শিখুন।",
                TargetKeys = "ক ত চ প র স হ",
                PracticeText = "কতচপ রসহ কতচপ রসহ কতচপ রসহ কতচপ রসহ",
                RecommendedFinger = "বাম ও ডান হাতের আঙুল হোম রো-তে রাখুন"
            });

            _lessons.Add(new LessonModel
            {
                Id = "ben_les_02",
                Language = PassageLanguage.Bengali,
                LessonNumber = 2,
                Category = "কার ও মাত্রা (Vowel Signs)",
                Title = "আ-কার, ই-কার, ঈ-কার ও উ-কার",
                Description = "বাংলা স্বরচিহ্ন (া ি ী ু ূ ে ৈ ো ৌ) অনুশীলন।",
                TargetKeys = "া ি ী ু ূ ে ৈ ো ৌ",
                PracticeText = "বাংলা সরকার শিক্ষা সমাজ বিকাশ আদালত নিয়ম",
                RecommendedFinger = "বাম হাত দিয়ে নির্ভুলভাবে কার চিহ্ন টাইপ করুন"
            });

            _lessons.Add(new LessonModel
            {
                Id = "ben_les_03",
                Language = PassageLanguage.Bengali,
                LessonNumber = 3,
                Category = "যুক্তাক্ষর (Yuktakkhor)",
                Title = "হসন্ত ও যুক্তাক্ষর তৈরি",
                Description = "হসন্ত (্) ব্যবহার করে যুক্তাক্ষর লেখার সঠিক কৌশল।",
                TargetKeys = "প্র ক্ত স্ত ত্র জ্ঞ শ্র",
                PracticeText = "রাষ্ট্রীয় স্বাধীনতা প্রক্রিয়া ন্যায় বিজ্ঞান চরিত্র",
                RecommendedFinger = "হসন্তের সঠিক ব্যবহারে যুক্তাক্ষর তৈরি করুন"
            });

            _lessons.Add(new LessonModel
            {
                Id = "ben_les_04",
                Language = PassageLanguage.Bengali,
                LessonNumber = 4,
                Category = "WBPSC / High Court",
                Title = "সরকারি ও আইনি পরিভাষা",
                Description = "পশ্চিমবঙ্গ সরকারি ক্লার্কশিপ ও আদালতের শব্দাবলী।",
                TargetKeys = "দাপ্তরিক শব্দ",
                PracticeText = "দরখাস্তকারী পঞ্চায়েত আধিকারিক নির্দেশনামা প্রতিবেদন",
                RecommendedFinger = "গতি ও নির্ভুলতার দিকে নজর দিন"
            });

            _lessons.Add(new LessonModel
            {
                Id = "ben_les_05",
                Language = PassageLanguage.Bengali,
                LessonNumber = 5,
                Category = "Exam Paragraph",
                Title = "বাংলা গতি পরীক্ষা অনুশীলন",
                Description = "সরকারি পরীক্ষার জন্য পূর্ণাঙ্গ বাক্য টাইপিং।",
                TargetKeys = "পূর্ণ বাক্য",
                PracticeText = "পশ্চিমবঙ্গ একটি ঐতিহ্যবাহী রাজ্য যার সমৃদ্ধ সংস্কৃতি ও সাহিত্য সারা বিশ্বে সুপরিচিত।",
                RecommendedFinger = "নিয়মিত অনুশীলনে ৩০ ডব্লিউপিএম অর্জন করুন"
            });

            // ==========================================
            // 9. TAMIL LESSONS (தமிழ் தட்டச்சு)
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "tam_les_01",
                Language = PassageLanguage.Tamil,
                LessonNumber = 1,
                Category = "தொடக்க நிலை (Home Row)",
                Title = "தமிழ் தட்டச்சு அடிப்படை (க த ச ப ர ந ல)",
                Description = "தமிழ் இன்ஸ்கிரிப்ட் விசைப்பலகையில் விரல் நிலை.",
                TargetKeys = "க த ச ப ர ந ல",
                PracticeText = "கதசப ரநல கதசப ரநல கதசப ரநல கதசப ரநல",
                RecommendedFinger = "விரல்களை ஹோம் ரோவில் நிலைநிறுத்தவும்"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tam_les_02",
                Language = PassageLanguage.Tamil,
                LessonNumber = 2,
                Category = "உயிர்மெய் குறிகள் (Vowels)",
                Title = "உயிர்மெய் குறிகள் (ா ி ீ ு ூ ெ ே ை ொ ோ)",
                Description = "தமிழ் உயிர்மெய் குறிகளின் துல்லிய பயன்பாடு.",
                TargetKeys = "ா ி ீ ு ூ ெ ே ை ொ ோ",
                PracticeText = "தமிழ்நாடு அரசு கல்வி வளர்ச்சி நீதிமன்றம் திட்டம்",
                RecommendedFinger = "இடது கை விரல்களால் உயிர் குறிகளை தட்டச்சு செய்யவும்"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tam_les_03",
                Language = PassageLanguage.Tamil,
                LessonNumber = 3,
                Category = "மெய் எழுத்துக்கள் (Pulli)",
                Title = "புள்ளி வைத்த மெய்யெழுத்துக்கள் (்)",
                Description = "புள்ளி வைத்து மெய் எழுத்துக்களை உருவாக்கும் முறை.",
                TargetKeys = "க் ங் ச் ஞ் ட் ண் த் ந் ப் ம்",
                PracticeText = "மக்களாட்சி சட்டம் சமுதாயம் சுதந்திரம் வெற்றி",
                RecommendedFinger = "புள்ளி விசையை சரியான முறையில் அழுத்தவும்"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tam_les_04",
                Language = PassageLanguage.Tamil,
                LessonNumber = 4,
                Category = "TNPSC Typist Drills",
                Title = "அரசு அலுவலக கலைச்சொற்கள்",
                Description = "டி.என்.பி.எஸ்.சி தட்டச்சர் தேர்வுக்கான முக்கிய சொற்கள்.",
                TargetKeys = "அலுவலக சொற்கள்",
                PracticeText = "விண்ணப்பதாரர் வருவாய்த்துறை ஆணை சான்றிதழ்",
                RecommendedFinger = "வேகத்தையும் துல்லியத்தையும் அதிகரிக்கவும்"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tam_les_05",
                Language = PassageLanguage.Tamil,
                LessonNumber = 5,
                Category = "Speed Sentences",
                Title = "முழு வாக்கிய தேர்வுப் பயிற்சி",
                Description = "அரசு தட்டச்சு தேர்விற்கான முழு வாக்கியப் பயிற்சி.",
                TargetKeys = "முழு வாக்கியம்",
                PracticeText = "தமிழ்நாடு அரசு பொதுமக்களின் நலனுக்காக பல்வேறு சமூக நலத்திட்டங்களை முனைப்புடன் செயல்படுத்தி வருகிறது.",
                RecommendedFinger = "தொடர்ச்சியான தாள லயத்துடன் தட்டச்சு செய்யவும்"
            });

            // ==========================================
            // 10. TELUGU LESSONS (తెలుగు టైపింగ్)
            // ==========================================
            _lessons.Add(new LessonModel
            {
                Id = "tel_les_01",
                Language = PassageLanguage.Telugu,
                LessonNumber = 1,
                Category = "హోమ్ రో (Home Row)",
                Title = "తెలుగు హోమ్ రో బేసిక్స్ (క త చ ప ర స హ)",
                Description = "తెలుగు కీబోర్డులో హోమ్ రో వేళ్ల సరైన స్థానం.",
                TargetKeys = "క త చ ప ర స హ",
                PracticeText = "కతచప రసహ కతచప రసహ కతచప రసహ కతచప రసహ",
                RecommendedFinger = "వేళ్లను హోమ్ రో పై స్థిరంగా ఉంచండి"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tel_les_02",
                Language = PassageLanguage.Telugu,
                LessonNumber = 2,
                Category = "గుణింతాలు (Guninthalu)",
                Title = "దీర్ఘం, గుడి, కొమ్ము మరియు ఏత్వాలు",
                Description = "తెలుగు అచ్చుల గుర్తులు (ా ి ీ ు ూ ె ే ై ొ ో ౌ) సాధన.",
                TargetKeys = "ా ి ీ ు ూ ె ే ై ొ ో ౌ",
                PracticeText = "ఆంధ్రప్రదేశ్ ప్రభుత్వం అభివృద్ధి న్యాయస్థానం విద్య",
                RecommendedFinger = "ఎడమ చేతి వేళ్లతో అచ్చు గుర్తులను ఖచ్చితంగా టైప్ చేయండి"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tel_les_03",
                Language = PassageLanguage.Telugu,
                LessonNumber = 3,
                Category = "వత్తులు (Vattulu)",
                Title = "పొల్లు మరియు వత్తుల నిర్మాణం (్)",
                Description = "పొల్లు (్) ఉపయోగించి వత్తులను రూపొందించే విధానం.",
                TargetKeys = "క్క గ్గ చ్చ ట్ట త్త ప్ప",
                PracticeText = "రాష్ట్రీయ స్వాతంత్ర్యం ప్రక్రియ న్యాయం దృష్టి",
                RecommendedFinger = "పొల్లు కీ ఉపయోగించి వత్తులు రాయండి"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tel_les_04",
                Language = PassageLanguage.Telugu,
                LessonNumber = 4,
                Category = "APPSC / TSPSC",
                Title = "ప్రభుత్వ అధికారిక పదజాలం",
                Description = "టైపిస్ట్ మరియు కోర్టు ఉద్యోగాల పరీక్ష పదాలు.",
                TargetKeys = "అధికారిక పదాలు",
                PracticeText = "దరఖాస్తుదారు రెవెన్యూ అధికారి నివేదిక ఆదేశాలు",
                RecommendedFinger = "ఖచ్చితత్వాన్ని 95% కంటే ఎక్కువగా ఉంచండి"
            });

            _lessons.Add(new LessonModel
            {
                Id = "tel_les_05",
                Language = PassageLanguage.Telugu,
                LessonNumber = 5,
                Category = "Exam Paragraph",
                Title = "తెలుగు స్పీడ్ టెస్ట్ ప్రాక్టీస్",
                Description = "ప్రభుత్వ పరీక్షల కోసం పూర్తి వాక్య టైపింగ్.",
                TargetKeys = "పూర్తి వాక్యం",
                PracticeText = "ఆధునిక సమాజంలో పౌర సేవల వేగవంతమైన విస్తరణ కోసం డిజిటల్ పరిపాలన అత్యంత కీలకమైనదిగా మారింది.",
                RecommendedFinger = "30 పదాలు ప్రతి నిమిషం సాధించండి"
            });
        }
    }
}
