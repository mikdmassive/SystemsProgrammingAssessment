using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsProgrammingAssignment
{
    internal class Program
    {
        //data
        List<Vehicle> vehicles = new List<Vehicle>() { };

        //class handling
        Vehicle GetVehicleFromID(string ID)
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                Vehicle v = vehicles[i];
                if (v.ReturnVehicleID()==ID)
                {
                    return v;
                }
            }
            return null;
        }

        //main
        static void Main(string[] args)
        {
            //ARGUMENTS
            if (args.Length>0) 
            {
            }

        }
    }
}
