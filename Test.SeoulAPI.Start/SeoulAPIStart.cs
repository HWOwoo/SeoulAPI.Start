using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Test.SeoulAPI.Start
{
    /// <summary>
    /// 원하는 측정일자로 실행
    /// </summary>
    class SeoulApiStart
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("측정일자 yyyymmdd 형태의 매개변수가 필요함");
                return;
            }

            string data = args[0];
            var apiStart = new SeoulApiJson();
            apiStart.ReadJsonData(data);

        }
    }
}
