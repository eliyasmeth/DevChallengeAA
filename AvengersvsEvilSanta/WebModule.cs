using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Nancy;
using Newtonsoft.Json;


namespace AvengersvsEvilSanta
{
    public class WebModule : NancyModule
    {
        private const string Uri = "http://internal-devchallenge-2-dev.apphb.com";
        private static string guid = "";
        internal class ResponseWords
        {
            public string [] words { get; set; }
            public int startingFibonacciNumber { get; set; }
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
            request.Method = "GET";
            request.Accept = "application/json";
            string repo = "https://github.com/eliyasmeth/DevChallengeAA";
            string webhook = "";

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
                    string phrase = "";
                    if (m.algorithm.ToUpper() == "IRONMAN")
                    {
                        phrase = IronMan(m.words);
                    }
                    else if (m.algorithm.ToUpper() == "HULK")
                    {
                        phrase = Hulk(m.words);
                    }
                    
                    
                    //json = HttpPost(phrase, m.algorithm);


                }

                return "HOLA";
            };

            //Get["/IronMan/{words}"] = parameters =>
            //{
            //    string words = parameters.words;

            //    string phrase = IronMan(words);

            //    return phrase;
            //};
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

            return wArray;
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
    }
}