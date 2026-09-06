using System;
using System.Collections.Generic;
using System.Text;
using TypingTutor.Models;

namespace TypingTutor.Services
{
    public static class PassageDataGenerator
    {
        public static List<TypingPassage> GenerateAllPassages()
        {
            var list = new List<TypingPassage>(1050);

            GenerateLanguagePassages(list, PassageLanguage.English, "eng", EnglishExamThemes, EnglishParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Hindi_Remington, "hin_rem", HindiRemThemes, HindiParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Hindi_Inscript, "hin_ins", HindiInsThemes, HindiParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Hindi_Kruti, "hin_kru", HindiKruThemes, KrutiParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Marathi, "mar_ins", MarathiThemes, MarathiParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Punjabi_Raavi, "pan_raa", PunjabiThemes, PunjabiParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Gujarati, "guj_ins", GujaratiThemes, GujaratiParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Bengali, "ben_ins", BengaliThemes, BengaliParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Tamil, "tam_ins", TamilThemes, TamilParagraphs);
            GenerateLanguagePassages(list, PassageLanguage.Telugu, "tel_ins", TeluguThemes, TeluguParagraphs);

            return list;
        }

        private static void GenerateLanguagePassages(
            List<TypingPassage> target,
            PassageLanguage lang,
            string prefix,
            (string Exam, string Category, string[] Topics) examMeta,
            string[] paragraphs)
        {
            int topicIndex = 0;

            for (int i = 1; i <= 100; i++)
            {
                var topic = examMeta.Topics[topicIndex % examMeta.Topics.Length];
                topicIndex++;

                var difficulty = i switch
                {
                    <= 30 => DifficultyLevel.Easy,
                    <= 70 => DifficultyLevel.Medium,
                    _ => DifficultyLevel.Hard
                };

                string content = BuildLongPassage(paragraphs, i, 1000);

                var passage = new TypingPassage
                {
                    Id = $"{prefix}_test_{i:D3}",
                    Title = $"{examMeta.Exam} Official Test {i:D2} - {topic}",
                    Category = examMeta.Category,
                    Difficulty = difficulty,
                    Language = lang,
                    IsPremium = i > 15, // First 15 are free for all, remaining are pro
                    Content = content
                };

                target.Add(passage);
            }
        }

        private static string BuildLongPassage(string[] pool, int testIndex, int targetWords = 1000)
        {
            var sb = new StringBuilder();
            int wordCount = 0;
            int poolLen = pool.Length;
            int step = 0;

            while (wordCount < targetWords)
            {
                int pIndex = (testIndex * 3 + step * 2) % poolLen;
                string para = pool[pIndex];
                sb.AppendLine(para);
                sb.AppendLine();

                wordCount += para.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
                step++;
            }

            return sb.ToString().Trim();
        }

        #region English Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) EnglishExamThemes =
        (
            "SSC CGL / CHSL",
            "SSC CGL/CHSL",
            new[]
            {
                "Economic Resilience and Capital Investment",
                "Digital Governance and Transparent Public Delivery",
                "Renewable Energy Transition and Solar Initiatives",
                "Administrative Reforms and Public Service Delivery",
                "Artificial Intelligence in Modern Public Infrastructure",
                "Environmental Sustainability and Forest Conservation",
                "Constitutional Foundations and Judicial Accountability",
                "Agricultural Supply Chains and Farmer Welfare",
                "High-Speed Rail Connectivity and Logistics Corridors",
                "Universal Healthcare and Medical Infrastructure Growth",
                "Space Research, Satellite Innovation and Planetary Science",
                "Financial Inclusion and Microfinance Opportunities"
            }
        );

        private static readonly string[] EnglishParagraphs = new[]
        {
            "The rapid transformation of modern governance systems relies fundamentally on transparent digital infrastructure and accountable institutional frameworks. Across public institutions, administrative efficiency has been significantly bolstered through the implementation of automated transaction portals, eliminating redundant paperwork and minimizing citizen wait times.",
            "Economic expansion requires cohesive monetary and fiscal strategies that prioritize productive capital expenditure, sustainable industrial incentives, and comprehensive labor empowerment. Robust domestic demand, combined with strategic international export partnerships, establishes a resilient foundation capable of withstanding worldwide macroeconomic fluctuations.",
            "Technological self-reliance in high-precision manufacturing, semiconductors, and telecommunications represents a vital prerequisite for modern sovereign competitiveness. Continuous investment in specialized vocational institutes ensures that technical talent can readily adapt to the evolving demands of artificial intelligence and robotics.",
            "Environmental preservation demands an urgent shift toward renewable energy alternatives, including solar arrays, offshore wind turbines, and grid-scale battery storage facilities. Mitigating carbon emissions while expanding industrial output is achievable through strict regulatory benchmarks and innovative green technologies.",
            "Judicial processes and legal administrative standards benefit immensely from systematic digitization, case management software, and indexed digital archives. Timely dispute resolution enhances commercial confidence, facilitates direct foreign investments, and safeguards the fundamental statutory protections guaranteed to all citizens.",
            "Rural development initiatives focusing on solar-powered cold storage units, drip irrigation mechanisms, and localized commodity aggregation hubs have revolutionized agricultural value chains. By connecting remote farming cooperatives directly with wholesale urban consumption centers, income volatility is drastically mitigated.",
            "The expansion of universal healthcare networks through primary health sub-centers and telemedicine consultations ensures equitable medical care access across rural demographics. Preventive health screenings and integrated national pharmaceutical procurement have substantially lowered out-of-pocket household expenditures.",
            "Urban mobility and metropolitan connectivity have advanced through dedicated freight corridors, expansion of rapid transit metro lines, and intermodal freight terminal developments. Decreasing logistical turnaround delays directly reduces retail inflation and empowers regional manufacturing enterprises.",
            "A well-structured educational curriculum incorporating analytical thinking, scientific inquiry, and interactive digital simulations prepares upcoming generations for high-value careers. Bridging urban-rural pedagogical gaps remains essential for achieving equitable demographic dividend dividends across diverse communities.",
            "Cybersecurity standards and cryptographic resilience have grown into paramount priorities as essential public utilities transition into cloud-native digital ecosystems. Collaborative information-sharing protocols between enforcement agencies and software providers help preemptively thwart sophisticated digital disruptions."
        };
        #endregion

        #region Hindi Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) HindiRemThemes =
        (
            "UPSSSC / High Court",
            "UPSSSC Junior Assistant",
            new[]
            {
                "भारतीय अर्थव्यवस्था में डिजिटल बैंकिंग का योगदान",
                "न्यायिक सुधार एवं त्वरित न्याय व्यवस्था",
                "कृषि आधुनिकीकरण एवं किसान कल्याण योजनाएं",
                "पर्यावरण संरक्षण एवं जल संचयन तकनीक",
                "राष्ट्रीय शिक्षा नीति एवं कौशल विकास",
                "प्रशासनिक पारदर्शिता एवं ई-गवर्नेंस",
                "सार्वजनिक स्वास्थ्य एवं चिकित्सा अवसंरचना",
                "रेलवे आधुनिकीकरण एवं यात्री सुरक्षा",
                "ऊर्जा आत्मनिर्भरता एवं सौर मिशन",
                "ग्राम पंचायत एवं स्थानीय स्वशासन"
            }
        );

        private static readonly (string Exam, string Category, string[] Topics) HindiInsThemes =
        (
            "Railway NTPC / SSC",
            "Railway NTPC",
            new[]
            {
                "आपदा प्रबंधन एवं राष्ट्रीय सुरक्षा बल",
                "डिजिटल भारत एवं नागरिक सेवाएं",
                "भारतीय रेल की नई विकास यात्रा",
                "सूचना प्रौद्योगिकी एवं रोजगार अवसर",
                "संसदीय लोकतंत्र एवं नागरिक अधिकार",
                "वन संरक्षण एवं जैव विविधता",
                "महिला सशक्तिकरण एवं सामाजिक न्याय",
                "ग्रामीण औद्योगिकीकरण एवं सूक्ष्म उद्यम",
                "विज्ञान अनुसंधान एवं अंतरिक्ष उपलब्धियां",
                "राजकोषीय नीति एवं आर्थिक स्थिरता"
            }
        );

        private static readonly (string Exam, string Category, string[] Topics) HindiKruThemes =
        (
            "Civil Court Kruti Dev",
            "High Court RO/ARO",
            new[]
            {
                "Hkkjrh; U;k; fO/kku ,oa U;kf;d izfØ;k",
                "d`f" + "k mUufr ,oa xzkeh.k fodkl",
                "f'k{kk uhfr ,oa ;qok jkstxkj",
                "i;kZoj.k lqj{kk ,oa ty laj{k.k",
                "iz'kklfud lq/kkj ,oa ikjnf'kZrk",
                "yksdra= ,oa ukxfjd vf/kdkj",
                "jsyos izca/ku ,oa ;k=h lqfo/kk",
                "LokLF; lsok ,oa fpfdRlk lalk/ku",
                "vkfFkZd fodkl ,oa O;kikj uhfr",
                "fMftVy lk{kjrk ,oa rduhd"
            }
        );

