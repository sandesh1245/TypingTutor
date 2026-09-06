using System;
using System.Collections.Generic;
using System.Linq;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public class VocabularyService
    {
        private static VocabularyService? _instance;
        public static VocabularyService Instance => _instance ??= new VocabularyService();

        private readonly List<VocabDrill> _drills = new List<VocabDrill>();

        public VocabularyService()
        {
            InitializeDrills();
        }

        public List<VocabDrill> GetDrills(PassageLanguage lang)
        {
            return _drills.Where(d => d.Language == lang).ToList();
        }

        public List<VocabDrill> GetAllDrills()
        {
            return _drills.ToList();
        }

        private void InitializeDrills()
        {
            // ==========================================
            // 1. ENGLISH VOCABULARY DRILLS
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_eng_hf_01",
                Title = "SSC High Frequency Words Drill",
                Category = "High Frequency Words",
                Language = PassageLanguage.English,
                Description = "Top high-frequency words appearing in official SSC CGL/CHSL DEST passages.",
                WordList = new List<string>
                {
                    "government", "development", "constitution", "parliament", "administration",
                    "infrastructure", "economic", "financial", "authority", "department",
                    "commission", "committee", "national", "international", "implementation",
                    "organization", "policy", "reform", "growth", "investment", "technology",
                    "governance", "public", "service", "system", "resources", "empowerment",
                    "strategic", "initiatives", "resilience", "production", "management",
                    "agriculture", "industrial", "employment", "opportunity", "security",
                    "community", "welfare", "sustainable", "protection", "transportation"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_eng_sym_01",
                Title = "Numbers & Special Symbols Drill",
                Category = "Symbols & Numbers",
                Language = PassageLanguage.English,
                Description = "Master numbers, shift symbols (!@#$%^&*), brackets, and punctuation.",
                WordList = new List<string>
                {
                    "12345", "67890", "2026-09-06", "$1,250.50", "100%", "#102/B", "(2024-2025)",
                    "section@gov.in", "[Ref: 589/A]", "WPM>45", "{item_count: 50}", "price: 85%",
                    "No. 12/2024-CHSL", "Rs. 15,000/-", "(10/05/1998)", "http://ssc.gov.in"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_eng_dbl_01",
                Title = "Double-Key Dexterity Drill",
                Category = "Double Characters",
                Language = PassageLanguage.English,
                Description = "Improve muscle memory for words containing consecutive repeated letters (ee, ll, mm, ss, tt, ff).",
                WordList = new List<string>
                {
                    "committee", "successful", "possession", "assess", "accept", "traffic",
                    "address", "occurred", "necessary", "bulletin", "approval", "different",
                    "effective", "efficiency", "support", "opportunity", "accountability"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_eng_legal_01",
                Title = "High Court & Legal Terminology",
                Category = "Legal Terminology",
                Language = PassageLanguage.English,
                Description = "Common legal words from High Court and Supreme Court typing examinations.",
                WordList = new List<string>
                {
                    "petitioner", "respondent", "jurisdiction", "affidavit", "plaintiff",
                    "defendant", "appellant", "proceedings", "judiciary", "testimony",
                    "adjudication", "prosecution", "statutory", "injunction", "arbitration",
                    "magistrate", "constitutional", "tribunal", "decree", "subordinate"
                }
            });

            // ==========================================
            // 2. HINDI REMINGTON GAIL DRILLS
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_hin_rem_01",
                Title = "UPSSSC / High Court उच्च आवृत्ति शब्द",
                Category = "High Frequency Words",
                Language = PassageLanguage.Hindi_Remington,
                Description = "सरकारी परीक्षाओं में पूछे जाने वाले महत्वपूर्ण हिंदी शब्द।",
                WordList = new List<string>
                {
                    "सरकार", "प्रशासन", "संविधान", "न्यायालय", "विकास", "योजना", "अधिकारी",
                    "विभाग", "कार्यालय", "आयोग", "समिति", "राष्ट्रीय", "क्षेत्रीय", "प्रक्रिया",
                    "सुरक्षा", "कल्याण", "निरीक्षण", "रिपोर्ट", "प्रस्ताव", "स्वीकृति",
                    "महत्वपूर्ण", "कर्मचारी", "अधिनियम", "संसद", "मंत्रालय", "निर्देशन"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_hin_rem_02",
                Title = "रेमिंगटन गेल प्रशासनिक शब्दावली",
                Category = "Administrative Terms",
                Language = PassageLanguage.Hindi_Remington,
                Description = "मंत्रालय एवं सचिवालयीन आधिकारिक हिंदी शब्द।",
                WordList = new List<string>
                {
                    "अनुमोदन", "कार्यान्वयन", "परिपत्र", "अधिसूचना", "टिप्पणी", "पत्राचार",
                    "वित्तीय", "लेखांकन", "सत्यापन", "प्राधिकरण", "संशोधन", "नियमन",
                    "दायित्व", "उत्तरदायित्व", "कार्यप्रणाली", "साक्षात्कार", "पदोन्नति"
                }
            });

            // ==========================================
            // 3. HINDI INSCRIPT / MANGAL DRILLS
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_hin_ins_01",
                Title = "एसएससी इन्स्क्रिप्ट उच्च आवृत्ति शब्द",
                Category = "High Frequency Words",
                Language = PassageLanguage.Hindi_Inscript,
                Description = "एसएससी एवं रेलवे इन्स्क्रिप्ट टाइपिंग हेतु मानक शब्द।",
                WordList = new List<string>
                {
                    "लोकतंत्र", "अधिकार", "कर्तव्य", "संस्थान", "परिवहन", "उद्योग",
                    "कृषि", "रोजगार", "उत्पादन", "आर्थिक", "प्रौद्योगिकी", "सशक्तिकरण",
                    "पर्यावरण", "संरक्षण", "नागरिक", "समाज", "समानता", "स्वतंत्रता"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_hin_ins_02",
                Title = "संयुक्ताक्षर एवं हलंत गति ड्रिल",
                Category = "Compound Letters",
                Language = PassageLanguage.Hindi_Inscript,
                Description = "हलंत (D की) के संयोजन से निर्मित क्लिष्ट संयुक्ताक्षर।",
                WordList = new List<string>
                {
                    "राष्ट्रीय", "दृष्टिकोण", "उल्लेखनीय", "प्रशंसा", "व्यक्तित्व",
                    "प्रतिष्ठा", "न्यायिक", "प्रोत्साहन", "प्रतिबद्धता", "व्यावहारिक",
                    "स्थापना", "सम्मान", "सद्भावना", "उत्तरदायित्व", "उत्कृष्टता"
                }
            });

            // ==========================================
            // 4. HINDI KRUTI DEV 010 DRILLS
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_hin_kru_01",
                Title = "कृति देव 010 - मुख्य शब्द संग्रह",
                Category = "High Frequency Words",
                Language = PassageLanguage.Hindi_Kruti,
                Description = "इलाहाबाद हाई कोर्ट व अधीनस्थ न्यायालयों हेतु कृति देव शब्द।",
                WordList = new List<string>
                {
                    "प्रार्थी", "विपक्षी", "निर्णय", "आदेश", "मुकदमा", "गवाही",
                    "वकील", "तहसील", "रजिस्ट्री", "अपील", "जमानत", "अधिवक्ता",
                    "हलफनामा", "मुकदमेबाजी", "कार्रवाई", "पुनरावलोकन", "निस्तारण"
                }
            });

            // ==========================================
            // 5. MARATHI VOCABULARY DRILLS (मराठी)
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_mar_01",
                Title = "MPSC व शासकीय उच्च वारंवारता शब्द",
                Category = "High Frequency Words",
                Language = PassageLanguage.Marathi,
                Description = "महाराष्ट्र लोकसेवा आयोग व जिल्हा परिषद टंकलेखन परीक्षा शब्द.",
                WordList = new List<string>
                {
                    "महाराष्ट्र", "प्रशासन", "मंत्रालय", "जिल्हाधिकारी", "ग्रामपंचायत",
                    "महानगरपालिका", "विकास", "योजना", "शिक्षण", "आरोग्य", "रोजगार",
                    "अधिकारी", "कर्मचारी", "कार्यालय", "शासकीय", "निर्णय", "अहवाल",
                    "परिपत्रक", "अधिसूचना", "महसूल", "उद्योग", "कृषी", "सहकार"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_mar_02",
                Title = "न्यायालयीन व विधी विषयक शब्दसंग्रह",
                Category = "Legal Terminology",
                Language = PassageLanguage.Marathi,
                Description = "मुंबई उच्च न्यायालय व सत्र न्यायालय भरती परीक्षेसाठी विधी शब्द.",
                WordList = new List<string>
                {
                    "अर्जदार", "प्रतिवादी", "याचिका", "न्यायाधीश", "वकील", "साक्षीदार",
                    "जामीन", "खटला", "निकाल", "आदेशपत्र", "शपथपत्र", "दस्तऐवज",
                    "अधिकारक्षेत्र", "नोंदणी", "कायदेशीर", "सुनावणी", "तपासणी"
                }
            });

            // ==========================================
            // 6. PUNJABI RAAVI DRILLS (ਪੰਜਾਬੀ ਰਾਵੀ)
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_pun_01",
                Title = "PSSSB ਕਲਰਕ ਉੱਚ ਆਵਿਰਤੀ ਸ਼ਬਦ",
                Category = "High Frequency Words",
                Language = PassageLanguage.Punjabi_Raavi,
                Description = "ਪੰਜਾਬ ਅਧੀਨ ਸੇਵਾਵਾਂ ਚੋਣ ਬੋਰਡ ਪ੍ਰੀਖਿਆ ਲਈ ਮਹੱਤਵਪੂਰਨ ਸ਼ਬਦਾਵਲੀ।",
                WordList = new List<string>
                {
                    "ਸਰਕਾਰ", "ਪ੍ਰਸ਼ਾਸਨ", "ਪੰਜਾਬ", "ਵਿਕਾਸ", "ਅਧਿਕਾਰੀ", "ਕਰਮਚਾਰੀ",
                    "ਦਫ਼ਤਰ", "ਸਿੱਖਿਆ", "ਸਿਹਤ", "ਰੋਜ਼ਗਾਰ", "ਯੋਜਨਾ", "ਕਮਿਸ਼ਨ",
                    "ਨਿਯਮਾਵਲੀ", "ਸੂਚਨਾ", "ਸੁਰੱਖਿਆ", "ਸਮਾਜਿਕ", "ਭਲਾਈ", "ਵਿੱਤੀ",
                    "ਵਿਧਾਨਸਭਾ", "ਮੰਤਰਾਲਾ", "ਬਜਟ", "ਪੰਚਾਇਤ", "ਜ਼ਿਲ੍ਹਾ"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_pun_02",
                Title = "ਅਦਾਲਤੀ ਅਤੇ ਕਾਨੂੰਨੀ ਸ਼ਬਦਾਵਲੀ",
                Category = "Court & Legal Words",
                Language = PassageLanguage.Punjabi_Raavi,
                Description = "ਪੰਜਾਬ ਅਤੇ ਹਰਿਆਣਾ ਹਾਈ ਕੋਰਟ ਪ੍ਰੀਖਿਆ ਲਈ ਸ਼ਬਦ।",
                WordList = new List<string>
                {
                    "ਅਰਜ਼ੀਦਾਰ", "ਜੱਜ", "ਵਕੀਲ", "ਅਦਾਲਤ", "ਫੈਸਲਾ", "ਗਵਾਹ", "ਜ਼ਮਾਨਤ",
                    "ਮੁਕੱਦਮਾ", "ਸੰਵਿਧਾਨ", "ਹਲਫ਼ਨਾਮਾ", "ਅਧਿਕਾਰ", "ਜਾਂਚ", "ਨਿਆਂਇਕ",
                    "ਹੁਕਮ", "ਰਜਿਸਟਰਾਰ", "ਸੁਣਵਾਈ", "ਦਸਤਾਵੇਜ਼"
                }
            });

            // ==========================================
            // 7. GUJARATI DRILLS (ગુજરાતી)
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_guj_01",
                Title = "GPSC & High Court ઉચ્ચ આવૃત્તિ શબ્દો",
                Category = "High Frequency Words",
                Language = PassageLanguage.Gujarati,
                Description = "ગુજરાત સરકારી ભરતી અને હાઈકોર્ટ કલાર્ક પરીક્ષાના શબ્દો.",
                WordList = new List<string>
                {
                    "ગુજરાત", "સરકાર", "વહીવટ", "સચિવાલય", "કલેક્ટર", "પંચાયત",
                    "વિકાસ", "યોજના", "શિક્ષણ", "આરોગ્ય", "રોજગાર", "અધિકારી",
                    "કર્મચારી", "પરિપત્ર", "જાહેરનામું", "મહેસૂલ", "ઉદ્યોગ", "કૃષિ",
                    "સહકાર", "વિધાનસભા", "અંદાજપત્ર", "નાગરિક", "સુરક્ષા"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_guj_02",
                Title = "અદાલતી કાયદાકીય પરિભાષા",
                Category = "Court & Legal Words",
                Language = PassageLanguage.Gujarati,
                Description = "ગુજરાત હાઈકોર્ટ અને જિલ્લા અદાલત સ્ટેનો-ટાઈપિસ્ટ શબ્દો.",
                WordList = new List<string>
                {
                    "અરજદાર", "પ્રતિવાદી", "ન્યાયાધીશ", "વકીલ", "સાક્ષી", "જામીન",
                    "કેસ", "ચુકાદો", "હુકમ", "સોગંદનામું", "દસ્તાવેજ", "તપાસ",
                    "અધિકારક્ષેત્ર", "સુનાવણી", "કાનૂની", "ન્યાયિક", "રજિસ્ટ્રાર"
                }
            });

            // ==========================================
            // 8. BENGALI DRILLS (বাংলা)
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_ben_01",
                Title = "WBPSC উচ্চ কম্পাঙ্কের শব্দাবলী",
                Category = "High Frequency Words",
                Language = PassageLanguage.Bengali,
                Description = "পশ্চিমবঙ্গ পাবলিক সার্ভিস কমিশন ক্লার্কশিপ টাইপিং শব্দ।",
                WordList = new List<string>
                {
                    "সরকার", "প্রশাসন", "পশ্চিমবঙ্গ", "উন্নয়ন", "আধিকারিক", "কর্মচারী",
                    "দপ্তর", "শিক্ষা", "স্বাস্থ্য", "কর্মসংস্থান", "পরিকল্পনা", "কমিশন",
                    "বিজ্ঞপ্তি", "নথি", "নিরাপত্তা", "সামাজিক", "কল্যাণ", "অর্থনৈতিক",
                    "পঞ্চায়েত", "পৌরসভা", "সচিবালয়", "বাজেট", "নাগরিক"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_ben_02",
                Title = "আদালত ও আইনি শব্দমালা",
                Category = "Court & Legal Words",
                Language = PassageLanguage.Bengali,
                Description = "কলকাতা হাইকোর্ট ও জেলা আদালত টাইপিস্ট পরীক্ষা শব্দ।",
                WordList = new List<string>
                {
                    "দরখাস্তকারী", "বিচারপতি", "আইনজীবী", "আদালত", "রায়", "সাক্ষী", "জামিন",
                    "মোকদ্দমা", "হলফনামা", "সংবিধান", "আইনি", "তদন্ত", "বিচারবিভাগ",
                    "আদেশনামা", "শুনানি", "দলিলপত্র", "প্রমাণ"
                }
            });

            // ==========================================
            // 9. TAMIL DRILLS (தமிழ்)
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_tam_01",
                Title = "TNPSC அதிக பயன்பாட்டு சொற்கள்",
                Category = "High Frequency Words",
                Language = PassageLanguage.Tamil,
                Description = "டி.என்.பி.எஸ்.சி தட்டச்சர் மற்றும் சுருக்கெழுத்தர் தேர்வுக்கான முக்கிய சொற்கள்.",
                WordList = new List<string>
                {
                    "அரசு", "நிர்வாகம்", "தமிழ்நாடு", "வளர்ச்சி", "அதிகாரி", "பணியாளர்",
                    "அலுவலகம்", "கல்வி", "சுகாதாரம்", "வேலைவாய்ப்பு", "திட்டம்", "ஆணையம்",
                    "அறிவிக்கை", "கோப்பு", "பாதுகாப்பு", "சமூக", "நலத்துறை", "நிதி",
                    "பஞ்சாயத்து", "மாநகராட்சி", "தலைமைச்செயலகம்", "நிதிநிலை", "மக்கள்"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_tam_02",
                Title = "நீதிமன்ற சட்ட கலைச்சொற்கள்",
                Category = "Court & Legal Words",
                Language = PassageLanguage.Tamil,
                Description = "சென்னை உயர்நீதிமன்றம் மற்றும் மாவட்ட நீதிமன்ற தட்டச்சு சொற்கள்.",
                WordList = new List<string>
                {
                    "மனுதாரர்", "நீதிபதி", "வழக்கறிஞர்", "நீதிமன்றம்", "தீர்ப்பு", "சாட்சி",
                    "பிணை", "வழக்கு", "பிரமாணப்பத்திரம்", "அரசியலமைப்பு", "சட்டரீதியான",
                    "விசாரணை", "நீதித்துறை", "ஆணை", "ஆவணங்கள்", "சான்றிதழ்"
                }
            });

            // ==========================================
            // 10. TELUGU DRILLS (తెలుగు)
            // ==========================================
            _drills.Add(new VocabDrill
            {
                Id = "vocab_tel_01",
                Title = "APPSC & TSPSC అధిక ఫ్రీక్వెన్సీ పదాలు",
                Category = "High Frequency Words",
                Language = PassageLanguage.Telugu,
                Description = "ఆంధ్రప్రదేశ్ మరియు తెలంగాణ పబ్లిక్ సర్వీస్ కమిషన్ పరీక్ష పదాలు.",
                WordList = new List<string>
                {
                    "ప్రభుత్వం", "పరిపాలన", "ఆంధ్రప్రదేశ్", "తెలంగాణ", "అభివృద్ధి", "అధికారి",
                    "ఉద్యోగి", "కార్యాలయం", "విద్య", "ఆరోగ్యం", "ఉపాధి", "పథకం", "కమిషన్",
                    "ప్రకటన", "భద్రత", "సామాజిక", "సంక్షేమం", "ఆర్థిక", "పంచాయతీ",
                    "మున్సిపాలిటీ", "సచివాలయం", "బడ్జెట్", "పౌరులు"
                }
            });

            _drills.Add(new VocabDrill
            {
                Id = "vocab_tel_02",
                Title = "న్యాయస్థాన చట్టపరమైన పదాలు",
                Category = "Court & Legal Words",
                Language = PassageLanguage.Telugu,
                Description = "హైకోర్టు మరియు జిల్లా కోర్టుల టైపిస్ట్ పరీక్షల చట్టపరమైన పదాలు.",
                WordList = new List<string>
                {
                    "దరఖాస్తుదారు", "న్యాయమూర్తి", "న్యాయవాది", "న్యాయస్థానం", "తీర్పు", "సాక్షి",
                    "బెయిల్", "కేసు", "అఫిడవిట్", "రాజ్యాంగం", "చట్టబద్ధమైన", "విచారణ",
                    "న్యాయవ్యవస్థ", "ఉత్తర్వులు", "పత్రాలు", "ధృవీకరణ"
                }
            });
        }
    }
}
