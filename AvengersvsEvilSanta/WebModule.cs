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
using Nancy.ModelBinding;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using RestSharp;


namespace AvengersvsEvilSanta
{
    public class WebModule : NancyModule
    {
        private const string Uri = "http://internal-devchallenge-2-dev.apphb.com";
        private static string guid = "";
        private  static List<string> resultList = new List<string>();
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

        internal class ResponsePost
        {
            public string status { get; set; }
            public string message { get; set; }
        }

        internal class ResponseSecret
        {
            public string secret { get; set; }
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
            string repo = "https://github.com/eliyasmeth/DevChallengeAA";
            string webhook = "http://cd7a09e5.ngrok.io";

            #region WebRequest

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUri);
            //request.Method = "POST";
            //request.Accept = "application/json";

            //Encoding encoding = new UTF8Encoding();
            //string postData = "{\"encodedValue\":\"" + phrase + "\"," +
            //                  "\"emailAddress\":\"eliseo@acklenavenue.com\"," +
            //                  "\"name\":\"Eliseo García\"," +
            //                  "\"webhookUrl\":\"" + webhook + "\"," +
            //                  "\"repoUrl\":\"" + repo + "\"}";

            //byte[] data = encoding.GetBytes(postData);

            //request.ContentType = "application/json";//charset=UTF-8";
            //request.ContentLength = data.Length;

            //Stream stream = request.GetRequestStream();
            //stream.Write(data, 0, data.Length);
            //stream.Close();

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //using (var sr = new StreamReader(response.GetResponseStream()))
            //{
            //    return sr.ReadToEnd();
            //}

            #endregion
            

            #region RestSharp

            var client = new RestClient(Uri);
            var req = new RestRequest("/values/" + guid + "/" + algorithm, Method.POST);
            //req.AddHeader("Accept", "application/json");
            //req.RequestFormat = DataFormat.Json;
            req.AddParameter("encodedValue", phrase);
            req.AddParameter("emailAddress", "eliseo@acklenavenue.com");
            req.AddParameter("name", "Eliseo García");
            req.AddParameter("webhookUrl", webhook);
            req.AddParameter("repoUrl", repo);

            RestResponse res = (RestResponse) client.Execute(req);
            var content = res.Content;

            return content;

            #endregion
        }
        public WebModule()
        {
            Get["/"] = parameters =>
            {
                #region Testing Methods

                //List<string> l = new List<string>() { "hEllo", "bOok", "read", "NeEd", "paliNdromE", "happy" };

                //List<string> l2 = new List<string>() { "dog", "cat", "bird" };

                //List<string> l3 = new List<string>() { "supermanisamazing", "hEllo", "cleancode" };

                //List<string> l4 = new List<string>() { "DoG", "CaT", "BiRd" };

                //Switching(l);
                //Concatenate(l2);

                //Concatenate(l2,true);
                //ReplaceVowelByFib(l2, 5);
                //Alternate(l4);
                //Split(l3);

                #endregion

                #region Avengers
                //I commented the code just to not running every time the app start.
 
                //for (int i = 0; i < 20; i++)
                //{
                //    string json = HttpGet();
                //    var m = JsonConvert.DeserializeObject<ResponseWords>(json);
                //    FibN = GetFibN(m.startingFibonacciNumber);
                //    string phrase = "";
                //    if (m.algorithm.ToUpper() == "IRONMAN")
                //    {
                //        phrase = IronMan(m.words);
                //    }
                //    else if (m.algorithm.ToUpper() == "THEINCREDIBLEHULK")
                //    {
                //        phrase = Hulk(m.words);
                //    }
                //    else if (m.algorithm.ToUpper() == "THOR")
                //    {
                //        phrase = Thor(m.words, m.startingFibonacciNumber);
                //    }
                //    else
                //    {
                //        phrase = CaptainAmerica(m.words, m.startingFibonacciNumber);
                //    }


                //    json = HttpPost(phrase, m.algorithm);
                //    var p = JsonConvert.DeserializeObject<ResponsePost>(json);
                //    resultList.Add(m.algorithm + " : " + p.status + " / " + p.message);
                //}

                //if (resultList.Contains("Winner"))
                //    return "Winner Once!";
                //return resultList.Contains("CrashAndBurn") == true ? "CrashAndBurn" : "Success";

                #endregion

                return "Done!";
            };

            Get["/JARVIS"] = parametes => "<h1> eliseo@acklenavenue.com : 5 is the magic! </h1>";

            Post["/"] = parameters =>
            {
                var response = this.Bind<ResponseSecret>();
                File.WriteAllText(@"C:\Users\Eliseo Yasmeth\Documents\Acklen Avenue\Projects\DevChallengeAA\AvengersvsEvilSanta\secret.txt", response.secret);
                return response.secret;
            };
        }