        private static readonly string[] HindiParagraphs = new[]
        {
            "भारत में प्रशासनिक सुधारों और शासन प्रणाली के आधुनिकीकरण ने सरकारी सेवाओं को आम नागरिकों तक पहुँचाने में युगांतरकारी भूमिका निभाई है। डिजिटल पोर्टलों और एकल खिड़की व्यवस्था के माध्यम से अब प्रमाण पत्र, भूमि अभिलेख और वित्तीय सहायता बिना किसी मध्यस्थ के सीधे नागरिकों के खातों में पहुँच रही है।",
            "आर्थिक विकास की गति को बनाए रखने के लिए बुनियादी ढांचे में निरंतर निवेश अत्यंत आवश्यक है। आधुनिक एक्सप्रेसवे, समर्पित मालवाहक गलियारे और आधुनिक बंदरगाहों का निर्माण देश के औद्योगिक उत्पादन को गति प्रदान कर रहा है। इससे न केवल परिवहन लागत में कमी आई है, बल्कि स्थानीय स्तर पर रोजगार के नए अवसर भी सृजित हुए हैं।",
            "कृषि क्षेत्र भारतीय अर्थव्यवस्था की रीढ़ है। आधुनिक कृषि तकनीकों, सूक्ष्म सिंचाई प्रणालियों और उन्नत किस्म के बीजों के उपयोग से खाद्यान्न उत्पादन में निरंतर वृद्धि हो रही है। फसल बीमा योजना और प्रत्यक्ष आय सहायता से किसानों को प्राकृतिक आपदाओं के जोखिम से सुरक्षा मिली है तथा उनकी वित्तीय स्थिति सुदृढ़ हुई है।",
            "पर्यावरण संतुलन बनाए रखने के लिए नवीकरणीय ऊर्जा स्रोतों का विकास आज की सबसे बड़ी प्राथमिकता है। सौर ऊर्जा संयंत्रों और पवन ऊर्जा परियोजनाओं की स्थापना से पारंपरिक जीवाश्म ईंधनों पर निर्भरता कम हो रही है। इसके साथ ही सघन वृक्षारोपण और भूजल पुनर्भरण अभियानों से पर्यावरण क्षरण को रोकने में सहायता मिली है।",
            "न्यायपालिका समाज में विधि के शासन और नागरिकों के मौलिक अधिकारों की संरक्षक है। अदालतों में मुकदमों के त्वरित निस्तारण के लिए ई-कोर्ट मिशन और राष्ट्रीय न्यायिक डेटा ग्रिड जैसी पहलों ने न्याय प्रक्रिया को अधिक पारदर्शी और सुलभ बनाया है। विधिक साक्षरता शिविरों के माध्यम से वंचित वर्गों को कानूनी सहायता उपलब्ध कराई जा रही है।",
            "शिक्षा किसी भी राष्ट्र के बौद्धिक और सांस्कृतिक विकास की आधारशिला है। नई राष्ट्रीय शिक्षा नीति में मातृभाषा में प्राथमिक शिक्षा, व्यावहारिक कौशल प्रशिक्षण और अनुसंधान को विशेष महत्व दिया गया है। इससे विद्यार्थियों में नवाचार और स्वतंत्र चिंतन की क्षमता विकसित हो रही है जो उन्हें वैश्विक प्रतिस्पर्धा के योग्य बनाती है।",
            "सार्वजनिक स्वास्थ्य सेवाओं के सुदृढ़ीकरण से मातृ एवं शिशु मृत्यु दर में उल्लेखनीय गिरावट दर्ज की गई है। प्राथमिक स्वास्थ्य केंद्रों का उन्नयन, मुफ्त दवाओं का वितरण और टेलीमेडिसिन सेवाओं के विस्तार ने ग्रामीण क्षेत्रों में गुणवत्तापूर्ण चिकित्सा परामर्श को सहज बना दिया है। संचारी रोगों की रोकथाम हेतु चलाए गए टीकाकरण अभियानों ने देश को स्वस्थ बनाया है।",
            "भारतीय रेल देश की जीवन रेखा है। पटरियों का दोहरीकरण, पूर्ण विद्युतीकरण और नई पीढ़ी की आधुनिक रेलगाड़ियों का संचालन यात्रियों को सुरक्षित और आरामदायक यात्रा का अनुभव प्रदान कर रहा है। रेलवे स्टेशनों का पुनर्विकास और स्वचालित सिग्नलिंग प्रणाली ने ट्रेनों की समयबद्धता और सुरक्षा मानकों को अंतरराष्ट्रीय स्तर पर पहुंचाया है।",
            "पंचायती राज संस्थाएं ग्रामीण लोकतंत्र को जमीनी स्तर पर सशक्त बनाती हैं। ग्राम सभाओं की नियमित बैठकों से विकास योजनाओं के चयन में पारदर्शिता आई है और ग्रामीणों को अपनी प्राथमिकताओं के अनुसार निर्णय लेने का अधिकार प्राप्त हुआ है। महिला प्रतिनिधियों की बढ़ती भागीदारी ने ग्रामीण नेतृत्व को अधिक संवेदनशील बनाया है।",
            "विज्ञान और प्रौद्योगिकी के क्षेत्र में भारत की उपलब्धियां वैश्विक स्तर पर सराही जा रही हैं। अंतरिक्ष अनुसंधान, उपग्रह प्रक्षेपण और जैव प्रौद्योगिकी के क्षेत्र में स्वदेशी अनुसंधानों ने देश को आत्मनिर्भर बनाया है। युवा वैज्ञानिकों को प्रोत्साहन देकर और अनुसंधान प्रयोगशालाओं का विस्तार करके नवाचार की एक नई संस्कृति स्थापित की गई है।"
        };

        private static readonly string[] KrutiParagraphs = new[]
        {
            "Hkkjr esa iz'kklfud lq/kkjksa vkSj 'kklu iz.kkyh ds vk/kqfudhdj.k us ljdkjh lsokvksa dks vke ukxfjdksa rd igqapkus esa ;qxkarjdkjh Hkwfedk fuHkkbZ gSA fMftVy iksVZyksa vkSj ,dy f[kM+dh O;oLFkk ds ek/;e ls vc izek.k i=] Hkwfe vfHkys[k vkSj foÙkh; lgk;rk fcuk fdlh e/;LFk ds lh/ks ukxfjdksa ds [kkrksa esa igqap jgh gSA",
            "vkfFkZd fodkl dh xfr dks cuk, j[kus ds fy, cqfu;knh <kaps esa fujarj fuos'k vR;ar vko';d gSA vk/kqfud ,Dlizslos] lefiZr ekyokgd xfy;kjs vkSj vk/kqfud canjxkgksa dk fuekZ.k ns'k ds vkS|ksfxd mRiknu dks xfr iznku dj jgk gSA blls u dsoy ifjogu ykxr esa deh vkbZ gS] cfYd LFkkuh; Lrj ij jkstxkj ds u, volj Hkh l`ftr gq, gSaA",
            "d`f" + "k {ks= Hkkjrh; vFkZO;oLFkk dh jh<+ gSA vk/kqfud d`f" + "k rduhdksa] lw{e flapkbZ iz.kkfy;ksa vkSj mUur fdLe ds chtksa ds mi;ksx ls [kk|kUu mRiknu esa fujarj o`f) gks jgh gSA Qly chek ;kstuk vkSj izR;{k vk; lgk;rk ls fdlkuksa dks izkd`frd vkinkvksa ds tksf[ke ls lqj{kk feyh gS rFkk mudh foÙkh; fLFkfr lqn`<+ gqbZ gSA",
            "i;kZoj.k larqyu cuk, j[kus ds fy, uohdj.kh; mtkZ lzksrksa dk fodkl vkt dh lcls cM+h izkFkfedrk gSA lkSj mtkZ la;a=ksa vkSj iou mtkZ ifj;kstukvksa dh LFkkiuk ls ikjaifjd thok'e bZa/kuksa ij fuHkZjrk de gks jgh gSA blds lkFk gh l?ku o`{kkjksi.k vkSj Hkwty iquHkZj.k vfHk;kuksa ls i;kZoj.k {kj.k dks jksdus esa lgk;rk feyh gSA",
            "U;k;ikfydk lekt esa fof/k ds 'kklu vkSj ukxfjdksa ds ekSfyd vf/kdkjksa dh laj{kd gSA vnkyrksa esa eqdneksa ds Rofjr fuLrkj.k ds fy, bZ&dksVZ fe'ku vkSj jk" + "Vªh; U;kf;d MsVk fxzM tSlh igyksa us U;k; izfØ;k dks vf/kd ikjn'khZ vkSj lqyHk cuk;k gSA fof/kd lk{kjrk f'kfojksa ds ek/;e ls oafpr oxksZa dks dkuwuh lgk;rk miyC/k djkbZ tk jgh gSA",
            "f'k{kk fdlh Hkh jk" + "Vª ds ckSf)d vkSj lkaLd`frd fodkl dh vk/kkjf'kyk gSA ubZ jk" + "Vªh; f'k{kk uhfr esa ekr`Hkk" + "kk esa izkFkfed f'k{kk] O;kogkfjd dkS'ky izf'k{k.k vkSj vuqla/kku dks fo'ks" + "k egRo fn;k x;k gSA blls fo|kfFkZ;ksa esa uokpkj vkSj Lora= fparu dh {kerk fodflr gks jgh gS tks mUgsa oSf'od izfrLi/kkZ ds ;ksX; cukrh gSA",
            "lkoZtfud LokLF; lsokvksa ds lqn`<+hdj.k ls ekr` ,oa f'k'kq e`R;q nj esa mYys[kuh; fxjkoV ntZ dh xbZ gSA izkFkfed LokLF; dsanzksa dk mUu;u] eq¶r nokvksa dk forj.k vkSj VsyhesfMflu lsokvksa ds foLrkj us xzkeh.k {ks=ksa esa xq.koÙkkiw.kZ fpfdRlk ijke'kZ dks lgt cuk fn;k gSA lapkjh jksxksa dh jksdFkke gsrq pyk, x, Vhdkdj.k vfHk;kuksa us ns'k dks LoLFk cuk;k gSA",
            "Hkkjrh; jsy ns'k dh thou js[kk gSA ifVª;ksa dk nksgjhdj.k] iw.kZ fo|qrhdj.k vkSj ubZ ih<+h dh vk/kqfud jsyxkfM+;ksa dk lapkyu ;kf=;ksa dks lqjf{kr vkSj vkjkensg ;k=k dk vuqHko iznku dj jgk gSA jsyos LVs'kuksa dk iqufoZdkl vkSj Lopkfyr flXufyax iz.kkyh us Vªsuksa dh le;c)rk vkSj lqj{kk ekudksa dks varjjk" + "Vªh; Lrj ij igqapk;k gSA",
            "iapk;rh jkt laLFkk,a xzkeh.k yksdra= dks tehuh Lrj ij l'kDr cukrh gSaA xzke lHkkvksa dh fu;fer cSBdksa ls fodkl ;kstukvksa ds p;u esa ikjnf'kZrk vkbZ gS vkSj xzkeh.kksa dks viuh izkFkfedrkvksa ds vuqlkj fu.kZ; ysus dk vf/kdkj izkIr gqvk gSA efgyk izfrfuf/k;ksa dh c<+rh Hkkxhnkjh us xzkeh.k usr`Ro dks vf/kd laosnu'khy cuk;k gSA",
            "foKku vkSj izkS|ksfxdh ds {ks= esa Hkkjr dh miyfC/k;ka oSf'od Lrj ij ljkgh tk jgh gSaA varfj{k vuqla/kku] mixzg iz{ksi.k vkSj tSo izkS|ksfxdh ds {ks= esa Lons'kh vuqla/kkuksa us ns'k dks vkReuSHkZj cuk;k gSA ;qok oSKkfudksa dks izksRlkgu nsdj vkSj vuqla/kku iz;ksx'kkykvksa dk foLrkj djds uokpkj dh ,d ubZ laLd`fr LFkkfir dh xbZ gSA"
        };
        #endregion

