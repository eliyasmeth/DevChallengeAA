using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Nancy;

namespace AvengersvsEvilSanta
{
    public class WebModule : NancyModule
    {
        public WebModule()
        {
            Get["/IronMan/{words}"] = parameters =>
            {
                string words = parameters.words;

                string phrase = IronMan(words);

                return phrase;
            };
        }

        private string IronMan(string words)
        {
            #region STEP1:SORT
            List<string> wArray = words.Split('|').ToList();
            #endregion

            #region STEP2: SWITCHING

            const string vowels = "aeiouy";
            const string vowelsCap = "AEIOUY";

            for (int i = 0; i < wArray.Count; i++)
            {
                StringBuilder word = new StringBuilder(wArray[i]);
                

                for (int j = 0; j < word.Length; j++)
                {
                    if (!vowels.Contains(word[j]) && !vowelsCap.Contains(word[j])) continue;
                    if (j == (word.Length - 1))
                    {
                        var temp = word[j];
                        word[j] = word[0];
                        word[0] = temp;
                    }
                    else
                    {
                        if ((vowels.Contains(word[j + 1]) || vowelsCap.Contains(word[j + 1])))
                        {
                            if ((vowels.Contains(word[j]) && vowelsCap.Contains(word[j + 1]) ||
                                 (vowelsCap.Contains(word[j]) && vowels.Contains(word[j + 1]))))
                            {
                                var temp = word[j + 1];
                                word[j + 1] = word[j];
                                word[j] = temp;
                            }
                        }
                        else
                        {
                            var temp = word[j + 1];
                            word[j + 1] = word[j];
                            word[j] = temp;
                        }

                    }
                }
                wArray[i] = word.ToString();
            }

            #endregion

            #region STEP3: CONCATENATE

            string wordsCon = "";

            for (int i = 0; i < wArray.Count; i++)
            {
                if (i == 0)
                    wordsCon += wArray[i] + ((int) wArray[wArray.Count - 1][0]);
                else
                {
                    wordsCon += wArray[i] + ((int)wArray[i-1][0]);
                }
            }

            #endregion

            #region STEP4: CONVERT BASE64

            var bytes = Encoding.UTF8.GetBytes(wordsCon);
            var base64 = Convert.ToBase64String(bytes);

            #endregion

            return base64;
        }
    }
}