        private int GetFibN(double n)
        {
            int FibN = 0;
            bool fibNfound = false;
            while (!fibNfound)
            {
                if (n == Fib(FibN)) { fibNfound = true; break; };
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

        /// <summary>
        /// DONE!
        /// </summary>
        /// <param name="wArray"></param>
        /// <param name="asterisk"></param>
        /// <returns></returns>
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

        /// <summary>
        /// DONE!
        /// </summary>
        /// <param name="wArray"></param>
        /// <returns></returns>
        private static List<string> Switching(List<string> wArray)
        {
            for (int i = 0; i < wArray.Count; i++)
            {
                StringBuilder word = new StringBuilder(wArray[i]);
                
                for (int j = 0; j < word.Length; j++)
                {
                    if (!vowels.Contains(wArray[i][j]) && !vowelsCap.Contains(wArray[i][j])) continue;
                    //word = rightRotateShift(word, 1);
                    //word = word.Substring(word.Length-1,1) + word.Substring(0, word.Length-j); 

                    if (j == (word.Length - 1))
                    {
                        var temp = word[j];
                        //word[j] = word[0];
                        //word[0] = temp;
                        word.Insert(0, temp);
                        word.Remove(word.Length - 1,1);
                    }
                    else
                    {
                        if ((vowels.Contains(word[j + 1]) || vowelsCap.Contains(word[j + 1])))
                        {
                            var temp = word[j + 1];
                            word[j + 1] = word[j];
                            word[j] = temp;
                            j++;
                            //}
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
                    if (!vowels.Contains(tempW[j]) && !vowelsCap.Contains(tempW[j]))
                    {
                        newW += isUpper ? char.ToUpper(tempW[j]) : char.ToLower(tempW[j]);
                        isUpper = !isUpper;
                    }
                    else
                    {
                        newW += tempW[j];
                    }
                }

                wArray[i] = newW;
            }

            return wArray;
        }

        private List<string> Split(List<string> wArray)
        {
            var newArray = new List<string>();
            bool foundW = false;
            for (int i = 0; i < wArray.Count; i++)
            {
                string w =  wArray[i];
                foundW = false;
                for (int j = 0; j < englishWords.Count; j++)
                {
                    if (w.IndexOf(englishWords[j],StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        if (englishWords[j].Length == w.Length)
                        {
                            newArray.Add(w);
                            w = w.Remove(0, w.Length);
                        }
                        else
                        {

                            var indexOf = w.IndexOf(englishWords[j], StringComparison.OrdinalIgnoreCase);
                            newArray.Add(w.Substring(indexOf, englishWords[j].Length));
                            w = w.Remove(indexOf, englishWords[j].Length);

                            //newArray.Add(indexOf == 0
                            //    ? wArray[i].Substring(indexOf + englishWords[j].Length)
                            //    : wArray[i].Substring(0, englishWords[j].Length - indexOf));
                        }
                        if (w.Any()) continue;
                        foundW = true;
                        break;
                    }
                }
                if(!foundW) newArray.Add(w);
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
                    if (!vowels.Contains(tempW[j]) && !vowelsCap.Contains(tempW[j]))
                    {
                        newW += tempW[j];
                    }
                    else
                    {
                        if (!startingFibUsed)
                        {
                            newW += startingFibonacciNumber;
                            startingFibUsed = true;
                        }
                        else
                        {
                            newW += Fib(tempFibN).ToString();
                        }
                        tempFibN += 1;
                    }
                }
                wArray[i] = newW;
                newW = "";
            }

            return wArray;
        }

    }
}