        #region Marathi Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) MarathiThemes =
        (
            "MPSC / Mantralaya Typist",
            "State Govt Exams",
            new[]
            {
                "महाराष्ट्राचा आर्थिक विकास व औद्योगिक वाटचाल",
                "माहितीचा अधिकार व शासकीय पारदर्शकता",
                "जलयुक्त शिवार व पर्यावरण संवर्धन मोहीम",
                "कृषी व सहकार चळवळीचे महाराष्ट्रातील योगदान",
                "शिक्षण व कौशल्य विकास रोजगार योजना",
                "सार्वजनिक आरोग्य व ग्रामीण वैद्यकीय सेवा",
                "पायाभूत सुविधा व समृद्धी महामार्ग प्रकल्प",
                "छत्रपती शिवाजी महाराज व स्वराज्य संकल्पना",
                "पंचायत राज व स्थानिक स्वराज्य संस्थांचे सक्षमीकरण",
                "ई-प्रशासन व डिजिटल सेवांची व्याप्ती"
            }
        );

        private static readonly string[] MarathiParagraphs = new[]
        {
            "महाराष्ट्र हे भारतातील अग्रगण्य औद्योगिक आणि आर्थिकदृष्ट्या प्रगत राज्य मानले जाते. राज्याची राजधानी मुंबई ही देशाची आर्थिक राजधानी असून येथे बँक, शेअर बाजार आणि आंतरराष्ट्रीय व्यापार यांचे मुख्य केंद्र आहे. राज्याने उद्योगस्नेही धोरणे राबवून विदेशी गुंतवणुकीला मोठ्या प्रमाणावर आकर्षित केले आहे, ज्यामुळे लाखो तरुणांना रोजगाराच्या नव्या संधी उपलब्ध झाल्या आहेत.",
            "माहितीचा अधिकार हा नागरिकांना लोकशाही कारभारात प्रत्यक्ष सहभागी करून घेणारा अत्यंत शक्तिशाली कायदा आहे. शासकीय कार्यालयातील निर्णय प्रक्रियेत पारदर्शकता आणि उत्तरदायित्व निर्माण करण्यात या कायद्याने मोलाची भूमिका बजावली आहे. प्रशासनातील विहित मुदतीत सेवा देण्याचा कायदा लागू झाल्याने शासकीय कामांची गतिमानता अधिक वाढली आहे.",
            "महाराष्ट्रातील कृषी क्षेत्र हे सहकार चळवळीशी घट्ट जोडलेले आहे. सहकारी साखर कारखाने, दुग्ध व्यवसाय संस्था आणि जिल्हा मध्यवर्ती सहकारी बँकांनी ग्रामीण अर्थव्यवस्थेत क्रांती घडवून आणली आहे. शेतकऱ्यांना वाजवी दरात पतपुरवठा, बियाणे आणि खते उपलब्ध करून दिल्यामुळे कृषी उत्पादनात लक्षणीय वाढ झाली असून शेतकऱ्यांचे राहणीमान उंचावले आहे.",
            "पर्यावरण संवर्धन आणि जलसंधारण ही आजच्या काळातील सर्वात निकडीची गरज बनली आहे. राज्यातील भूजल पातळी वाढवण्यासाठी बंधारे, शेततळी आणि वृक्षारोपण यांच्या माध्यमातून लोकसहभागातून मोठी चळवळ उभी राहिली आहे. जलसंधारणाच्या कामांमुळे दुष्काळग्रस्त भागातील गावांना पिण्याच्या पाण्याचा आणि शेतीसाठी पाण्याचा शाश्वत स्त्रोत मिळाला आहे.",
            "छत्रपती शिवाजी महाराजांची स्वराज्य संकल्पना ही रयतेचे कल्याण आणि न्यायावर आधारित आदर्श प्रशासनाचे मूर्तिमंत उदाहरण आहे. गडकोटांचे संरक्षण, आरमार उभारणी, पर्यावरण रक्षण आणि शेतकऱ्यांच्या धान्याला संरक्षण देणारे त्यांचे धोरण आजही जगभरातील राज्यकर्त्यांना आणि विचारवंतांना मार्गदर्शक ठरते.",
            "राज्यातील शिक्षण व्यवस्थेने ग्रामीण व दुर्गम भागातील विद्यार्थ्यांपर्यंत शिक्षणाची गंगा पोहोचवण्यात मोठे यश संपादन केले आहे. प्राथमिक शाळांमध्ये डिजिटल वर्गखोल्या, संगणक प्रशिक्षण आणि पोषण आहार योजनेमुळे विद्यार्थ्यांची गळती रोखण्यात यश आले आहे. व्यावसायिक आणि तांत्रिक शिक्षणावर भर दिल्यामुळे कुशल मनुष्यबळ तयार होत आहे.",
            "सार्वजनिक आरोग्य क्षेत्राचा विस्तार करताना प्राथमिक आरोग्य केंद्रे आणि उपकेंद्रांचे आधुनिकीकरण करण्यात आले आहे. माता व बाल आरोग्य सुधारण्यासाठी सुरू केलेल्या योजनांमुळे आरोग्य निर्देशांकात लक्षणीय प्रगती झाली आहे. महात्मा ज्योतिराव फुले जनआरोग्य योजनेद्वारे गरीब कुटुंबांना दर्जेदार रुग्णालयात मोफत उपचारांची सुविधा उपलब्ध झाली आहे.",
            "पायाभूत सुविधांचा विकास हा राज्याच्या सर्वांगीण प्रगतीचा कणा आहे. समृद्धी महामार्ग, सागरी सेतू, मेट्रो रेल्वे आणि आंतरराष्ट्रीय विमानतळांची उभारणी यामुळे प्रवासाचा वेळ वाचला असून मालाची वाहतूक जलद गतीने शक्य झाली आहे. यामुळे महाराष्ट्रातील अविकसित जिल्ह्यांमध्ये नवीन उद्योग सुरू होण्यास मोठी चालना मिळाली आहे.",
            "पंचायत राज व्यवस्थेने सत्तेचे विकेंद्रीकरण करून ग्रामीण जनतेला स्वतःच्या गावाचा विकास स्वतः ठरवण्याचा अधिकार दिला आहे. ग्रामपंचायतींमध्ये महिलांना पन्नास टक्के आरक्षण मिळाल्यामुळे महिला नेतृत्वाचा उदय झाला आहे. गावातील स्वच्छता, पाणीपुरवठा, रस्ते आणि दिवाबत्ती या मूलभूत सोयींचा दर्जा उंचावला आहे.",
            "ई-प्रशासन आणि माहिती तंत्रज्ञानाचा प्रभावी वापर करून शासकीय सेवा थेट नागरिकांच्या दारात पोहोचवल्या जात आहेत. महसूल विभागाचे सातबारा उतारे, जन्म-मृत्यू दाखले आणि विविध परवाने आता मोबाईलवर एका क्लिकवर उपलब्ध आहेत. कागदविरहित कारभारामुळे भ्रष्टाचाराला आळा बसला असून प्रशासनाची विश्वासार्हता वाढली आहे."
        };
        #endregion

