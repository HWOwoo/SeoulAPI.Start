using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Test.SeoulAPI
{
    #region 미세먼지 저장
    /// <summary>
    /// {0} : 인증키
    /// {1} : 매개인자값, 원하는 날자
    /// inputKey : 공공데이터 인증키  참고* App.Config
    /// </summary>
    public class SeoulApiJson
    {
        string inputUrl = "http://openapi.seoul.go.kr:8088/{0}/json/DailyAverageRoadside/1/100/{1}";
        /*string inputKey = "5378624a4973757239367644476152";*/
        string json = "";
        string serviceKey = ConfigurationManager.AppSettings["serviceKey"];

        public void ReadJsonData(string data)
        {
            string apiUrl = string.Format(inputUrl, serviceKey, data);

            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8; // 한글깨짐 encoding
                json = client.DownloadString(apiUrl);
            }

            // json 값을 JObject를 통해 역직렬화 
            JObject jObject = JObject.Parse(json);
            var rows = jObject["DailyAverageRoadside"]["row"];

            foreach (var row in rows)
            {
                string msrDtDe = Convert.ToString(row["MSRDT_DE"]);
                string msrClsCd = Convert.ToString(row["MSRCLS_CD"]);
                string msrSteNm = Convert.ToString(row["MSRSTE_NM"]);
                string pm10 = Convert.ToString(row["PM10"]);
                string o3 = Convert.ToString(row["O3"]);
                string no2 = Convert.ToString(row["NO2"]);
                string co = Convert.ToString(row["CO"]);
                string so2 = Convert.ToString(row["SO2"]);

                // msrdtde의 값을 substring을 통해서 년/월/일을 나누어 파일경로를 생성
                string folderPath = $"{msrDtDe.Substring(0, 4)}/{msrDtDe.Substring(4, 2)}/{msrDtDe.Substring(6, 2)}/{msrClsCd}/{msrSteNm}";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = $"{folderPath}/PM{pm10}.txt";

                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine($"측정일자: {msrDtDe}");
                    writer.WriteLine($"측정소명: {msrSteNm}");
                    writer.WriteLine($"미세먼지(㎍/㎥) : {pm10}");
                    writer.WriteLine($"오존(ppm) : {o3}");
                    writer.WriteLine($"이산화질소농도(ppm) : {no2}");
                    writer.WriteLine($"일산화탄소농도(ppm) : {co}");
                    writer.WriteLine($"아황산가스농도(ppm) : {so2}");
                }
            }

        }

    }
    #endregion
}


