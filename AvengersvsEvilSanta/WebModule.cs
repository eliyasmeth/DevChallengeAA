using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using Nancy;
using Nancy.TinyIoc;
using Newtonsoft.Json;


namespace AvengersvsEvilSanta
{
    public class WebModule : NancyModule
    {
        private const string Uri = "http://internal-devchallenge-2-dev.apphb.com";
        private static string guid = "";
        const string vowels = "aeiouy";
        const string vowelsCap = "AEIOUY";
        private int FibN = 0;
        private static readonly List<string> englishWords = new List<string>()
        {"drool","cats","clean","code","dogs","materials","needed","this","is","hard",
            "what","are","you","smoking","shot","gun","down","river","super","man","rule",
            "acklen","developers","are","amazing"};

        internal class ResponseWords
        {
            public string [] words { get; set; }
            public double startingFibonacciNumber { get; set; }
            public string algorithm { get; set; }
        }
        public static string HttpGet()
        {
            guid = Guid.NewGuid().ToString();
            string getUri = Uri + "/values/" + guid;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUri);
            request.Method = "GET";
            request.Accept = "application/json";
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }

        }

        public static string HttpPost(string phrase, string algorithm)
        {
            string getUri = Uri + "/values/" + guid + "/" + algorithm;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUri);
            request.Method = "POST";
            request.Accept = "application/json";
            string repo = "https://github.com/eliyasmeth/DevChallengeAA";
            string webhook = "https://7a79ba9f.ngrok.io/JARVIS";

            Encoding encoding = new UTF8Encoding();
            string postData = "{\"encodedValue\":\""+ phrase +"\"," +
                              "\"emailAddress\":\"eliseo@acklenavenue.com\"," +
                              "\"name\":\"Eliseo García\"," +
                              "\"webhookUrl\":\"" + webhook + "\"," +
                              "\"repoUrl\":\"" + repo + "\"}";

            byte[] data = encoding.GetBytes(postData);

            request.ContentType = "application/json";//charset=UTF-8";
            request.ContentLength = data.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
        public WebModule()
        {
            Get["/"] = parameters =>
            {
                for (int i = 0; i < 20; i++)
                {
                    string json = HttpGet();
                    var m = JsonConvert.DeserializeObject<ResponseWords>(json);
                    FibN = GetFibN(m.startingFibonacciNumber);
                    string phrase = "";
                    if (m.algorithm.ToUpper() == "IRONMAN")
                    {
                        phrase = IronMan(m.words);
                    }
                    else if (m.algorithm.ToUpper() == "THEINCREDIBLEHULK")
                    {
                        phrase = Hulk(m.words);
                    }
                    else if(m.algorithm.ToUpper() == "THOR")
                    {
                        phrase = Thor(m.words,m.startingFibonacciNumber);
                    }
                    else
                    {
                        phrase = CaptainAmerica(m.words, m.startingFibonacciNumber);
                    }
                    
                    
                    json = HttpPost(phrase, m.algorithm);
                }

                return "HOLA";
            };

            Post["/JARVIS"] = parameters => this.Request.Body.ToString();
        }

        private int GetFibN(double n)
        {
            int FibN = 0;
            bool fibNfound = false;
            while (!fibNfound)
            {
                if (n == Fib(FibN)) fibNfound = true;
                FibN++;
            }
            return FibN;
        }

        private int fib(int n)
        {
            int fib2 = Enumerable.Range(1, n)
                     .AsParallel()
                     .Skip(2)
                     .Aggregate(new { Current = 1, Prev = 1 },
                                (x, index) => new { Current = x.Prev + x.Current, Prev = x.Current })
                     .Current;
            return fib2;
        }

        private long Fib(int n)
        {
            if (n < 2)
                return n;
            long[] f = new long[n + 1];
            f[0] = 0;
            f[1] = 1;

            for (int i = 2; i <= n; i++)
            {
                f[i] = f[i - 1] + f[i - 2];
            }
            return f[n];
        }

        private string CaptainAmerica(string[] words, double startingFibonacciNumber)
        {
            List<string> wArray = words.ToList();

            #region STEP1: SWITCHING

            wArray = Switching(wArray);

            #endregion

            #region STEP2: SORT REVERSE

            wArray.Sort();
            wArray.Reverse();

            #endregion

            #region STEP3: REPLACE VOWELS FOR FIBONACCI

            wArray = ReplaceVowelByFib(wArray, startingFibonacciNumber);

            #endregion

            #region STEP4: CONCATENATE

            var wordsCon = Concatenate(wArray);

            #endregion

            #region STEP4: CONVERT BASE64

            var base64 = ConvertBase64(wordsCon);

            #endregion

            return base64;
        }

        private string Hulk(string[] words)
        {
            List<string> wArray = words.ToList();

            #region STEP1: SWITCHING

            Switching(wArray);

            #endregion

            #region STEP2: SORT REVERSE

            wArray.Sort();
            wArray.Reverse();

            #endregion

            #region STEP3: CONCATENATE

            var wordsCon = Concatenate(wArray, true);

            #endregion

            #region STEP4: CONVERT BASE64

            var base64 = ConvertBase64(wordsCon);

            #endregion

            return base64;
        }

        private string Thor(string[] words, double startingFibonacciNumber)
        {
            List<string> wArray = words.ToList();

            #region STEP1: SPLIT

            wArray = Split(wArray);

            #endregion

            #region STEP2: SORT
            
            wArray.Sort();

            #endregion

            #region STEP3: ALTERNATE CONSONANTS

            wArray = Alternate(wArray);

            #endregion

            #region STEP4: REPLACE VOWELS FOR FIBONACCI

            wArray = ReplaceVowelByFib(wArray, startingFibonacciNumber);

            #endregion

            #region STEP5: CONCATENATE

            var wordsCon = Concatenate(wArray, true);

            #endregion

            #region STEP4: CONVERT BASE64

            var base64 = ConvertBase64(wordsCon);

            #endregion

            return base64;
        }

       

        private string IronMan(string [] words)
        {
            #region STEP1:SORT
            List<string> wArray = words.ToList();
            wArray.Sort();
            #endregion

            #region STEP2: SWITCHING

            wArray = Switching(wArray);

            #endregion

            #region STEP3: CONCATENATE

            var wordsCon = Concatenate(wArray);

            #endregion

            #region STEP4: CONVERT BASE64

            var base64 = ConvertBase64(wordsCon);

            #endregion

            return base64;
        }

        private static string ConvertBase64(string wordsCon)
        {
            var bytes = Encoding.UTF8.GetBytes(wordsCon);
            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        private static string Concatenate(List<string> wArray, bool asterisk = false)
        {
            string wordsCon = "";

            if(asterisk)
                for (int i = 0; i < wArray.Count; i++)
                {
                    wordsCon += (i < wArray.Count-1) ? wArray[i] + "*" : wArray[i];
                }
            else
            {
                for (int i = 0; i < wArray.Count; i++)
                {
                    if (i == 0)
                        wordsCon += wArray[i] + ((int)wArray[wArray.Count - 1][0]);
                    else
                    {
                        wordsCon += wArray[i] + ((int)wArray[i - 1][0]);
                    }
                }
            }
            
            return wordsCon;
        }

        private static List<string> Switching(List<string> wArray)
        {
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

            return wArray;
        }
        private List<string> Alternate(List<string> wArray)
        {
            bool isUpper = char.IsUpper(wArray[0][0]);

            for (int i = 0; i < wArray.Count; i++)
            {
                string tempW = wArray[i];
                string newW = "";
                for (int j = 0; j < tempW.Length; j++)
                {
                    newW += isUpper ? char.ToUpper(tempW[j]) : char.ToLower(tempW[j]);
                    isUpper = !isUpper;
                }

                wArray[i] = newW;
            }

            return wArray;
        }

        private List<string> Split(List<string> wArray)
        {
            List<string> newArray = new List<string>();

            for (int i = 0; i < wArray.Count; i++)
            {
                for (int j = 0; j < englishWords.Count; j++)
                {
                    if (englishWords[j].Contains(wArray[i]))
                    {
                        if (englishWords[j].Length == wArray[i].Length)
                            newArray.Add(wArray[i]);
                        else
                        {
                            var indexOf = wArray[i].IndexOf(englishWords[j]);
                            newArray.Add(wArray[i].Substring(indexOf, wArray[i].Length));
                            newArray.Add(indexOf == 0
                                ? wArray[i].Substring(indexOf + wArray[i].Length)
                                : wArray[i].Substring(0, wArray[i].Length - indexOf));
                        }
                    }
                }
            }

            return newArray;
        }
        private List<string> ReplaceVowelByFib(List<string> wArray, double startingFibonacciNumber)
        {
            string tempW = "";
            string newW = "";
            int tempFibN = FibN;
            bool startingFibUsed = false;
            for (int i = 0; i < wArray.Count; i++)
            {
                tempW = wArray[i];
                for (int j = 0; j < tempW.Length; j++)
                {
                    if (vowels.Contains(tempW[j]) || vowelsCap.Contains(tempW[j]))
                    {
                        if (!startingFibUsed)
                        {
                            newW += startingFibonacciNumber;
                            startingFibUsed = true;
                        }
                        else { newW += Fib(tempFibN); }
                        tempFibN++;
                    }
                }
                wArray[i] = newW;
                newW = "";
            }

            return wArray;
        }

        


    }
}