        #region Punjabi Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) PunjabiThemes =
        (
            "PSSSB / Punjab High Court",
            "State Govt Exams",
            new[]
            {
                "ਪੰਜਾਬ ਦੀ ਖੇਤੀਬਾੜੀ ਅਤੇ ਆਧੁਨਿਕ ਸਿੰਚਾਈ",
                "ਸ੍ਰੀ ਗੁਰੂ ਨਾਨਕ ਦੇਵ ਜੀ ਦੇ ਸਮਾਜਿਕ ਉਪਦੇਸ਼",
                "ਪੰਜਾਬੀ ਸੱਭਿਆਚਾਰ, ਲੋਕਧਾਰਾ ਅਤੇ ਵਿਰਸਾ",
                "ਉਦਯੋਗਿਕ ਤਰੱਕੀ ਅਤੇ ਰੁਜ਼ਗਾਰ ਦੇ ਮੌਕੇ",
                "ਵਾਤਾਵਰਨ ਸੰਭਾਲ ਅਤੇ ਧਰਤੀ ਹੇਠਲਾ ਪਾਣੀ",
                "ਪੰਚਾਇਤੀ ਰਾਜ ਅਤੇ ਪਿੰਡਾਂ ਦਾ ਵਿਕਾਸ",
                "ਸਿੱਖਿਆ ਖੇਤਰ ਵਿੱਚ ਡਿਜੀਟਲ ਸੁਧਾਰ",
                "ਸਿਹਤ ਸੇਵਾਵਾਂ ਅਤੇ ਪੇਂਡੂ ਹਸਪਤਾਲ",
                "ਕਾਨੂੰਨ ਦਾ ਰਾਜ ਅਤੇ ਅਦਾਲਤੀ ਪ੍ਰਣਾਲੀ",
                "ਸੂਚਨਾ ਤਕਨਾਲੋਜੀ ਅਤੇ ਨੌਜਵਾਨ ਸ਼ਕਤੀ"
            }
        );

        private static readonly string[] PunjabiParagraphs = new[]
        {
            "ਪੰਜਾਬ ਭਾਰਤ ਦਾ ਇੱਕ ਅਹਿਮ ਸਰਹੱਦੀ ਰਾਜ ਹੈ ਜੋ ਆਪਣੀ ਅਮੀਰ ਖੇਤੀਬਾੜੀ ਅਤੇ ਮਿਹਨਤਕਸ਼ ਕਿਸਾਨਾਂ ਲਈ ਪੂਰੀ ਦੁਨੀਆ ਵਿੱਚ ਪ੍ਰਸਿੱਧ ਹੈ। ਹਰੀ ਕ੍ਰਾਂਤੀ ਦੇ ਦੌਰਾਨ ਪੰਜਾਬ ਦੇ ਕਿਸਾਨਾਂ ਨੇ ਦੇਸ਼ ਦੇ ਅੰਨ ਭੰਡਾਰ ਭਰ ਕੇ ਭਾਰਤ ਨੂੰ ਅਨਾਜ ਪੱਖੋਂ ਆਤਮਨਿਰਭਰ ਬਣਾਉਣ ਵਿੱਚ ਸਭ ਤੋਂ ਵੱਡਾ ਯੋਗਦਾਨ ਪਾਇਆ ਸੀ।",
            "ਸ੍ਰੀ ਗੁਰੂ ਨਾਨਕ ਦੇਵ ਜੀ ਨੇ ਸਮਾਜ ਨੂੰ ਕਿਰਤ ਕਰੋ, ਨਾਮ ਜਪੋ ਅਤੇ ਵੰਡ ਛਕੋ ਦਾ ਸਰਬਸਾਂਝਾ ਸੁਨੇਹਾ ਦਿੱਤਾ। ਉਨ੍ਹਾਂ ਨੇ ਜਾਤ-ਪਾਤ ਅਤੇ ਧਾਰਮਿਕ ਵਿਤਕਰੇ ਨੂੰ ਰੱਦ ਕਰਦਿਆਂ ਬਰਾਬਰੀ ਅਤੇ ਸਾਂਝੀਵਾਲਤਾ ਵਾਲੇ ਸਮਾਜ ਦੀ ਨੀਂਹ ਰੱਖੀ। ਗੁਰੂ ਸਾਹਿਬ ਦੇ ਫ਼ਲਸਫ਼ੇ ਨੇ ਪੰਜਾਬੀ ਸੱਭਿਆਚਾਰ ਵਿੱਚ ਦਲੇਰੀ ਅਤੇ ਦਾਨਸ਼ੀਲਤਾ ਭਰੀ।",
            "ਧਰਤੀ ਹੇਠਲੇ ਪਾਣੀ ਦੇ ਡਿੱਗ ਰਹੇ ਪੱਧਰ ਨੂੰ ਬਚਾਉਣਾ ਅੱਜ ਦੇ ਸਮੇਂ ਦੀ ਸਭ ਤੋਂ ਵੱਡੀ ਚੁਣੌਤੀ ਹੈ। ਕਿਸਾਨਾਂ ਨੂੰ ਝੋਨੇ ਦੇ ਰਵਾਇਤੀ ਫ਼ਸਲੀ ਚੱਕਰ ਵਿੱਚੋਂ ਕੱਢ ਕੇ ਘੱਟ ਪਾਣੀ ਵਾਲੀਆਂ ਫ਼ਸਲਾਂ ਜਿਵੇਂ ਦਾਲਾਂ, ਮੱਕੀ ਅਤੇ ਤੇਲ ਬੀਜਾਂ ਵੱਲ ਪ੍ਰੇਰਿਤ ਕੀਤਾ ਜਾ ਰਿਹਾ ਹੈ। ਤੁਪਕਾ ਸਿੰਚਾਈ ਅਤੇ ਮੀਂਹ ਦੇ ਪਾਣੀ ਦੀ ਸੰਭਾਲ ਲਈ ਵਿਸ਼ੇਸ਼ ਮੁਹਿੰਮਾਂ ਚਲਾਈਆਂ ਜਾ ਰਹੀਆਂ ਹਨ।",
            "ਪੰਜਾਬੀ ਸੱਭਿਆਚਾਰ ਅਤੇ ਵਿਰਸਾ ਸੰਸਾਰ ਭਰ ਵਿੱਚ ਆਪਣੀ ਵੱਖਰੀ ਪਛਾਣ ਰੱਖਦਾ ਹੈ। ਭੰਗੜਾ, ਗਿੱਧਾ, ਬੋਲੀਆਂ ਅਤੇ ਲੋਕ ਗੀਤ ਪੰਜਾਬੀਆਂ ਦੀ ਖ਼ੁਸ਼ਦਿਲ ਅਤੇ ਜੋਸ਼ੀਲੀ ਤਬੀਅਤ ਨੂੰ ਦਰਸਾਉਂਦੇ ਹਨ। ਮੇਲੇ ਅਤੇ ਤਿਉਹਾਰ ਜਿਵੇਂ ਵਿਸਾਖੀ, ਲੋਹੜੀ ਅਤੇ ਦੀਵਾਲੀ ਸਮਾਜਿਕ ਏਕਤਾ ਅਤੇ ਆਪਸੀ ਭਾਈਚਾਰਕ ਸਾਂਝ ਨੂੰ ਹੋਰ ਪੱਕਾ ਕਰਦੇ ਹਨ।",
            "ਪੇਂਡੂ ਵਿਕਾਸ ਲਈ ਪੰਚਾਇਤਾਂ ਦੀ ਭੂਮਿਕਾ ਬਹੁਤ ਮਹੱਤਵਪੂਰਨ ਹੈ। ਪਿੰਡਾਂ ਵਿੱਚ ਪੱਕੀਆਂ ਗਲੀਆਂ, ਨਾਲੀਆਂ, ਸਟਰੀਟ ਲਾਈਟਾਂ ਅਤੇ ਸਾਫ਼ ਪੀਣ ਵਾਲੇ ਪਾਣੀ ਦੀ ਵਿਵਸਥਾ ਨਾਲ ਪੇਂਡੂ ਜੀਵਨ ਪੱਧਰ ਉੱਚਾ ਹੋਇਆ ਹੈ। ਔਰਤਾਂ ਨੂੰ ਪੰਚਾਇਤਾਂ ਵਿੱਚ ਪ੍ਰਤੀਨਿਧਤਾ ਮਿਲਣ ਨਾਲ ਪਿੰਡਾਂ ਦੇ ਫ਼ੈਸਲੇ ਵਧੇਰੇ ਸੰਤੁਲਿਤ ਅਤੇ ਪਾਰਦਰਸ਼ੀ ਹੋ ਰਹੇ ਹਨ।",
            "ਸਕੂਲੀ ਸਿੱਖਿਆ ਨੂੰ ਆਧੁਨਿਕ ਬਣਾਉਣ ਲਈ ਸਮਾਰਟ ਸਕੂਲ ਪ੍ਰੋਜੈਕਟ ਸ਼ੁਰੂ ਕੀਤੇ ਗਏ ਹਨ। ਸਕੂਲਾਂ ਵਿੱਚ ਕੰਪਿਊਟਰ ਲੈਬਾਂ, ਵਿਗਿਆਨ ਪ੍ਰਯੋਗਸ਼ਾਲਾਵਾਂ ਅਤੇ ਖੇਡ ਮੈਦਾਨਾਂ ਦਾ ਨਿਰਮਾਣ ਕੀਤਾ ਗਿਆ ਹੈ ਤਾਂ ਜੋ ਵਿਦਿਆਰਥੀਆਂ ਦਾ ਸਰਬਪੱਖੀ ਵਿਕਾਸ ਹੋ ਸਕੇ। ਤਕਨੀਕੀ ਸਿੱਖਿਆ ਨਾਲ ਨੌਜਵਾਨਾਂ ਨੂੰ ਆਧੁਨਿਕ ਰੁਜ਼ਗਾਰ ਦੇ ਕਾਬਲ ਬਣਾਇਆ ਜਾ ਰਿਹਾ ਹੈ।",
            "ਸਿਹਤ ਖੇਤਰ ਵਿੱਚ ਸਰਕਾਰੀ ਹਸਪਤਾਲਾਂ ਅਤੇ ਆਮ ਆਦਮੀ ਕਲੀਨਿਕਾਂ ਦੀ ਸਥਾਪਨਾ ਨਾਲ ਆਮ ਲੋਕਾਂ ਨੂੰ ਮੁਫ਼ਤ ਜਾਂਚ ਅਤੇ ਦਵਾਈਆਂ ਮਿਲਣੀਆਂ ਯਕੀਨੀ ਹੋਈਆਂ ਹਨ। ਪਿੰਡਾਂ ਦੇ ਪੱਧਰ 'ਤੇ ਮੁੱਢਲੀਆਂ ਸਿਹਤ ਸੇਵਾਵਾਂ ਮਿਲਣ ਨਾਲ ਸ਼ਹਿਰੀ ਵੱਡੇ ਹਸਪਤਾਲਾਂ 'ਤੇ ਬੋਝ ਘਟਿਆ ਹੈ ਅਤੇ ਗ਼ਰੀਬ ਪਰਿਵਾਰਾਂ ਦਾ ਇਲਾਜ ਦਾ ਖ਼ਰਚਾ ਬਚਿਆ ਹੈ।",
            "ਸਨਅਤੀ ਖੇਤਰ ਵਿੱਚ ਲੁਧਿਆਣਾ ਦੇ ਹੋਜ਼ਰੀ ਉਦਯੋਗ, ਜਲੰਧਰ ਦੇ ਖੇਡ ਸਾਮਾਨ ਅਤੇ ਬਟਾਲਾ ਦੇ ਇੰਜੀਨੀਅਰਿੰਗ ਉਦਯੋਗ ਨੇ ਵਿਸ਼ਵ ਪੱਧਰ 'ਤੇ ਆਪਣਾ ਲੋਹਾ ਮਨਵਾਇਆ ਹੈ। ਨਵੀਆਂ ਉਦਯੋਗਿਕ ਨੀਤੀਆਂ ਨਾਲ ਪੰਜਾਬ ਵਿੱਚ ਨਿਵੇਸ਼ਕਾਂ ਨੂੰ ਸਿੰਗਲ ਵਿੰਡੋ ਸਿਸਟਮ ਰਾਹੀਂ ਤੇਜ਼ੀ ਨਾਲ ਪ੍ਰਵਾਨਗੀਆਂ ਮਿਲ ਰਹੀਆਂ ਹਨ।",
            "ਅਦਾਲਤੀ ਪ੍ਰਣਾਲੀ ਵਿੱਚ ਪੰਜਾਬੀ ਭਾਸ਼ਾ ਦੀ ਵਰਤੋਂ ਵਧਣ ਨਾਲ ਆਮ ਨਾਗਰਿਕਾਂ ਨੂੰ ਕਾਨੂੰਨੀ ਕਾਰਵਾਈ ਸਮਝਣੀ ਆਸਾਨ ਹੋ ਗਈ ਹੈ। ਲੋਕ ਅਦਾਲਤਾਂ ਅਤੇ ਵਿਚੋਲਗੀ ਕੇਂਦਰਾਂ ਰਾਹੀਂ ਝਗੜਿਆਂ ਦਾ ਆਪਸੀ ਸਹਿਮਤੀ ਨਾਲ ਨਿਪਟਾਰਾ ਕਰਕੇ ਸਮੇਂ ਅਤੇ ਧਨ ਦੀ ਬੱਚਤ ਕੀਤੀ ਜਾ ਰਹੀ ਹੈ।",
            "ਡਿਜੀਟਲ ਗਵਰਨੈਂਸ ਰਾਹੀਂ ਸਰਕਾਰੀ ਦਫ਼ਤਰਾਂ ਵਿੱਚ ਭ੍ਰਿਸ਼ਟਾਚਾਰ ਨੂੰ ਠੱਲ੍ਹ ਪਈ ਹੈ। ਜ਼ਮੀਨਾਂ ਦੀਆਂ ਫ਼ਰਦਾਂ, ਜਾਤੀ ਸਰਟੀਫਿਕੇਟ ਅਤੇ ਰਿਹਾਇਸ਼ੀ ਸਰਟੀਫਿਕੇਟ ਹੁਣ ਸੇਵਾ ਕੇਂਦਰਾਂ ਰਾਹੀਂ ਬਿਨਾਂ ਕਿਸੇ ਖੱਜਲ-ਖ਼ੁਆਰੀ ਦੇ ਤੈਅ ਸਮੇਂ ਅੰਦਰ ਮਿਲ ਰਹੇ ਹਨ।"
        };
        #endregion

