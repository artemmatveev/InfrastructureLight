using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureLight.DAL.Factory;

namespace CryptoConnectionString
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine(new ConnectionManager("nSW3QGLpp4i529CQ", "CompanyEntities").ConnectionString);
            Console.ReadKey();
        }
    }
}