        #region Gujarati Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) GujaratiThemes =
        (
            "GSSSB / GPSC Typist",
            "State Govt Exams",
            new[]
            {
                "ગુજરાતનો ઔદ્યોગિક વિકાસ અને વેપારી સાહસ",
                "સરદાર વલ્લભભાઈ પટેલ અને અખંડ ભારત",
                "નર્મદા યોજના અને ગુજરાતની જીવાદોરી",
                "ડિજિટલ ગવર્નન્સ અને ઈ-સેવાઓનો વિસ્તાર",
                "સહકારી ડેરી ઉદ્યોગ અને શ્વેતક્રાંતિ",
                "બંદરોનો વિકાસ અને આંતરરાષ્ટ્રીય વ્યાપાર",
                "ગુજરાતી સાહિત્ય, કલા અને ગરબાની વૈશ્વિક ખ્યાતિ",
                "શિક્ષણ અને કૌશલ્ય નિર્માણ યોજનાઓ",
                "પર્યાવરણ રક્ષણ અને રિન્યુએબલ એનર્જી",
                "પંચાયતી રાજ અને ગ્રામીણ સમૃદ્ધિ"
            }
        );

        private static readonly string[] GujaratiParagraphs = new[]
        {
            "ગુજરાત ભારતના પશ્ચિમ કિનારે આવેલું એક અત્યંત સમૃદ્ધ અને ઔદ્યોગિક રીતે અગ્રેસર રાજ્ય છે. રાજ્યની પ્રગતિમાં તેના સાહસિક ઉદ્યોગપતિઓ, વેપારીઓ અને શાંતિપ્રિય નાગરિકોનો મોટો ફાળો રહ્યો છે. વાઈબ્રન્ટ ગુજરાત જેવા કાર્યક્રમો દ્વારા વિશ્વભરમાંથી મોટા પાયે મૂડીરોકાણ આકર્ષવામાં આવ્યું છે.",
            "સરદાર વલ્લભભાઈ પટેલ ભારતના લોખંડી પુરુષ તરીકે ઇતિહાસમાં અમર છે. આઝાદી પછી દેશના પાંચસોથી વધુ રજવાડાઓનું એકીકરણ કરીને તેમણે અખંડ ભારતનું નિર્માણ કર્યું. તેમનું વહીવટી કૌશલ્ય, મક્કમ મનોબળ અને રાષ્ટ્રભક્તિ આજના દરેક યુવાન માટે પ્રેરણારૂપ છે.",
            "નર્મદા નદી પર બંધાયેલો સરદાર સરોવર ડેમ ગુજરાતની સાચી જીવાદોરી સાબિત થયો છે. નર્મદાના નીર સૌરાષ્ટ્ર અને કચ્છના સૂકા વિસ્તારો સુધી પહોંચતા ત્યાં ખેતી અને પીવાના પાણીની સમસ્યાનું કાયમી નિરાકરણ આવ્યું છે. સિંચાઈની સુવિધાથી ખેડૂતો વર્ષમાં ત્રણ પાક લેતા થયા છે.",
            "ગુજરાતનો ડેરી ઉદ્યોગ વિશ્વભરમાં સહકારી મોડેલનું ઉત્કૃષ્ટ ઉદાહરણ છે. અમૂલ ડેરીની સ્થાપના સાથે શરૂ થયેલી શ્વેતક્રાંતિએ લાખો ગ્રામીણ મહિલાઓ અને પશુપાલકોને આર્થિક રીતે આત્મનિર્ભર બનાવ્યા છે. દૂધ ઉત્પાદન સાથે સંકળાયેલા ખેડૂતોને તેમના ઉત્પાદનનું યોગ્ય વળતર સીધું તેમના બેંક ખાતામાં મળે છે.",
            "ગુજરાતનો ૧૬૦૦ કિલોમીટર લાંબો દરિયાકિનારો રાજ્યના વ્યાપાર માટે વરદાનરૂપ છે. કંડલા, મુન્દ્રા અને દહેજ જેવા આધુનિક બંદરો મારફતે ભારતનો મોટો આંતરરાષ્ટ્રીય વ્યાપાર સંચાલિત થાય છે. આ બંદરો સાથે જોડાયેલા સ્પેશિયલ ઇકોનોમિક ઝોનથી નિકાસમાં ભારે ઉછાળો આવ્યો છે.",
            "ગુજરાતની સંસ્કૃતિ અને લોકનૃત્ય ગરબાને યુનેસ્કો દ્વારા અમૂર્ત સાંસ્કૃતિક વારસા તરીકે માન્યતા આપવામાં આવી છે. નવરાત્રીનો ઉત્સવ ભક્તિ, ઉત્સાહ અને સામૂહિકતાનું અનુપમ પ્રતીક છે. આ તહેવારો દ્વારા સમાજમાં પરસ્પર પ્રેમ અને ભાઈચારાની ભાવના સુદ્રઢ બને છે.",
            "રિન્યુએબલ એનર્જીના ક્ષેત્રમાં ગુજરાતે સમગ્ર દેશમાં નેતૃત્વ લીધું છે. ચારણકાનો સોલર પાર્ક અને કચ્છમાં બની રહેલો વિશ્વનો સૌથી મોટો હાઇબ્રિડ રિન્યુએબલ એનર્જી પાર્ક સ્વચ્છ ઊર્જા ઉત્પાદનમાં મોટો ફાળો આપી રહ્યા છે. સોલાર રૂફટોપ યોજનામાં ગુજરાત દેશમાં પ્રથમ ક્રમે છે.",
            "શિક્ષણ ક્ષેત્રે બાળકોના સર્વાંગી વિકાસ માટે શાળા પ્રવેશોત્સવ અને ગુણોત્સવ જેવા અભિયાનો ચલાવવામાં આવ્યા છે. ડિજિટલ ક્લાસરૂમ અને વૈજ્ઞાનિક પ્રયોગશાળાઓ દ્વારા ગ્રામીણ શાળાઓનું સ્તર ઊંચું આવ્યું છે. યુનિવર્સિટીઓમાં સંશોધન અને નવીનતાને પ્રોત્સાહન આપવામાં આવી રહ્યું છે.",
            "પંચાયતી રાજ વ્યવસ્થાથી છેવાડાના ગામડાઓ સુધી વહીવટ પહોંચ્યો છે. ઈ-ગ્રામ યોજના હેઠળ દરેક પંચાયતમાં કોમ્પ્યુટર અને બ્રોડબેન્ડ કનેક્ટિવિટી ઉપલબ્ધ કરાઈ છે, જેથી ગામના ખેડૂતોને ૭/૧૨ અને ૮-અ ના ઉતારા પોતાના ગામમાં જ મળી રહે છે.",
            "ન્યાયતંત્ર અને વહીવટી પારદર્શિતા માટે કાયદાકીય સેવાઓનું ડિજિટલાઈઝેશન કરવામાં આવ્યું છે. ઈ-કોર્ટ અને વિડિયો કોન્ફરન્સિંગ દ્વારા કેસોનો ઝડપી નિકાલ થઈ રહ્યો છે. નાગરિકોને સરકારી કચેરીઓના ધક્કા ખાધા વિના ઘેરબેઠાં પ્રમાણપત્રો આપવા માટે ડિજિટલ ગુજરાત પોર્ટલ કાર્યરત છે."
        };
        #endregion

        #region Bengali Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) BengaliThemes =
        (
            "WBPSC / Calcutta High Court",
            "State Govt Exams",
            new[]
            {
                "পশ্চিমবঙ্গের সাহিত্য ও সাংস্কৃতিক ঐতিহ্য",
                "রবীন্দ্রনাথ ঠাকুর ও বিশ্বভারতীর শিক্ষাদর্শ",
                "সুন্দরবনের জীববৈচিত্র্য ও পরিবেশ সংকট",
                "কৃষি সংস্কার ও ক্ষুদ্র চাষীদের অর্থনৈতিক নিরাপত্তা",
                "শিল্প পুনরুজ্জীবন ও তথ্যপ্রযুক্তির বিকাশ",
                "পঞ্চায়েত ব্যবস্থা ও গ্রামীণ স্বশাসন",
                "কন্যাশ্রী ও সামাজিক সুরক্ষা প্রকল্প",
                "স্বাস্থ্য পরিকাঠামো ও স্বাস্থ্যসাথী ব্যবস্থা",
                "কলকাতা মেট্রো ও নগর পরিবহন ব্যবস্থার রূপান্তর",
                "আইনশৃঙ্খলা ও বিচারবিভাগীয় সংস্কার"
            }
        );

        private static readonly string[] BengaliParagraphs = new[]
        {
            "পশ্চিমবঙ্গ ভারতের সাহিত্য, সংস্কৃতি ও মুক্তচিন্তার অন্যতম প্রধান পীঠস্থান। ঊনবিংশ শতকের বাংলার নবজাগরণ সমগ্র দেশের সমাজ সংস্কার ও জাতীয়তাবাদী ভাবধারাকে গভীরভাবে প্রভাবিত করেছিল। রামমোহন রায়, ঈশ্বরচন্দ্র বিদ্যাসাগর ও স্বামী বিবেকানন্দের মতো মনীষীদের চিন্তাধারা আজও আমাদের সঠিক পথের দিশা দেখায়।",
            "রবীন্দ্রনাথ ঠাকুরের শিক্ষাদর্শ ছিল প্রকৃতির মুক্ত অঙ্গনে সার্বিক মানবকল্যাণ সাধন। শান্তিনিকেতনে বিশ্বভারতী প্রতিষ্ঠার মাধ্যমে তিনি প্রাচ্য ও পাশ্চাত্যের সংস্কৃতির এক অনুপম মিলনক্ষেত্র গড়ে তুলেছিলেন। তাঁর রচিত গান ও সাহিত্য বাঙালি জীবনের প্রতিটি অনুভূতি ও সংকটের চিরন্তন আশ্রয়স্থল।",
            "সুন্দরবন পৃথিবীর বৃহত্তম ম্যানগ্রোভ অরণ্য এবং রয়েল বেঙ্গল টাইগারের বিশ্বখ্যাত বাসস্থান। উপকূলবর্তী অঞ্চলের প্রাকৃতিক ভারসাম্য রক্ষা ও ঘূর্ণিঝড় প্রতিহত করতে এই বনাঞ্চলের ভূমিকা অপরিসীম। তবে জলবায়ু পরিবর্তন ও সমুদ্রপৃষ্ঠের উচ্চতা বৃদ্ধির ফলে সৃষ্ট প্রাকৃতিক দুর্যোগ সুন্দরবনের বাস্তুতন্ত্রকে বিপন্ন করে তুলছে।",
            "পশ্চিমবঙ্গের কৃষি ব্যবস্থা মূলত ক্ষুদ্র ও প্রান্তিক চাষীদের ওপর নির্ভরশীল। ধান, পাট, চা এবং শাকসবজি উৎপাদને রাজ্য উল্লেখযোগ্য স্থান অধিকার করে রয়েছে। কৃষকবন্ধু প্রকল্পের মাধ্যমে কৃষকদের আর্থিক সহায়তা ও শস্যবীমা প্রদান করায় গ্রামীણ অর্থনীতিতে এক নতুন গতি সঞ্চারিত হয়েছে।",
            "তথ্যপ্রযুক্তি ও পরিষেবা ক্ষেত্রে কলকাতার নিউ টাউন ও সল্টলেক দেশের অন্যতম প্রধান কেন্দ্র হিসেবে আত্মপ্রকাশ করেছে। দেশি-বিদেশি বৃহৎ সফটওয়্যার কোম্পানিগুলি এখানে তাদের শাখা স্থাপন করায় রাজ্যের লক্ষ লক্ষ শিক্ষিত যুবক-যুবতীর কর্মসংস্থানের সুযোগ সৃষ্টি হয়েছে।",
            "কন্যাশ্রী ও রূপশ্রী প্রকল্পের মতো সামাজিক কল্যাণমূলক উদ্যোগ রাষ্ট্রসংঘের মঞ্চেও পুরস্কৃত হয়েছে। মেয়েদের বাল্যবিবাহ রোধ করে তাদের উচ্চশিক্ষায় উৎসাহিত করার ক্ষেত্রে এই প্রকল্পগুলি এক সামাজিক বিপ্লব এনে দিয়েছে। মহিলাদের স্বনির্ভর করতে স্বনির্ভর গোষ্ঠীগুলিকে সহজ শর্তে ঋণ প্রদান করা হচ্ছে।",
            "স্বাস্থ্য পরিকাঠামোর মানোন্নয়নে জেলা ও মহকুমা স্তরের হাসপাতালগুলিতে সুপার স্পেশালিটি পরিষেবা চালু করা হয়েছে। স্বাস্থ্যসাথী কার্ডের মাধ্যমে প্রতিটি পরিবারের জন্য বিনামূল্যে চিকিৎসার সুযোগ নিশ্চিত করা হয়েছে। ন্যায্যমূল্যের ওষুধের দোকান থেকে সাধারণ মানুষ সুলভে জীবনদায়ী ওষুধ পাচ্ছেন।",
            "পরিবহন পরিকাঠামোর আধুনিকায়নে কলকাতা মেট্রোর সম্প্রসারণ এক ঐতিহাসিক পদক্ষেপ। গঙ্গার তলদেশ দিয়ে মেট্রো রেল চলাচল ভারতীয় প্রকৌশলবিদ্যার এক অভূতপূর্ব নিদর্শন। এর ফলে কলকাতা ও হাওড়ার মধ্যে যাতায়াতের সময় অনেকটাই হ্রাস পেয়েছে এবং যানজট থেকে মুক্তি মিলেছে।",
            "পঞ্চায়েত ব্যবস্থার সফল প্রয়োগে গ্রামীণ বাংলায় পরিকাঠামো উন্নয়নের কর্মযজ্ঞ চলছে। পাকা রাস্তা, পানীয় জলের নল ও ১০০ দিনের কাজের মাধ্যমে গ্রামীણ পরিবারগুলির আয়ের পথ সুগম হয়েছে। পঞ্চায়েতের কার্যকলাপে সাধারণ মানুষের সরাসরি মতামত গ্রহণ করায় কাজের স্বচ্ছতা বৃদ্ধি পেয়েছে।",
            "আইনি পরিষেবা ও বিচারব্যবস্থার আধুনিকীকরণে ই-কোর্ট ব্যবস্থার প্রয়োগ ইতিবাচক ফল দিচ্ছে। নাগরিকদের মৌলিক অধিকার সুরক্ষা ও বাণিজ্যিক বিরোধ নিষ্পত্তিতে কলকাতা হাইকোর্ট দেশের প্রাচীনতম ও অন্যতম প্রধান বিচারালয় হিসেবে তার গৌরবময় ঐতিহ্য অক্ষুণ্ণ রেখেছে।"
        };
        #endregion

        #region Tamil Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) TamilThemes =
        (
            "TNPSC / Madras High Court",
            "State Govt Exams",
            new[]
            {
                "தமிழ் மொழியின் தொன்மையும் செம்மொழி சிறப்பும்",
                "திருக்குறள் காட்டும் வாழ்வியல் நெறிமுறைகள்",
                "தமிழகத்தின் தொழில் வளர்ச்சி மற்றும் உற்பத்தித் துறை",
                "காவிரி டெல்டா மற்றும் இயற்கை விவசாய முன்னேற்றம்",
                "கல்வி வளர்ச்சி மற்றும் நான் முதல்வன் திட்டம்",
                "பொது சுகாதாரக் கட்டமைப்பு மற்றும் முதலமைச்சர் காப்பீடு",
                "துறைமுகங்கள் மற்றும் கடல்வழி வணிக வளர்ச்சி",
                "சமூக நீதி மற்றும் சுயமரியாதை இயக்க வரலாறு",
                "உள்ளாட்சி அமைப்புகள் மற்றும் பெண்கள் பிரதிநிதித்துவம்",
                "சுற்றுச்சூழல் பாதுகாப்பு மற்றும் பசுமைத் தமிழ்நாடு இயக்கம்"
            }
        );

        private static readonly string[] TamilParagraphs = new[]
        {
            "தமிழ்மொழி உலகிலேயே மிகப்பழமையான செம்மொழிகளில் ஒன்றாக விளங்குகின்றது. இரண்டாயிரத்திற்கும் மேற்பட்ட ஆண்டுகள் பழமையான சங்க இலக்கியங்கள், தமிழர்களின் உயர்ந்த நாகரிகம், பண்பாடு, வீரம் மற்றும் அறநெறிகளைப் பறைசாற்றுகின்றன. தொல்காப்பியம் காட்டும் இலக்கணச் செழுமையும், சிலப்பதிகாரம் உணர்த்தும் நீதியும் தமிழ் மொழியின் இலக்கியச் சிகரங்களாகும்.",
            "திருவள்ளுவர் அருளிய திருக்குறள் உலக மக்கள் அனைவருக்கும் பொதுவான வாழ்வியல் வழிகாட்டியாகும். மதம், இனம், மொழி கடந்து மனித நேயம், வாய்மை, கல்வி, ஒழுக்கம் மற்றும் நடுவுநிலைமை ஆகிய நற்பண்புகளை அது வலியுறுத்துகிறது. உலகப் பொதுமறையாம் திருக்குறள் போற்றும் அறநெறிகள் தற்கால நிர்வாகத்திற்கும் சிறந்த வழிகாட்டியாகத் திகழ்கின்றன.",
            "தமிழகம் இந்தியாவின் முன்னணி தொழில் மாநிலங்களில் ஒன்றாகத் திகழ்கிறது. வாகன உற்பத்தி, மின்னணுப் பொருட்கள், ஜவுளி மற்றும் தோல் பொருட்கள் தயாரிப்பில் சர்வதேச முதலீட்டாளர்களை ஈர்த்து வருகிறது. கோயம்புத்தூர், சென்னை, ஒசூர் மற்றும் திருச்சிராப்பள்ளி போன்ற தொழில் நகரங்கள் மாநிலத்தின் பொருளாதார வளர்ச்சிக்கு முதுகெலும்பாக உள்ளன.",
            "காவிரி டெல்டா பகுதி தமிழகத்தின் நெற்களஞ்சியமாகப் போற்றப்படுகிறது. நவீன வேளாண் உத்திகள், சொட்டுநீர்ப் பாசனம் மற்றும் இயற்கை உரப் பயன்பாட்டின் மூலம் நெல் உற்பத்தி தொடர்ந்து அதிகரிக்கப்பட்டு வருகிறது. விவசாயிகளுக்கு இலவச மின்சாரம் மற்றும் பயிர்க் காப்பீட்டுத் திட்டங்கள் வழங்கி அவர்களின் வாழ்வாதாரம் பாதுகாக்கப்பட்டு வருகிறது.",
            "கல்வித் துறையில் மாணவர்களின் சேர்க்கை விகிதத்தில் தமிழகம் தேசிய அளவில் முதலிடத்தில் உள்ளது. அரசுப் பள்ளிகளில் ஸ்மார்ட் வகுப்பறைகள், மடிக்கணினிகள் மற்றும் காலை உணவுத் திட்டம் ஆகியவற்றின் மூலம் கல்வித் தரம் மேம்படுத்தப்பட்டுள்ளது. நான் முதல்வன் திட்டத்தின் மூலம் கல்லூரி மாணவர்களுக்குத் தொழில் திறன் பயிற்சிகள் வழங்கப்படுகின்றன.",
            "மக்களின் நல்வாழ்வை உறுதி செய்வதில் தமிழகத்தின் பொது சுகாதாரக் கட்டமைப்பு நாட்டிற்கே முன்மாதிரியாக விளங்குகிறது. ஆரம்ப சுகாதார நிலையங்கள் முதல் அரசு மருத்துவக் கல்லூரி மருத்துவமனைகள் வரை ஏழை எளிய மக்களுக்கு உயர்தர மருத்துவ சிகிச்சைகள் இலவசமாக வழங்கப்படுகின்றன. மக்களைத் தேடி மருத்துவம் திட்டம் மூலம் வீடுகளிலேயே பரிசோதனைகள் செய்யப்படுகின்றன.",
            "சென்னை, தூத்துக்குடி மற்றும் எண்ணூர் ஆகிய மூன்று பெரிய துறைமுகங்கள் மூலம் தமிழகத்தின் கடல்வழி வணிகம் தடையின்றி நடைபெறுகிறது. சாலை மற்றும் ரயில்வே இணைப்புகள் இத்துறைமுகங்களுடன் திறம்பட இணைக்கப்பட்டுள்ளதால் சரக்குப் போக்குவரத்து விரைவாக நடைபெற்று அந்நியச் செலாவணி வருவாய் பெருகுகிறது.",
            "தந்தை பெரியார் மற்றும் அண்ணாவின் சுயமரியாதை இயக்கம் சமூகத்தில் ஒடுக்கப்பட்ட மக்களின் கல்வி மற்றும் வேலைவாய்ப்பு உரிமைகளை மீட்டெடுத்தது. சமூக நீதித் தத்துவத்தின் அடிப்படையில் உருவாக்கப்பட்ட இடஒதுக்கீட்டுக் கொள்கை அனைத்துத் தரப்பு மக்களுக்கும் அதிகாரப் பகிர்வை உறுதி செய்துள்ளது.",
            "உள்ளாட்சித் தேர்தல்களில் பெண்களுக்கு ஐம்பது சதவீத இடஒதுக்கீடு வழங்கப்பட்டதன் மூலம் கிராம மற்றும் நகர நிர்வாகங்களில் பெண்களின் ஆளுமைத் திறன் வெளிப்பட்டுள்ளது. மகளிர் சுயஉதவிக் குழுக்கள் மூலம் கோடிக்கணக்கான பெண்கள் பொருளாதார ரீதியாகத் தற்சார்பு அடைந்து குடும்ப முன்னேற்றத்திற்கு வழிவகுத்துள்ளனர்.",
            "பசுமைத் தமிழ்நாடு இயக்கத்தின் மூலம் காடுகளின் பரப்பளவை அதிகரிக்க தீவிர மரக்கன்றுகள் நடும் பணிகள் நடைபெற்று வருகின்றன. பிளாஸ்டிக் பயன்பாட்டிற்குத் தடை விதிக்கப்பட்டு மீண்டும் மஞ்சப்பை இயக்கம் மக்களிடையே விழிப்புணர்வை ஏற்படுத்தியுள்ளது. சூரியசக்தி மற்றும் காற்றாலை மூலம் புதுப்பிக்கத்தக்க எரிசக்தி உற்பத்தியில் தமிழகம் முன்னோடியாக உள்ளது."
        };
        #endregion

        #region Telugu Themes & Paragraphs
        private static readonly (string Exam, string Category, string[] Topics) TeluguThemes =
        (
            "APPSC / TSPSC Junior Assistant",
            "State Govt Exams",
            new[]
            {
                "తెలుగు భాషా ప్రాచీనత మరియు సాహిత్య వైభవం",
                "ఆంధ్రప్రదేశ్ - తెలంగాణ పారిశ్రామికాభివృద్ధి",
                "గోదావరి - కృష్ణా నదుల అనుసంధానం మరియు ప్రాజెక్టులు",
                "సమాచార సాంకేతిక రంగం మరియు సైబరాబాద్ విజయం",
                "రైతు సంక్షేమ పథకాలు మరియు వ్యవసాయ ప్రగతి",
                "ప్రజా పంపిణీ వ్యవస్థ మరియు సామాజిక భద్రత",
                "విద్యా సంస్కరణలు మరియు డిజిటల్ తరగతి గదులు",
                "వైద్య ఆరోగ్య సేవలు మరియు ఆరోగ్యశ్రీ పథకం",
                "పంచాయతీరాజ్ వ్యవస్థ మరియు స్థానిక స్వపరిపాలన",
                "పర్యావరణ పరిరక్షణ మరియు హరితహారం కార్యక్రమం"
            }
        );

        private static readonly string[] TeluguParagraphs = new[]
        {
            "తెలుగు భాష భారత దేశంలోని అత్యంత ప్రాచీనమైన, మాధుర్యమున్న భాషలలో ఒకటి. ఇటాలియన్ ఆఫ్ ది ఈస్ట్ గా ప్రసిద్ధి చెందిన తెలుగు భాషలో నన్నయ, తిక్కన, ఎర్రనల కవిత్రయం నుండి శ్రీకృష్ణదేవరాయల స్వర్ణయుగం వరకు అద్భుత సాహిత్య సృష్టి జరిగింది. పదకవితా పితామహుడు అన్నమయ్య, త్యాగరాజుల కీర్తనలు తెలుగు సంస్కృతికి శాశ్వత కీర్తిని చేకూర్చాయి.",
            "తెలుగు రాష్ట్రాల ఆర్థిక ప్రగతిలో సమాచార సాంకేతిక రంగం కీలక పాత్ర పోషిస్తోంది. హైదరాబాద్ లోని హైటెక్ సిటీ ప్రపంచ స్థాయి సాఫ్ట్‌వేర్ సంస్థలకు నిలయంగా మారి లక్షలాది మంది యువతకు ఉపాధి అవకాశాలు కల్పిస్తోంది. ఫార్మా రంగంలో మరియు వ్యాక్సిన్ తయారీలో ప్రపంచంలోనే అగ్రగామి కేంద్రంగా నిలవడం విశేషం.",
            "రైతాంగ సంక్షేమం కోసం ప్రభుత్వం అమలు చేస్తున్న రైతుబంధు, రైతుభరోసా వంటి పథకాలు వ్యవసాయ రంగానికి కొత్త ఊపునిచ్చాయి. నాణ్యమైన విత్తనాలు, ఎరువులు మరియు నిరంతర ఉచిత విద్యుత్ సరఫరా వల్ల ధాన్యం ఉత్పత్తిలో రికార్డు స్థాయి దిగుబడులు సాధించడం సాధ్యమైంది. రైతులకు పంట నష్ట పరిహారం సకాలంలో అందిస్తున్నారు.",
            "గోదావరి మరియు కృష్ణా నదులపై నిర్మించిన భారీ నీటిపారుదల ప్రాజెక్టులు బీడు భూములను సస్యశ్యామలం చేశాయి. కాళేశ్వరం, పోలవరం ప్రాజెక్టుల ద్వారా కోట్లాది ఎకరాలకు సాగునీరు మరియు తాగునీరు అందుతోంది. మిషన్ కాకతీయ ద్వారా పునరుద్ధరించబడిన చెరువులు భూగర్భ జలాలను గణనీయంగా పెంచాయి.",
            "పాఠశాల విద్యలో విప్లవాత్మక మార్పులు తెస్తూ ప్రభుత్వ బడులలో మౌలిక వసతులు అభివృద్ధి చేయబడ్డాయి. డిజిటల్ తరగతి గదులు, ఇంగ్లీష్ మీడియం బోధన మరియు పౌష్టికాహార మధ్యాహ్న భోజనం వల్ల విద్యార్థుల హాజరు శాతం పెరిగింది. ఉన్నత విద్యలో పేద విద్యార్థులకు పూర్తి ఫీజు రీయింబర్స్‌మెంట్ సౌకర్యం కల్పిస్తున్నారు.",
            "ప్రజలందరికీ నాణ్యమైన వైద్య సేవలు అందించడంలో ఆరోగ్యశ్రీ పథకం విశేష సేవలందిస్తోంది. కార్పొరేట్ ఆసుపత్రులలో ఉచితంగా గుండె, క్యాన్సర్ మరియు మెదడు సంబంధిత శస్త్రచికిత్సలు చేయించుకునే అవకాశం పేదలకు లభించింది. బస్తీ దవాఖానాలు మరియు ప్రాథమిక ఆరోగ్య కేంద్రాల ద్వారా నిరంతర వైద్య పరీక్షలు జరుగుతున్నాయి.",
            "స్థానిక స్వపరిపాలనను బలోపేతం చేయడంలో పంచాయతీల పాత్ర అమోఘమైనది. పల్లె ప్రగతి కార్యక్రమాల ద్వారా గ్రామాలలో వైకుంఠ ధామాలు, డంపింగ్ యార్డులు మరియు పల్లె ప్రకృతి వనాలు వెలిశాయి. గ్రామ పంచాయతీలకు నిధులు సమకూర్చడం వల్ల పారిశుధ్యం మెరుగుపడి గ్రామాలు ఆదర్శవంతంగా మారాయి.",
            "విశాఖపట్నం పోర్టు మరియు కృష్ణపట్నం పోర్టుల ద్వారా విదేశీ వాణిజ్యం భారీగా విస్తరించింది. తీరప్రాంత పారిశ్రామిక కారిడార్లు కొత్త తయారీ పరిశ్రమలను ఆకర్షిస్తున్నాయి. రవాణా సౌకర్యాల మెరుగుదల కోసం నాలుగు వరుసల జాతీయ రహదారులు మరియు ఎక్స్‌ప్రెస్‌వేలు వేగంగా నిర్మించబడుతున్నాయి.",
            "పర్యావరణ సమతుల్యత కాపాడటానికి హరితహారం మరియు సామాజిక అటవీకరణ కార్యక్రమాల ద్వారా కోట్లాది మొక్కలు నాటబడ్డాయి. సౌర విద్యుత్ మరియు పవన విద్యుత్ ప్రాజెక్టులను ప్రోత్సహించడం ద్వారా పర్యావరణ హిత ఇంధన ఉత్పత్తిలో తెలుగు రాష్ట్రాలు ముందంజలో ఉన్నాయి.",
            "పరిపాలనా సంస్కరణలలో భాగంగా సచివాలయ వ్యవస్థ మరియు మీసేవ కేంద్రాల ద్వారా ప్రభుత్వ సేవలు పారదర్శకంగా ప్రజలకు అందుతున్నాయి. కుల, ఆదాయ, జనన ధ్రువీకరణ పత్రాలు ఎలాంటి అవినీతి లేకుండా త్వరితగతిన పంపిణీ చేయబడుతున్నాయి, ఇది ప్రజల విశ్వాసాన్ని మరింత పెంచింది."
        };
        #endregion
    }
}
